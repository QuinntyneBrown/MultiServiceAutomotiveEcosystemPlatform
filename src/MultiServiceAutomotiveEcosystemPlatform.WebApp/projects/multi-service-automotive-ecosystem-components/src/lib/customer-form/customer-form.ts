import { Component, Input, Output, EventEmitter, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';

export interface Customer {
  id?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: {
    street: string;
    street2?: string;
    city: string;
    province: string;
    postalCode: string;
    country: string;
  };
  preferences: {
    contactMethod: 'email' | 'phone' | 'sms';
    preferredTime?: 'morning' | 'afternoon' | 'evening' | null;
  };
  notes?: string;
  tags?: string[];
  createdAt?: string;
  updatedAt?: string;
}

export interface FormError {
  field?: string;
  code: string;
  message: string;
}

export interface DuplicateWarning {
  email?: string;
  phone?: string;
  customerId?: string;
}

/**
 * Customer Create/Edit Form Component (REQ-CM-F008)
 *
 * A comprehensive form component for creating and editing customer records.
 * Supports all customer fields including contact information, address,
 * preferences, and internal notes with robust validation.
 *
 * @example
 * ```html
 * <ms-customer-form
 *   [mode]="'create'"
 *   (save)="handleSave($event)"
 *   (cancel)="handleCancel()">
 * </ms-customer-form>
 * ```
 */
@Component({
  selector: 'ms-customer-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './customer-form.html',
  styleUrl: './customer-form.scss',
})
export class CustomerForm implements OnInit {
  /** Customer data for editing (undefined for create) */
  @Input() customer?: Customer;

  /** Form mode */
  @Input() mode: 'create' | 'edit' = 'create';

  /** Show duplicate warning */
  @Input() duplicateWarning?: DuplicateWarning;

  /** Enable auto-save (for edit mode) */
  @Input() autoSave = false;

  /** Initial form values */
  @Input() initialValues?: Partial<Customer>;

  @Output() save = new EventEmitter<Customer>();
  @Output() cancel = new EventEmitter<void>();
  @Output() formError = new EventEmitter<FormError>();

  customerForm!: FormGroup;
  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);
  autoSaveStatus = signal<'idle' | 'saving' | 'saved' | 'error'>('idle');
  lastSaved = signal<Date | null>(null);
  newTag = signal('');

  readonly provinces = [
    { code: 'AB', name: 'Alberta' },
    { code: 'BC', name: 'British Columbia' },
    { code: 'MB', name: 'Manitoba' },
    { code: 'NB', name: 'New Brunswick' },
    { code: 'NL', name: 'Newfoundland and Labrador' },
    { code: 'NS', name: 'Nova Scotia' },
    { code: 'NT', name: 'Northwest Territories' },
    { code: 'NU', name: 'Nunavut' },
    { code: 'ON', name: 'Ontario' },
    { code: 'PE', name: 'Prince Edward Island' },
    { code: 'QC', name: 'Quebec' },
    { code: 'SK', name: 'Saskatchewan' },
    { code: 'YT', name: 'Yukon' }
  ];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.initializeForm();

    if (this.customer) {
      this.populateForm(this.customer);
    } else if (this.initialValues) {
      this.populateForm(this.initialValues as Customer);
    }
  }

  private initializeForm() {
    this.customerForm = this.fb.group({
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
      email: ['', [
        Validators.required,
        Validators.email,
        Validators.pattern(/^[^\s@]+@[^\s@]+\.[^\s@]+$/)
      ]],
      phone: ['', [
        Validators.required,
        Validators.pattern(/^\(\d{3}\) \d{3}-\d{4}$/)
      ]],
      address: this.fb.group({
        street: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
        street2: ['', [Validators.maxLength(100)]],
        city: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
        province: ['', [Validators.required]],
        postalCode: ['', [Validators.required, Validators.pattern(/^[ABCEGHJ-NPRSTVXY]\d[ABCEGHJ-NPRSTV-Z][ -]?\d[ABCEGHJ-NPRSTV-Z]\d$/i)]],
        country: ['CA']
      }),
      preferences: this.fb.group({
        contactMethod: ['email'],
        preferredTime: [null]
      }),
      notes: ['', [Validators.maxLength(1000)]],
      tags: this.fb.array([])
    });
  }

  private populateForm(data: Customer) {
    this.customerForm.patchValue({
      firstName: data.firstName,
      lastName: data.lastName,
      email: data.email,
      phone: data.phone,
      address: data.address,
      preferences: data.preferences,
      notes: data.notes
    });

    if (data.tags) {
      const tagsArray = this.customerForm.get('tags') as FormArray;
      tagsArray.clear();
      data.tags.forEach(tag => tagsArray.push(this.fb.control(tag)));
    }
  }

  formatPhoneNumber(event: Event) {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');

    if (value.length >= 10) {
      value = value.substring(0, 10);
    }

    if (value.length >= 6) {
      input.value = `(${value.substring(0, 3)}) ${value.substring(3, 6)}-${value.substring(6)}`;
    } else if (value.length >= 3) {
      input.value = `(${value.substring(0, 3)}) ${value.substring(3)}`;
    } else if (value.length > 0) {
      input.value = `(${value}`;
    }

    this.customerForm.patchValue({ phone: input.value });
  }

  get tagsArray(): FormArray {
    return this.customerForm.get('tags') as FormArray;
  }

  addTag() {
    const tag = this.newTag().trim();
    if (tag && tag.length >= 2 && tag.length <= 30 && this.tagsArray.length < 20) {
      const exists = this.tagsArray.controls.some(ctrl => ctrl.value.toLowerCase() === tag.toLowerCase());
      if (!exists) {
        this.tagsArray.push(this.fb.control(tag));
        this.newTag.set('');
      }
    }
  }

  removeTag(index: number) {
    this.tagsArray.removeAt(index);
  }

  onTagInputKeydown(event: KeyboardEvent) {
    if (event.key === 'Enter' || event.key === ',') {
      event.preventDefault();
      this.addTag();
    } else if (event.key === 'Backspace' && this.newTag() === '' && this.tagsArray.length > 0) {
      this.removeTag(this.tagsArray.length - 1);
    }
  }

  onSubmit() {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const formValue = this.customerForm.value;
    const customer: Customer = {
      ...formValue,
      tags: this.tagsArray.value,
      id: this.customer?.id
    };

    // Simulate API call
    setTimeout(() => {
      this.isSubmitting.set(false);
      this.save.emit(customer);
    }, 1000);
  }

  onCancel() {
    this.cancel.emit();
  }

  getError(controlName: string): string | null {
    const control = this.customerForm.get(controlName);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return `${this.formatFieldName(controlName)} is required`;
    if (control.hasError('minlength')) return `${this.formatFieldName(controlName)} is too short`;
    if (control.hasError('maxlength')) return `${this.formatFieldName(controlName)} is too long`;
    if (control.hasError('email')) return 'Please enter a valid email address';
    if (control.hasError('pattern')) {
      if (controlName === 'phone') return 'Format: (555) 123-4567';
      if (controlName === 'address.postalCode') return 'Postal code must be in format A1A 1A1';
      return `Invalid ${this.formatFieldName(controlName)} format`;
    }
    return null;
  }

  private formatFieldName(name: string): string {
    return name
      .replace('address.', '')
      .replace(/([A-Z])/g, ' $1')
      .replace(/^./, str => str.toUpperCase())
      .trim();
  }
}
