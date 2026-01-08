import { describe, it, expect, beforeEach } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerDetail } from './customer-detail';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { firstValueFrom, filter } from 'rxjs';

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

  it('should load customer data on init', async () => {
    component.ngOnInit();

    const customer = await firstValueFrom(
      component.customer$.pipe(filter((c): c is NonNullable<typeof c> => c != null))
    );

    expect(customer).toBeDefined();
    expect(customer.id).toBe('1');
    expect(customer.firstName).toBeDefined();
    expect(customer.lastName).toBeDefined();
  });

  it('should load activities on init', async () => {
    component.ngOnInit();
    const activities = await firstValueFrom(component.activities$);
    expect(activities).toBeDefined();
    expect(Array.isArray(activities)).toBe(true);
  });

  it('should load vehicles on init', async () => {
    component.ngOnInit();
    const vehicles = await firstValueFrom(component.vehicles$);
    expect(vehicles).toBeDefined();
    expect(Array.isArray(vehicles)).toBe(true);
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
