import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { Observable, of } from 'rxjs';

interface Customer {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: string;
  status: 'active' | 'inactive';
  ownership: 'my-customer' | 'referred' | 'shared';
  createdDate: Date;
  lastActivity: Date;
  notes: string;
}

interface Activity {
  id: string;
  type: 'inquiry' | 'referral' | 'service' | 'communication' | 'status-change';
  description: string;
  date: Date;
}

interface Vehicle {
  id: string;
  year: number;
  make: string;
  model: string;
  isPrimary: boolean;
}

@Component({
  selector: 'app-customer-detail',
  imports: [CommonModule, RouterModule],
  templateUrl: './customer-detail.html',
  styleUrl: './customer-detail.scss',
})
export class CustomerDetail implements OnInit {
  customerId: string = '';
  customer$: Observable<Customer | null> = of(null);
  activities$: Observable<Activity[]> = of([]);
  vehicles$: Observable<Vehicle[]> = of([]);

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.customerId = this.route.snapshot.params['id'];
    this.customer$ = this.loadCustomer(this.customerId);
    this.activities$ = this.loadActivities(this.customerId);
    this.vehicles$ = this.loadVehicles(this.customerId);
  }

  private loadCustomer(id: string): Observable<Customer> {
    // Mock data - replace with actual API call
    const mockCustomer: Customer = {
      id,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      phone: '(555) 123-4567',
      address: '123 Main St, Springfield, ON K1A 0B1',
      status: 'active',
      ownership: 'my-customer',
      createdDate: new Date('2024-01-15'),
      lastActivity: new Date('2024-02-01'),
      notes: 'Prefers morning appointments. Has expressed interest in premium services.'
    };

    return of(mockCustomer);
  }

  private loadActivities(customerId: string): Observable<Activity[]> {
    // Mock data - replace with actual API call
    const mockActivities: Activity[] = [
      {
        id: '1',
        type: 'inquiry',
        description: 'Submitted inquiry about brake service',
        date: new Date('2024-02-01')
      },
      {
        id: '2',
        type: 'communication',
        description: 'Sent follow-up email',
        date: new Date('2024-01-28')
      },
      {
        id: '3',
        type: 'referral',
        description: 'Referred to Bob\'s Auto Body',
        date: new Date('2024-01-25')
      }
    ];

    return of(mockActivities);
  }

  private loadVehicles(customerId: string): Observable<Vehicle[]> {
    // Mock data - replace with actual API call
    const mockVehicles: Vehicle[] = [
      {
        id: '1',
        year: 2020,
        make: 'Toyota',
        model: 'Camry',
        isPrimary: true
      },
      {
        id: '2',
        year: 2018,
        make: 'Honda',
        model: 'Civic',
        isPrimary: false
      }
    ];

    return of(mockVehicles);
  }

  getActivityIcon(type: string): string {
    const icons: Record<string, string> = {
      'inquiry': '❓',
      'referral': '🔗',
      'service': '🔧',
      'communication': '💬',
      'status-change': '🚩'
    };
    return icons[type] || '📋';
  }

  getActivityClass(type: string): string {
    return `customer-detail__activity-icon--${type}`;
  }
}
