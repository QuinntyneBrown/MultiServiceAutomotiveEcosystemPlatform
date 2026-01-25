import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { ShareReferralDialogService, ToastService, UserInfo } from 'multi-service-automotive-ecosystem-components';

interface ReferralStats {
  totalReferrals: number;
  successfulConversions: number;
  pendingReferrals: number;
  totalRewards: number;
  pendingRewards: number;
}

interface Referral {
  id: string;
  friendName: string;
  dateSent: Date;
  status: 'pending' | 'converted' | 'expired' | 'rewarded';
  rewardAmount: number;
}

@Component({
  selector: 'app-referral-dashboard',
  imports: [CommonModule],
  templateUrl: './referral-dashboard.html',
  styleUrl: './referral-dashboard.scss',
})
export class ReferralDashboard {
  // Observable pattern as per requirements
  stats$: Observable<ReferralStats> = this.loadStats();
  referrals$: Observable<Referral[]> = this.loadReferrals();
  referralCode = 'WELCOME2024';
  
  // Mock user info for share modal
  currentUser: UserInfo = {
    id: '1',
    firstName: 'John',
    lastName: 'Doe',
    email: 'john.doe@example.com'
  };

  constructor(
    private shareReferralDialog: ShareReferralDialogService,
    private toast: ToastService
  ) {}

  private loadStats(): Observable<ReferralStats> {
    // Mock data - replace with actual API call
    const mockStats: ReferralStats = {
      totalReferrals: 12,
      successfulConversions: 7,
      pendingReferrals: 3,
      totalRewards: 350.00,
      pendingRewards: 150.00
    };

    return of(mockStats);
  }

  private loadReferrals(): Observable<Referral[]> {
    // Mock data - replace with actual API call
    const mockReferrals: Referral[] = [
      {
        id: '1',
        friendName: 'John Doe',
        dateSent: new Date('2024-01-15'),
        status: 'converted',
        rewardAmount: 50.00
      },
      {
        id: '2',
        friendName: 'Jane Smith',
        dateSent: new Date('2024-01-20'),
        status: 'pending',
        rewardAmount: 50.00
      },
      {
        id: '3',
        friendName: 'Bob Johnson',
        dateSent: new Date('2024-01-10'),
        status: 'rewarded',
        rewardAmount: 50.00
      }
    ];

    return of(mockReferrals);
  }

  async copyReferralCode(): Promise<void> {
    try {
      if (navigator?.clipboard?.writeText) {
        await navigator.clipboard.writeText(this.referralCode);
        this.toast.success('Referral code copied to clipboard');
        return;
      }
    } catch {
      // fall through to legacy fallback
    }

    try {
      const textarea = document.createElement('textarea');
      textarea.value = this.referralCode;
      textarea.setAttribute('readonly', '');
      textarea.style.position = 'fixed';
      textarea.style.left = '-9999px';
      document.body.appendChild(textarea);
      textarea.select();
      const copied = document.execCommand?.('copy') ?? false;
      document.body.removeChild(textarea);

      if (copied) {
        this.toast.success('Referral code copied to clipboard');
      } else {
        this.toast.error('Unable to copy referral code');
      }
    } catch {
      this.toast.error('Unable to copy referral code');
    }
  }

  openShareModal(): void {
    const ref = this.shareReferralDialog.open({
      referralCode: this.referralCode,
      user: this.currentUser,
    });

    ref.closed.subscribe(() => {
      // no-op: hook available for analytics if needed
    });
  }

  onShareSuccess(event: any): void {
    console.log('Share success:', event);
    // Handle success - could show toast notification
  }

  onShareError(event: any): void {
    console.error('Share error:', event);
    // Handle error - could show error notification
  }

  getStatusClass(status: string): string {
    const statusMap: Record<string, string> = {
      'pending': 'referral-dashboard__status--pending',
      'converted': 'referral-dashboard__status--converted',
      'expired': 'referral-dashboard__status--expired',
      'rewarded': 'referral-dashboard__status--rewarded'
    };
    return statusMap[status] || '';
  }

  getStatusLabel(status: string): string {
    const labelMap: Record<string, string> = {
      'pending': 'Pending',
      'converted': 'Converted',
      'expired': 'Expired',
      'rewarded': 'Rewarded'
    };
    return labelMap[status] || status;
  }
}
