import { Injectable } from '@angular/core';
import { Dialog, DialogRef } from '@angular/cdk/dialog';

import { ShareReferral, ShareReferralDialogData } from './share-referral';

@Injectable({ providedIn: 'root' })
export class ShareReferralDialogService {
  constructor(private dialog: Dialog) {}

  open(data: ShareReferralDialogData): DialogRef<void, ShareReferral> {
    return this.dialog.open(ShareReferral, {
      data,
      backdropClass: 'ms-dialog-backdrop',
      closeOnNavigation: true,
      disableClose: false,
    });
  }
}
