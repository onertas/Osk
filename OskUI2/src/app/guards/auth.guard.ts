import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { HttpApiService } from "../services/http-api-service";
import { HttpClient } from "@angular/common/http";
import { catchError, map, of, switchMap } from "rxjs";
import { firstValueFrom } from "rxjs";
import { api } from "../constants/static";

export const authGuard: CanActivateFn = async (route, state) => {
  const router = inject(Router);
  const http = inject(HttpApiService);
  const httpClient = inject(HttpClient);

  try {
    const result = await firstValueFrom(
      http.get<any>('auth/IsAuthenticated').pipe(
        switchMap((res: any) => {
          if (res.success && res.data === true) {
            return of(true);
          }

          // Access token süresi dolmuş, refresh token ile yenilemeyi dene
          return httpClient.post(`${api}/auth/refreshToken`, {}, { withCredentials: true }).pipe(
            switchMap(() => {
              // Refresh başarılı, tekrar kontrol et
              return http.get<any>('auth/IsAuthenticated').pipe(
                map((retryRes: any) => {
                  if (retryRes.success && retryRes.data === true) {
                    return true;
                  }
                  router.navigate(['/login']);
                  return false;
                })
              );
            }),
            catchError(() => {
              // Refresh token da geçersiz, login'e yönlendir
              router.navigate(['/login']);
              return of(false);
            })
          );
        }),
        catchError(() => {
          router.navigate(['/login']);
          return of(false);
        })
      )
    );
    return result;
  } catch {
    router.navigate(['/login']);
    return false;
  }
};

