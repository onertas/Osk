import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Layout } from './pages/layout/layout';

import { authGuard } from './guards/auth.guard';
import { Login } from './pages/login/login';
import { HfHome } from './pages/hf-home/hf-home';
import { HfDetailComponent } from './pages/hf-detail/hf-detail';
import { ErrorComponent } from './components/error-component/error-component';
import { Personnel } from './pages/personnel/personnel';
import { HfType } from './pages/hf-type/hf-type';

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
        path: 'hf-types',
        component: HfType,
      },
      {
        path: 'hf-list/:code',
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
        path: 'hf-management',
        loadComponent: () => import('./pages/hf-management/hf-management').then(m => m.HfManagementComponent)
      },
      {
        path: 'title',
        loadComponent: () => import('./pages/title/title').then(m => m.TitleComponent)
      },
      {
        path: 'branch',
        loadComponent: () => import('./pages/branch/branch').then(m => m.BranchComponent)
      },
      {
        path: 'pm-type',
        loadComponent: () => import('./pages/pm-type/pm-type').then(m => m.PmTypeComponent)
      },
      {
        path: 'roles',
        loadComponent: () => import('./pages/role/role.component').then(m => m.RoleComponent)
      },
      {
        path: 'users',
        loadComponent: () => import('./pages/user/user.component').then(m => m.UserComponent)
      },
      { path: '**', component: ErrorComponent },
      { path: 'errorpage', component: ErrorComponent },
    ],
  },
];
