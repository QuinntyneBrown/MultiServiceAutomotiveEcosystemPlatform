import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Tenant Not Found Page Component (MT-F009)
 * 
 * A dedicated error page displayed when a user attempts to access a tenant subdomain 
 * that doesn't exist or is no longer active. Provides a friendly user experience while 
 * guiding users to the main platform.
 * 
 * @example
 * ```html
 * <ms-tenant-not-found
 *   [attemptedSubdomain]="'invalid-shop'"
 *   [mainPlatformUrl]="'https://www.platform.com'"
 *   (navigateToMain)="handleNavigate()">
 * </ms-tenant-not-found>
 * ```
 */
@Component({
  selector: 'ms-tenant-not-found',
  imports: [CommonModule],
  templateUrl: './tenant-not-found.html',
  styleUrl: './tenant-not-found.scss',
})
export class TenantNotFound {
  /** The invalid subdomain that was accessed */
  @Input() attemptedSubdomain?: string;

  /** URL to redirect to main platform */
  @Input() mainPlatformUrl: string = 'https://www.platform.com';

  /** URL for support contact */
  @Input() supportUrl: string = 'https://www.platform.com/contact';

  /** Custom error message (optional) */
  @Input() customMessage?: string;

  @Output() navigateToMain = new EventEmitter<void>();
  @Output() contactSupport = new EventEmitter<void>();

  get defaultMessage(): string {
    if (this.attemptedSubdomain) {
      return `We couldn't find the network "${this.attemptedSubdomain}". It may have been moved or is no longer available.`;
    }
    return "We couldn't find the network you're looking for. It may have been moved or is no longer available.";
  }

  onNavigateToMainClick(): void {
    this.navigateToMain.emit();
    if (this.mainPlatformUrl) {
      window.location.href = this.mainPlatformUrl;
    }
  }

  onContactSupportClick(): void {
    this.contactSupport.emit();
    if (this.supportUrl) {
      window.location.href = this.supportUrl;
    }
  }
}
