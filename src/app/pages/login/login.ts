import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { AuthService } from '../../services/auth.service';

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

  email: string = '';
  password: string = '';
  checked: boolean = false;

  Login() {
    this.http
      .post('auth/login', { username: this.email, password: this.password })
      .subscribe((res) => {
        // Başarılı giriş işlemi
//localStorage.setItem('isLoggedIn', 'true'); 
        this.router.navigate(['/']);
      });
  }
}
