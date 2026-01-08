import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable, of } from 'rxjs';

interface ReferralStats {
  sentThisMonth: number;
  receivedThisMonth: number;
  acceptanceRate: number;
  activeReferrals: number;
}

interface ProfessionalReferral {
  id: string;
  customerName: string;
  targetProfessionalName: string;
  serviceNeeded: string;
  status: 'pending' | 'accepted' | 'declined' | 'completed';
  date: Date;
  type: 'sent' | 'received';
}

@Component({
  selector: 'app-professional-referral-dashboard',
  imports: [CommonModule, RouterModule],
  templateUrl: './professional-referral-dashboard.html',
  styleUrl: './professional-referral-dashboard.scss',
})
export class ProfessionalReferralDashboard {
  stats$: Observable<ReferralStats> = this.loadStats();
  sentReferrals$: Observable<ProfessionalReferral[]> = this.loadSentReferrals();
  receivedReferrals$: Observable<ProfessionalReferral[]> = this.loadReceivedReferrals();

  private loadStats(): Observable<ReferralStats> {
    const mockStats: ReferralStats = {
      sentThisMonth: 8,
      receivedThisMonth: 12,
      acceptanceRate: 85,
      activeReferrals: 5
    };
    return of(mockStats);
  }

  private loadSentReferrals(): Observable<ProfessionalReferral[]> {
    const mockReferrals: ProfessionalReferral[] = [
      {
        id: '1',
        customerName: 'John Doe',
        targetProfessionalName: 'Bob\'s Auto Body',
        serviceNeeded: 'Paint Job',
        status: 'accepted',
        date: new Date('2024-02-05'),
        type: 'sent'
      },
      {
        id: '2',
        customerName: 'Jane Smith',
        targetProfessionalName: 'Quick Tires',
        serviceNeeded: 'Tire Replacement',
        status: 'pending',
        date: new Date('2024-02-04'),
        type: 'sent'
      }
    ];
    return of(mockReferrals);
  }

  private loadReceivedReferrals(): Observable<ProfessionalReferral[]> {
    const mockReferrals: ProfessionalReferral[]  = [
      {
        id: '3',
        customerName: 'Alice Williams',
        targetProfessionalName: 'Mike\'s Mechanic',
        serviceNeeded: 'Brake Service',
        status: 'pending',
        date: new Date('2024-02-06'),
        type: 'received'
      },
      {
        id: '4',
        customerName: 'Bob Johnson',
        targetProfessionalName: 'Sue\'s Service',
        serviceNeeded: 'Engine Diagnostics',
        status: 'accepted',
        date: new Date('2024-02-03'),
        type: 'received'
      }
    ];
    return of(mockReferrals);
  }

  getStatusClass(status: string): string {
    return `professional-referral-dashboard__status--${status}`;
  }

  getStatusLabel(status: string): string {
    const labels: Record<string, string> = {
      'pending': 'Pending',
      'accepted': 'Accepted',
      'declined': 'Declined',
      'completed': 'Completed'
    };
    return labels[status] || status;
  }
}
