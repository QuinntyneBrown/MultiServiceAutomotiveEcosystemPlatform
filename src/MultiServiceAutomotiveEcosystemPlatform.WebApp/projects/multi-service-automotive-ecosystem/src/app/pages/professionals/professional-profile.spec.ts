import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
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

  it('should load profile data based on route params', (done) => {
    component.profile$.subscribe(profile => {
      expect(profile).toBeTruthy();
      expect(profile.slug).toBe('test-professional');
      done();
    });
  });

  it('should display business name after data loads', (done) => {
    component.profile$.subscribe(() => {
      fixture.detectChanges();
      const businessName = fixture.nativeElement.querySelector('.professional-profile__business-name');
      expect(businessName).toBeTruthy();
      done();
    });
  });

  it('should display bio section', (done) => {
    component.profile$.subscribe(() => {
      fixture.detectChanges();
      const bio = fixture.nativeElement.querySelector('.professional-profile__bio');
      expect(bio).toBeTruthy();
      done();
    });
  });

  it('should display specialties', (done) => {
    component.profile$.subscribe(() => {
      fixture.detectChanges();
      const specialties = fixture.nativeElement.querySelectorAll('.professional-profile__specialty');
      expect(specialties.length).toBeGreaterThan(0);
      done();
    });
  });

  it('should display contact information', (done) => {
    component.profile$.subscribe(() => {
      fixture.detectChanges();
      const contact = fixture.nativeElement.querySelector('.professional-profile__contact');
      expect(contact).toBeTruthy();
      done();
    });
  });

  it('should have action buttons', (done) => {
    component.profile$.subscribe(() => {
      fixture.detectChanges();
      const buttons = fixture.nativeElement.querySelectorAll('.professional-profile__action-btn');
      expect(buttons.length).toBeGreaterThan(0);
      done();
    });
  });

  it('should display verified badge for verified professionals', (done) => {
    component.profile$.subscribe(profile => {
      if (profile.verified) {
        fixture.detectChanges();
        const badge = fixture.nativeElement.querySelector('.professional-profile__badge');
        expect(badge).toBeTruthy();
      }
      done();
    });
  });
});
