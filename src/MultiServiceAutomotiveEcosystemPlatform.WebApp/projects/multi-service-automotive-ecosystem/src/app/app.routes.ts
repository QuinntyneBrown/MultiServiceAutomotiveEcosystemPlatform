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
    path: 'customers',
    loadComponent: () => import('./pages/customers/customer-list').then(m => m.CustomerList)
  },
  {
    path: 'customers/new',
    loadComponent: () => import('./pages/customers/customer-form').then(m => m.CustomerForm)
  },
  {
    path: 'customers/:id',
    loadComponent: () => import('./pages/customers/customer-detail').then(m => m.CustomerDetail)
  },
  {
    path: 'customers/:id/edit',
    loadComponent: () => import('./pages/customers/customer-form').then(m => m.CustomerForm)
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
