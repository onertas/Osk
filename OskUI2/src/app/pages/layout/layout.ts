import { Component, inject, OnInit } from '@angular/core';
import { Navbar } from './navbar/navbar';
import { MainSidebar } from './main-sidebar/main-sidebar';
import { ControlSidebar } from './control-sidebar/control-sidebar';
import { Footer } from './footer/footer';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout',
  imports: [Navbar, MainSidebar, ControlSidebar, Footer, RouterOutlet],
  standalone: true,
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit {
  private authService = inject(AuthService);

  ngOnInit(): void {
    // Sayfa yenilendiğinde kullanıcı bilgisini tekrar yükle
    if (!this.authService.snapshot) {
      this.authService.loadUser();
    }
  }
}
