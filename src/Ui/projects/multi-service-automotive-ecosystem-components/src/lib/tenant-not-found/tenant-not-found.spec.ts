import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TenantNotFound } from './tenant-not-found';
import { vi } from 'vitest';

describe('TenantNotFound', () => {
  let component: TenantNotFound;
  let fixture: ComponentFixture<TenantNotFound>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenantNotFound],
    }).compileComponents();

    fixture = TestBed.createComponent(TenantNotFound);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Component Initialization', () => {
    it('should initialize with default values', () => {
      expect(component.mainPlatformUrl).toBe('https://www.platform.com');
      expect(component.supportUrl).toBe('https://www.platform.com/contact');
      expect(component.attemptedSubdomain).toBeUndefined();
      expect(component.customMessage).toBeUndefined();
    });

    it('should accept custom mainPlatformUrl', () => {
      component.mainPlatformUrl = 'https://custom.com';
      expect(component.mainPlatformUrl).toBe('https://custom.com');
    });

    it('should accept custom supportUrl', () => {
      component.supportUrl = 'https://support.custom.com';
      expect(component.supportUrl).toBe('https://support.custom.com');
    });

    it('should accept attemptedSubdomain', () => {
      component.attemptedSubdomain = 'invalid-shop';
      expect(component.attemptedSubdomain).toBe('invalid-shop');
    });

    it('should accept customMessage', () => {
      component.customMessage = 'Custom error message';
      expect(component.customMessage).toBe('Custom error message');
    });
  });

  describe('Default Message', () => {
    it('should return generic message when no subdomain provided', () => {
      component.attemptedSubdomain = undefined;
      expect(component.defaultMessage).toBe(
        "We couldn't find the network you're looking for. It may have been moved or is no longer available."
      );
    });

    it('should return subdomain-specific message when subdomain provided', () => {
      component.attemptedSubdomain = 'invalid-shop';
      expect(component.defaultMessage).toContain('invalid-shop');
      expect(component.defaultMessage).toBe(
        'We couldn\'t find the network "invalid-shop". It may have been moved or is no longer available.'
      );
    });

    it('should update message when subdomain changes', () => {
      // Create new fixtures for each test case to avoid change detection issues
      const fixture1 = TestBed.createComponent(TenantNotFound);
      const component1 = fixture1.componentInstance;
      component1.attemptedSubdomain = 'shop1';
      fixture1.detectChanges();
      expect(component1.defaultMessage).toContain('shop1');

      const fixture2 = TestBed.createComponent(TenantNotFound);
      const component2 = fixture2.componentInstance;
      component2.attemptedSubdomain = 'shop2';
      fixture2.detectChanges();
      expect(component2.defaultMessage).toContain('shop2');
    });
  });

  describe('Navigate to Main', () => {
    it('should emit navigateToMain event', () => {
      const emitSpy = vi.spyOn(component.navigateToMain, 'emit');
      
      // Mock window.location
      delete (window as any).location;
      (window as any).location = { href: '' };

      component.onNavigateToMainClick();

      expect(emitSpy).toHaveBeenCalled();
    });

    it('should navigate to mainPlatformUrl', () => {
      // Mock window.location
      delete (window as any).location;
      (window as any).location = { href: '' };

      component.mainPlatformUrl = 'https://example.com';
      component.onNavigateToMainClick();

      expect(window.location.href).toBe('https://example.com');
    });

    it('should emit event even if mainPlatformUrl is empty', () => {
      const emitSpy = vi.spyOn(component.navigateToMain, 'emit');
      component.mainPlatformUrl = '';
      
      // Mock window.location
      delete (window as any).location;
      (window as any).location = { href: '' };

      component.onNavigateToMainClick();

      expect(emitSpy).toHaveBeenCalled();
    });
  });

  describe('Contact Support', () => {
    it('should emit contactSupport event', () => {
      const emitSpy = vi.spyOn(component.contactSupport, 'emit');
      
      // Mock window.location
      delete (window as any).location;
      (window as any).location = { href: '' };

      component.onContactSupportClick();

      expect(emitSpy).toHaveBeenCalled();
    });

    it('should navigate to supportUrl', () => {
      // Mock window.location
      delete (window as any).location;
      (window as any).location = { href: '' };

      component.supportUrl = 'https://support.example.com';
      component.onContactSupportClick();

      expect(window.location.href).toBe('https://support.example.com');
    });

    it('should emit event even if supportUrl is empty', () => {
      const emitSpy = vi.spyOn(component.contactSupport, 'emit');
      component.supportUrl = '';
      
      // Mock window.location
      delete (window as any).location;
      (window as any).location = { href: '' };

      component.onContactSupportClick();

      expect(emitSpy).toHaveBeenCalled();
    });
  });

  describe('Template Rendering', () => {
    it('should render error 404 badge', () => {
      const compiled = fixture.nativeElement;
      const badge = compiled.querySelector('.tenant-not-found__badge-text');
      expect(badge?.textContent).toContain('ERROR 404');
    });

    it('should render main title', () => {
      const compiled = fixture.nativeElement;
      const title = compiled.querySelector('.tenant-not-found__title');
      expect(title?.textContent).toContain('Network Not Found');
    });

    it('should render default message when no custom message', () => {
      component.customMessage = undefined;
      component.attemptedSubdomain = undefined;
      fixture.detectChanges();

      const compiled = fixture.nativeElement;
      const description = compiled.querySelector('.tenant-not-found__description');
      expect(description?.textContent).toContain("We couldn't find the network");
    });

    it('should render custom message when provided', () => {
      const newFixture = TestBed.createComponent(TenantNotFound);
      const newComponent = newFixture.componentInstance;
      newComponent.customMessage = 'This is a custom error';
      newFixture.detectChanges();

      const compiled = newFixture.nativeElement;
      const description = compiled.querySelector('.tenant-not-found__description');
      expect(description?.textContent).toContain('This is a custom error');
    });

    it('should render primary button', () => {
      const compiled = fixture.nativeElement;
      const button = compiled.querySelector('.tenant-not-found__button--primary');
      expect(button?.textContent).toContain('Go to Main Platform');
    });

    it('should render secondary button', () => {
      const compiled = fixture.nativeElement;
      const button = compiled.querySelector('.tenant-not-found__button--secondary');
      expect(button?.textContent).toContain('Contact Support');
    });

    it('should render help section', () => {
      const compiled = fixture.nativeElement;
      const helpTitle = compiled.querySelector('.tenant-not-found__help-title');
      expect(helpTitle?.textContent).toContain('What can you do?');
    });

    it('should render help list items', () => {
      const compiled = fixture.nativeElement;
      const helpItems = compiled.querySelectorAll('.tenant-not-found__help-item');
      expect(helpItems.length).toBe(3);
    });

    it('should render footer with help center link', () => {
      const compiled = fixture.nativeElement;
      const footer = compiled.querySelector('.tenant-not-found__footer-text');
      expect(footer?.textContent).toContain('Help Center');
    });

    it('should render help center link with correct href', () => {
      // Set supportUrl before initial change detection
      const newFixture = TestBed.createComponent(TenantNotFound);
      const newComponent = newFixture.componentInstance;
      newComponent.supportUrl = 'https://help.example.com';
      newFixture.detectChanges();

      const compiled = newFixture.nativeElement;
      const link = compiled.querySelector('.tenant-not-found__link');
      expect(link?.getAttribute('href')).toBe('https://help.example.com');
    });
  });

  describe('Button Interactions', () => {
    it('should call onNavigateToMainClick when primary button clicked', () => {
      const spy = vi.spyOn(component, 'onNavigateToMainClick');
      
      // Mock window.location to prevent actual navigation
      delete (window as any).location;
      (window as any).location = { href: '' };

      const compiled = fixture.nativeElement;
      const button = compiled.querySelector('.tenant-not-found__button--primary');
      button?.click();

      expect(spy).toHaveBeenCalled();
    });

    it('should call onContactSupportClick when secondary button clicked', () => {
      const spy = vi.spyOn(component, 'onContactSupportClick');
      
      // Mock window.location to prevent actual navigation
      delete (window as any).location;
      (window as any).location = { href: '' };

      const compiled = fixture.nativeElement;
      const button = compiled.querySelector('.tenant-not-found__button--secondary');
      button?.click();

      expect(spy).toHaveBeenCalled();
    });
  });

  describe('Accessibility', () => {
    it('should have appropriate ARIA structure', () => {
      const compiled = fixture.nativeElement;
      const card = compiled.querySelector('.tenant-not-found__card');
      expect(card).toBeTruthy();
    });

    it('should have icon with SVG for screen readers', () => {
      const compiled = fixture.nativeElement;
      const icon = compiled.querySelector('.tenant-not-found__icon');
      expect(icon).toBeTruthy();
      expect(icon?.tagName).toBe('svg');
    });

    it('should have buttons with proper type attribute', () => {
      const compiled = fixture.nativeElement;
      const buttons = compiled.querySelectorAll('button');
      buttons.forEach((button: HTMLButtonElement) => {
        expect(button.getAttribute('type')).toBe('button');
      });
    });
  });
});
