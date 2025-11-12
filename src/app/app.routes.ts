import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  // Auth routes (signin, signup) - using AuthLayoutComponent
  {
    path: '',
    loadComponent: () => import('./components/auth-layout/auth-layout').then(m => m.AuthLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/auth/signup/signup').then(m => m.SignupComponent)
      },
      {
        path: 'signin',
        loadComponent: () => import('./features/auth/signin/signin').then(m => m.SigninComponent)
      }
    ]
  },
  
  
  {
    path: '',
    loadComponent: () => import('./components/main-layout/main-layout').then(m => m.MainLayoutComponent),
    canActivate: [AuthGuard],
    children: [
      {
        path: 'home',
        loadComponent: () => import('./home/home').then(m => m.Home)
      }

    ]
  },
  
  // Fallback route
  {
    path: '**',
    redirectTo: ''
  }
];