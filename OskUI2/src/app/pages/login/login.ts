import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { AuthService } from '../../services/auth.service';
import { SwalService } from '../../services/swall.service';

@Component({
  selector: 'app-login',
  imports: [SharedModule],
  standalone: true,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  router = inject(Router);
  http = inject(HttpApiService);
  auth = inject(AuthService);
  swal = inject(SwalService);

  email: string = '';
  password: string = '';
  checked: boolean = false;

  Login() {
    this.http
      .post('auth/login', { username: this.email, password: this.password })
      .subscribe({
        next: (res: any) => {
          if (res && res.success === false) {
            this.swal.showError(res.message || "Giriş başarısız! Lütfen bilgilerinizi kontrol edin.");
          } else {
            // Başarılı giriş işlemi
            this.auth.loadUser(); 
            this.router.navigate(['/']);
          }
        },
        error: (err: any) => {
          console.error("Login component error:", err);
          let errorMessage = "Sunucuya bağlanılamadı veya bir hata oluştu.";
          if (err.error && typeof err.error === 'string') {
              errorMessage = err.error;
          } else if (err.error && err.error.message) {
              errorMessage = err.error.message;
          }
          this.swal.showError(errorMessage);
        }
      });
  }
}
