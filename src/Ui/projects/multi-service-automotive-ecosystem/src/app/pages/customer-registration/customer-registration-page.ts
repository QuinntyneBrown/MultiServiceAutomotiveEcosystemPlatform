import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CustomerRegistration, RegisteredUser, RegistrationError } from 'multi-service-automotive-ecosystem-components';

@Component({
  selector: 'app-customer-registration-page',
  imports: [CommonModule, CustomerRegistration],
  templateUrl: './customer-registration-page.html',
  styleUrl: './customer-registration-page.scss',
})
export class CustomerRegistrationPage {
  constructor(private router: Router) {}

  onRegister(user: RegisteredUser): void {
    console.log('User registered:', user);
    // Navigate to login or welcome page
    this.router.navigate(['/customer/login']);
  }

  onError(error: RegistrationError): void {
    console.error('Registration error:', error);
  }
}
