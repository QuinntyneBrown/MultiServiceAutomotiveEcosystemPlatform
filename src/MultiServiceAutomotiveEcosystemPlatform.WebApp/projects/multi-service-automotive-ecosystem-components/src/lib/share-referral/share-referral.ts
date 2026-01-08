import { Component, Input, Output, EventEmitter, signal, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

export type ShareMethod = 'copy-link' | 'email' | 'sms' | 'facebook' | 'twitter' | 'whatsapp' | 'linkedin' | 'qr-code';

export interface UserInfo {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}

export interface BrandingOptions {
  logo?: string;
  accentColor?: string;
  discount?: string;
}

export interface ShareMetadata {
  timestamp: Date;
  recipientCount?: number;
  platform?: string;
}

export interface ShareError {
  code: 'NETWORK_ERROR' | 'INVALID_EMAIL' | 'INVALID_PHONE' | 'RATE_LIMIT' | 'SERVICE_UNAVAILABLE';
  message: string;
}

export interface ShareSuccessEvent {
  method: ShareMethod;
  metadata: ShareMetadata;
}

export interface ShareErrorEvent {
  method: ShareMethod;
  error: ShareError;
}

/**
 * Share Referral Interface Component (REQ-RF-F002)
 *
 * A comprehensive modal interface for sharing referral links through
 * multiple channels including copy link, email, SMS, social media, and QR codes.
 *
 * @example
 * ```html
 * <ms-share-referral
 *   [referralCode]="'ABC123'"
 *   [user]="currentUser"
 *   (closeModal)="onClose()"
 *   (shareSuccess)="onShareSuccess($event)">
 * </ms-share-referral>
 * ```
 */
@Component({
  selector: 'ms-share-referral',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './share-referral.html',
  styleUrl: './share-referral.scss',
})
export class ShareReferral implements OnInit, OnDestroy {
  /** User's unique referral code */
  @Input() referralCode!: string;

  /** Full referral URL (auto-generated if not provided) */
  @Input() referralUrl?: string;

  /** User information for personalization */
  @Input() user!: UserInfo;

  /** Default tab to display */
  @Input() defaultTab: ShareMethod = 'copy-link';

  /** Available share methods */
  @Input() enabledMethods: ShareMethod[] = ['copy-link', 'email', 'sms', 'social', 'qr-code'] as any[];

  /** Pre-filled message template */
  @Input() defaultMessage?: string;

  /** Custom branding/styling */
  @Input() branding?: BrandingOptions;

  /** Analytics tracking enabled */
  @Input() enableAnalytics = true;

  @Output() closeModal = new EventEmitter<void>();
  @Output() shareSuccess = new EventEmitter<ShareSuccessEvent>();
  @Output() shareError = new EventEmitter<ShareErrorEvent>();

  @ViewChild('urlInput') urlInput!: ElementRef<HTMLInputElement>;

  activeTab = signal<ShareMethod>('copy-link');
  isCopied = signal(false);
  isLoading = signal(false);
  qrCodeDataUrl = signal<string | null>(null);

  emailForm!: FormGroup;
  smsForm!: FormGroup;

  private baseUrl = 'https://autoservice.com/r/';

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.activeTab.set(this.defaultTab);

    if (!this.referralUrl) {
      this.referralUrl = this.baseUrl + this.referralCode;
    }

    this.initializeForms();
    this.generateQRCode();
  }

  ngOnDestroy() {
    // Cleanup if needed
  }

  private initializeForms() {
    const defaultEmailMessage = this.defaultMessage ||
      `Hey! I've been using AutoService Platform and thought you might be interested. Use my referral link to get ${this.branding?.discount || '15%'} off your first service!`;

    const defaultSmsMessage =
      `Check out AutoService! Get ${this.branding?.discount || '15%'} off: ${this.referralUrl}`;

    this.emailForm = this.fb.group({
      recipients: ['', [Validators.required, Validators.pattern(/^[^\s@]+@[^\s@]+\.[^\s@]+(,\s*[^\s@]+@[^\s@]+\.[^\s@]+)*$/)]],
      subject: [`${this.user.firstName} invited you to AutoService`],
      message: [defaultEmailMessage, [Validators.required, Validators.maxLength(2000)]]
    });

    this.smsForm = this.fb.group({
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\(\d{3}\) \d{3}-\d{4}$/)]],
      message: [defaultSmsMessage, [Validators.required, Validators.maxLength(160)]]
    });
  }

  private async generateQRCode() {
    // Simplified QR code generation (in production, use a library like qrcode)
    const qrApiUrl = `https://api.qrserver.com/v1/create-qr-code/?size=240x240&data=${encodeURIComponent(this.referralUrl!)}`;
    this.qrCodeDataUrl.set(qrApiUrl);
  }

  switchTab(tab: ShareMethod) {
    this.activeTab.set(tab);
    this.isCopied.set(false);
  }

  async copyLink() {
    try {
      await navigator.clipboard.writeText(this.referralUrl!);
      this.isCopied.set(true);

      this.shareSuccess.emit({
        method: 'copy-link',
        metadata: { timestamp: new Date() }
      });

      // Reset after 3 seconds
      setTimeout(() => this.isCopied.set(false), 3000);
    } catch {
      this.shareError.emit({
        method: 'copy-link',
        error: { code: 'SERVICE_UNAVAILABLE', message: 'Failed to copy link' }
      });
    }
  }

  selectUrlInput() {
    this.urlInput?.nativeElement.select();
  }

  async sendEmail() {
    if (this.emailForm.invalid) {
      this.emailForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));

      const recipients = this.emailForm.value.recipients.split(',').map((e: string) => e.trim());

      this.shareSuccess.emit({
        method: 'email',
        metadata: {
          timestamp: new Date(),
          recipientCount: recipients.length
        }
      });

      this.emailForm.reset();
      this.initializeForms();
    } catch {
      this.shareError.emit({
        method: 'email',
        error: { code: 'NETWORK_ERROR', message: 'Failed to send email' }
      });
    } finally {
      this.isLoading.set(false);
    }
  }

  async sendSms() {
    if (this.smsForm.invalid) {
      this.smsForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));

      this.shareSuccess.emit({
        method: 'sms',
        metadata: {
          timestamp: new Date(),
          recipientCount: 1
        }
      });

      this.smsForm.reset();
      this.initializeForms();
    } catch {
      this.shareError.emit({
        method: 'sms',
        error: { code: 'NETWORK_ERROR', message: 'Failed to send SMS' }
      });
    } finally {
      this.isLoading.set(false);
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

    this.smsForm.patchValue({ phoneNumber: input.value });
  }

  shareSocial(platform: 'facebook' | 'twitter' | 'whatsapp' | 'linkedin') {
    const message = encodeURIComponent(`Check out AutoService! Get ${this.branding?.discount || '15%'} off your first service.`);
    const url = encodeURIComponent(this.referralUrl!);
    let shareUrl = '';

    switch (platform) {
      case 'facebook':
        shareUrl = `https://www.facebook.com/sharer/sharer.php?u=${url}&quote=${message}`;
        break;
      case 'twitter':
        shareUrl = `https://twitter.com/intent/tweet?url=${url}&text=${message}`;
        break;
      case 'whatsapp':
        shareUrl = `https://wa.me/?text=${message}%20${url}`;
        break;
      case 'linkedin':
        shareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${url}`;
        break;
    }

    window.open(shareUrl, '_blank', 'width=600,height=400');

    this.shareSuccess.emit({
      method: platform,
      metadata: {
        timestamp: new Date(),
        platform
      }
    });
  }

  downloadQR(format: 'png' | 'svg') {
    const url = `https://api.qrserver.com/v1/create-qr-code/?size=1024x1024&format=${format}&data=${encodeURIComponent(this.referralUrl!)}`;

    const link = document.createElement('a');
    link.href = url;
    link.download = `referral-qr-${this.referralCode}.${format}`;
    link.click();

    this.shareSuccess.emit({
      method: 'qr-code',
      metadata: { timestamp: new Date() }
    });
  }

  close() {
    this.closeModal.emit();
  }

  onKeydown(event: KeyboardEvent) {
    if (event.key === 'Escape') {
      this.close();
    }
  }

  get emailCharCount(): number {
    return this.emailForm.get('message')?.value?.length || 0;
  }

  get smsCharCount(): number {
    return this.smsForm.get('message')?.value?.length || 0;
  }

  getEmailError(field: string): string | null {
    const control = this.emailForm.get(field);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return 'This field is required';
    if (control.hasError('pattern')) return 'Please enter valid email addresses (comma-separated)';
    if (control.hasError('maxlength')) return 'Message is too long';
    return null;
  }

  getSmsError(field: string): string | null {
    const control = this.smsForm.get(field);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return 'This field is required';
    if (control.hasError('pattern')) return 'Format: (555) 123-4567';
    if (control.hasError('maxlength')) return 'Message exceeds 160 characters';
    return null;
  }
}
