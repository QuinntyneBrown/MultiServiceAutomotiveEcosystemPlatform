import { Component, Input, Output, EventEmitter, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';

export interface Specialty {
  id: string;
  name: string;
  icon?: string;
  yearsOfExperience: number;
  order: number;
  isCustom: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export enum SpecialtyCategory {
  ENGINE = 'engine',
  TRANSMISSION = 'transmission',
  ELECTRICAL = 'electrical',
  BODY = 'body',
  HVAC = 'hvac',
  SUSPENSION = 'suspension',
  BRAKES = 'brakes',
  EXHAUST = 'exhaust',
  DIAGNOSTIC = 'diagnostic',
  MAINTENANCE = 'maintenance',
  SPECIALTY_VEHICLES = 'specialty_vehicles',
  OTHER = 'other'
}

export interface SpecialtyCatalogItem {
  id: string;
  name: string;
  description: string;
  icon: string;
  category: SpecialtyCategory;
  keywords: string[];
  isPopular?: boolean;
}

export interface Certification {
  id: string;
  name: string;
  fileName: string;
  fileUrl: string;
  fileSize: number;
  mimeType: string;
  uploadedAt: Date;
  expirationDate?: Date | null;
  isVerified?: boolean;
}

@Component({
  selector: 'ms-specialty-management',
  imports: [CommonModule, ReactiveFormsModule, DragDropModule],
  templateUrl: './specialty-management.html',
  styleUrl: './specialty-management.scss',
})
export class SpecialtyManagement {
  @Input() currentSpecialties: Specialty[] = [];
  @Input() availableSpecialties: SpecialtyCatalogItem[] = [];
  @Input() certifications: Certification[] = [];
  @Input() maxSpecialties = 10;
  @Input() maxCertifications = 20;
  @Input() allowCustomSpecialties = true;
  @Input() isLoading = false;
  @Input() error: string | null = null;

  @Output() addSpecialties = new EventEmitter<Specialty[]>();
  @Output() removeSpecialty = new EventEmitter<string>();
  @Output() reorderSpecialties = new EventEmitter<Specialty[]>();
  @Output() addCertification = new EventEmitter<File>();
  @Output() removeCertification = new EventEmitter<string>();

  isModalOpen = signal(false);
  searchQuery = signal('');
  selectedSpecialtyIds = signal<Set<string>>(new Set());
  customSpecialtyForm: FormGroup;
  uploadingFile = signal<string | null>(null);
  uploadProgress = signal(0);
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);
  confirmRemoveId = signal<string | null>(null);
  isDragOver = signal(false);

  filteredSpecialties = computed(() => {
    const query = this.searchQuery().toLowerCase();
    if (!query) {
      return this.availableSpecialties;
    }
    return this.availableSpecialties.filter(s =>
      s.name.toLowerCase().includes(query) ||
      s.description.toLowerCase().includes(query) ||
      s.keywords.some(k => k.toLowerCase().includes(query))
    );
  });

  canAddMore = computed(() => this.currentSpecialties.length < this.maxSpecialties);
  canUploadMore = computed(() => this.certifications.length < this.maxCertifications);

  constructor(private fb: FormBuilder) {
    this.customSpecialtyForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      yearsOfExperience: [0, [Validators.required, Validators.min(0), Validators.max(99)]]
    });
  }

  openModal(): void {
    if (!this.canAddMore()) return;
    this.isModalOpen.set(true);
    this.searchQuery.set('');
    this.selectedSpecialtyIds.set(new Set());
    this.customSpecialtyForm.reset({ name: '', yearsOfExperience: 0 });
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.selectedSpecialtyIds.set(new Set());
  }

  onSearchChange(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchQuery.set(value);
  }

  clearSearch(): void {
    this.searchQuery.set('');
  }

  toggleSpecialtySelection(id: string): void {
    const current = this.selectedSpecialtyIds();
    const newSet = new Set(current);
    if (newSet.has(id)) {
      newSet.delete(id);
    } else {
      // Check if already in current specialties
      if (this.currentSpecialties.some(s => s.id === id)) {
        this.showError('You already have this specialty');
        return;
      }
      // Check max limit
      const totalAfterAdd = this.currentSpecialties.length + newSet.size + 1;
      if (totalAfterAdd > this.maxSpecialties) {
        this.showError(`Maximum ${this.maxSpecialties} specialties allowed`);
        return;
      }
      newSet.add(id);
    }
    this.selectedSpecialtyIds.set(newSet);
  }

  isSelected(id: string): boolean {
    return this.selectedSpecialtyIds().has(id);
  }

  isAlreadyAdded(id: string): boolean {
    return this.currentSpecialties.some(s => s.id === id);
  }

  addSelectedSpecialties(): void {
    const selectedIds = this.selectedSpecialtyIds();
    const customName = this.customSpecialtyForm.get('name')?.value?.trim();
    const customYears = this.customSpecialtyForm.get('yearsOfExperience')?.value || 0;

    const newSpecialties: Specialty[] = [];
    const now = new Date();
    let order = this.currentSpecialties.length;

    // Add catalog selections
    selectedIds.forEach(id => {
      const catalogItem = this.availableSpecialties.find(s => s.id === id);
      if (catalogItem) {
        newSpecialties.push({
          id: `specialty-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
          name: catalogItem.name,
          icon: catalogItem.icon,
          yearsOfExperience: 0, // Could add experience selection per item
          order: order++,
          isCustom: false,
          createdAt: now,
          updatedAt: now
        });
      }
    });

    // Add custom specialty if entered
    if (customName && this.customSpecialtyForm.valid) {
      // Check for duplicate
      if (this.currentSpecialties.some(s => s.name.toLowerCase() === customName.toLowerCase())) {
        this.showError('A specialty with this name already exists');
        return;
      }
      newSpecialties.push({
        id: `specialty-${Date.now()}-custom-${Math.random().toString(36).substr(2, 9)}`,
        name: customName,
        yearsOfExperience: customYears,
        order: order++,
        isCustom: true,
        createdAt: now,
        updatedAt: now
      });
    }

    if (newSpecialties.length === 0) {
      this.showError('Please select at least one specialty or enter a custom specialty');
      return;
    }

    this.addSpecialties.emit(newSpecialties);
    this.showSuccess(`${newSpecialties.length} specialty(ies) added`);
    this.closeModal();
  }

  onDrop(event: CdkDragDrop<Specialty[]>): void {
    if (event.previousIndex === event.currentIndex) return;

    const reordered = [...this.currentSpecialties];
    moveItemInArray(reordered, event.previousIndex, event.currentIndex);

    // Update order property
    reordered.forEach((specialty, index) => {
      specialty.order = index;
    });

    this.reorderSpecialties.emit(reordered);
  }

  confirmRemove(id: string): void {
    this.confirmRemoveId.set(id);
  }

  cancelRemove(): void {
    this.confirmRemoveId.set(null);
  }

  onRemoveSpecialty(id: string): void {
    this.removeSpecialty.emit(id);
    this.confirmRemoveId.set(null);
    this.showSuccess('Specialty removed');
  }

  getSpecialtyName(id: string): string {
    return this.currentSpecialties.find(s => s.id === id)?.name || '';
  }

  // Certification upload methods
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver.set(true);
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver.set(false);
  }

  onFileDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragOver.set(false);

    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleFileUpload(files[0]);
    }
  }

  onFileSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.handleFileUpload(input.files[0]);
      input.value = ''; // Reset for same file selection
    }
  }

  private handleFileUpload(file: File): void {
    // Validate file count
    if (!this.canUploadMore()) {
      this.showError(`Maximum ${this.maxCertifications} certifications allowed`);
      return;
    }

    // Validate file type
    const validTypes = ['application/pdf', 'image/jpeg', 'image/jpg', 'image/png'];
    if (!validTypes.includes(file.type)) {
      this.showError('Only PDF, JPG, and PNG files are supported');
      return;
    }

    // Validate file size (5MB)
    const maxSize = 5 * 1024 * 1024;
    if (file.size > maxSize) {
      this.showError('File size exceeds 5MB limit');
      return;
    }

    // Simulate upload progress
    this.uploadingFile.set(file.name);
    this.uploadProgress.set(0);

    const interval = setInterval(() => {
      const current = this.uploadProgress();
      if (current >= 100) {
        clearInterval(interval);
        this.uploadingFile.set(null);
        this.uploadProgress.set(0);
        this.addCertification.emit(file);
        this.showSuccess('Certification uploaded');
      } else {
        this.uploadProgress.set(current + 20);
      }
    }, 200);
  }

  confirmRemoveCertification(id: string): void {
    // For simplicity, directly emit. In production, show confirmation dialog.
    this.removeCertification.emit(id);
    this.showSuccess('Certification removed');
  }

  formatFileSize(bytes: number): string {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(1) + ' MB';
  }

  private showSuccess(message: string): void {
    this.successMessage.set(message);
    setTimeout(() => this.successMessage.set(null), 3000);
  }

  private showError(message: string): void {
    this.errorMessage.set(message);
    setTimeout(() => this.errorMessage.set(null), 5000);
  }
}
