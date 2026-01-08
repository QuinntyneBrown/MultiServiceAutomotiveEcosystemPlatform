import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

export interface Professional {
  id: string;
  name: string;
  businessType: string;
}

export interface ReferralInvitationData {
  friendName: string;
  friendEmail: string;
  friendPhone?: string;
  professionalId?: string;
  message?: string;
}

export interface InvitationSuccessEvent {
  invitation: ReferralInvitationData;
  timestamp: Date;
}

export interface InvitationErrorEvent {
  error: string;
}

/**
 * Referral Invitation Form Component (REQ-RF-F003)
 *
 * Direct referral invitation form allowing customers to send
 * personalized referral invitations to friends.
 *
 * @example
 * ```html
 * <ms-referral-invitation
 *   [professionals]="professionals"
 *   (invitationSent)="onSuccess($event)"
 *   (invitationError)="onError($event)">
 * </ms-referral-invitation>
 * ```
 */
@Component({
  selector: 'ms-referral-invitation',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './referral-invitation.html',
  styleUrl: './referral-invitation.scss',
})
export class ReferralInvitation implements OnInit {
  /** List of professionals for optional selection */
  @Input() professionals: Professional[] = [];

  /** Pre-fill friend's name */
  @Input() friendName?: string;

  /** Pre-fill friend's email */
  @Input() friendEmail?: string;

  @Output() invitationSent = new EventEmitter<InvitationSuccessEvent>();
  @Output() invitationError = new EventEmitter<InvitationErrorEvent>();
  @Output() cancel = new EventEmitter<void>();

  invitationForm!: FormGroup;
  isSubmitting = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.initializeForm();
  }

  private initializeForm() {
    this.invitationForm = this.fb.group({
      friendName: [this.friendName || '', [Validators.required, Validators.minLength(2)]],
      friendEmail: [this.friendEmail || '', [Validators.required, Validators.email]],
      friendPhone: [''],
      professionalId: [''],
      message: ['', [Validators.maxLength(500)]]
    });
  }

  async sendInvitation() {
    if (this.invitationForm.invalid) {
      this.invitationForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));

      const invitationData: ReferralInvitationData = this.invitationForm.value;

      this.invitationSent.emit({
        invitation: invitationData,
        timestamp: new Date()
      });

      this.invitationForm.reset();
      this.initializeForm();
    } catch (error) {
      this.invitationError.emit({
        error: 'Failed to send invitation. Please try again.'
      });
    } finally {
      this.isSubmitting = false;
    }
  }

  onCancel() {
    this.cancel.emit();
  }

  getFieldError(fieldName: string): string | null {
    const control = this.invitationForm.get(fieldName);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return 'This field is required';
    if (control.hasError('email')) return 'Please enter a valid email address';
    if (control.hasError('minLength')) return 'Name must be at least 2 characters';
    if (control.hasError('maxlength')) return 'Message is too long (max 500 characters)';
    return null;
  }

  get messageCharCount(): number {
    return this.invitationForm.get('message')?.value?.length || 0;
  }
}
