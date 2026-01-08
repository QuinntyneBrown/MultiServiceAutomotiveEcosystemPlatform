import { Routes } from '@angular/router';
import { AppLayout } from './layouts/app-layout';

export const routes: Routes = [
  {
    path: '',
    component: AppLayout,
    children: [
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
        path: 'professional/dashboard',
        loadComponent: () => import('./pages/professional-dashboard/professional-dashboard-home').then(m => m.ProfessionalDashboardHome)
      },
      {
        path: 'professional/profile',
        loadComponent: () => import('./pages/professional-dashboard/profile-management').then(m => m.ProfileManagement)
      },
      {
        path: 'professional/specialties',
        loadComponent: () => import('./pages/professional-dashboard/specialty-management').then(m => m.SpecialtyManagement)
      },
      {
        path: 'professional/referrals',
        loadComponent: () => import('./pages/professional-referrals/professional-referral-dashboard').then(m => m.ProfessionalReferralDashboard)
      },
      {
        path: 'professional/referrals/send',
        loadComponent: () => import('./pages/professional-referrals/send-referral/send-professional-referral').then(m => m.SendProfessionalReferral)
      },
      {
        path: 'professional/referrals/received/:id',
        loadComponent: () => import('./pages/professional-referrals/referral-action/referral-action').then(m => m.ReferralAction)
      },
      {
        path: 'referrals',
        loadComponent: () => import('./pages/referrals/referral-dashboard').then(m => m.ReferralDashboard)
      }
    ]
  },
  {
    path: 'r/:code',
    loadComponent: () => import('./pages/referral-landing/referral-landing').then(m => m.ReferralLanding)
  },
  {
    path: '**',
    redirectTo: ''
  }
];
