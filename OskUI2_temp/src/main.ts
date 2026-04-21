import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

declare var $: any;
(window as any).jQuery = $;
(window as any).$ = $;

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
