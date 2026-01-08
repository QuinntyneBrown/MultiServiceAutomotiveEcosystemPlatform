import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Observable, of } from 'rxjs';

interface Specialty {
  id: string;
  name: string;
  category: string;
  isCustom: boolean;
}

interface ProfessionalSpecialty {
  id: string;
  specialtyId: string;
  specialty: Specialty;
  experienceYears: number;
  displayOrder: number;
}

@Component({
  selector: 'app-specialty-management',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './specialty-management.html',
  styleUrl: './specialty-management.scss',
})
export class SpecialtyManagement {
  mySpecialties$: Observable<ProfessionalSpecialty[]> = this.loadMySpecialties();
  availableSpecialties$: Observable<Specialty[]> = this.loadAvailableSpecialties();
  
  showAddModal = false;
  searchQuery = '';
  selectedCategory = 'all';

  private loadMySpecialties(): Observable<ProfessionalSpecialty[]> {
    // Mock data - replace with actual API call
    const mockSpecialties: ProfessionalSpecialty[] = [
      {
        id: '1',
        specialtyId: 'brake',
        specialty: {
          id: 'brake',
          name: 'Brake Service',
          category: 'General Repair',
          isCustom: false
        },
        experienceYears: 15,
        displayOrder: 1
      },
      {
        id: '2',
        specialtyId: 'engine',
        specialty: {
          id: 'engine',
          name: 'Engine Diagnostics',
          category: 'General Repair',
          isCustom: false
        },
        experienceYears: 20,
        displayOrder: 2
      },
      {
        id: '3',
        specialtyId: 'transmission',
        specialty: {
          id: 'transmission',
          name: 'Transmission Repair',
          category: 'Specialized',
          isCustom: false
        },
        experienceYears: 12,
        displayOrder: 3
      }
    ];

    return of(mockSpecialties);
  }

  private loadAvailableSpecialties(): Observable<Specialty[]> {
    // Mock data - replace with actual API call
    const mockSpecialties: Specialty[] = [
      { id: 'oil', name: 'Oil Change', category: 'Maintenance', isCustom: false },
      { id: 'tire', name: 'Tire Service', category: 'Maintenance', isCustom: false },
      { id: 'ac', name: 'A/C Repair', category: 'Climate Control', isCustom: false },
      { id: 'electrical', name: 'Electrical Systems', category: 'Specialized', isCustom: false },
      { id: 'suspension', name: 'Suspension Work', category: 'Specialized', isCustom: false }
    ];

    return of(mockSpecialties);
  }

  openAddModal(): void {
    this.showAddModal = true;
  }

  closeAddModal(): void {
    this.showAddModal = false;
    this.searchQuery = '';
    this.selectedCategory = 'all';
  }

  addSpecialty(specialty: Specialty): void {
    console.log('Adding specialty:', specialty);
    // Mock API call - replace with actual API call
    alert(`Added: ${specialty.name}`);
    this.closeAddModal();
  }

  removeSpecialty(id: string): void {
    if (confirm('Are you sure you want to remove this specialty?')) {
      console.log('Removing specialty:', id);
      // Mock API call - replace with actual API call
      alert('Specialty removed');
    }
  }

  updateExperienceYears(id: string, years: number): void {
    console.log('Updating experience years:', id, years);
    // Mock API call - replace with actual API call
  }
}
