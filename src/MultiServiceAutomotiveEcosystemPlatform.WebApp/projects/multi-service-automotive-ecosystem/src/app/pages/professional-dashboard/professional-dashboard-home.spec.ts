import { describe, it, expect, beforeEach } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfessionalDashboardHome } from './professional-dashboard-home';
import { RouterModule } from '@angular/router';

describe('ProfessionalDashboardHome', () => {
  let component: ProfessionalDashboardHome;
  let fixture: ComponentFixture<ProfessionalDashboardHome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfessionalDashboardHome, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfessionalDashboardHome);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load stats on init', (done) => {
    component.stats$.subscribe(stats => {
      expect(stats).toBeDefined();
      expect(stats.totalCustomers).toBeGreaterThanOrEqual(0);
      expect(stats.newInquiries).toBeGreaterThanOrEqual(0);
      expect(stats.pendingReferrals).toBeGreaterThanOrEqual(0);
      expect(stats.monthlyReferrals).toBeGreaterThanOrEqual(0);
      done();
    });
  });

  it('should load activities on init', (done) => {
    component.activities$.subscribe(activities => {
      expect(activities).toBeDefined();
      expect(Array.isArray(activities)).toBe(true);
      done();
    });
  });

  it('should return correct activity icon', () => {
    expect(component.getActivityIcon('customer')).toBe('👤');
    expect(component.getActivityIcon('inquiry')).toBe('❓');
    expect(component.getActivityIcon('referral')).toBe('🔗');
    expect(component.getActivityIcon('message')).toBe('💬');
    expect(component.getActivityIcon('unknown')).toBe('📋');
  });

  it('should return correct activity class', () => {
    expect(component.getActivityClass('customer')).toBe('professional-dashboard-home__activity-icon--customer');
    expect(component.getActivityClass('inquiry')).toBe('professional-dashboard-home__activity-icon--inquiry');
  });

  it('should have a professional name', () => {
    expect(component.professionalName).toBeDefined();
    expect(component.professionalName.length).toBeGreaterThan(0);
  });

  it('should have a profile completion percentage', () => {
    expect(component.profileCompletionPercentage).toBeGreaterThanOrEqual(0);
    expect(component.profileCompletionPercentage).toBeLessThanOrEqual(100);
  });
});
