import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingSpinnerComponent } from './components/loading.spinner.component';
import { AuthService } from './services/auth.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoadingSpinnerComponent],
  template: '<app-loading-spinner></app-loading-spinner><router-outlet></router-outlet>',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('OskUI');

  auth=inject(AuthService);
  ngOnInit(): void {
     this.auth.loadUser();
  } 
}
