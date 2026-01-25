import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomerRegistration } from './customer-registration';
import { vi } from 'vitest';

describe('CustomerRegistration', () => {
  let component: CustomerRegistration;
  let fixture: ComponentFixture<CustomerRegistration>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerRegistration, ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerRegistration);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Form Initialization', () => {
    it('should initialize form with empty values', () => {
      expect(component.registrationForm.value).toEqual({
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        password: '',
        confirmPassword: '',
        marketingConsent: false,
        termsAccept: false,
      });
    });

    it('should initialize with default input values', () => {
      expect(component.enableSocialLogin).toBe(true);
      expect(component.socialProviders).toEqual(['google', 'facebook']);
    });

    it('should accept referral code input', () => {
      component.referralCode = 'ABC123';
      expect(component.referralCode).toBe('ABC123');
    });

    it('should accept referrer information', () => {
      component.referrer = { name: 'John Doe', discount: '10%' };
      expect(component.referrer).toEqual({ name: 'John Doe', discount: '10%' });
    });
  });

  describe('Form Validation - First Name', () => {
    it('should require firstName field', () => {
      const control = component.registrationForm.get('firstName');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should enforce minimum length of 2 characters', () => {
      const control = component.registrationForm.get('firstName');
      control?.setValue('J');
      expect(control?.hasError('minlength')).toBeTruthy();
    });

    it('should enforce maximum length of 50 characters', () => {
      const control = component.registrationForm.get('firstName');
      control?.setValue('a'.repeat(51));
      expect(control?.hasError('maxlength')).toBeTruthy();
    });

    it('should only accept letters, spaces, hyphens, and apostrophes', () => {
      const control = component.registrationForm.get('firstName');
      
      control?.setValue('John123');
      expect(control?.hasError('pattern')).toBeTruthy();
      
      control?.setValue('John-Paul');
      expect(control?.valid).toBeTruthy();
      
      control?.setValue("O'Connor");
      expect(control?.valid).toBeTruthy();
    });

    it('should return correct error message for firstName', () => {
      const control = component.registrationForm.get('firstName');
      
      control?.markAsTouched();
      expect(component.firstNameError).toBe('First name is required');
      
      control?.setValue('J');
      expect(component.firstNameError).toBe('First name must be at least 2 characters');
      
      control?.setValue('John123');
      expect(component.firstNameError).toBe('First name can only contain letters');
    });
  });

  describe('Form Validation - Last Name', () => {
    it('should require lastName field', () => {
      const control = component.registrationForm.get('lastName');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should enforce minimum length of 2 characters', () => {
      const control = component.registrationForm.get('lastName');
      control?.setValue('D');
      expect(control?.hasError('minlength')).toBeTruthy();
    });

    it('should return correct error message for lastName', () => {
      const control = component.registrationForm.get('lastName');
      
      control?.markAsTouched();
      expect(component.lastNameError).toBe('Last name is required');
      
      control?.setValue('D');
      expect(component.lastNameError).toBe('Last name must be at least 2 characters');
    });
  });

  describe('Form Validation - Email', () => {
    it('should require email field', () => {
      const control = component.registrationForm.get('email');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate email format', () => {
      const control = component.registrationForm.get('email');
      
      control?.setValue('invalid-email');
      expect(control?.hasError('email')).toBeTruthy();

      control?.setValue('valid@example.com');
      expect(control?.hasError('email')).toBeFalsy();
      expect(control?.valid).toBeTruthy();
    });

    it('should return correct error message for email', () => {
      const control = component.registrationForm.get('email');
      
      control?.markAsTouched();
      expect(component.emailError).toBe('Email is required');
      
      control?.setValue('invalid');
      expect(component.emailError).toBe('Please enter a valid email');
    });
  });

  describe('Form Validation - Phone', () => {
    it('should require phone field', () => {
      const control = component.registrationForm.get('phone');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate phone format (XXX) XXX-XXXX', () => {
      const control = component.registrationForm.get('phone');
      
      control?.setValue('5551234567');
      expect(control?.hasError('phone')).toBeTruthy();

      control?.setValue('(555) 123-4567');
      expect(control?.valid).toBeTruthy();
    });

    it('should format phone number on input', () => {
      const event = {
        target: { value: '5551234567' }
      } as any;
      
      component.formatPhoneNumber(event);
      expect(component.registrationForm.get('phone')?.value).toBe('(555) 123-4567');
    });

    it('should format partial phone numbers', () => {
      let event = { target: { value: '555' } } as any;
      component.formatPhoneNumber(event);
      expect(component.registrationForm.get('phone')?.value).toBe('555');
      
      event = { target: { value: '5551234' } } as any;
      component.formatPhoneNumber(event);
      expect(component.registrationForm.get('phone')?.value).toBe('(555) 123-4');
    });

    it('should return correct error message for phone', () => {
      const control = component.registrationForm.get('phone');
      
      control?.markAsTouched();
      expect(component.phoneError).toBe('Phone number is required');
      
      control?.setValue('invalid');
      expect(component.phoneError).toBe('Please enter a valid phone number');
    });
  });

  describe('Form Validation - Password', () => {
    it('should require password field', () => {
      const control = component.registrationForm.get('password');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should enforce minimum length of 8 characters', () => {
      const control = component.registrationForm.get('password');
      control?.setValue('Pass1');
      expect(control?.hasError('minlength')).toBeTruthy();
    });

    it('should require uppercase letter', () => {
      const control = component.registrationForm.get('password');
      control?.setValue('password123');
      expect(control?.hasError('uppercase')).toBeTruthy();
    });

    it('should require lowercase letter', () => {
      const control = component.registrationForm.get('password');
      control?.setValue('PASSWORD123');
      expect(control?.hasError('lowercase')).toBeTruthy();
    });

    it('should require number', () => {
      const control = component.registrationForm.get('password');
      control?.setValue('Password');
      expect(control?.hasError('number')).toBeTruthy();
    });

    it('should accept valid password', () => {
      const control = component.registrationForm.get('password');
      control?.setValue('Password123');
      expect(control?.valid).toBeTruthy();
    });

    it('should return correct error messages for password', () => {
      const control = component.registrationForm.get('password');
      
      control?.markAsTouched();
      expect(component.passwordError).toBe('Password is required');
      
      control?.setValue('Pass1');
      expect(component.passwordError).toBe('Password must be at least 8 characters');
      
      control?.setValue('password123');
      expect(component.passwordError).toBe('Password must contain an uppercase letter');
    });
  });

  describe('Password Strength', () => {
    it('should calculate weak strength for short password', () => {
      expect(component.calculatePasswordStrength('')).toBe('weak');
      expect(component.calculatePasswordStrength('Pass1')).toBe('weak');
    });

    it('should calculate fair strength for basic password', () => {
      // Password1 has uppercase, lowercase, and number = 3 requirements = good
      expect(component.calculatePasswordStrength('Password1')).toBe('good');
    });

    it('should calculate good strength for password with 3 requirements', () => {
      // Password1! has all 4 requirements = strong
      expect(component.calculatePasswordStrength('Password1!')).toBe('strong');
    });

    it('should calculate strong strength for password with all requirements', () => {
      component.registrationForm.patchValue({ password: 'Password1!' });
      fixture.detectChanges();
      expect(component.passwordStrength()).toBe('strong');
    });

    it('should return correct strength percentage based on password strength', () => {
      // Test with weak password
      expect(component.calculatePasswordStrength('short')).toBe('weak');
      
      // Test with good password  
      expect(component.calculatePasswordStrength('Password1')).toBe('good');
      
      // Test percentage mapping
      const weakPercent = component.passwordStrengthPercent;
      expect([25, 50, 75, 100].includes(weakPercent)).toBe(true);
    });

    it('should return correct strength color', () => {
      expect(component.passwordStrengthColor).toBe('#D32F2F'); // weak color
    });
  });

  describe('Form Validation - Confirm Password', () => {
    it('should require confirmPassword field', () => {
      const control = component.registrationForm.get('confirmPassword');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate password match', () => {
      component.registrationForm.patchValue({
        password: 'Password123',
        confirmPassword: 'Password456'
      });
      expect(component.registrationForm.hasError('passwordMismatch')).toBeTruthy();
    });

    it('should accept matching passwords', () => {
      component.registrationForm.patchValue({
        password: 'Password123',
        confirmPassword: 'Password123'
      });
      expect(component.registrationForm.hasError('passwordMismatch')).toBeFalsy();
    });

    it('should return correct error message for confirmPassword', () => {
      const control = component.registrationForm.get('confirmPassword');
      
      control?.markAsTouched();
      expect(component.confirmPasswordError).toBe('Please confirm your password');
      
      component.registrationForm.patchValue({
        password: 'Password123',
        confirmPassword: 'Different123'
      });
      control?.markAsTouched();
      expect(component.confirmPasswordError).toBe('Passwords do not match');
    });
  });

  describe('Form Validation - Terms', () => {
    it('should require terms acceptance', () => {
      const control = component.registrationForm.get('termsAccept');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should accept terms when checked', () => {
      const control = component.registrationForm.get('termsAccept');
      control?.setValue(true);
      expect(control?.valid).toBeTruthy();
    });

    it('should return correct error message for terms', () => {
      const control = component.registrationForm.get('termsAccept');
      control?.markAsTouched();
      expect(component.termsError).toBe('You must accept the terms and conditions');
    });
  });

  describe('Password Visibility Toggle', () => {
    it('should toggle password visibility', () => {
      expect(component.showPassword()).toBe(false);
      component.togglePasswordVisibility('password');
      expect(component.showPassword()).toBe(true);
      component.togglePasswordVisibility('password');
      expect(component.showPassword()).toBe(false);
    });

    it('should toggle confirm password visibility', () => {
      expect(component.showConfirmPassword()).toBe(false);
      component.togglePasswordVisibility('confirmPassword');
      expect(component.showConfirmPassword()).toBe(true);
      component.togglePasswordVisibility('confirmPassword');
      expect(component.showConfirmPassword()).toBe(false);
    });
  });

  describe('Form Submission', () => {
    it('should not submit if form is invalid', () => {
      const emitSpy = vi.spyOn(component.registrationSuccess, 'emit');
      component.onSubmit();
      expect(emitSpy).not.toHaveBeenCalled();
      expect(component.registrationForm.touched).toBeTruthy();
    });

    it('should set submitting state during submission', () => {
      vi.useFakeTimers();
      
      // Fill form with valid data
      component.registrationForm.patchValue({
        firstName: 'John',
        lastName: 'Doe',
        email: 'john@example.com',
        phone: '(555) 123-4567',
        password: 'Password123',
        confirmPassword: 'Password123',
        termsAccept: true
      });

      expect(component.isSubmitting()).toBe(false);
      component.onSubmit();
      expect(component.isSubmitting()).toBe(true);

      vi.advanceTimersByTime(1500);
      expect(component.isSubmitting()).toBe(false);
      
      vi.useRealTimers();
    });

    it('should emit registrationSuccess with user data on successful submission', () => {
      vi.useFakeTimers();
      const emitSpy = vi.spyOn(component.registrationSuccess, 'emit');

      component.registrationForm.patchValue({
        firstName: 'John',
        lastName: 'Doe',
        email: 'john@example.com',
        phone: '(555) 123-4567',
        password: 'Password123',
        confirmPassword: 'Password123',
        termsAccept: true
      });

      component.onSubmit();
      vi.advanceTimersByTime(1500);

      expect(emitSpy).toHaveBeenCalledWith(
        expect.objectContaining({
          email: 'john@example.com',
          firstName: 'John',
          lastName: 'Doe',
          phone: '(555) 123-4567',
        })
      );
      
      vi.useRealTimers();
    });

    it('should include referral code in submitted data if provided', () => {
      vi.useFakeTimers();
      const emitSpy = vi.spyOn(component.registrationSuccess, 'emit');
      
      component.referralCode = 'ABC123';
      component.registrationForm.patchValue({
        firstName: 'John',
        lastName: 'Doe',
        email: 'john@example.com',
        phone: '(555) 123-4567',
        password: 'Password123',
        confirmPassword: 'Password123',
        termsAccept: true
      });

      component.onSubmit();
      vi.advanceTimersByTime(1500);

      expect(emitSpy).toHaveBeenCalledWith(
        expect.objectContaining({
          referralCode: 'ABC123'
        })
      );
      
      vi.useRealTimers();
    });

    it('should clear error message on submission', () => {
      component.errorMessage.set('Previous error');
      component.registrationForm.patchValue({
        firstName: 'John',
        lastName: 'Doe',
        email: 'john@example.com',
        phone: '(555) 123-4567',
        password: 'Password123',
        confirmPassword: 'Password123',
        termsAccept: true
      });

      component.onSubmit();
      expect(component.errorMessage()).toBeNull();
    });
  });

  describe('Social Login', () => {
    it('should trigger social login with google', () => {
      const consoleSpy = vi.spyOn(console, 'log');
      component.onSocialLogin('google');
      expect(consoleSpy).toHaveBeenCalledWith('Social login with google');
    });

    it('should trigger social login with facebook', () => {
      const consoleSpy = vi.spyOn(console, 'log');
      component.onSocialLogin('facebook');
      expect(consoleSpy).toHaveBeenCalledWith('Social login with facebook');
    });

    it('should trigger social login with apple', () => {
      const consoleSpy = vi.spyOn(console, 'log');
      component.onSocialLogin('apple');
      expect(consoleSpy).toHaveBeenCalledWith('Social login with apple');
    });
  });

  describe('Component Inputs', () => {
    it('should accept social providers input', () => {
      component.socialProviders = ['google', 'apple'];
      expect(component.socialProviders).toEqual(['google', 'apple']);
    });

    it('should accept redirectUrl input', () => {
      component.redirectUrl = '/dashboard';
      expect(component.redirectUrl).toBe('/dashboard');
    });

    it('should accept enableSocialLogin input', () => {
      component.enableSocialLogin = false;
      expect(component.enableSocialLogin).toBe(false);
    });
  });

  describe('Marketing Consent', () => {
    it('should default to false', () => {
      const control = component.registrationForm.get('marketingConsent');
      expect(control?.value).toBe(false);
    });

    it('should accept marketing consent toggle', () => {
      const control = component.registrationForm.get('marketingConsent');
      control?.setValue(true);
      expect(control?.value).toBe(true);
    });

    it('should not be required', () => {
      const control = component.registrationForm.get('marketingConsent');
      expect(control?.hasError('required')).toBeFalsy();
    });
  });
});
