import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorService } from '../services/error-service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const errorService = inject(ErrorService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Merkezi hata yönetimine gönder
      errorService.errorHandler(error);
      
      // Hatayı yukarı fırlatmaya devam et (Bileşen isterse kendisi de yakalayabilsin)
      return throwError(() => error);
    })
  );
};
