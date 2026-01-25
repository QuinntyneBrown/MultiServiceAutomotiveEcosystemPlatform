import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PageHeader } from 'multi-service-automotive-ecosystem-components';

interface Professional {
  id: string;
  slug: string;
  businessName: string;
  professionalName: string;
  businessType: string;
  specialties: string[];
  rating: number;
  reviewCount: number;
  verified: boolean;
  photoUrl: string;
}

@Component({
  selector: 'app-professional-directory',
  imports: [CommonModule, RouterLink, PageHeader],
  templateUrl: './professional-directory.html',
  styleUrl: './professional-directory.scss',
})
export class ProfessionalDirectory {
  // Observable pattern as per requirements
  professionals$: Observable<Professional[]> = this.loadProfessionals();

  private loadProfessionals(): Observable<Professional[]> {
    // Mock data - replace with actual API call
    const mockProfessionals: Professional[] = [
      {
        id: '1',
        slug: 'acme-auto-repair',
        businessName: 'Acme Auto Repair',
        professionalName: 'John Smith',
        businessType: 'Domestic Mechanic',
        specialties: ['General Repairs', 'Oil Changes', 'Brake Service'],
        rating: 4.8,
        reviewCount: 127,
        verified: true,
        photoUrl: '/assets/professional-placeholder.jpg'
      },
      {
        id: '2',
        slug: 'euro-motors-specialist',
        businessName: 'Euro Motors Specialist',
        professionalName: 'Maria Garcia',
        businessType: 'German Specialist Mechanic',
        specialties: ['BMW', 'Mercedes', 'Audi'],
        rating: 4.9,
        reviewCount: 89,
        verified: true,
        photoUrl: '/assets/professional-placeholder.jpg'
      },
      {
        id: '3',
        slug: 'precision-auto-body',
        businessName: 'Precision Auto Body',
        professionalName: 'Mike Johnson',
        businessType: 'Auto Body Professional',
        specialties: ['Collision Repair', 'Paint', 'Bodywork'],
        rating: 4.7,
        reviewCount: 156,
        verified: true,
        photoUrl: '/assets/professional-placeholder.jpg'
      }
    ];

    return of(mockProfessionals);
  }
}
