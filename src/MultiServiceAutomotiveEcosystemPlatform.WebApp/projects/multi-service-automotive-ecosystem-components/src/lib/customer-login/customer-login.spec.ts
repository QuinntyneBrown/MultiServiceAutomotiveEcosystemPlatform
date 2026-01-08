import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomerLogin } from './customer-login';
import { vi } from 'vitest';

describe('CustomerLogin', () => {
  let component: CustomerLogin;
  let fixture: ComponentFixture<CustomerLogin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerLogin, ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerLogin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Form Initialization', () => {
    it('should initialize form with empty values', () => {
      expect(component.loginForm.value).toEqual({
        email: '',
        password: '',
        rememberMe: false,
      });
    });

    it('should prefill email when provided', () => {
      component.prefillEmail = 'test@example.com';
      component.ngOnInit();
      expect(component.loginForm.value.email).toBe('test@example.com');
    });

    it('should initialize with default input values', () => {
      expect(component.enableSocialLogin).toBe(false);
      expect(component.enableRememberMe).toBe(true);
      expect(component.socialProviders).toEqual([]);
    });
  });

  describe('Form Validation', () => {
    it('should require email field', () => {
      const emailControl = component.loginForm.get('email');
      expect(emailControl?.valid).toBeFalsy();
      expect(emailControl?.hasError('required')).toBeTruthy();
    });

    it('should validate email format', () => {
      const emailControl = component.loginForm.get('email');
      emailControl?.setValue('invalid-email');
      expect(emailControl?.hasError('email')).toBeTruthy();

      emailControl?.setValue('valid@example.com');
      expect(emailControl?.hasError('email')).toBeFalsy();
    });

    it('should require password field', () => {
      const passwordControl = component.loginForm.get('password');
      expect(passwordControl?.valid).toBeFalsy();
      expect(passwordControl?.hasError('required')).toBeTruthy();
    });

    it('should accept valid password', () => {
      const passwordControl = component.loginForm.get('password');
      passwordControl?.setValue('password123');
      expect(passwordControl?.valid).toBeTruthy();
    });
  });

  describe('Error Messages', () => {
    it('should return email required error', () => {
      const emailControl = component.loginForm.get('email');
      emailControl?.markAsTouched();
      expect(component.emailError).toBe('Email is required');
    });

    it('should return email format error', () => {
      const emailControl = component.loginForm.get('email');
      emailControl?.setValue('invalid');
      emailControl?.markAsTouched();
      expect(component.emailError).toBe('Please enter a valid email address');
    });

    it('should return password required error', () => {
      const passwordControl = component.loginForm.get('password');
      passwordControl?.markAsTouched();
      expect(component.passwordError).toBe('Password is required');
    });

    it('should return null when field is valid', () => {
      const emailControl = component.loginForm.get('email');
      emailControl?.setValue('test@example.com');
      emailControl?.markAsTouched();
      expect(component.emailError).toBeNull();
    });
  });

  describe('Password Visibility Toggle', () => {
    it('should initialize with password hidden', () => {
      expect(component.showPassword()).toBe(false);
    });

    it('should toggle password visibility', () => {
      component.togglePasswordVisibility();
      expect(component.showPassword()).toBe(true);

      component.togglePasswordVisibility();
      expect(component.showPassword()).toBe(false);
    });
  });

  describe('Form Submission', () => {
    it('should not submit if form is invalid', () => {
      const spy = vi.spyOn(component.loginSuccess, 'emit');
      component.onSubmit();
      expect(spy).not.toHaveBeenCalled();
    });

    it('should mark all fields as touched on invalid submit', () => {
      component.onSubmit();
      expect(component.loginForm.get('email')?.touched).toBeTruthy();
      expect(component.loginForm.get('password')?.touched).toBeTruthy();
    });

    it('should set isSubmitting to true during submission', () => {
      component.loginForm.setValue({
        email: 'test@example.com',
        password: 'password123',
        rememberMe: false,
      });
      
      component.onSubmit();
      expect(component.isSubmitting()).toBe(true);
    });

    it('should clear error message on new submission', () => {
      component.errorMessage.set('Previous error');
      component.loginForm.setValue({
        email: 'test@example.com',
        password: 'password123',
        rememberMe: false,
      });
      
      component.onSubmit();
      expect(component.errorMessage()).toBeNull();
    });

    it('should emit loginSuccess with user data', (ctx) => {
      return new Promise<void>((resolve) => {
        component.loginForm.setValue({
          email: 'test@example.com',
          password: 'password123',
          rememberMe: false,
        });

        component.loginSuccess.subscribe((user) => {
          expect(user.email).toBe('test@example.com');
          expect(user.role).toBe('customer');
          expect(user.token).toBe('mock-jwt-token');
          resolve();
        });

        component.onSubmit();
      });
    });
  });

  describe('Social Login', () => {
    it('should handle social login button click', () => {
      const consoleSpy = vi.spyOn(console, 'log');
      component.onSocialLogin('google');
      expect(consoleSpy).toHaveBeenCalledWith('Social login with google');
    });
  });

  describe('Forgot Password', () => {
    it('should handle forgot password click', () => {
      const consoleSpy = vi.spyOn(console, 'log');
      component.onForgotPassword();
      expect(consoleSpy).toHaveBeenCalledWith('Forgot password clicked');
    });
  });

  describe('Component Inputs', () => {
    it('should accept custom redirectUrl', () => {
      component.redirectUrl = '/dashboard';
      expect(component.redirectUrl).toBe('/dashboard');
    });

    it('should accept social providers', () => {
      component.socialProviders = ['google', 'facebook'];
      expect(component.socialProviders).toEqual(['google', 'facebook']);
    });

    it('should accept success message', () => {
      component.successMessage = 'Password reset successful';
      expect(component.successMessage).toBe('Password reset successful');
    });

    it('should toggle social login', () => {
      component.enableSocialLogin = true;
      expect(component.enableSocialLogin).toBe(true);
    });

    it('should toggle remember me', () => {
      component.enableRememberMe = false;
      expect(component.enableRememberMe).toBe(false);
    });
  });

  describe('UI State', () => {
    it('should manage submitting state', () => {
      expect(component.isSubmitting()).toBe(false);
      component.isSubmitting.set(true);
      expect(component.isSubmitting()).toBe(true);
    });

    it('should manage error message state', () => {
      expect(component.errorMessage()).toBeNull();
      component.errorMessage.set('Error message');
      expect(component.errorMessage()).toBe('Error message');
    });

    it('should manage password visibility state', () => {
      expect(component.showPassword()).toBe(false);
      component.showPassword.set(true);
      expect(component.showPassword()).toBe(true);
    });
  });
});

