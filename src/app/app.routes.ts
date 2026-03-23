import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Layout } from './pages/layout/layout';

import { authGuard } from './guards/auth.guard';
import { Login } from './pages/login/login';
import { HfHome } from './pages/hf-home/hf-home';
import { HfDetailComponent } from './pages/hf-detail/hf-detail';
import { ErrorComponent } from './components/error-component/error-component';
import { Personnel } from './pages/personnel/personnel';

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
      {
        path: 'hf/:code',
        component: HfHome,
      },
      {
        path: 'personnel',
        component: Personnel,
      },
      {
        path: 'hf-detail/:id',
        component: HfDetailComponent,
      },
      {
        path: 'title',
        loadComponent: () => import('./pages/title/title').then(m => m.TitleComponent)
      },
      {
        path: 'branch',
        loadComponent: () => import('./pages/branch/branch').then(m => m.BranchComponent)
      },
      { path: '**', component: ErrorComponent },
      { path: 'errorpage', component: ErrorComponent },
    ],
  },
];
