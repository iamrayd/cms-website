import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard';
import { DashboardComponent } from './componnents/dashboard/dashboard.components';

export const routes: Routes = [
  // Auth routes (signin, signup) - using AuthLayoutComponent
  {
    path: 'auth',
    loadComponent: () => import('./components/auth-layout/auth-layout').then(m => m.AuthLayoutComponent),
    children: [
      {
        path: '',
        redirectTo: 'signin',
        pathMatch: 'full'
      },
      {
        path: 'signin',
        loadComponent: () => import('./features/auth/signin/signin').then(m => m.SigninComponent)
      },
      {
        path: 'signup',
        loadComponent: () => import('./features/auth/signup/signup').then(m => m.SignupComponent)
      }
    ]
  },
  
  // Protected routes (post-login) - using MainLayoutComponent
  {
    path: '',
    loadComponent: () => import('./components/main-layout/main-layout').then(m => m.MainLayoutComponent),
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      },
      {
        path: 'home',
        loadComponent: () => import('./home/home').then(m => m.Home)
      },
      { path: 'dashboard', component: DashboardComponent }
    ]
  },
  
  // Fallback route
  {
    path: '**',
    redirectTo: 'auth/signin'
  }
];