import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

interface ProfessionalProfile {
  id: string;
  slug: string;
  businessName: string;
  professionalName: string;
  businessType: string;
  bio: string;
  specialties: string[];
  yearsInBusiness: number;
  serviceArea: string;
  rating: number;
  reviewCount: number;
  verified: boolean;
  photoUrl: string;
  coverPhotoUrl: string;
  phone: string;
  email: string;
}

@Component({
  selector: 'app-professional-profile',
  imports: [CommonModule],
  templateUrl: './professional-profile.html',
  styleUrl: './professional-profile.scss',
})
export class ProfessionalProfile {
  // Observable pattern as per requirements
  profile$: Observable<ProfessionalProfile>;

  constructor(private route: ActivatedRoute) {
    this.profile$ = this.route.params.pipe(
      switchMap(params => this.loadProfile(params['slug']))
    );
  }

  private loadProfile(slug: string): Observable<ProfessionalProfile> {
    // Mock data - replace with actual API call
    const mockProfile: ProfessionalProfile = {
      id: '1',
      slug,
      businessName: 'Acme Auto Repair',
      professionalName: 'John Smith',
      businessType: 'Domestic Mechanic',
      bio: 'With over 15 years of experience in automotive repair, we pride ourselves on delivering quality service and customer satisfaction.',
      specialties: ['General Repairs', 'Oil Changes', 'Brake Service', 'Engine Diagnostics'],
      yearsInBusiness: 15,
      serviceArea: 'Greater Metro Area',
      rating: 4.8,
      reviewCount: 127,
      verified: true,
      photoUrl: '/assets/professional-placeholder.jpg',
      coverPhotoUrl: '/assets/cover-placeholder.jpg',
      phone: '(555) 123-4567',
      email: 'contact@acmeautorepair.com'
    };

    return of(mockProfile);
  }
}
