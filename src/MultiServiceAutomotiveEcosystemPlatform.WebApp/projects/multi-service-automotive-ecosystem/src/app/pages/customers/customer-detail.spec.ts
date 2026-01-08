import { describe, it, expect, beforeEach } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerDetail } from './customer-detail';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('CustomerDetail', () => {
  let component: CustomerDetail;
  let fixture: ComponentFixture<CustomerDetail>;

  beforeEach(async () => {
    const mockActivatedRoute = {
      snapshot: {
        params: { id: '1' }
      }
    };

    await TestBed.configureTestingModule({
      imports: [CustomerDetail, RouterModule.forRoot([])],
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerDetail);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set customer ID from route params', () => {
    component.ngOnInit();
    expect(component.customerId).toBe('1');
  });

  it('should load customer data on init', (done) => {
    component.ngOnInit();
    component.customer$.subscribe(customer => {
      if (customer) {
        expect(customer).toBeDefined();
        expect(customer.id).toBe('1');
        expect(customer.firstName).toBeDefined();
        expect(customer.lastName).toBeDefined();
        done();
      }
    });
  });

  it('should load activities on init', (done) => {
    component.ngOnInit();
    component.activities$.subscribe(activities => {
      expect(activities).toBeDefined();
      expect(Array.isArray(activities)).toBe(true);
      done();
    });
  });

  it('should load vehicles on init', (done) => {
    component.ngOnInit();
    component.vehicles$.subscribe(vehicles => {
      expect(vehicles).toBeDefined();
      expect(Array.isArray(vehicles)).toBe(true);
      done();
    });
  });

  it('should return correct activity icon', () => {
    expect(component.getActivityIcon('inquiry')).toBe('❓');
    expect(component.getActivityIcon('referral')).toBe('🔗');
    expect(component.getActivityIcon('service')).toBe('🔧');
    expect(component.getActivityIcon('communication')).toBe('💬');
    expect(component.getActivityIcon('status-change')).toBe('🚩');
    expect(component.getActivityIcon('unknown')).toBe('📋');
  });

  it('should return correct activity class', () => {
    expect(component.getActivityClass('inquiry')).toBe('customer-detail__activity-icon--inquiry');
    expect(component.getActivityClass('referral')).toBe('customer-detail__activity-icon--referral');
  });
});
