import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Layout } from './pages/layout/layout';

import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';
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
      // ─── Herkesin erişebildiği sayfalar ───
      {
        path: '',
        component: Home,
      },
      {
        path: 'hf-list/:code',
        component: HfHome,
      },
      {
        path: 'hf-detail/:id',
        component: HfDetailComponent,
      },

      // ─── Sadece Admin rolü erişebilir ───
      {
        path: 'hf-types',
        component: HfType,
        canActivate: [adminGuard],
      },
      {
        path: 'personnel',
        component: Personnel,
        canActivate: [adminGuard],
      },
      {
        path: 'hf-management',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/hf-management/hf-management').then(m => m.HfManagementComponent)
      },
      {
        path: 'title',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/title/title').then(m => m.TitleComponent)
      },
      {
        path: 'branch',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/branch/branch').then(m => m.BranchComponent)
      },
      {
        path: 'pm-type',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/pm-type/pm-type').then(m => m.PmTypeComponent)
      },
      {
        path: 'roles',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/role/role.component').then(m => m.RoleComponent)
      },
      {
        path: 'users',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/user/user.component').then(m => m.UserComponent)
      },
      {
        path: 'staff',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/staff/staff').then(m => m.StaffComponent)
      },
      {
        path: 'temporaray-staff',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/temporaray-staff/temporaray-staff').then(m => m.TemporarayStaffComponent)
      },
      {
        path: 'pm-report',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/pm-report/pm-report').then(m => m.PmReportComponent)
      },
      {
        path: 'ic-bed',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/ic-bed/ic-bed.component').then(m => m.IcBedComponent)
      },
      {
        path: 'hf-report',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/hf-report/hf-report').then(m => m.HfReportComponent)
      },
      {
        path: 'bed-report',
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/bed-report/bed-report').then(m => m.BedReportComponent)
      },
      { path: '**', component: ErrorComponent },
      { path: 'errorpage', component: ErrorComponent },
    ],
  },
];
