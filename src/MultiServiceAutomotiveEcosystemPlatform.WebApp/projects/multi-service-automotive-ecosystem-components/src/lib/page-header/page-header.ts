import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Page Header Component
 *
 * A reusable header component for pages that displays a title, optional subtitle,
 * and optional action buttons. Uses the consistent design system styling with
 * a blue gradient background.
 *
 * @example
 * ```html
 * <ms-page-header
 *   title="Welcome back, John!"
 *   subtitle="Here's what's happening with your business">
 *   <button actions class="action-button">Add Customer</button>
 * </ms-page-header>
 * ```
 */
@Component({
  selector: 'ms-page-header',
  imports: [CommonModule],
  templateUrl: './page-header.html',
  styleUrl: './page-header.scss',
})
export class PageHeader {
  /** The main title displayed in the header */
  @Input({ required: true }) title!: string;

  /** Optional subtitle displayed below the title */
  @Input() subtitle?: string;

  /** Whether to use the hero variant (larger padding, different background style) */
  @Input() variant: 'default' | 'hero' = 'default';
}
