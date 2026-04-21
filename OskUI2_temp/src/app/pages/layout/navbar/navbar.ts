import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '../../../modules/shared.module';
import { HttpApiService } from '../../../services/http-api-service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [SharedModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  router = inject(Router);
  http = inject(HttpApiService);
  auth = inject(AuthService);

  logout() {
    this.auth.clear();
    this.http.post('auth/logout', {}).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: () => {
        this.router.navigate(['/login']);
      }
    });
  }
}
