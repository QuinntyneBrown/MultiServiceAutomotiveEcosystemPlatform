import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink } from '@angular/router';

/**
 * Navigation Item interface for the main layout header
 */
export interface NavItem {
  label: string;
  route: string;
  icon?: string;
}

/**
 * Main Layout Component
 *
 * A reusable layout component that provides a consistent structure for all pages
 * including a site-wide navigation header and content area with router outlet.
 *
 * @example
 * ```html
 * <ms-main-layout
 *   [brandName]="'Multi-Service Auto'"
 *   [navItems]="navigationItems"
 *   [showNavigation]="true">
 * </ms-main-layout>
 * ```
 */
@Component({
  selector: 'ms-main-layout',
  imports: [CommonModule, RouterOutlet, RouterLink],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {
  /** Brand name displayed in the header */
  @Input() brandName: string = 'Multi-Service Auto';

  /** Navigation items for the header */
  @Input() navItems: NavItem[] = [];

  /** Whether to show the navigation bar */
  @Input() showNavigation: boolean = true;

  /** Whether the mobile menu is open */
  mobileMenuOpen = false;

  toggleMobileMenu(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen = false;
  }
}
