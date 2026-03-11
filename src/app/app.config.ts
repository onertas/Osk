import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { loadingInterceptor } from './interceptors/loading.interceptor';
import { authInterceptor } from './interceptors/auth.interceptor';

import { providePrimeNG } from 'primeng/config';
import Lara from '@primeuix/themes/lara';
import Aura from '@primeuix/themes/aura';
import Metarial from '@primeuix/themes/material';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient( withInterceptors([authInterceptor,loadingInterceptor])), // 2. Buraya ekle,
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
     providePrimeNG({
            theme: {
                preset: Metarial,
                options: {
                    darkModeSelector: false,
                    prefix: 'p',
               
                    cssLayer: false,
                    
                }
            }
        })
    
    
  ]

  
};

