import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Layout } from './pages/layout/layout';
import { ErrorComponent } from './error-component/error-component';

export const routes: Routes = [
  //  {
  //     path: 'login',
  //     component: LoginComponent,
  //   },

  {
    path: '',
    component: Layout,
    // canActivateChild: [authGuard],
    children: [
      {
        path: '',
        component: Home,
      },
        { path: '**', component: ErrorComponent },
{ path: 'errorpage', component: ErrorComponent },
      
    ],
    
  },

];
