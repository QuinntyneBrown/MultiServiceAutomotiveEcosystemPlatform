import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Observable, of } from 'rxjs';

interface ReferralDetail {
  id: string;
  customerName: string;
  customerEmail: string;
  customerPhone: string;
  sourceProfessionalName: string;
  serviceNeeded: string;
  reasonForReferral: string;
  priority: 'normal' | 'high' | 'urgent';
  hasDiscount: boolean;
  discountType?: 'percentage' | 'fixed';
  discountValue?: number;
  discountExpiration?: string;
  internalNotes?: string;
  receivedDate: Date;
  vehicles: string[];
}

/**
 * Referral Acceptance/Decline Flow (REQ-RF-F008, REQ-RF-F009)
 * Page for viewing referral details and accepting or declining
 */
@Component({
  selector: 'app-referral-action',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './referral-action.html',
  styleUrl: './referral-action.scss',
})
export class ReferralAction implements OnInit {
  referral$: Observable<ReferralDetail | null> = of(null);
  showAcceptModal = signal(false);
  showDeclineModal = signal(false);
  acceptForm!: FormGroup;
  declineForm!: FormGroup;
  isSubmitting = false;

  declineReasons = [
    'Too busy / No capacity',
    'Outside service area',
    'Not my specialty',
    'Customer already known',
    'Other'
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    const referralId = this.route.snapshot.paramMap.get('id');
    if (referralId) {
      this.referral$ = this.loadReferralDetail(referralId);
    }
    this.initializeForms();
  }

  private loadReferralDetail(id: string): Observable<ReferralDetail> {
    // Mock data - replace with API call
    const mockReferral: ReferralDetail = {
      id,
      customerName: 'Alice Williams',
      customerEmail: 'alice.williams@example.com',
      customerPhone: '(555) 123-4567',
      sourceProfessionalName: 'Mike\'s Mechanic Shop',
      serviceNeeded: 'Brake Service',
      reasonForReferral: 'Customer needs brake replacement. I don\'t have the equipment for this specific job.',
      priority: 'high',
      hasDiscount: true,
      discountType: 'percentage',
      discountValue: 15,
      discountExpiration: '2024-03-15',
      internalNotes: 'Customer mentioned grinding noise when braking.',
      receivedDate: new Date('2024-02-06'),
      vehicles: ['2019 Honda CR-V']
    };
    return of(mockReferral);
  }

  private initializeForms() {
    this.acceptForm = this.fb.group({
      messageToSender: [''],
      honorDiscount: [true],
      expectedFollowUpDate: ['']
    });

    this.declineForm = this.fb.group({
      reason: ['', Validators.required],
      otherReason: [''],
      messageToSender: [''],
      suggestAlternative: ['']
    });

    // Watch for "Other" reason selection
    this.declineForm.get('reason')?.valueChanges.subscribe(value => {
      const otherControl = this.declineForm.get('otherReason');
      if (value === 'Other') {
        otherControl?.setValidators([Validators.required]);
      } else {
        otherControl?.clearValidators();
      }
      otherControl?.updateValueAndValidity();
    });
  }

  openAcceptModal() {
    this.showAcceptModal.set(true);
  }

  closeAcceptModal() {
    this.showAcceptModal.set(false);
  }

  openDeclineModal() {
    this.showDeclineModal.set(true);
  }

  closeDeclineModal() {
    this.showDeclineModal.set(false);
  }

  async acceptReferral() {
    if (this.acceptForm.invalid) {
      this.acceptForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));

      // Navigate back to referrals dashboard
      this.router.navigate(['/professional/referrals']);
    } catch (error) {
      console.error('Failed to accept referral:', error);
    } finally {
      this.isSubmitting = false;
    }
  }

  async declineReferral() {
    if (this.declineForm.invalid) {
      this.declineForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));

      // Navigate back to referrals dashboard
      this.router.navigate(['/professional/referrals']);
    } catch (error) {
      console.error('Failed to decline referral:', error);
    } finally {
      this.isSubmitting = false;
    }
  }

  getPriorityClass(priority: string): string {
    return `referral-action__priority--${priority}`;
  }

  getPriorityLabel(priority: string): string {
    const labels: Record<string, string> = {
      'normal': 'Normal',
      'high': 'High Priority',
      'urgent': 'Urgent'
    };
    return labels[priority] || priority;
  }

  contactCustomer(method: 'phone' | 'email', referral: ReferralDetail) {
    if (method === 'phone') {
      window.location.href = `tel:${referral.customerPhone}`;
    } else {
      window.location.href = `mailto:${referral.customerEmail}`;
    }
  }

  getFieldError(form: FormGroup, fieldName: string): string | null {
    const control = form.get(fieldName);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return 'This field is required';
    return null;
  }
}
