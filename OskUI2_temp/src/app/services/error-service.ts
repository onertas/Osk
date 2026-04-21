import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SwalService } from './swall.service';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  private route = inject(Router);
  private swalService = inject(SwalService);

  private lastErrorMessage: string = '';
  private lastErrorTime: number = 0;
  private readonly FLOOD_THRESHOLD = 2000; // 2 saniye içinde aynı hatayı gösterme

  errorHandler(err: HttpErrorResponse) {
    const silentUrls = ['auth/IsAuthenticated', 'auth/refreshToken', 'auth/isauthenticated', 'auth/refreshtoken'];
    const isSilent = silentUrls.some(url => err.url?.toLowerCase().includes(url.toLowerCase()));

    // Sessiz kontrollerde (IsAuthenticated vb.) 401 hatası gelirse hiçbir şey yapmıyoruz.
    if (isSilent && err.status === 401) {
      return;
    }

    const errorMsg = this.extractErrorMessage(err);
    const currentTime = Date.now();

    // Flood Prevention: Aynı hata mesajı kısa süre içinde tekrar gelirse gösterme
    if (errorMsg === this.lastErrorMessage && (currentTime - this.lastErrorTime) < this.FLOOD_THRESHOLD) {
      return;
    }

    this.lastErrorMessage = errorMsg;
    this.lastErrorTime = currentTime;

    // Detaylı Konsol Loglama (Sadece gerçek hatalar için)
    console.group('🚀 API Error Details');
    console.error('URL:', err.url);
    console.error('Status:', err.status);
    console.error('Status Text:', err.statusText);
    console.error('Raw Error Body:', err.error);
    console.error('Extracted Message:', errorMsg);
    console.groupEnd();

    switch (err.status) {
      case 0:
        this.swalService.showError("Sunucuya ulaşılamıyor. Lütfen internet bağlantınızı veya sunucu durumunu kontrol edin.");
        break;

      case 401:
        localStorage.removeItem('loggedUser');
        this.swalService.showError("Oturumunuz sona erdi. Güvenliğiniz için tekrar giriş yapmanız gerekiyor.");
        this.route.navigateByUrl('/login');
        break;

      case 403:
        this.swalService.showError("Bu işlem için yetkiniz bulunmuyor. Lütfen yönetici ile iletişime geçin.");
        break;

      case 404:
        this.swalService.showError("İstediğiniz kaynak sunucuda bulunamadı.");
        // Gerekirse sayfaya yönlendir: this.route.navigateByUrl('/errorpage');
        break;

      case 400:
        this.swalService.showError(errorMsg);
        break;

      case 500:
        this.swalService.showError("Sunucu taraflı bir hata oluştu. Lütfen teknik ekibe bildirin.");
        break;

      case 405:
        this.swalService.showError('Bu işlem için kullanılan HTTP metodu sunucu tarafından reddedildi.');
        break;

      default:
        this.swalService.showError(errorMsg || "Beklenmedik bir hata oluştu.");
        break;
    }
  }

  /**
   * API'den gelen karmaşık hata objelerini kullanıcı dostu bir metne dönüştürür.
   */
  private extractErrorMessage(err: HttpErrorResponse): string {
    if (!err.error) return err.message || 'Bilinmeyen Hata';

    // 1. Düz Metin
    if (typeof err.error === 'string') return err.error;

    // 2. ASP.NET Validation Errors (ModelState/Identity)
    if (err.error.errors) {
      const messages: string[] = [];
      Object.entries(err.error.errors).forEach(([key, value]) => {
        if (Array.isArray(value)) {
          messages.push(...value);
        } else if (typeof value === 'string') {
          messages.push(value);
        }
      });
      return messages.join('\n');
    }

    // 3. Custom Result Format { success: false, message: "..." }
    if (err.error.message) return err.error.message;

    // 4. Detaylı Hata (ProblemDetails formatı)
    if (err.error.detail) return err.error.detail;

    // 5. Başlık (Title)
    if (err.error.title) return err.error.title;

    return err.message || 'Bilinmeyen bir hata oluştu.';
  }
}
