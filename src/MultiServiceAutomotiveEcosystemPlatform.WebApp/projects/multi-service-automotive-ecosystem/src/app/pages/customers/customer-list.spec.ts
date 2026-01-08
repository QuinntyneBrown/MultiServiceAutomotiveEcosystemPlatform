import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerList } from './customer-list';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { of } from 'rxjs';

describe('CustomerList', () => {
  let component: CustomerList;
  let fixture: ComponentFixture<CustomerList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerList, RouterModule.forRoot([]), FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerList);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load customers on init', (done) => {
    component.customers$.subscribe(customers => {
      expect(customers).toBeDefined();
      expect(Array.isArray(customers)).toBe(true);
      expect(customers.length).toBeGreaterThan(0);
      done();
    });
  });

  it('should return correct ownership label', () => {
    expect(component.getOwnershipLabel('my-customer')).toBe('My Customer');
    expect(component.getOwnershipLabel('referred')).toBe('Referred');
    expect(component.getOwnershipLabel('shared')).toBe('Shared');
  });

  it('should return correct ownership class', () => {
    expect(component.getOwnershipClass('my-customer')).toBe('customer-list__badge--my-customer');
    expect(component.getOwnershipClass('referred')).toBe('customer-list__badge--referred');
  });

  it('should return correct status class', () => {
    expect(component.getStatusClass('active')).toBe('customer-list__status--active');
    expect(component.getStatusClass('inactive')).toBe('customer-list__status--inactive');
  });

  it('should initialize with default filter values', () => {
    expect(component.searchQuery).toBe('');
    expect(component.selectedOwnership).toBe('all');
    expect(component.selectedStatus).toBe('all');
    expect(component.sortField).toBe('lastName');
    expect(component.sortDirection).toBe('asc');
  });
});
