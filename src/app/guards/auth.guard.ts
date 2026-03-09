import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { HttpApiService } from "../services/http-api-service";
import { firstValueFrom } from "rxjs";

export const authGuard: CanActivateFn = async (route, state) => {
  const router = inject(Router);
  const http = inject(HttpApiService);

  try {
    await firstValueFrom(http.get<any>('auth/IsAuthenticated'));
    return true;
  } catch (err: any) {
    // 401 veya başka bir hata fark etmeksizin,
    // girişi yasak olan kullanıcıyı login sayfasına gönderin.
    router.navigate(['/login']);
    return false;
  }
};