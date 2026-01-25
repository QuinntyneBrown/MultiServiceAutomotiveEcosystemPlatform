import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReferralDashboard } from './referral-dashboard';
import { firstValueFrom } from 'rxjs';

describe('ReferralDashboard', () => {
  let component: ReferralDashboard;
  let fixture: ComponentFixture<ReferralDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReferralDashboard]
    }).compileComponents();

    fixture = TestBed.createComponent(ReferralDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have stats$ observable', () => {
    expect(component.stats$).toBeTruthy();
  });

  it('should have referrals$ observable', () => {
    expect(component.referrals$).toBeTruthy();
  });

  it('should have a referral code', () => {
    expect(component.referralCode).toBeTruthy();
    expect(typeof component.referralCode).toBe('string');
  });

  it('should load statistics data', async () => {
    const stats = await firstValueFrom(component.stats$);
    expect(stats).toBeTruthy();
    expect(stats.totalReferrals).toBeDefined();
    expect(stats.successfulConversions).toBeDefined();
    expect(stats.pendingReferrals).toBeDefined();
  });

  it('should load referrals data', async () => {
    const referrals = await firstValueFrom(component.referrals$);
    expect(referrals).toBeTruthy();
    expect(Array.isArray(referrals)).toBe(true);
  });

  it('should display referral code', () => {
    const compiled = fixture.nativeElement;
    const codeDisplay = compiled.querySelector('.referral-dashboard__code');
    expect(codeDisplay).toBeTruthy();
    expect(codeDisplay.textContent).toBe(component.referralCode);
  });

  it('should have copy button', () => {
    const compiled = fixture.nativeElement;
    const copyBtn = compiled.querySelector('.referral-dashboard__copy-btn');
    expect(copyBtn).toBeTruthy();
  });

  it('should display statistics cards after data loads', async () => {
    await firstValueFrom(component.stats$);
    fixture.detectChanges();
    const statCards = fixture.nativeElement.querySelectorAll('.referral-dashboard__stat-card');
    expect(statCards.length).toBeGreaterThan(0);
  });

  it('should display referral table', () => {
    const compiled = fixture.nativeElement;
    const table = compiled.querySelector('.referral-dashboard__table');
    expect(table).toBeTruthy();
  });

  it('should call copyReferralCode when copy button is clicked', () => {
    vi.spyOn(component, 'copyReferralCode');
    const copyBtn = fixture.nativeElement.querySelector('.referral-dashboard__copy-btn');
    copyBtn.click();
    expect(component.copyReferralCode).toHaveBeenCalled();
  });

  it('should return correct status class', () => {
    expect(component.getStatusClass('pending')).toBe('referral-dashboard__status--pending');
    expect(component.getStatusClass('converted')).toBe('referral-dashboard__status--converted');
    expect(component.getStatusClass('rewarded')).toBe('referral-dashboard__status--rewarded');
    expect(component.getStatusClass('expired')).toBe('referral-dashboard__status--expired');
  });

  it('should return correct status label', () => {
    expect(component.getStatusLabel('pending')).toBe('Pending');
    expect(component.getStatusLabel('converted')).toBe('Converted');
    expect(component.getStatusLabel('rewarded')).toBe('Rewarded');
    expect(component.getStatusLabel('expired')).toBe('Expired');
  });

  it('should display referral rows after data loads', async () => {
    await firstValueFrom(component.referrals$);
    fixture.detectChanges();
    const rows = fixture.nativeElement.querySelectorAll('.referral-dashboard__table-row');
    expect(rows.length).toBeGreaterThan(0);
  });
});
