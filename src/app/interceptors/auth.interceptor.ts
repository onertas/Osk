import { SwalService } from './../services/swall.service';
import { HttpClient, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { api } from '../constants/static';
import Swal from 'sweetalert2';
import { SweetAlertService } from '../services/sweetAlert.Service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<boolean>(false);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const http = inject(HttpClient);
  const swal = inject(SweetAlertService);

  // Credentials (Cookie) desteği ekle
  const cloned = req.clone({ withCredentials: true });

  return next(cloned).pipe(
    catchError((error: HttpErrorResponse) => {
      // 1. Sunucuya erişilemiyor (Network Error)
      if (error.status === 0) {
        swal.showError('Sunucuya bağlanılamıyor. Lütfen internet bağlantınızı kontrol edin.');
        return throwError(() => error);
      }

      // 2. Eğer zaten refresh token isteği başarısız olduysa, login'e at
      if (req.url.includes('/refreshToken')) {
        isRefreshing = false;
        router.navigate(['/login']);
        return throwError(() => error);
      }

      // 3. 401 Hatası ve Refresh işlemi başlatılmamışsa
      if (error.status === 401 && !isRefreshing) {
        isRefreshing = true;
        refreshTokenSubject.next(false); // Diğerlerini beklet

        return http.post(`${api}/auth/refreshToken`, {}, { withCredentials: true }).pipe(
          switchMap(() => {
            isRefreshing = false;
            refreshTokenSubject.next(true); // Bekleyen isteklere "yol açık" sinyali ver
            return next(cloned);
          }),
          catchError((err) => {
            isRefreshing = false;
            router.navigate(['/login']);
            return throwError(() => err);
          }),
        );
      }

      // 4. 401 Hatası geldi ama şu an zaten bir refresh işlemi yürütülüyorsa
      if (error.status === 401 && isRefreshing) {
        return refreshTokenSubject.pipe(
          filter((success) => success === true), // Sadece refresh başarıyla bitince devam et
          take(1),
          switchMap(() => next(cloned)),
        );
      }

      // 5. Yetki hatası (403)
      if (error.status === 403) {
        router.navigate(['auth/access']);
      }

      return throwError(() => error);
    }),
  );
};
