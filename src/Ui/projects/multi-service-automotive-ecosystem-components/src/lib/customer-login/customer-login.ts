import { Component, Input, Output, EventEmitter, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

export interface AuthenticatedUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'customer' | 'professional' | 'admin';
  token: string;
}

export interface AuthError {
  code: 'INVALID_CREDENTIALS' | 'ACCOUNT_LOCKED' | 'EMAIL_NOT_VERIFIED' | 'SERVER_ERROR';
  message: string;
  lockoutMinutes?: number;
  attemptsRemaining?: number;
}

/**
 * Customer Login Component (CM-F002)
 * 
 * A secure login page allowing customers to authenticate using email/password.
 * Includes remember me functionality and forgot password flow.
 * 
 * @example
 * ```html
 * <ms-customer-login
 *   [redirectUrl]="'/dashboard'"
 *   [enableRememberMe]="true"
 *   (loginSuccess)="handleLoginSuccess($event)"
 *   (loginError)="handleLoginError($event)">
 * </ms-customer-login>
 * ```
 */
@Component({
  selector: 'ms-customer-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './customer-login.html',
  styleUrl: './customer-login.scss',
})
export class CustomerLogin {
  /** Redirect URL after successful login */
  @Input() redirectUrl?: string;
  
  /** Enable/disable social login */
  @Input() enableSocialLogin = false;
  
  /** Enable/disable remember me */
  @Input() enableRememberMe = true;
  
  /** Pre-filled email (from registration or password reset) */
  @Input() prefillEmail?: string;
  
  /** Show success message (e.g., after password reset) */
  @Input() successMessage?: string;
  
  /** Available social login providers */
  @Input() socialProviders: ('google' | 'facebook' | 'apple')[] = [];

  @Output() loginSuccess = new EventEmitter<AuthenticatedUser>();
  @Output() loginError = new EventEmitter<AuthError>();

  loginForm: FormGroup;
  showPassword = signal(false);
  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      rememberMe: [false]
    });
  }

  ngOnInit() {
    if (this.prefillEmail) {
      this.loginForm.patchValue({ email: this.prefillEmail });
    }
  }

  togglePasswordVisibility() {
    this.showPassword.update(value => !value);
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    // Simulate API call
    setTimeout(() => {
      // For demo purposes, accept any email/password
      const formValue = this.loginForm.value;
      
      // Simulate successful login
      const user: AuthenticatedUser = {
        id: '123',
        email: formValue.email,
        firstName: 'Test',
        lastName: 'User',
        role: 'customer',
        token: 'mock-jwt-token'
      };

      this.isSubmitting.set(false);
      this.loginSuccess.emit(user);
    }, 1000);
  }

  onForgotPassword() {
    // Emit event or navigate to forgot password page
    console.log('Forgot password clicked');
  }

  onSocialLogin(provider: string) {
    console.log(`Social login with ${provider}`);
  }

  get emailControl() {
    return this.loginForm.get('email');
  }

  get passwordControl() {
    return this.loginForm.get('password');
  }

  get emailError(): string | null {
    const control = this.emailControl;
    if (control?.hasError('required') && control?.touched) {
      return 'Email is required';
    }
    if (control?.hasError('email') && control?.touched) {
      return 'Please enter a valid email address';
    }
    return null;
  }

  get passwordError(): string | null {
    const control = this.passwordControl;
    if (control?.hasError('required') && control?.touched) {
      return 'Password is required';
    }
    return null;
  }
}
