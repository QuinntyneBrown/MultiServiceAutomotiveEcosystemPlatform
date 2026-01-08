import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

interface Professional {
  id: string;
  name: string;
  businessType: string;
  specialties: string[];
}

interface Customer {
  id: string;
  name: string;
  email: string;
  vehicles: string[];
}

/**
 * Send Professional Referral Form (REQ-RF-F006)
 * Form for professionals to refer customers to other professionals
 */
@Component({
  selector: 'app-send-professional-referral',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './send-professional-referral.html',
  styleUrl: './send-professional-referral.scss',
})
export class SendProfessionalReferral implements OnInit {
  referralForm!: FormGroup;
  professionals$: Observable<Professional[]> = this.loadProfessionals();
  customers$: Observable<Customer[]> = this.loadCustomers();
  isSubmitting = false;
  showPreview = false;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.initializeForm();
  }

  private initializeForm() {
    this.referralForm = this.fb.group({
      targetProfessionalId: ['', Validators.required],
      customerId: ['', Validators.required],
      serviceNeeded: ['', [Validators.required, Validators.minLength(5)]],
      reasonForReferral: ['', [Validators.required, Validators.minLength(10)]],
      priority: ['normal'],
      internalNotes: [''],
      enableDiscount: [false],
      discountType: ['percentage'],
      discountValue: [0],
      discountExpiration: ['']
    });

    // Watch discount toggle to enable/disable discount fields
    this.referralForm.get('enableDiscount')?.valueChanges.subscribe(enabled => {
      const discountFields = ['discountType', 'discountValue', 'discountExpiration'];
      discountFields.forEach(field => {
        const control = this.referralForm.get(field);
        if (enabled) {
          control?.setValidators([Validators.required]);
        } else {
          control?.clearValidators();
        }
        control?.updateValueAndValidity();
      });
    });
  }

  private loadProfessionals(): Observable<Professional[]> {
    // Mock data - replace with API call
    const mockProfessionals: Professional[] = [
      {
        id: '1',
        name: 'Bob\'s Auto Body',
        businessType: 'Auto Body Shop',
        specialties: ['Paint', 'Dent Removal', 'Collision Repair']
      },
      {
        id: '2',
        name: 'Quick Tires & Wheels',
        businessType: 'Tire Shop',
        specialties: ['Tire Installation', 'Wheel Alignment', 'Balancing']
      },
      {
        id: '3',
        name: 'Premium Detailing',
        businessType: 'Detailing Service',
        specialties: ['Interior Detailing', 'Exterior Detailing', 'Ceramic Coating']
      }
    ];
    return of(mockProfessionals);
  }

  private loadCustomers(): Observable<Customer[]> {
    // Mock data - replace with API call
    const mockCustomers: Customer[] = [
      {
        id: '1',
        name: 'John Doe',
        email: 'john.doe@example.com',
        vehicles: ['2020 Toyota Camry', '2018 Honda Accord']
      },
      {
        id: '2',
        name: 'Jane Smith',
        email: 'jane.smith@example.com',
        vehicles: ['2019 Ford F-150']
      },
      {
        id: '3',
        name: 'Alice Johnson',
        email: 'alice.johnson@example.com',
        vehicles: ['2021 Tesla Model 3']
      }
    ];
    return of(mockCustomers);
  }

  togglePreview() {
    this.showPreview = !this.showPreview;
  }

  async submitReferral() {
    if (this.referralForm.invalid) {
      this.referralForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));

      // Navigate back to referrals dashboard
      this.router.navigate(['/professional/referrals']);
    } catch (error) {
      console.error('Failed to send referral:', error);
      // Show error notification
    } finally {
      this.isSubmitting = false;
    }
  }

  cancel() {
    this.router.navigate(['/professional/referrals']);
  }

  getFieldError(fieldName: string): string | null {
    const control = this.referralForm.get(fieldName);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return 'This field is required';
    if (control.hasError('minLength')) {
      const minLength = control.errors?.['minLength']?.requiredLength;
      return `Minimum ${minLength} characters required`;
    }
    return null;
  }

  getSelectedProfessional(): Professional | undefined {
    const id = this.referralForm.get('targetProfessionalId')?.value;
    let professional: Professional | undefined;
    this.professionals$.subscribe(pros => {
      professional = pros.find(p => p.id === id);
    });
    return professional;
  }

  getSelectedCustomer(): Customer | undefined {
    const id = this.referralForm.get('customerId')?.value;
    let customer: Customer | undefined;
    this.customers$.subscribe(custs => {
      customer = custs.find(c => c.id === id);
    });
    return customer;
  }
}
