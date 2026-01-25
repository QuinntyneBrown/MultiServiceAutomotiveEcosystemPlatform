import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ToastService } from './toast.service';

@Component({
  selector: 'ms-toast-container',
  imports: [CommonModule],
  template: `
    <div class="toast-stack" aria-label="Notifications">
      @for (toast of toastService.toasts(); track toast.id) {
        <div
          class="toast"
          [class.toast--success]="toast.type === 'success'"
          [class.toast--error]="toast.type === 'error'"
          [attr.role]="toast.type === 'error' ? 'alert' : 'status'"
          [attr.aria-live]="toast.type === 'error' ? 'assertive' : 'polite'"
        >
          {{ toast.message }}
        </div>
      }
    </div>
  `,
  styleUrl: './toast-container.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToastContainer {
  protected readonly toastService = inject(ToastService);
}
