import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PageHeader } from 'multi-service-automotive-ecosystem-components';

interface DashboardStats {
  totalCustomers: number;
  newInquiries: number;
  pendingReferrals: number;
  monthlyReferrals: number;
}

interface Activity {
  id: string;
  type: 'customer' | 'inquiry' | 'referral' | 'message';
  description: string;
  date: Date;
}

@Component({
  selector: 'app-professional-dashboard-home',
  imports: [CommonModule, RouterModule, PageHeader],
  templateUrl: './professional-dashboard-home.html',
  styleUrl: './professional-dashboard-home.scss',
})
export class ProfessionalDashboardHome {
  stats$: Observable<DashboardStats> = this.loadStats();
  activities$: Observable<Activity[]> = this.loadActivities();
  profileCompletionPercentage = 85;
  professionalName = 'John Professional';

  private loadStats(): Observable<DashboardStats> {
    // Mock data - replace with actual API call
    const mockStats: DashboardStats = {
      totalCustomers: 127,
      newInquiries: 5,
      pendingReferrals: 3,
      monthlyReferrals: 12
    };

    return of(mockStats);
  }

  private loadActivities(): Observable<Activity[]> {
    // Mock data - replace with actual API call
    const mockActivities: Activity[] = [
      {
        id: '1',
        type: 'inquiry',
        description: 'New inquiry from Jane Smith for brake service',
        date: new Date('2024-02-05T10:30:00')
      },
      {
        id: '2',
        type: 'referral',
        description: 'Referral from Bob\'s Auto accepted',
        date: new Date('2024-02-04T14:20:00')
      },
      {
        id: '3',
        type: 'customer',
        description: 'New customer John Doe added',
        date: new Date('2024-02-03T09:15:00')
      },
      {
        id: '4',
        type: 'message',
        description: 'New message from Alice Williams',
        date: new Date('2024-02-02T16:45:00')
      }
    ];

    return of(mockActivities);
  }

  getActivityIcon(type: string): string {
    const icons: Record<string, string> = {
      'customer': '👤',
      'inquiry': '❓',
      'referral': '🔗',
      'message': '💬'
    };
    return icons[type] || '📋';
  }

  getActivityClass(type: string): string {
    return `professional-dashboard-home__activity-icon--${type}`;
  }
}
