import { Injectable, signal, computed } from '@angular/core';
import { Observable, BehaviorSubject, of, throwError } from 'rxjs';
import { catchError, map, shareReplay } from 'rxjs/operators';

export interface Tenant {
  tenantId: string;
  name: string;
  subdomain: string;
  config: TenantConfig;
  branding: TenantBranding;
}

export interface TenantConfig {
  features: {
    loyaltyProgram: boolean;
    newsletters: boolean;
    professionalReferrals: boolean;
    customerPortal: boolean;
  };
}

export interface TenantBranding {
  primaryColor: string;
  secondaryColor: string;
  logoUrl: string;
  faviconUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class TenantContextService {
  private _tenant$ = new BehaviorSubject<Tenant | null>(null);
  private _isLoading$ = new BehaviorSubject<boolean>(true);
  private _error$ = new BehaviorSubject<Error | null>(null);

  tenant$ = this._tenant$.asObservable();
  isLoading$ = this._isLoading$.asObservable();
  error$ = this._error$.asObservable();

  constructor() {
    this.loadTenantFromSubdomain();
  }

  private loadTenantFromSubdomain(): void {
    const subdomain = this.getSubdomain();
    
    if (!subdomain) {
      this._error$.next(new Error('No subdomain found'));
      this._isLoading$.next(false);
      return;
    }

    // Simulate API call - replace with actual HTTP call
    this.fetchTenant(subdomain).subscribe({
      next: (tenant) => {
        this._tenant$.next(tenant);
        this._isLoading$.next(false);
      },
      error: (error) => {
        this._error$.next(error);
        this._isLoading$.next(false);
      }
    });
  }

  private getSubdomain(): string | null {
    const hostname = window.location.hostname;
    const parts = hostname.split('.');
    
    // For localhost development, return default
    if (hostname === 'localhost' || hostname === '127.0.0.1') {
      return 'default';
    }
    
    // For subdomain.domain.com, return subdomain
    if (parts.length >= 3) {
      return parts[0];
    }
    
    return null;
  }

  private fetchTenant(subdomain: string): Observable<Tenant> {
    // Mock tenant data - replace with actual API call
    const mockTenant: Tenant = {
      tenantId: '1',
      name: 'Demo Tenant',
      subdomain,
      config: {
        features: {
          loyaltyProgram: true,
          newsletters: true,
          professionalReferrals: true,
          customerPortal: true
        }
      },
      branding: {
        primaryColor: '#00529F',
        secondaryColor: '#FFD520',
        logoUrl: '/assets/logo.png',
        faviconUrl: '/assets/favicon.ico'
      }
    };

    return of(mockTenant);
  }

  getTenant(): Tenant | null {
    return this._tenant$.value;
  }

  isFeatureEnabled(feature: keyof TenantConfig['features']): boolean {
    const tenant = this._tenant$.value;
    return tenant?.config.features[feature] ?? false;
  }
}
