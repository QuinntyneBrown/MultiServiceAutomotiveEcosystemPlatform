import { describe, it, expect, beforeEach } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfessionalDirectory } from './professional-directory';
import { RouterTestingModule } from '@angular/router/testing';
import { firstValueFrom } from 'rxjs';

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

  it('should load professionals data', async () => {
    const professionals = await firstValueFrom(component.professionals$);
    expect(professionals).toBeTruthy();
    expect(professionals.length).toBeGreaterThan(0);
  });

  it('should display page title', () => {
    const compiled = fixture.nativeElement;
    const title = compiled.querySelector('.page-header__title');
    expect(title).toBeTruthy();
    expect(title.textContent).toContain('Find Automotive Professionals');
  });

  it('should display professional cards after data loads', async () => {
    await firstValueFrom(component.professionals$);
    fixture.detectChanges();
    const cards = fixture.nativeElement.querySelectorAll('.professional-card');
    expect(cards.length).toBeGreaterThan(0);
  });

  it('should display business name in cards', async () => {
    await firstValueFrom(component.professionals$);
    fixture.detectChanges();
    const businessName = fixture.nativeElement.querySelector('.professional-card__business-name');
    expect(businessName).toBeTruthy();
  });

  it('should display professional type badge', async () => {
    await firstValueFrom(component.professionals$);
    fixture.detectChanges();
    const typeBadge = fixture.nativeElement.querySelector('.professional-card__type');
    expect(typeBadge).toBeTruthy();
  });

  it('should display rating information', async () => {
    await firstValueFrom(component.professionals$);
    fixture.detectChanges();
    const rating = fixture.nativeElement.querySelector('.professional-card__rating');
    expect(rating).toBeTruthy();
  });

  it('should have View Profile links', async () => {
    await firstValueFrom(component.professionals$);
    fixture.detectChanges();
    const links = fixture.nativeElement.querySelectorAll('.professional-card__action');
    expect(links.length).toBeGreaterThan(0);
  });
});
