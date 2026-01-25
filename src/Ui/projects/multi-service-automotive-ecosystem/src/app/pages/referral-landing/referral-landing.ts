import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Observable, of } from 'rxjs';

interface ReferrerInfo {
  name: string;
  discountPercent: number;
  professionalName?: string;
  professionalType?: string;
}

/**
 * Referral Landing Page (REQ-RF-F015)
 * Public landing page for referral links
 */
@Component({
  selector: 'app-referral-landing',
  imports: [CommonModule, RouterModule],
  templateUrl: './referral-landing.html',
  styleUrl: './referral-landing.scss',
})
export class ReferralLanding implements OnInit {
  referrerInfo$: Observable<ReferrerInfo | null> = of(null);
  referralCode = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    // Get referral code from route parameter
    this.referralCode = this.route.snapshot.paramMap.get('code') || '';
    
    if (this.referralCode) {
      this.referrerInfo$ = this.loadReferrerInfo(this.referralCode);
    }
  }

  private loadReferrerInfo(code: string): Observable<ReferrerInfo> {
    // Mock data - replace with API call
    const mockInfo: ReferrerInfo = {
      name: 'John Doe',
      discountPercent: 15,
      professionalName: 'Mike\'s Auto Repair',
      professionalType: 'Auto Repair Shop'
    };
    return of(mockInfo);
  }

  signUp() {
    // Navigate to signup with referral code pre-filled
    this.router.navigate(['/customer/register'], {
      queryParams: { ref: this.referralCode }
    });
  }
}
