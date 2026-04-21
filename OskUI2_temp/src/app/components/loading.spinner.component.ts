import { Component, inject } from '@angular/core';
import { LoadingService } from '../services/loading.service';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  template: `
    @if (loadingService.isLoading()) {
      <div class="spinner-overlay" @fadeInOut>
        <div class="spinner-content">
          <div class="spinner-grow text-danger" role="status" style="width: 3rem; height: 3rem;">
            <span class="visually-hidden">Loading...</span>
          </div>
          <div class="loading-text">Yükleniyor...</div>
        </div>
      </div>
    }
  `,
  styles: [
    `
      .spinner-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(255, 255, 255, 0.6);
        backdrop-filter: blur(4px);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 9999;
      }
      .spinner-content {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 1rem;
      }
      .loading-text {
        color: #333;
        font-weight: 500;
        font-size: 1.1rem;
        letter-spacing: 0.5px;
      }
    `,
  ],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms ease-out', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0 })),
      ]),
    ]),
  ],
})
export class LoadingSpinnerComponent {
  loadingService = inject(LoadingService);
}
