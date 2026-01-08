import { TestBed } from '@angular/core/testing';
import { TenantContextService } from './tenant-context.service';

describe('TenantContextService', () => {
  let service: TenantContextService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TenantContextService]
    });
    service = TestBed.inject(TenantContextService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have tenant$ observable', () => {
    expect(service.tenant$).toBeTruthy();
  });

  it('should have isLoading$ observable', () => {
    expect(service.isLoading$).toBeTruthy();
  });

  it('should have error$ observable', () => {
    expect(service.error$).toBeTruthy();
  });

  it('should load tenant data on initialization', (done) => {
    service.tenant$.subscribe(tenant => {
      if (tenant) {
        expect(tenant.tenantId).toBeDefined();
        expect(tenant.name).toBeDefined();
        expect(tenant.subdomain).toBeDefined();
        expect(tenant.config).toBeDefined();
        expect(tenant.branding).toBeDefined();
        done();
      }
    });
  });

  it('should have tenant configuration with features', (done) => {
    service.tenant$.subscribe(tenant => {
      if (tenant) {
        expect(tenant.config.features).toBeDefined();
        expect(tenant.config.features.loyaltyProgram).toBeDefined();
        expect(tenant.config.features.newsletters).toBeDefined();
        expect(tenant.config.features.professionalReferrals).toBeDefined();
        expect(tenant.config.features.customerPortal).toBeDefined();
        done();
      }
    });
  });

  it('should have tenant branding information', (done) => {
    service.tenant$.subscribe(tenant => {
      if (tenant) {
        expect(tenant.branding.primaryColor).toBeDefined();
        expect(tenant.branding.secondaryColor).toBeDefined();
        expect(tenant.branding.logoUrl).toBeDefined();
        expect(tenant.branding.faviconUrl).toBeDefined();
        done();
      }
    });
  });

  it('should return tenant from getTenant method', (done) => {
    setTimeout(() => {
      const tenant = service.getTenant();
      expect(tenant).toBeTruthy();
      done();
    }, 100);
  });

  it('should check feature flags correctly', (done) => {
    setTimeout(() => {
      const isEnabled = service.isFeatureEnabled('loyaltyProgram');
      expect(typeof isEnabled).toBe('boolean');
      done();
    }, 100);
  });

  it('should set isLoading to false after loading', (done) => {
    setTimeout(() => {
      service.isLoading$.subscribe(isLoading => {
        expect(isLoading).toBe(false);
        done();
      });
    }, 100);
  });
});
