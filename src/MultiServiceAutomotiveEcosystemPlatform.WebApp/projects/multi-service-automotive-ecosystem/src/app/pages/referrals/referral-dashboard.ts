import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, of } from 'rxjs';
import { ShareReferral, UserInfo } from 'multi-service-automotive-ecosystem-components';

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
  imports: [CommonModule, ShareReferral],
  templateUrl: './referral-dashboard.html',
  styleUrl: './referral-dashboard.scss',
})
export class ReferralDashboard {
  // Observable pattern as per requirements
  stats$: Observable<ReferralStats> = this.loadStats();
  referrals$: Observable<Referral[]> = this.loadReferrals();
  referralCode = 'WELCOME2024';
  showShareModal = signal(false);
  
  // Mock user info for share modal
  currentUser: UserInfo = {
    id: '1',
    firstName: 'John',
    lastName: 'Doe',
    email: 'john.doe@example.com'
  };

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

  openShareModal(): void {
    this.showShareModal.set(true);
  }

  closeShareModal(): void {
    this.showShareModal.set(false);
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
