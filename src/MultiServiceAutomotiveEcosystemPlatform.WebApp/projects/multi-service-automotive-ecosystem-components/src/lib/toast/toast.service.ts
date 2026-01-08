import { Inject, Injectable, Injector, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';

import { ToastContainer } from './toast-container';
import type { ToastMessage, ToastType } from './toast.model';

@Injectable({ providedIn: 'root' })
export class ToastService {
  private overlayRef: OverlayRef | null = null;
  private readonly timeouts = new Map<string, number>();
  private readonly isBrowser: boolean;

  readonly toasts = signal<ToastMessage[]>([]);

  constructor(
    private readonly overlay: Overlay,
    private readonly injector: Injector,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  success(message: string, durationMs = 3000): void {
    this.show('success', message, durationMs);
  }

  error(message: string, durationMs = 5000): void {
    this.show('error', message, durationMs);
  }

  private show(type: ToastType, message: string, durationMs: number): void {
    if (!this.isBrowser) {
      return;
    }

    this.ensureOverlay();

    const id = `toast-${Date.now()}-${Math.random().toString(36).slice(2)}`;
    const toast: ToastMessage = { id, type, message, durationMs };

    this.toasts.update(current => [...current, toast]);

    const timeoutId = window.setTimeout(() => this.dismiss(id), durationMs);
    this.timeouts.set(id, timeoutId);
  }

  private ensureOverlay(): void {
    if (this.overlayRef) {
      return;
    }

    this.overlayRef = this.overlay.create({
      positionStrategy: this.overlay.position().global().top('20px').right('20px'),
      scrollStrategy: this.overlay.scrollStrategies.noop(),
      hasBackdrop: false,
      panelClass: ['ms-toast-panel'],
    });

    this.overlayRef.attach(new ComponentPortal(ToastContainer, undefined, this.injector));
  }

  private dismiss(id: string): void {
    const timeoutId = this.timeouts.get(id);
    if (timeoutId) {
      window.clearTimeout(timeoutId);
      this.timeouts.delete(id);
    }

    this.toasts.update(current => current.filter(t => t.id !== id));
  }
}
