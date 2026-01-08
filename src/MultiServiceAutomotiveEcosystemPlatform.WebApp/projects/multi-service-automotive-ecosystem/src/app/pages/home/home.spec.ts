import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Home } from './home';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Home, RouterTestingModule]
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the main title', () => {
    const compiled = fixture.nativeElement;
    const title = compiled.querySelector('.page-header__title');
    expect(title).toBeTruthy();
    expect(title.textContent).toContain('Welcome');
  });

  it('should display the subtitle', () => {
    const compiled = fixture.nativeElement;
    const subtitle = compiled.querySelector('.page-header__subtitle');
    expect(subtitle).toBeTruthy();
    expect(subtitle.textContent).toContain('Connect with trusted automotive professionals');
  });

  it('should have action buttons', () => {
    const compiled = fixture.nativeElement;
    const buttons = compiled.querySelectorAll('.action-button');
    expect(buttons.length).toBeGreaterThan(0);
  });

  it('should display features section', () => {
    const compiled = fixture.nativeElement;
    const featuresSection = compiled.querySelector('.home__features');
    expect(featuresSection).toBeTruthy();
  });

  it('should display three feature cards', () => {
    const compiled = fixture.nativeElement;
    const featureCards = compiled.querySelectorAll('.home__feature');
    expect(featureCards.length).toBe(3);
  });

  it('should have Find Professionals link', () => {
    const compiled = fixture.nativeElement;
    const link = compiled.querySelector('a[routerLink="/professionals"]');
    expect(link).toBeTruthy();
  });

  it('should have Get Started link', () => {
    const compiled = fixture.nativeElement;
    const link = compiled.querySelector('a[routerLink="/customer/register"]');
    expect(link).toBeTruthy();
  });
});
