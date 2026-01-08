import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomerForm, Customer } from './customer-form';
import { vi } from 'vitest';

describe('CustomerForm', () => {
  let component: CustomerForm;
  let fixture: ComponentFixture<CustomerForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerForm, ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Form Initialization', () => {
    it('should initialize form with empty values in create mode', () => {
      expect(component.customerForm.value.firstName).toBe('');
      expect(component.customerForm.value.lastName).toBe('');
      expect(component.customerForm.value.email).toBe('');
      expect(component.customerForm.value.phone).toBe('');
    });

    it('should initialize with default mode as create', () => {
      expect(component.mode).toBe('create');
    });

    it('should populate form when customer is provided', () => {
      const customer: Customer = {
        firstName: 'John',
        lastName: 'Doe',
        email: 'john@example.com',
        phone: '(555) 123-4567',
        address: {
          street: '123 Main St',
          city: 'Springfield',
          state: 'IL',
          zipCode: '62701',
          country: 'US'
        },
        preferences: {
          contactMethod: 'email'
        },
        tags: ['VIP', 'Fleet']
      };

      component.customer = customer;
      component.ngOnInit();

      expect(component.customerForm.value.firstName).toBe('John');
      expect(component.customerForm.value.lastName).toBe('Doe');
      expect(component.customerForm.value.email).toBe('john@example.com');
    });
  });

  describe('Form Validation', () => {
    it('should require first name', () => {
      const control = component.customerForm.get('firstName');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate first name minimum length', () => {
      const control = component.customerForm.get('firstName');
      control?.setValue('J');
      expect(control?.hasError('minlength')).toBeTruthy();
    });

    it('should validate first name pattern', () => {
      const control = component.customerForm.get('firstName');
      control?.setValue('John123');
      expect(control?.hasError('pattern')).toBeTruthy();
    });

    it('should accept valid first name', () => {
      const control = component.customerForm.get('firstName');
      control?.setValue('John');
      expect(control?.valid).toBeTruthy();
    });

    it('should validate email format', () => {
      const control = component.customerForm.get('email');
      control?.setValue('invalid-email');
      expect(control?.hasError('email')).toBeTruthy();

      control?.setValue('valid@example.com');
      expect(control?.valid).toBeTruthy();
    });

    it('should validate phone format', () => {
      const control = component.customerForm.get('phone');
      control?.setValue('1234567890');
      expect(control?.hasError('pattern')).toBeTruthy();

      control?.setValue('(555) 123-4567');
      expect(control?.valid).toBeTruthy();
    });

    it('should validate ZIP code format', () => {
      const control = component.customerForm.get('address.zipCode');
      control?.setValue('123');
      expect(control?.hasError('pattern')).toBeTruthy();

      control?.setValue('12345');
      expect(control?.valid).toBeTruthy();
    });

    it('should require state', () => {
      const control = component.customerForm.get('address.state');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate notes max length', () => {
      const control = component.customerForm.get('notes');
      control?.setValue('a'.repeat(1001));
      expect(control?.hasError('maxlength')).toBeTruthy();
    });
  });

  describe('Phone Number Formatting', () => {
    it('should format phone number as user types', () => {
      const event = {
        target: { value: '5551234567' }
      } as unknown as Event;

      component.formatPhoneNumber(event);
      expect(component.customerForm.value.phone).toBe('(555) 123-4567');
    });

    it('should handle partial phone numbers', () => {
      const event = {
        target: { value: '555' }
      } as unknown as Event;

      component.formatPhoneNumber(event);
      expect(component.customerForm.value.phone).toBe('(555');
    });
  });

  describe('Tags Management', () => {
    it('should add a valid tag', () => {
      component.newTag.set('VIP');
      component.addTag();
      expect(component.tagsArray.length).toBe(1);
      expect(component.tagsArray.at(0).value).toBe('VIP');
    });

    it('should clear new tag input after adding', () => {
      component.newTag.set('VIP');
      component.addTag();
      expect(component.newTag()).toBe('');
    });

    it('should not add duplicate tags', () => {
      component.newTag.set('VIP');
      component.addTag();
      component.newTag.set('VIP');
      component.addTag();
      expect(component.tagsArray.length).toBe(1);
    });

    it('should not add duplicate tags (case insensitive)', () => {
      component.newTag.set('VIP');
      component.addTag();
      component.newTag.set('vip');
      component.addTag();
      expect(component.tagsArray.length).toBe(1);
    });

    it('should not add tags shorter than 2 characters', () => {
      component.newTag.set('V');
      component.addTag();
      expect(component.tagsArray.length).toBe(0);
    });

    it('should not add tags longer than 30 characters', () => {
      component.newTag.set('a'.repeat(31));
      component.addTag();
      expect(component.tagsArray.length).toBe(0);
    });

    it('should not add more than 20 tags', () => {
      for (let i = 0; i < 25; i++) {
        component.newTag.set(`Tag${i}`);
        component.addTag();
      }
      expect(component.tagsArray.length).toBe(20);
    });

    it('should remove tag at specified index', () => {
      component.newTag.set('Tag1');
      component.addTag();
      component.newTag.set('Tag2');
      component.addTag();

      component.removeTag(0);
      expect(component.tagsArray.length).toBe(1);
      expect(component.tagsArray.at(0).value).toBe('Tag2');
    });

    it('should add tag on Enter key', () => {
      component.newTag.set('VIP');
      const event = new KeyboardEvent('keydown', { key: 'Enter' });
      vi.spyOn(event, 'preventDefault');

      component.onTagInputKeydown(event);

      expect(event.preventDefault).toHaveBeenCalled();
      expect(component.tagsArray.length).toBe(1);
    });

    it('should add tag on comma key', () => {
      component.newTag.set('VIP');
      const event = new KeyboardEvent('keydown', { key: ',' });
      vi.spyOn(event, 'preventDefault');

      component.onTagInputKeydown(event);

      expect(event.preventDefault).toHaveBeenCalled();
      expect(component.tagsArray.length).toBe(1);
    });

    it('should remove last tag on Backspace when input is empty', () => {
      component.newTag.set('Tag1');
      component.addTag();
      component.newTag.set('Tag2');
      component.addTag();

      component.newTag.set('');
      const event = new KeyboardEvent('keydown', { key: 'Backspace' });
      component.onTagInputKeydown(event);

      expect(component.tagsArray.length).toBe(1);
    });
  });

  describe('Form Submission', () => {
    it('should not submit invalid form', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.onSubmit();
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('should mark all fields as touched on invalid submit', () => {
      component.onSubmit();
      expect(component.customerForm.get('firstName')?.touched).toBeTruthy();
      expect(component.customerForm.get('lastName')?.touched).toBeTruthy();
      expect(component.customerForm.get('email')?.touched).toBeTruthy();
    });

    it('should set isSubmitting to true during submission', () => {
      fillValidForm(component);
      component.onSubmit();
      expect(component.isSubmitting()).toBe(true);
    });

    it('should emit save with customer data on valid submit', () => {
      return new Promise<void>((resolve) => {
        fillValidForm(component);

        component.save.subscribe((customer) => {
          expect(customer.firstName).toBe('John');
          expect(customer.lastName).toBe('Doe');
          expect(customer.email).toBe('john@example.com');
          resolve();
        });

        component.onSubmit();
      });
    });
  });

  describe('Cancel Action', () => {
    it('should emit cancel event', () => {
      const cancelSpy = vi.spyOn(component.cancel, 'emit');
      component.onCancel();
      expect(cancelSpy).toHaveBeenCalled();
    });
  });

  describe('Error Messages', () => {
    it('should return error message for required field', () => {
      const control = component.customerForm.get('firstName');
      control?.markAsTouched();
      expect(component.getError('firstName')).toBe('First Name is required');
    });

    it('should return error message for email format', () => {
      const control = component.customerForm.get('email');
      control?.setValue('invalid');
      control?.markAsTouched();
      expect(component.getError('email')).toBe('Please enter a valid email address');
    });

    it('should return error message for phone format', () => {
      const control = component.customerForm.get('phone');
      control?.setValue('123');
      control?.markAsTouched();
      expect(component.getError('phone')).toBe('Format: (555) 123-4567');
    });

    it('should return null for valid untouched field', () => {
      const control = component.customerForm.get('firstName');
      control?.setValue('John');
      expect(component.getError('firstName')).toBeNull();
    });
  });

  describe('Component Inputs', () => {
    it('should accept customer input', () => {
      const customer: Customer = {
        firstName: 'Jane',
        lastName: 'Smith',
        email: 'jane@example.com',
        phone: '(555) 987-6543',
        address: {
          street: '456 Oak Ave',
          city: 'Chicago',
          state: 'IL',
          zipCode: '60601',
          country: 'US'
        },
        preferences: {
          contactMethod: 'phone'
        }
      };

      component.customer = customer;
      expect(component.customer).toEqual(customer);
    });

    it('should accept mode input', () => {
      component.mode = 'edit';
      expect(component.mode).toBe('edit');
    });

    it('should accept autoSave input', () => {
      component.autoSave = true;
      expect(component.autoSave).toBe(true);
    });

    it('should accept duplicateWarning input', () => {
      component.duplicateWarning = { email: 'duplicate@example.com' };
      expect(component.duplicateWarning?.email).toBe('duplicate@example.com');
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
      component.errorMessage.set('Error occurred');
      expect(component.errorMessage()).toBe('Error occurred');
    });

    it('should manage autoSave status', () => {
      expect(component.autoSaveStatus()).toBe('idle');
      component.autoSaveStatus.set('saving');
      expect(component.autoSaveStatus()).toBe('saving');
    });
  });

  describe('States List', () => {
    it('should have all 50 US states', () => {
      expect(component.states.length).toBe(50);
    });

    it('should have California in states list', () => {
      const california = component.states.find(s => s.code === 'CA');
      expect(california?.name).toBe('California');
    });
  });
});

function fillValidForm(component: CustomerForm) {
  component.customerForm.patchValue({
    firstName: 'John',
    lastName: 'Doe',
    email: 'john@example.com',
    phone: '(555) 123-4567',
    address: {
      street: '123 Main St',
      city: 'Springfield',
      state: 'IL',
      zipCode: '62701',
      country: 'US'
    },
    preferences: {
      contactMethod: 'email'
    }
  });
}
