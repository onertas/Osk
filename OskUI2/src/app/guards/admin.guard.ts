import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { filter, map, take } from "rxjs";

/**
 * Admin rolüne sahip olmayan kullanıcıların yetkili sayfalarına
 * URL ile doğrudan erişimini engeller.
 */
export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.user$.pipe(
    filter(user => user !== null),   // Kullanıcı bilgisi yüklenene kadar bekle
    take(1),
    map(user => {
      if (user!.roles.includes('Admin')) {
        return true;
      }
      router.navigate(['/']);
      return false;
    })
  );
};
