import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';

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

  copyReferralCode(): void {
    navigator.clipboard.writeText(this.referralCode);
    // Show toast notification - implement later
    console.log('Referral code copied to clipboard');
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
