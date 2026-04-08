import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { HttpApiService } from "../services/http-api-service";
import { firstValueFrom } from "rxjs";

export const authGuard: CanActivateFn = async (route, state) => {
  const router = inject(Router);
  const http = inject(HttpApiService);
  try {
    const res = await firstValueFrom(http.get<any>('auth/IsAuthenticated'));
    if (res.success && res.data === true) {
      return true;
    }
    
    router.navigate(['/login']);
    return false;
  } catch (err: any) {
    router.navigate(['/login']);
    return false;
  }
};
