import { describe, it, expect, beforeEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { TenantContextService } from './tenant-context.service';
import { firstValueFrom, filter } from 'rxjs';

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

  it('should load tenant data on initialization', async () => {
    const tenant = await firstValueFrom(service.tenant$.pipe(filter((t): t is NonNullable<typeof t> => t != null)));
    expect(tenant.tenantId).toBeDefined();
    expect(tenant.name).toBeDefined();
    expect(tenant.subdomain).toBeDefined();
    expect(tenant.config).toBeDefined();
    expect(tenant.branding).toBeDefined();
  });

  it('should have tenant configuration with features', async () => {
    const tenant = await firstValueFrom(service.tenant$.pipe(filter((t): t is NonNullable<typeof t> => t != null)));
    expect(tenant.config.features).toBeDefined();
    expect(tenant.config.features.loyaltyProgram).toBeDefined();
    expect(tenant.config.features.newsletters).toBeDefined();
    expect(tenant.config.features.professionalReferrals).toBeDefined();
    expect(tenant.config.features.customerPortal).toBeDefined();
  });

  it('should have tenant branding information', async () => {
    const tenant = await firstValueFrom(service.tenant$.pipe(filter((t): t is NonNullable<typeof t> => t != null)));
    expect(tenant.branding.primaryColor).toBeDefined();
    expect(tenant.branding.secondaryColor).toBeDefined();
    expect(tenant.branding.logoUrl).toBeDefined();
    expect(tenant.branding.faviconUrl).toBeDefined();
  });

  it('should return tenant from getTenant method', async () => {
    await firstValueFrom(service.tenant$.pipe(filter((t): t is NonNullable<typeof t> => t != null)));
    const tenant = service.getTenant();
    expect(tenant).toBeTruthy();
  });

  it('should check feature flags correctly', async () => {
    await firstValueFrom(service.tenant$.pipe(filter((t): t is NonNullable<typeof t> => t != null)));
    const isEnabled = service.isFeatureEnabled('loyaltyProgram');
    expect(typeof isEnabled).toBe('boolean');
  });

  it('should set isLoading to false after loading', async () => {
    const isLoading = await firstValueFrom(service.isLoading$.pipe(filter(v => v === false)));
    expect(isLoading).toBe(false);
  });
});
