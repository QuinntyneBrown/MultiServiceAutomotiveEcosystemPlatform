import { Component, Input, Output, EventEmitter, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface ReferralCodeValidation {
  isValid: boolean;
  referrerName?: string;
  discount?: string;
  message?: string;
}

/**
 * Referral Code Entry Component (REQ-RF-F016)
 *
 * Interface for entering and validating referral codes during signup.
 * Supports auto-application from URL parameters.
 *
 * @example
 * ```html
 * <ms-referral-code-entry
 *   [autoAppliedCode]="codeFromUrl"
 *   (codeValidated)="onCodeValidated($event)">
 * </ms-referral-code-entry>
 * ```
 */
@Component({
  selector: 'ms-referral-code-entry',
  imports: [CommonModule, FormsModule],
  templateUrl: './referral-code-entry.html',
  styleUrl: './referral-code-entry.scss',
})
export class ReferralCodeEntry implements OnInit {
  /** Pre-filled referral code (e.g., from URL parameter) */
  @Input() autoAppliedCode?: string;

  /** Whether the code field is required */
  @Input() required = false;

  @Output() codeValidated = new EventEmitter<ReferralCodeValidation>();
  @Output() codeRemoved = new EventEmitter<void>();

  referralCode = '';
  isValidating = signal(false);
  validation = signal<ReferralCodeValidation | null>(null);
  showOptional = signal(false);

  ngOnInit() {
    if (this.autoAppliedCode) {
      this.referralCode = this.autoAppliedCode;
      this.validateCode();
    }
  }

  async validateCode() {
    if (!this.referralCode.trim()) {
      this.validation.set(null);
      return;
    }

    this.isValidating.set(true);

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 500));

      // Mock validation - replace with actual API call
      const isValid = this.referralCode.length >= 6;
      const result: ReferralCodeValidation = isValid
        ? {
            isValid: true,
            referrerName: 'John Doe',
            discount: '15% off',
            message: 'Referral code applied successfully!'
          }
        : {
            isValid: false,
            message: 'Invalid referral code. Please check and try again.'
          };

      this.validation.set(result);
      this.codeValidated.emit(result);
    } catch (error) {
      this.validation.set({
        isValid: false,
        message: 'Unable to validate code. Please try again.'
      });
    } finally {
      this.isValidating.set(false);
    }
  }

  removeCode() {
    this.referralCode = '';
    this.validation.set(null);
    this.codeRemoved.emit();
  }

  skipCode() {
    this.showOptional.set(false);
    this.referralCode = '';
    this.validation.set(null);
  }

  toggleOptional() {
    this.showOptional.set(!this.showOptional());
  }
}
