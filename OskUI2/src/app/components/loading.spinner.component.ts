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
          <div class="custom-spinner"></div>
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
        background: rgba(255, 255, 255, 0.7);
        backdrop-filter: blur(8px);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 9999;
      }
      .spinner-content {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 1.2rem;
      }
      .custom-spinner {
        width: 60px;
        height: 60px;
        border: 4px solid transparent;
        border-top-color: #0b22a5;
        border-bottom-color: #3a5ce5;
        border-radius: 50%;
        animation: spin 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
        position: relative;
        box-shadow: 0 0 20px rgba(11, 34, 165, 0.15);
      }
      .custom-spinner::before {
        content: '';
        position: absolute;
        top: 4px;
        left: 4px;
        right: 4px;
        bottom: 4px;
        border: 3px solid transparent;
        border-left-color: #3a5ce5;
        border-right-color: #0b22a5;
        border-radius: 50%;
        animation: spin-reverse 0.8s linear infinite;
      }
      @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
      }
      @keyframes spin-reverse {
        0% { transform: rotate(360deg); }
        100% { transform: rotate(0deg); }
      }
      .loading-text {
        color: #0b22a5;
        font-weight: 600;
        font-size: 1.1rem;
        letter-spacing: 2px;
        text-transform: uppercase;
        animation: pulseText 1.5s ease-in-out infinite;
      }
      @keyframes pulseText {
        0%, 100% { opacity: 1; text-shadow: 0 0 8px rgba(11, 34, 165, 0); }
        50% { opacity: 0.6; text-shadow: 0 0 8px rgba(11, 34, 165, 0.4); }
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
