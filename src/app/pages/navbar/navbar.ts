import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';

@Component({
  selector: 'app-navbar',
  imports: [SharedModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
router = inject(Router);
http=inject(HttpApiService);


   logout() {
  
    this.http.post('auth/logout', {}).subscribe(() => {
      // Başarılı çıkış işlemi
      this.router.navigate(['/login']);
    }, (error) => {
      // Hata durumunda da login sayfasına yönlendir
      this.router.navigate(['/login']);
    });
    this.router.navigate(['/login']);
  }
}
