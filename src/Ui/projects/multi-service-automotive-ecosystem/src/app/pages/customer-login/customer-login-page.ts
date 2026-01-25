import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CustomerLogin, AuthenticatedUser, AuthError } from 'multi-service-automotive-ecosystem-components';

@Component({
  selector: 'app-customer-login-page',
  imports: [CommonModule, CustomerLogin],
  templateUrl: './customer-login-page.html',
  styleUrl: './customer-login-page.scss',
})
export class CustomerLoginPage {
  constructor(private router: Router) {}

  onLogin(user: AuthenticatedUser): void {
    console.log('User logged in:', user);
    // Navigate to customer dashboard or home
    this.router.navigate(['/']);
  }

  onError(error: AuthError): void {
    console.error('Login error:', error);
  }
}
