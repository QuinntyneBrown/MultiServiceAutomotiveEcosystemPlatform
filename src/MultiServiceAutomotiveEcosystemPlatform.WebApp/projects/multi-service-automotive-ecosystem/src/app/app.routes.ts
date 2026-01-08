import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home').then(m => m.Home)
  },
  {
    path: 'customer/login',
    loadComponent: () => import('./pages/customer-login/customer-login-page').then(m => m.CustomerLoginPage)
  },
  {
    path: 'customer/register',
    loadComponent: () => import('./pages/customer-registration/customer-registration-page').then(m => m.CustomerRegistrationPage)
  },
  {
    path: 'professionals',
    loadComponent: () => import('./pages/professionals/professional-directory').then(m => m.ProfessionalDirectory)
  },
  {
    path: 'professionals/:slug',
    loadComponent: () => import('./pages/professionals/professional-profile').then(m => m.ProfessionalProfile)
  },
  {
    path: 'referrals',
    loadComponent: () => import('./pages/referrals/referral-dashboard').then(m => m.ReferralDashboard)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
