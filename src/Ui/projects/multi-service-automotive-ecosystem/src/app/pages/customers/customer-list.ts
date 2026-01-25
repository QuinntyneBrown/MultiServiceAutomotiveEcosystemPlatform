import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable, of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { PageHeader } from 'multi-service-automotive-ecosystem-components';

interface Customer {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  status: 'active' | 'inactive';
  ownership: 'my-customer' | 'referred' | 'shared';
  createdDate: Date;
  lastActivity: Date;
}

@Component({
  selector: 'app-customer-list',
  imports: [CommonModule, RouterModule, FormsModule, PageHeader],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.scss',
})
export class CustomerList {
  customers$: Observable<Customer[]> = this.loadCustomers();
  
  searchQuery = '';
  selectedOwnership = 'all';
  selectedStatus = 'all';
  sortField = 'lastName';
  sortDirection: 'asc' | 'desc' = 'asc';

  private loadCustomers(): Observable<Customer[]> {
    // Mock data - replace with actual API call
    const mockCustomers: Customer[] = [
      {
        id: '1',
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        phone: '(555) 123-4567',
        status: 'active',
        ownership: 'my-customer',
        createdDate: new Date('2024-01-15'),
        lastActivity: new Date('2024-02-01')
      },
      {
        id: '2',
        firstName: 'Jane',
        lastName: 'Smith',
        email: 'jane.smith@example.com',
        phone: '(555) 234-5678',
        status: 'active',
        ownership: 'referred',
        createdDate: new Date('2024-01-20'),
        lastActivity: new Date('2024-01-28')
      },
      {
        id: '3',
        firstName: 'Bob',
        lastName: 'Johnson',
        email: 'bob.johnson@example.com',
        phone: '(555) 345-6789',
        status: 'inactive',
        ownership: 'my-customer',
        createdDate: new Date('2024-01-10'),
        lastActivity: new Date('2024-01-15')
      },
      {
        id: '4',
        firstName: 'Alice',
        lastName: 'Williams',
        email: 'alice.williams@example.com',
        phone: '(555) 456-7890',
        status: 'active',
        ownership: 'my-customer',
        createdDate: new Date('2024-02-01'),
        lastActivity: new Date('2024-02-05')
      }
    ];

    return of(mockCustomers);
  }

  getOwnershipLabel(ownership: string): string {
    const labels: Record<string, string> = {
      'my-customer': 'My Customer',
      'referred': 'Referred',
      'shared': 'Shared'
    };
    return labels[ownership] || ownership;
  }

  getOwnershipClass(ownership: string): string {
    return `customer-list__badge--${ownership}`;
  }

  getStatusClass(status: string): string {
    return `customer-list__status--${status}`;
  }
}
