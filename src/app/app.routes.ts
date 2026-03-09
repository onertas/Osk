import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Layout } from './pages/layout/layout';
import { ErrorComponent } from './error-component/error-component';
import { authGuard } from './guards/auth.guard';
import { Login } from './pages/login/login';

export const routes: Routes = [
   {
      path: 'login',
      component: Login,
    },

  {
    path: '',
    component: Layout,
    canActivateChild: [authGuard],
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
