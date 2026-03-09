
import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SwalService } from './swall.service';


@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  constructor(

    private route: Router,
    private swalService: SwalService
   
  ) {}

  errorHandler(err: HttpErrorResponse) {

   
    if (err.status == 0 || err.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('loggedUser');
      this.swalService.showError("Server Error or Unauthorized Request")
      this.route.navigateByUrl('/login');
    }
    if (err.status === 403) {
      let errorMessage = '';
      for (const e of err.error.ErrorMessages) {
        errorMessage += e + '\n';
      }
      this.swalService.showError(errorMessage);
    }
    if (err.status === 404) {
       this.route.navigateByUrl('/errorpage');
      let errorMessage = '';
      for (const e of err.error.ErrorMessages) {
        errorMessage += e + '\n';
      }
         }
    if (err.status === 500) {
  
      this.swalService.showError(
              err.error['detail']
      );
    }
    if (err.status === 400) {
      if (err.error && err.error.errors) {
        const errorMessages = Object.entries(err.error.errors)
          .map(([key, value]) => `${key}: ${(value as string[]).join(", ")}`)
          .join("\n");
      
        this.swalService.showError(errorMessages);
      } else {
        this.swalService.showError(err.error);
      }
      
      
    }
    if (err.status === 405) {
      this.swalService.showError('metod izinli değil');
    }
  }
}
