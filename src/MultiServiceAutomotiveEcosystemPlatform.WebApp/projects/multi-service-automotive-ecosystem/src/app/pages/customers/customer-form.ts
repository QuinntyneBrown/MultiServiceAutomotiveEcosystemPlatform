import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

interface Customer {
  id?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: string;
  city: string;
  province: string;
  postalCode: string;
  notes: string;
}

@Component({
  selector: 'app-customer-form',
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './customer-form.html',
  styleUrl: './customer-form.scss',
})
export class CustomerForm implements OnInit {
  customerForm: FormGroup;
  customerId: string | null = null;
  isEditMode = false;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.customerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      address: [''],
      city: [''],
      province: [''],
      postalCode: [''],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.customerId = this.route.snapshot.params['id'];
    this.isEditMode = !!this.customerId;

    if (this.isEditMode) {
      this.loadCustomer(this.customerId!);
    }
  }

  private loadCustomer(id: string): void {
    // Mock data - replace with actual API call
    const mockCustomer: Customer = {
      id,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      phone: '(555) 123-4567',
      address: '123 Main St',
      city: 'Springfield',
      province: 'ON',
      postalCode: 'K1A 0B1',
      notes: 'Prefers morning appointments.'
    };

    this.customerForm.patchValue(mockCustomer);
  }

  onSubmit(): void {
    if (this.customerForm.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      
      const customerData: Customer = this.customerForm.value;
      
      // Mock API call - replace with actual API call
      console.log('Saving customer:', customerData);
      
      setTimeout(() => {
        this.isSubmitting = false;
        this.router.navigate(['/customers']);
      }, 500);
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.customerForm.controls).forEach(key => {
        this.customerForm.controls[key].markAsTouched();
      });
    }
  }

  onCancel(): void {
    if (this.isEditMode) {
      this.router.navigate(['/customers', this.customerId]);
    } else {
      this.router.navigate(['/customers']);
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.customerForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  getFieldError(fieldName: string): string {
    const field = this.customerForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return 'This field is required';
      if (field.errors['email']) return 'Please enter a valid email address';
    }
    return '';
  }
}
