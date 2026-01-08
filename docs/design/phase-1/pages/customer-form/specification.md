# Customer Form - Angular Implementation Specification

## Overview

This document provides detailed Angular implementation specifications for the Customer Create/Edit Form page, implementing requirement **REQ-CM-F008**.

**Feature**: Customer Create/Edit Form
**Phase**: Phase 1
**Priority**: P0
**Route**: `/customers/new`, `/customers/:id/edit`

---

## Requirements Reference

### REQ-CM-F008: Customer Create/Edit Form

**Acceptance Criteria**:
- ✓ All required fields validated (First Name, Last Name, Email, Phone)
- ✓ Phone number formatting
- ✓ Duplicate warning
- ✓ Save and continue option

**Form Sections**:
- Contact Information (required fields)
- Personal Details (optional)
- Address (optional)
- Preferences
- Notes and Tags

---

## Component Architecture

### Component Structure

```
customer-form/
├── customer-form.component.ts          # Main form component
├── customer-form.component.html        # Template
├── customer-form.component.scss        # Styles
├── customer-form.component.spec.ts     # Unit tests
└── components/
    ├── contact-info-section/           # Contact Information section
    ├── personal-details-section/       # Personal Details section
    ├── address-section/                # Address section
    ├── preferences-section/            # Preferences section
    └── notes-tags-section/             # Notes & Tags section
```

### Module Dependencies

```typescript
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
```

---

## Data Model

### Customer Interface

```typescript
export interface Customer {
  id?: string;

  // Contact Information (Required)
  firstName: string;
  lastName: string;
  email: string;
  phone: string;

  // Personal Details (Optional)
  dateOfBirth?: Date;
  gender?: 'male' | 'female' | 'other' | 'prefer-not-to-say';
  preferredLanguage?: string;
  companyName?: string;

  // Address (Optional)
  address?: {
    street: string;
    street2?: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
  };

  // Communication Preferences
  contactPreferences: {
    email: boolean;
    sms: boolean;
    phone: boolean;
    bestTimeToContact?: 'morning' | 'afternoon' | 'evening' | 'anytime';
    marketingConsent: boolean;
  };

  // Notes and Tags
  internalNotes?: string;
  tags?: string[];

  // Metadata
  createdAt?: Date;
  updatedAt?: Date;
  createdBy?: string;
  ownerId?: string;
  status?: 'active' | 'inactive' | 'pending';
}
```

### Form Value Interface

```typescript
export interface CustomerFormValue {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  dateOfBirth: string | null;
  gender: string | null;
  preferredLanguage: string;
  companyName: string;
  street: string;
  street2: string;
  city: string;
  state: string;
  zipCode: string;
  contactEmail: boolean;
  contactSMS: boolean;
  contactPhone: boolean;
  bestTimeToContact: string;
  marketingConsent: boolean;
  internalNotes: string;
  tags: string[];
}
```

---

## Component Implementation

### customer-form.component.ts

```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { CustomerService } from '@services/customer.service';
import { NotificationService } from '@services/notification.service';
import { Customer, CustomerFormValue } from '@models/customer.model';

@Component({
  selector: 'app-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.scss']
})
export class CustomerFormComponent implements OnInit, OnDestroy {
  customerForm: FormGroup;
  isEditMode = false;
  customerId: string | null = null;
  isSubmitting = false;
  saveAndContinue = false;
  showDuplicateWarning = false;
  duplicateCustomer: Customer | null = null;

  private destroy$ = new Subject<void>();

  // Dropdown options
  genderOptions = [
    { value: 'male', label: 'Male' },
    { value: 'female', label: 'Female' },
    { value: 'other', label: 'Other' },
    { value: 'prefer-not-to-say', label: 'Prefer not to say' }
  ];

  languageOptions = [
    { value: 'en', label: 'English' },
    { value: 'es', label: 'Spanish' },
    { value: 'fr', label: 'French' },
    { value: 'de', label: 'German' }
  ];

  timeOptions = [
    { value: 'morning', label: 'Morning (8am - 12pm)' },
    { value: 'afternoon', label: 'Afternoon (12pm - 5pm)' },
    { value: 'evening', label: 'Evening (5pm - 8pm)' },
    { value: 'anytime', label: 'Anytime' }
  ];

  stateOptions = [
    { value: 'AL', label: 'Alabama' },
    { value: 'AK', label: 'Alaska' },
    { value: 'AZ', label: 'Arizona' },
    { value: 'CA', label: 'California' },
    { value: 'NY', label: 'New York' },
    { value: 'TX', label: 'Texas' },
    // ... all states
  ];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private customerService: CustomerService,
    private notificationService: NotificationService
  ) {
    this.customerForm = this.createForm();
  }

  ngOnInit(): void {
    // Check if edit mode
    this.customerId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.customerId;

    if (this.isEditMode && this.customerId) {
      this.loadCustomer(this.customerId);
    }

    // Setup duplicate detection
    this.setupDuplicateDetection();

    // Setup phone number formatting
    this.setupPhoneFormatting();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private createForm(): FormGroup {
    return this.fb.group({
      // Contact Information (Required)
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, this.phoneValidator]],

      // Personal Details (Optional)
      dateOfBirth: [null],
      gender: [''],
      preferredLanguage: ['en'],
      companyName: [''],

      // Address (Optional)
      street: [''],
      street2: [''],
      city: [''],
      state: [''],
      zipCode: ['', [Validators.pattern(/^\d{5}$/)]],

      // Communication Preferences
      contactEmail: [true],
      contactSMS: [true],
      contactPhone: [false],
      bestTimeToContact: [''],
      marketingConsent: [false],

      // Notes and Tags
      internalNotes: ['', [Validators.maxLength(5000)]],
      tags: [[]]
    });
  }

  private loadCustomer(id: string): void {
    this.customerService.getCustomer(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (customer) => {
          this.patchFormWithCustomer(customer);
        },
        error: (error) => {
          this.notificationService.error('Failed to load customer');
          this.router.navigate(['/customers']);
        }
      });
  }

  private patchFormWithCustomer(customer: Customer): void {
    this.customerForm.patchValue({
      firstName: customer.firstName,
      lastName: customer.lastName,
      email: customer.email,
      phone: customer.phone,
      dateOfBirth: customer.dateOfBirth,
      gender: customer.gender,
      preferredLanguage: customer.preferredLanguage || 'en',
      companyName: customer.companyName,
      street: customer.address?.street,
      street2: customer.address?.street2,
      city: customer.address?.city,
      state: customer.address?.state,
      zipCode: customer.address?.zipCode,
      contactEmail: customer.contactPreferences.email,
      contactSMS: customer.contactPreferences.sms,
      contactPhone: customer.contactPreferences.phone,
      bestTimeToContact: customer.contactPreferences.bestTimeToContact,
      marketingConsent: customer.contactPreferences.marketingConsent,
      internalNotes: customer.internalNotes,
      tags: customer.tags || []
    });
  }

  private setupDuplicateDetection(): void {
    // Watch email and phone for potential duplicates
    this.customerForm.get('email')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(email => {
        if (email && this.customerForm.get('email')?.valid) {
          this.checkForDuplicates(email, 'email');
        }
      });

    this.customerForm.get('phone')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(phone => {
        if (phone && this.customerForm.get('phone')?.valid) {
          this.checkForDuplicates(phone, 'phone');
        }
      });
  }

  private checkForDuplicates(value: string, field: 'email' | 'phone'): void {
    this.customerService.checkDuplicate(field, value, this.customerId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          if (result.isDuplicate && result.customer) {
            this.showDuplicateWarning = true;
            this.duplicateCustomer = result.customer;
          } else {
            this.showDuplicateWarning = false;
            this.duplicateCustomer = null;
          }
        }
      });
  }

  private setupPhoneFormatting(): void {
    this.customerForm.get('phone')?.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(value => {
        if (value) {
          const formatted = this.formatPhoneNumber(value);
          if (formatted !== value) {
            this.customerForm.get('phone')?.setValue(formatted, { emitEvent: false });
          }
        }
      });
  }

  private formatPhoneNumber(value: string): string {
    // Remove all non-digits
    const digits = value.replace(/\D/g, '');

    // Format as (XXX) XXX-XXXX
    if (digits.length === 0) return '';
    if (digits.length <= 3) return `(${digits}`;
    if (digits.length <= 6) return `(${digits.slice(0, 3)}) ${digits.slice(3)}`;
    return `(${digits.slice(0, 3)}) ${digits.slice(3, 6)}-${digits.slice(6, 10)}`;
  }

  private phoneValidator(control: AbstractControl): { [key: string]: any } | null {
    if (!control.value) return null;

    const digits = control.value.replace(/\D/g, '');
    if (digits.length !== 10) {
      return { invalidPhone: true };
    }

    return null;
  }

  onSubmit(): void {
    if (this.customerForm.invalid) {
      this.markFormGroupTouched(this.customerForm);
      this.notificationService.error('Please fix the errors in the form');
      return;
    }

    this.isSubmitting = true;
    const customer = this.mapFormToCustomer(this.customerForm.value);

    const operation = this.isEditMode && this.customerId
      ? this.customerService.updateCustomer(this.customerId, customer)
      : this.customerService.createCustomer(customer);

    operation
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (savedCustomer) => {
          const message = this.isEditMode
            ? 'Customer updated successfully'
            : 'Customer created successfully';
          this.notificationService.success(message);

          if (this.saveAndContinue && !this.isEditMode) {
            // Reset form for new entry
            this.customerForm.reset({
              preferredLanguage: 'en',
              contactEmail: true,
              contactSMS: true,
              contactPhone: false,
              marketingConsent: false,
              tags: []
            });
            this.showDuplicateWarning = false;
            this.duplicateCustomer = null;
          } else {
            // Navigate to customer detail or list
            this.router.navigate(['/customers', savedCustomer.id]);
          }
        },
        error: (error) => {
          this.notificationService.error('Failed to save customer');
        },
        complete: () => {
          this.isSubmitting = false;
        }
      });
  }

  private mapFormToCustomer(formValue: CustomerFormValue): Customer {
    return {
      firstName: formValue.firstName.trim(),
      lastName: formValue.lastName.trim(),
      email: formValue.email.trim().toLowerCase(),
      phone: formValue.phone,
      dateOfBirth: formValue.dateOfBirth ? new Date(formValue.dateOfBirth) : undefined,
      gender: formValue.gender || undefined,
      preferredLanguage: formValue.preferredLanguage,
      companyName: formValue.companyName?.trim() || undefined,
      address: this.hasAddress(formValue) ? {
        street: formValue.street.trim(),
        street2: formValue.street2?.trim() || undefined,
        city: formValue.city.trim(),
        state: formValue.state,
        zipCode: formValue.zipCode,
        country: 'US'
      } : undefined,
      contactPreferences: {
        email: formValue.contactEmail,
        sms: formValue.contactSMS,
        phone: formValue.contactPhone,
        bestTimeToContact: formValue.bestTimeToContact || undefined,
        marketingConsent: formValue.marketingConsent
      },
      internalNotes: formValue.internalNotes?.trim() || undefined,
      tags: formValue.tags || []
    };
  }

  private hasAddress(formValue: CustomerFormValue): boolean {
    return !!(formValue.street && formValue.city && formValue.state);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  onCancel(): void {
    if (this.customerForm.dirty) {
      if (confirm('You have unsaved changes. Are you sure you want to cancel?')) {
        this.router.navigate(['/customers']);
      }
    } else {
      this.router.navigate(['/customers']);
    }
  }

  onViewDuplicate(): void {
    if (this.duplicateCustomer?.id) {
      this.router.navigate(['/customers', this.duplicateCustomer.id]);
    }
  }

  onDismissDuplicateWarning(): void {
    this.showDuplicateWarning = false;
  }

  // Tag management
  addTag(tag: string): void {
    const currentTags = this.customerForm.get('tags')?.value || [];
    if (tag.trim() && !currentTags.includes(tag.trim())) {
      this.customerForm.patchValue({
        tags: [...currentTags, tag.trim()]
      });
    }
  }

  removeTag(tag: string): void {
    const currentTags = this.customerForm.get('tags')?.value || [];
    this.customerForm.patchValue({
      tags: currentTags.filter((t: string) => t !== tag)
    });
  }

  // Helper methods for template
  isFieldInvalid(fieldName: string): boolean {
    const field = this.customerForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.customerForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return 'This field is required';
    if (field.errors['email']) return 'Please enter a valid email address';
    if (field.errors['minlength']) return `Minimum length is ${field.errors['minlength'].requiredLength}`;
    if (field.errors['maxlength']) return `Maximum length is ${field.errors['maxlength'].requiredLength}`;
    if (field.errors['pattern']) return 'Invalid format';
    if (field.errors['invalidPhone']) return 'Phone number must be 10 digits';

    return 'Invalid value';
  }

  get pageTitle(): string {
    return this.isEditMode ? 'Edit Customer' : 'Add New Customer';
  }

  get pageSubtitle(): string {
    return this.isEditMode
      ? 'Update customer information'
      : 'Complete the form below to add a new customer to your database';
  }
}
```

---

## Service Implementation

### customer.service.ts

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '@models/customer.model';

export interface DuplicateCheckResult {
  isDuplicate: boolean;
  customer?: Customer;
}

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = '/api/customers';

  constructor(private http: HttpClient) {}

  createCustomer(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>(this.apiUrl, customer);
  }

  updateCustomer(id: string, customer: Customer): Observable<Customer> {
    return this.http.put<Customer>(`${this.apiUrl}/${id}`, customer);
  }

  getCustomer(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiUrl}/${id}`);
  }

  checkDuplicate(
    field: 'email' | 'phone',
    value: string,
    excludeId?: string | null
  ): Observable<DuplicateCheckResult> {
    let params = new HttpParams()
      .set('field', field)
      .set('value', value);

    if (excludeId) {
      params = params.set('excludeId', excludeId);
    }

    return this.http.get<DuplicateCheckResult>(
      `${this.apiUrl}/check-duplicate`,
      { params }
    );
  }
}
```

---

## Template Structure

### customer-form.component.html (Key Sections)

```html
<!-- Page Header -->
<div class="page-header">
  <div class="page-header__left">
    <button class="back-btn" (click)="onCancel()">
      <!-- Back icon -->
    </button>
    <div>
      <h1 class="page-header__title">{{ pageTitle }}</h1>
      <p class="page-header__subtitle">{{ pageSubtitle }}</p>
    </div>
  </div>
</div>

<!-- Duplicate Warning Alert -->
<div
  class="alert alert--warning duplicate-warning"
  *ngIf="showDuplicateWarning && duplicateCustomer"
  role="alert">
  <!-- Warning content -->
  <button (click)="onViewDuplicate()">View existing customer</button>
  <button (click)="onDismissDuplicateWarning()">Dismiss</button>
</div>

<!-- Form -->
<form [formGroup]="customerForm" (ngSubmit)="onSubmit()">

  <!-- Contact Information Section -->
  <div class="form-card">
    <app-contact-info-section
      [formGroup]="customerForm"
      [isFieldInvalid]="isFieldInvalid.bind(this)"
      [getFieldError]="getFieldError.bind(this)">
    </app-contact-info-section>
  </div>

  <!-- Personal Details Section -->
  <div class="form-card">
    <app-personal-details-section
      [formGroup]="customerForm"
      [genderOptions]="genderOptions"
      [languageOptions]="languageOptions">
    </app-personal-details-section>
  </div>

  <!-- Address Section -->
  <div class="form-card">
    <app-address-section
      [formGroup]="customerForm"
      [stateOptions]="stateOptions"
      [isFieldInvalid]="isFieldInvalid.bind(this)"
      [getFieldError]="getFieldError.bind(this)">
    </app-address-section>
  </div>

  <!-- Preferences Section -->
  <div class="form-card">
    <app-preferences-section
      [formGroup]="customerForm"
      [timeOptions]="timeOptions">
    </app-preferences-section>
  </div>

  <!-- Notes and Tags Section -->
  <div class="form-card">
    <app-notes-tags-section
      [formGroup]="customerForm"
      [tags]="customerForm.get('tags')?.value || []"
      (tagAdded)="addTag($event)"
      (tagRemoved)="removeTag($event)">
    </app-notes-tags-section>
  </div>

  <!-- Form Actions -->
  <div class="form-actions">
    <div class="form-actions__left">
      <button type="button" class="btn btn--ghost" (click)="onCancel()">
        Cancel
      </button>
    </div>
    <div class="form-actions__right">
      <mat-checkbox
        [(ngModel)]="saveAndContinue"
        [ngModelOptions]="{standalone: true}"
        *ngIf="!isEditMode">
        Save and add another
      </mat-checkbox>
      <button
        type="submit"
        class="btn btn--primary"
        [disabled]="isSubmitting">
        <mat-spinner *ngIf="isSubmitting" diameter="20"></mat-spinner>
        {{ isEditMode ? 'Update Customer' : 'Save Customer' }}
      </button>
    </div>
  </div>
</form>
```

---

## Validation Rules

### Field Validation

| Field | Required | Validation Rules |
|-------|----------|------------------|
| First Name | Yes | Min 2 characters |
| Last Name | Yes | Min 2 characters |
| Email | Yes | Valid email format, lowercase |
| Phone | Yes | 10 digits, auto-formatted as (XXX) XXX-XXXX |
| Date of Birth | No | Valid date, not future |
| ZIP Code | No | 5 digits |
| Internal Notes | No | Max 5000 characters |

### Custom Validators

```typescript
// Phone number validator
phoneValidator(control: AbstractControl): ValidationErrors | null {
  if (!control.value) return null;
  const digits = control.value.replace(/\D/g, '');
  return digits.length === 10 ? null : { invalidPhone: true };
}

// Future date validator (for date of birth)
notFutureDateValidator(control: AbstractControl): ValidationErrors | null {
  if (!control.value) return null;
  const date = new Date(control.value);
  return date > new Date() ? { futureDate: true } : null;
}
```

---

## State Management

### Form States

1. **Pristine**: Form has not been modified
2. **Dirty**: User has modified at least one field
3. **Valid**: All validations pass
4. **Invalid**: One or more validations fail
5. **Submitting**: Form is being submitted
6. **Error**: Submission failed

### Unsaved Changes Guard

```typescript
@Injectable()
export class UnsavedChangesGuard implements CanDeactivate<CustomerFormComponent> {
  canDeactivate(component: CustomerFormComponent): boolean {
    if (component.customerForm.dirty && !component.isSubmitting) {
      return confirm('You have unsaved changes. Are you sure you want to leave?');
    }
    return true;
  }
}
```

---

## Accessibility Features

### WCAG 2.1 AA Compliance

1. **Form Labels**: All inputs have associated labels
2. **Error Messages**: Linked to fields via `aria-describedby`
3. **Required Fields**: Marked with asterisk and `aria-required="true"`
4. **Error States**: `aria-invalid="true"` when field has errors
5. **Focus Management**: Automatic focus on first error on submit
6. **Keyboard Navigation**: Full keyboard support for all interactions

### ARIA Attributes

```html
<input
  type="text"
  id="firstName"
  formControlName="firstName"
  [attr.aria-invalid]="isFieldInvalid('firstName')"
  [attr.aria-describedby]="isFieldInvalid('firstName') ? 'firstName-error' : null"
  aria-required="true">

<div
  id="firstName-error"
  class="form-field__error"
  *ngIf="isFieldInvalid('firstName')"
  role="alert">
  {{ getFieldError('firstName') }}
</div>
```

---

## Performance Optimizations

1. **Debounced Duplicate Detection**: 500ms debounce on email/phone changes
2. **OnPush Change Detection**: Use for sub-components
3. **Virtual Scrolling**: For large dropdown lists (states)
4. **Lazy Loading**: Form sections loaded on demand
5. **Memoization**: Cache formatted phone numbers

---

## Testing Strategy

### Unit Tests

```typescript
describe('CustomerFormComponent', () => {
  it('should create form with required fields', () => {
    expect(component.customerForm.get('firstName')).toBeDefined();
    expect(component.customerForm.get('email')).toBeDefined();
  });

  it('should validate required fields', () => {
    component.customerForm.patchValue({
      firstName: '',
      lastName: '',
      email: '',
      phone: ''
    });
    expect(component.customerForm.invalid).toBe(true);
  });

  it('should format phone number correctly', () => {
    component.customerForm.patchValue({ phone: '5551234567' });
    expect(component.customerForm.get('phone')?.value).toBe('(555) 123-4567');
  });

  it('should detect duplicate email', fakeAsync(() => {
    const checkDuplicateSpy = spyOn(customerService, 'checkDuplicate')
      .and.returnValue(of({ isDuplicate: true, customer: mockCustomer }));

    component.customerForm.patchValue({ email: 'test@example.com' });
    tick(500);

    expect(checkDuplicateSpy).toHaveBeenCalled();
    expect(component.showDuplicateWarning).toBe(true);
  }));

  it('should submit form with valid data', () => {
    const createCustomerSpy = spyOn(customerService, 'createCustomer')
      .and.returnValue(of(mockCustomer));

    component.customerForm.patchValue(validFormData);
    component.onSubmit();

    expect(createCustomerSpy).toHaveBeenCalled();
  });

  it('should handle save and continue option', () => {
    component.saveAndContinue = true;
    component.customerForm.patchValue(validFormData);
    component.onSubmit();

    // Form should reset after successful save
    expect(component.customerForm.pristine).toBe(true);
  });
});
```

### E2E Tests

```typescript
describe('Customer Form E2E', () => {
  it('should create new customer', () => {
    cy.visit('/customers/new');
    cy.get('#firstName').type('John');
    cy.get('#lastName').type('Doe');
    cy.get('#email').type('john.doe@example.com');
    cy.get('#phone').type('5551234567');
    cy.get('button[type="submit"]').click();
    cy.url().should('include', '/customers/');
  });

  it('should show duplicate warning', () => {
    cy.visit('/customers/new');
    cy.get('#email').type('existing@example.com');
    cy.get('#email').blur();
    cy.get('.duplicate-warning').should('be.visible');
  });

  it('should format phone number on input', () => {
    cy.visit('/customers/new');
    cy.get('#phone').type('5551234567');
    cy.get('#phone').should('have.value', '(555) 123-4567');
  });
});
```

---

## Error Handling

### Error Scenarios

1. **Network Error**: Show retry option
2. **Validation Error**: Highlight fields, show error messages
3. **Duplicate Customer**: Show warning with link to existing customer
4. **Server Error**: Show user-friendly message
5. **Timeout**: Show timeout message with retry option

### Error Messages

```typescript
const ERROR_MESSAGES = {
  NETWORK_ERROR: 'Unable to connect. Please check your internet connection.',
  SAVE_FAILED: 'Failed to save customer. Please try again.',
  LOAD_FAILED: 'Failed to load customer information.',
  DUPLICATE_ERROR: 'A customer with this information already exists.',
  VALIDATION_ERROR: 'Please fix the errors in the form before submitting.'
};
```

---

## Responsive Design

### Breakpoints

- **Mobile**: < 576px (Single column, stacked inputs)
- **Tablet**: 576px - 991px (2 columns where applicable)
- **Desktop**: ≥ 992px (Full layout with 2-3 columns)

### Mobile Optimizations

1. Single column layout for all sections
2. Larger touch targets (48px minimum)
3. Simplified navigation
4. Auto-focus on field errors
5. Native date/time pickers

---

## API Endpoints

### Customer API

```typescript
// Create customer
POST /api/customers
Body: Customer
Response: Customer

// Update customer
PUT /api/customers/:id
Body: Customer
Response: Customer

// Get customer
GET /api/customers/:id
Response: Customer

// Check duplicate
GET /api/customers/check-duplicate?field=email&value=test@example.com&excludeId=123
Response: DuplicateCheckResult
```

---

## Security Considerations

1. **Input Sanitization**: All text inputs sanitized before submission
2. **XSS Protection**: Angular's built-in sanitization
3. **CSRF Protection**: Token required for POST/PUT requests
4. **Data Privacy**: Sensitive data encrypted in transit and at rest
5. **Permission Checks**: Verify user has permission to create/edit customers

---

## Future Enhancements (Phase 2+)

1. **Address Autocomplete**: Google Places API integration
2. **Photo Upload**: Customer profile photo
3. **Vehicle Management**: Add vehicles directly from form
4. **Import from vCard**: Import contact from vCard file
5. **Auto-save**: Save draft automatically every 30 seconds
6. **Form Versioning**: Track form changes and revisions
