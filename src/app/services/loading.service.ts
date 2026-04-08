import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private loadingCount = 0;
  private loadingTimeout: any;
  isLoading = signal<boolean>(false);

  show() {
    this.loadingCount++;
    if (this.loadingCount === 1) {
      this.loadingTimeout = setTimeout(() => {
        this.isLoading.set(true);
      }, 200);
    }
  }

  hide() {
    this.loadingCount--;
    if (this.loadingCount <= 0) {
      this.loadingCount = 0;
      if (this.loadingTimeout) {
        clearTimeout(this.loadingTimeout);
        this.loadingTimeout = null;
      }
      this.isLoading.set(false);
    }
  }
}