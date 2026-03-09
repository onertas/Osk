import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingSpinnerComponent } from './components/loading.spinner.component';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoadingSpinnerComponent],
  template: '<app-loading-spinner></app-loading-spinner><router-outlet></router-outlet>',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('OskUI');
}
