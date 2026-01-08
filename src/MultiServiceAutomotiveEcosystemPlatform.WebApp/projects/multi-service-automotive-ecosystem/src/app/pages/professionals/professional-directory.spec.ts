import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfessionalDirectory } from './professional-directory';
import { RouterTestingModule } from '@angular/router/testing';

describe('ProfessionalDirectory', () => {
  let component: ProfessionalDirectory;
  let fixture: ComponentFixture<ProfessionalDirectory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfessionalDirectory, RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfessionalDirectory);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have professionals$ observable', () => {
    expect(component.professionals$).toBeTruthy();
  });

  it('should load professionals data', (done) => {
    component.professionals$.subscribe(professionals => {
      expect(professionals).toBeTruthy();
      expect(professionals.length).toBeGreaterThan(0);
      done();
    });
  });

  it('should display page title', () => {
    const compiled = fixture.nativeElement;
    const title = compiled.querySelector('.professional-directory__title');
    expect(title).toBeTruthy();
    expect(title.textContent).toContain('Find Automotive Professionals');
  });

  it('should display professional cards after data loads', (done) => {
    component.professionals$.subscribe(() => {
      fixture.detectChanges();
      const cards = fixture.nativeElement.querySelectorAll('.professional-card');
      expect(cards.length).toBeGreaterThan(0);
      done();
    });
  });

  it('should display business name in cards', (done) => {
    component.professionals$.subscribe(() => {
      fixture.detectChanges();
      const businessName = fixture.nativeElement.querySelector('.professional-card__business-name');
      expect(businessName).toBeTruthy();
      done();
    });
  });

  it('should display professional type badge', (done) => {
    component.professionals$.subscribe(() => {
      fixture.detectChanges();
      const typeBadge = fixture.nativeElement.querySelector('.professional-card__type');
      expect(typeBadge).toBeTruthy();
      done();
    });
  });

  it('should display rating information', (done) => {
    component.professionals$.subscribe(() => {
      fixture.detectChanges();
      const rating = fixture.nativeElement.querySelector('.professional-card__rating');
      expect(rating).toBeTruthy();
      done();
    });
  });

  it('should have View Profile links', (done) => {
    component.professionals$.subscribe(() => {
      fixture.detectChanges();
      const links = fixture.nativeElement.querySelectorAll('.professional-card__action');
      expect(links.length).toBeGreaterThan(0);
      done();
    });
  });
});
