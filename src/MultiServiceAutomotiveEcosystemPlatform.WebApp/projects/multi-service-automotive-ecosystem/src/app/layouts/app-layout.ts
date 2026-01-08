import { Component } from '@angular/core';
import { MainLayout, NavItem } from 'multi-service-automotive-ecosystem-components';

/**
 * App Layout Component
 *
 * Application-specific layout wrapper that configures the MainLayout
 * with the navigation items for this application.
 */
@Component({
  selector: 'app-layout',
  imports: [MainLayout],
  template: `
    <ms-main-layout
      [brandName]="brandName"
      [navItems]="navItems"
      [showNavigation]="true">
    </ms-main-layout>
  `,
})
export class AppLayout {
  brandName = 'Multi-Service Auto';

  navItems: NavItem[] = [
    { label: 'Home', route: '/' },
    { label: 'Find Professionals', route: '/professionals' },
    { label: 'Dashboard', route: '/professional/dashboard' },
    { label: 'Customers', route: '/customers' },
    { label: 'Referrals', route: '/referrals' },
    { label: 'Login', route: '/customer/login' },
  ];
}
