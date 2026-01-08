import { Component, Input, Output, EventEmitter, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';

export interface RegisteredUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  referralCode?: string;
}

export interface RegistrationError {
  field?: string;
  code: string;
  message: string;
}

export interface Referrer {
  name: string;
  discount?: string;
}

/**
 * Customer Registration Component (CM-F001)
 * 
 * A comprehensive registration form allowing customers to create accounts on the platform.
 * Supports standard email/password registration, social login options, and referral code integration.
 * 
 * @example
 * ```html
 * <ms-customer-registration
 *   [referralCode]="'ABC123'"
 *   [enableSocialLogin]="true"
 *   (registrationSuccess)="handleSuccess($event)"
 *   (registrationError)="handleError($event)">
 * </ms-customer-registration>
 * ```
 */
@Component({
  selector: 'ms-customer-registration',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './customer-registration.html',
  styleUrl: './customer-registration.scss',
})
export class CustomerRegistration {
  /** Pre-filled referral code from URL */
  @Input() referralCode?: string;

  /** Referrer information to display */
  @Input() referrer?: Referrer;

  /** Available social login providers */
  @Input() socialProviders: ('google' | 'facebook' | 'apple')[] = ['google', 'facebook'];

  /** Custom redirect URL after registration */
  @Input() redirectUrl?: string;

  /** Enable/disable social login */
  @Input() enableSocialLogin = true;

  @Output() registrationSuccess = new EventEmitter<RegisteredUser>();
  @Output() registrationError = new EventEmitter<RegistrationError>();

  registrationForm: FormGroup;
  showPassword = signal(false);
  showConfirmPassword = signal(false);
  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);
  
  passwordStrength = computed(() => {
    const password = this.registrationForm?.get('password')?.value || '';
    return this.calculatePasswordStrength(password);
  });

  constructor(private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      firstName: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50),
        Validators.pattern(/^[A-Za-z\s\-']+$/)
      ]],
      lastName: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50),
        Validators.pattern(/^[A-Za-z\s\-']+$/)
      ]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, this.phoneValidator]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        this.passwordValidator
      ]],
      confirmPassword: ['', [Validators.required]],
      marketingConsent: [false],
      termsAccept: [false, [Validators.requiredTrue]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit() {
    if (this.referralCode) {
      // Referral code is set via input
    }
  }

  phoneValidator(control: AbstractControl): ValidationErrors | null {
    const phonePattern = /^\(\d{3}\) \d{3}-\d{4}$/;
    if (control.value && !phonePattern.test(control.value)) {
      return { phone: true };
    }
    return null;
  }

  passwordValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;

    const errors: ValidationErrors = {};
    if (!/[A-Z]/.test(value)) errors['uppercase'] = true;
    if (!/[a-z]/.test(value)) errors['lowercase'] = true;
    if (!/\d/.test(value)) errors['number'] = true;

    return Object.keys(errors).length > 0 ? errors : null;
  }

  passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    
    if (confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }

  calculatePasswordStrength(password: string): 'weak' | 'fair' | 'good' | 'strong' {
    if (!password || password.length < 8) return 'weak';

    let score = 0;
    if (/[A-Z]/.test(password)) score++;
    if (/[a-z]/.test(password)) score++;
    if (/\d/.test(password)) score++;
    if (/[^A-Za-z0-9]/.test(password)) score++;

    if (score <= 2) return 'fair';
    if (score === 3) return 'good';
    return 'strong';
  }

  get passwordStrengthPercent(): number {
    const strength = this.passwordStrength();
    const map = { weak: 25, fair: 50, good: 75, strong: 100 };
    return map[strength];
  }

  get passwordStrengthColor(): string {
    const strength = this.passwordStrength();
    const map = { weak: '#D32F2F', fair: '#ED6C02', good: '#FFD520', strong: '#2E7D32' };
    return map[strength];
  }

  togglePasswordVisibility(field: 'password' | 'confirmPassword') {
    if (field === 'password') {
      this.showPassword.update(value => !value);
    } else {
      this.showConfirmPassword.update(value => !value);
    }
  }

  formatPhoneNumber(event: Event) {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');
    
    if (value.length <= 3) {
      value = value;
    } else if (value.length <= 6) {
      value = `(${value.slice(0, 3)}) ${value.slice(3)}`;
    } else {
      value = `(${value.slice(0, 3)}) ${value.slice(3, 6)}-${value.slice(6, 10)}`;
    }
    
    this.registrationForm.patchValue({ phone: value }, { emitEvent: false });
  }

  onSubmit() {
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    // Simulate API call
    setTimeout(() => {
      const formValue = this.registrationForm.value;
      
      const user: RegisteredUser = {
        id: Math.random().toString(36).substr(2, 9),
        email: formValue.email,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        phone: formValue.phone,
        referralCode: this.referralCode
      };

      this.isSubmitting.set(false);
      this.registrationSuccess.emit(user);
    }, 1500);
  }

  onSocialLogin(provider: 'google' | 'facebook' | 'apple') {
    console.log(`Social login with ${provider}`);
  }

  // Error message getters
  get firstNameError(): string | null {
    const control = this.registrationForm.get('firstName');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'First name is required';
    if (control.hasError('minlength')) return 'First name must be at least 2 characters';
    if (control.hasError('maxlength')) return 'First name must be less than 50 characters';
    if (control.hasError('pattern')) return 'First name can only contain letters';
    return null;
  }

  get lastNameError(): string | null {
    const control = this.registrationForm.get('lastName');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'Last name is required';
    if (control.hasError('minlength')) return 'Last name must be at least 2 characters';
    if (control.hasError('maxlength')) return 'Last name must be less than 50 characters';
    if (control.hasError('pattern')) return 'Last name can only contain letters';
    return null;
  }

  get emailError(): string | null {
    const control = this.registrationForm.get('email');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'Email is required';
    if (control.hasError('email')) return 'Please enter a valid email';
    return null;
  }

  get phoneError(): string | null {
    const control = this.registrationForm.get('phone');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'Phone number is required';
    if (control.hasError('phone')) return 'Please enter a valid phone number';
    return null;
  }

  get passwordError(): string | null {
    const control = this.registrationForm.get('password');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'Password is required';
    if (control.hasError('minlength')) return 'Password must be at least 8 characters';
    if (control.hasError('uppercase')) return 'Password must contain an uppercase letter';
    if (control.hasError('lowercase')) return 'Password must contain a lowercase letter';
    if (control.hasError('number')) return 'Password must contain a number';
    return null;
  }

  get confirmPasswordError(): string | null {
    const control = this.registrationForm.get('confirmPassword');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'Please confirm your password';
    if (this.registrationForm.hasError('passwordMismatch')) return 'Passwords do not match';
    return null;
  }

  get termsError(): string | null {
    const control = this.registrationForm.get('termsAccept');
    if (!control?.touched) return null;
    
    if (control.hasError('required')) return 'You must accept the terms and conditions';
    return null;
  }
}
