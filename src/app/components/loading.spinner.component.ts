// loading-spinner.component.ts
import { Component, inject } from '@angular/core';
import { LoadingService } from '../services/loading.service';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  template: `
    @if (loadingService.isLoading()) {
      <div class="spinner-overlay">
       <div class="spinner-grow text-danger" role="status">
  <span class="sr-only">Loading...</span>
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
        background: rgba(0, 0, 0, 0.8);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 9999;
      }
      .loader {
        /* CSS Spinner kodlarınız buraya */
        border: 5px solid #f3f3f3;
        border-top: 5px solid #a10000;
        border-radius: 50%;
        width: 50px;
        height: 50px;
        animation: spin 2s linear infinite;
      }
      @keyframes spin {
        0% {
          transform: rotate(0deg);
        }
        100% {
          transform: rotate(360deg);
        }
      }
    `,
  ],
})
export class LoadingSpinnerComponent {
  loadingService = inject(LoadingService);
}
