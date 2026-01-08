import { describe, it, expect, beforeEach } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of, firstValueFrom } from 'rxjs';
import { ProfessionalProfile } from './professional-profile';

describe('ProfessionalProfile', () => {
  let component: ProfessionalProfile;
  let fixture: ComponentFixture<ProfessionalProfile>;
  const mockActivatedRoute = {
    params: of({ slug: 'test-professional' })
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfessionalProfile],
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfessionalProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have profile$ observable', () => {
    expect(component.profile$).toBeTruthy();
  });

  it('should load profile data based on route params', async () => {
    const profile = await firstValueFrom(component.profile$);
    expect(profile).toBeTruthy();
    expect(profile.slug).toBe('test-professional');
  });

  it('should display business name after data loads', async () => {
    await firstValueFrom(component.profile$);
    fixture.detectChanges();
    const businessName = fixture.nativeElement.querySelector('.professional-profile__business-name');
    expect(businessName).toBeTruthy();
  });

  it('should display bio section', async () => {
    await firstValueFrom(component.profile$);
    fixture.detectChanges();
    const bio = fixture.nativeElement.querySelector('.professional-profile__bio');
    expect(bio).toBeTruthy();
  });

  it('should display specialties', async () => {
    await firstValueFrom(component.profile$);
    fixture.detectChanges();
    const specialties = fixture.nativeElement.querySelectorAll('.professional-profile__specialty');
    expect(specialties.length).toBeGreaterThan(0);
  });

  it('should display contact information', async () => {
    await firstValueFrom(component.profile$);
    fixture.detectChanges();
    const contact = fixture.nativeElement.querySelector('.professional-profile__contact');
    expect(contact).toBeTruthy();
  });

  it('should have action buttons', async () => {
    await firstValueFrom(component.profile$);
    fixture.detectChanges();
    const buttons = fixture.nativeElement.querySelectorAll('.professional-profile__action-btn');
    expect(buttons.length).toBeGreaterThan(0);
  });

  it('should display verified badge for verified professionals', async () => {
    const profile = await firstValueFrom(component.profile$);
    if (profile.verified) {
      fixture.detectChanges();
      const badge = fixture.nativeElement.querySelector('.professional-profile__badge');
      expect(badge).toBeTruthy();
    }
  });
});
