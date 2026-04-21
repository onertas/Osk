import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { loadingInterceptor } from './interceptors/loading.interceptor';
import { authInterceptor } from './interceptors/auth.interceptor';
import { errorInterceptor } from './interceptors/error.interceptor';

import { providePrimeNG } from 'primeng/config';
import Lara from '@primeuix/themes/lara';
import Aura from '@primeuix/themes/aura';
import Metarial from '@primeuix/themes/material';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, loadingInterceptor])), 
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Metarial,
        options: {
          darkModeSelector: false,
          prefix: 'p',
          cssLayer: false,

        }
      },
      translation: {
        dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
        dayNamesShort: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
        dayNamesMin: ["Pa", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
        monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
        monthNamesShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
        today: "Bugün",
        clear: "Temizle",
        weekHeader: "Hf",
        firstDayOfWeek: 1,
        accept: "Tamam",
        reject: "İptal",
        emptyMessage: "Kayıt bulunamadı",
        choose: "Seç",
        dateFormat: 'dd.mm.yy', // 'yy' 4 haneli yılı (2026) temsil eder
        
      }
    })


  ]


};

