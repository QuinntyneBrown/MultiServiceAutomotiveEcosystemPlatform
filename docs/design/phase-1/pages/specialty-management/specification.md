# Specialty Management Page Specification

## Overview

**Requirement ID:** REQ-SP-F007
**Phase:** 1
**Priority:** P0

The Specialty Management page enables service professionals to manage their automotive specialties, certifications, and expertise areas. Professionals can add specialties from a curated catalog or create custom ones, upload certification documents, set years of experience, and organize the display order through drag-and-drop reordering.

---

## Page Purpose

- Browse and select specialties from categorized catalog
- Add custom specialties not in the catalog
- Set years of experience for each specialty
- Upload and manage certification documents
- Reorder specialties via drag-and-drop
- Remove unwanted specialties
- Display certification status for each specialty

---

## Component Architecture

```
SpecialtyManagementPage/
├── specialty-management.component.ts       # Main page component
├── specialty-management.component.html     # Template
├── specialty-management.component.scss     # Styles
├── components/
│   ├── specialty-list/
│   │   ├── specialty-list.component.ts
│   │   ├── specialty-list.component.html
│   │   └── specialty-list.component.scss
│   ├── specialty-item/
│   │   ├── specialty-item.component.ts
│   │   ├── specialty-item.component.html
│   │   └── specialty-item.component.scss
│   ├── add-specialty-modal/
│   │   ├── add-specialty-modal.component.ts
│   │   ├── add-specialty-modal.component.html
│   │   └── add-specialty-modal.component.scss
│   ├── specialty-catalog/
│   │   ├── specialty-catalog.component.ts
│   │   ├── specialty-catalog.component.html
│   │   └── specialty-catalog.component.scss
│   └── certification-upload/
│       ├── certification-upload.component.ts
│       ├── certification-upload.component.html
│       └── certification-upload.component.scss
├── models/
│   ├── specialty.model.ts
│   ├── certification.model.ts
│   └── specialty-category.model.ts
├── services/
│   ├── specialty.service.ts
│   └── certification.service.ts
└── index.ts                                # Barrel export
```

---

## Data Models

### Specialty Model

```typescript
export interface Specialty {
  id: string;
  professionalId: string;
  name: string;
  description?: string;
  category: SpecialtyCategory;
  yearsOfExperience: number;
  isCertified: boolean;
  certificationName?: string;
  certificationStatus: CertificationStatus;
  certifications: Certification[];
  displayOrder: number;
  isCustom: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export enum SpecialtyCategory {
  Engine = 'engine',
  Transmission = 'transmission',
  Electrical = 'electrical',
  BodyWork = 'body_work',
  Brakes = 'brakes',
  Suspension = 'suspension',
  HVAC = 'hvac',
  Diagnostics = 'diagnostics',
  Tires = 'tires',
  Maintenance = 'maintenance',
  Custom = 'custom'
}

export enum CertificationStatus {
  None = 'none',
  Pending = 'pending',
  Verified = 'verified',
  Expired = 'expired'
}
```

### Certification Model

```typescript
export interface Certification {
  id: string;
  specialtyId: string;
  name: string;
  issuingOrganization: string;
  issueDate: Date;
  expirationDate?: Date;
  certificateNumber?: string;
  documentUrl?: string;
  documentName?: string;
  documentSize?: number;
  status: CertificationStatus;
  createdAt: Date;
  updatedAt: Date;
}
```

### Specialty Catalog Item Model

```typescript
export interface SpecialtyCatalogItem {
  id: string;
  name: string;
  description: string;
  category: SpecialtyCategory;
  isPopular: boolean;
  requiresCertification: boolean;
  suggestedCertifications?: string[];
}
```

---

## Component Structure

### Main Component: SpecialtyManagementComponent

```typescript
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-specialty-management',
  templateUrl: './specialty-management.component.html',
  styleUrls: ['./specialty-management.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    DragDropModule,
    SpecialtyListComponent,
    AddSpecialtyModalComponent
  ]
})
export class SpecialtyManagementComponent implements OnInit {
  private readonly specialtyService = inject(SpecialtyService);
  private readonly toastService = inject(ToastService);

  specialties: Specialty[] = [];
  isLoading = false;
  showAddModal = false;
  selectedSpecialtyForEdit: Specialty | null = null;

  ngOnInit(): void {
    this.loadSpecialties();
  }

  loadSpecialties(): void {
    this.isLoading = true;
    this.specialtyService.getMySpecialties()
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe({
        next: (specialties) => {
          this.specialties = specialties.sort((a, b) => a.displayOrder - b.displayOrder);
        },
        error: (error) => {
          this.toastService.error('Failed to load specialties');
          console.error('Error loading specialties:', error);
        }
      });
  }

  onDrop(event: CdkDragDrop<Specialty[]>): void {
    moveItemInArray(this.specialties, event.previousIndex, event.currentIndex);
    this.updateDisplayOrder();
  }

  updateDisplayOrder(): void {
    const updates = this.specialties.map((specialty, index) => ({
      id: specialty.id,
      displayOrder: index
    }));

    this.specialtyService.updateDisplayOrder(updates)
      .subscribe({
        next: () => {
          this.toastService.success('Specialty order updated');
        },
        error: (error) => {
          this.toastService.error('Failed to update order');
          console.error('Error updating display order:', error);
          this.loadSpecialties(); // Reload to reset order
        }
      });
  }

  openAddModal(): void {
    this.showAddModal = true;
  }

  closeAddModal(): void {
    this.showAddModal = false;
    this.selectedSpecialtyForEdit = null;
  }

  onSpecialtyAdded(specialty: Specialty): void {
    this.specialties.push(specialty);
    this.closeAddModal();
    this.toastService.success('Specialty added successfully');
  }

  onSpecialtyUpdated(updatedSpecialty: Specialty): void {
    const index = this.specialties.findIndex(s => s.id === updatedSpecialty.id);
    if (index !== -1) {
      this.specialties[index] = updatedSpecialty;
    }
    this.toastService.success('Specialty updated successfully');
  }

  onSpecialtyRemoved(specialtyId: string): void {
    const specialty = this.specialties.find(s => s.id === specialtyId);
    if (!specialty) return;

    const confirmed = confirm(
      `Are you sure you want to remove "${specialty.name}" from your specialties?`
    );

    if (confirmed) {
      this.specialtyService.deleteSpecialty(specialtyId)
        .subscribe({
          next: () => {
            this.specialties = this.specialties.filter(s => s.id !== specialtyId);
            this.toastService.success('Specialty removed successfully');
          },
          error: (error) => {
            this.toastService.error('Failed to remove specialty');
            console.error('Error removing specialty:', error);
          }
        });
    }
  }

  editSpecialty(specialty: Specialty): void {
    this.selectedSpecialtyForEdit = specialty;
    this.showAddModal = true;
  }
}
```

---

## Child Components

### SpecialtyItemComponent

Displays a single specialty with drag handle, details, and actions.

```typescript
@Component({
  selector: 'app-specialty-item',
  templateUrl: './specialty-item.component.html',
  styleUrls: ['./specialty-item.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class SpecialtyItemComponent implements OnInit {
  @Input() specialty!: Specialty;
  @Output() updated = new EventEmitter<Specialty>();
  @Output() removed = new EventEmitter<string>();
  @Output() uploadCertification = new EventEmitter<string>();

  private readonly specialtyService = inject(SpecialtyService);
  private readonly toastService = inject(ToastService);

  yearsControl = new FormControl(0, [
    Validators.required,
    Validators.min(0),
    Validators.max(50)
  ]);

  private debounceTimer?: number;

  ngOnInit(): void {
    this.yearsControl.setValue(this.specialty.yearsOfExperience);

    this.yearsControl.valueChanges
      .pipe(debounceTime(500))
      .subscribe(years => {
        if (this.yearsControl.valid && years !== null) {
          this.updateYearsOfExperience(years);
        }
      });
  }

  updateYearsOfExperience(years: number): void {
    this.specialtyService.updateSpecialty(this.specialty.id, {
      yearsOfExperience: years
    }).subscribe({
      next: (updated) => {
        this.updated.emit(updated);
      },
      error: (error) => {
        this.toastService.error('Failed to update years of experience');
        this.yearsControl.setValue(this.specialty.yearsOfExperience);
      }
    });
  }

  onRemove(): void {
    this.removed.emit(this.specialty.id);
  }

  onUploadCertification(): void {
    this.uploadCertification.emit(this.specialty.id);
  }

  getCertificationStatusIcon(): string {
    switch (this.specialty.certificationStatus) {
      case CertificationStatus.Verified:
        return 'check_circle';
      case CertificationStatus.Pending:
        return 'schedule';
      case CertificationStatus.Expired:
        return 'error';
      default:
        return 'info';
    }
  }

  getCertificationStatusColor(): string {
    switch (this.specialty.certificationStatus) {
      case CertificationStatus.Verified:
        return 'success';
      case CertificationStatus.Pending:
        return 'warning';
      case CertificationStatus.Expired:
        return 'error';
      default:
        return 'secondary';
    }
  }
}
```

### AddSpecialtyModalComponent

Modal for browsing catalog and adding specialties.

```typescript
@Component({
  selector: 'app-add-specialty-modal',
  templateUrl: './add-specialty-modal.component.html',
  styleUrls: ['./add-specialty-modal.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SpecialtyCatalogComponent
  ]
})
export class AddSpecialtyModalComponent implements OnInit {
  @Input() isOpen = false;
  @Input() editingSpecialty: Specialty | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() specialtyAdded = new EventEmitter<Specialty>();

  private readonly specialtyService = inject(SpecialtyService);
  private readonly toastService = inject(ToastService);

  selectedTab: 'catalog' | 'custom' = 'catalog';
  searchControl = new FormControl('');
  catalogItems: SpecialtyCatalogItem[] = [];
  filteredCatalogItems: SpecialtyCatalogItem[] = [];

  customSpecialtyForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(100)]),
    description: new FormControl('', [Validators.maxLength(500)]),
    category: new FormControl<SpecialtyCategory>(SpecialtyCategory.Custom, [Validators.required]),
    yearsOfExperience: new FormControl(0, [Validators.min(0), Validators.max(50)])
  });

  isSubmitting = false;

  ngOnInit(): void {
    this.loadCatalog();

    this.searchControl.valueChanges
      .pipe(debounceTime(300))
      .subscribe(searchTerm => {
        this.filterCatalog(searchTerm || '');
      });

    if (this.editingSpecialty) {
      this.populateFormForEdit();
    }
  }

  loadCatalog(): void {
    this.specialtyService.getSpecialtyCatalog()
      .subscribe({
        next: (items) => {
          this.catalogItems = items;
          this.filteredCatalogItems = items;
        },
        error: (error) => {
          this.toastService.error('Failed to load specialty catalog');
          console.error('Error loading catalog:', error);
        }
      });
  }

  filterCatalog(searchTerm: string): void {
    const term = searchTerm.toLowerCase();
    this.filteredCatalogItems = this.catalogItems.filter(item =>
      item.name.toLowerCase().includes(term) ||
      item.description.toLowerCase().includes(term)
    );
  }

  filterByCategory(category: SpecialtyCategory | 'all'): void {
    if (category === 'all') {
      this.filteredCatalogItems = this.catalogItems;
    } else {
      this.filteredCatalogItems = this.catalogItems.filter(
        item => item.category === category
      );
    }
  }

  switchTab(tab: 'catalog' | 'custom'): void {
    this.selectedTab = tab;
  }

  addFromCatalog(catalogItem: SpecialtyCatalogItem): void {
    if (this.isSubmitting) return;

    this.isSubmitting = true;
    this.specialtyService.addSpecialty({
      name: catalogItem.name,
      description: catalogItem.description,
      category: catalogItem.category,
      yearsOfExperience: 0,
      isCustom: false
    }).subscribe({
      next: (specialty) => {
        this.specialtyAdded.emit(specialty);
        this.isSubmitting = false;
        this.onClose();
      },
      error: (error) => {
        this.toastService.error('Failed to add specialty');
        console.error('Error adding specialty:', error);
        this.isSubmitting = false;
      }
    });
  }

  addCustomSpecialty(): void {
    if (this.customSpecialtyForm.invalid || this.isSubmitting) return;

    this.isSubmitting = true;
    const formValue = this.customSpecialtyForm.value;

    this.specialtyService.addSpecialty({
      name: formValue.name!,
      description: formValue.description || undefined,
      category: formValue.category!,
      yearsOfExperience: formValue.yearsOfExperience || 0,
      isCustom: true
    }).subscribe({
      next: (specialty) => {
        this.specialtyAdded.emit(specialty);
        this.isSubmitting = false;
        this.onClose();
      },
      error: (error) => {
        this.toastService.error('Failed to add custom specialty');
        console.error('Error adding custom specialty:', error);
        this.isSubmitting = false;
      }
    });
  }

  populateFormForEdit(): void {
    if (!this.editingSpecialty) return;

    this.customSpecialtyForm.patchValue({
      name: this.editingSpecialty.name,
      description: this.editingSpecialty.description || '',
      category: this.editingSpecialty.category,
      yearsOfExperience: this.editingSpecialty.yearsOfExperience
    });

    this.selectedTab = 'custom';
  }

  onClose(): void {
    this.customSpecialtyForm.reset();
    this.searchControl.reset();
    this.close.emit();
  }
}
```

### CertificationUploadComponent

Handles certification document uploads.

```typescript
@Component({
  selector: 'app-certification-upload',
  templateUrl: './certification-upload.component.html',
  styleUrls: ['./certification-upload.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class CertificationUploadComponent {
  @Input() specialtyId!: string;
  @Output() uploaded = new EventEmitter<Certification>();
  @Output() close = new EventEmitter<void>();

  private readonly certificationService = inject(CertificationService);
  private readonly toastService = inject(ToastService);

  certificationForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    issuingOrganization: new FormControl('', [Validators.required]),
    issueDate: new FormControl('', [Validators.required]),
    expirationDate: new FormControl(''),
    certificateNumber: new FormControl('')
  });

  selectedFile: File | null = null;
  uploadProgress = 0;
  isUploading = false;

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];

      // Validate file type
      const allowedTypes = ['application/pdf', 'image/jpeg', 'image/png', 'image/jpg'];
      if (!allowedTypes.includes(file.type)) {
        this.toastService.error('Only PDF and image files are allowed');
        return;
      }

      // Validate file size (max 5MB)
      const maxSize = 5 * 1024 * 1024;
      if (file.size > maxSize) {
        this.toastService.error('File size must be less than 5MB');
        return;
      }

      this.selectedFile = file;
    }
  }

  onSubmit(): void {
    if (this.certificationForm.invalid || !this.selectedFile || this.isUploading) {
      return;
    }

    this.isUploading = true;
    const formValue = this.certificationForm.value;

    const certificationData = {
      specialtyId: this.specialtyId,
      name: formValue.name!,
      issuingOrganization: formValue.issuingOrganization!,
      issueDate: new Date(formValue.issueDate!),
      expirationDate: formValue.expirationDate ? new Date(formValue.expirationDate) : undefined,
      certificateNumber: formValue.certificateNumber || undefined
    };

    this.certificationService.uploadCertification(certificationData, this.selectedFile)
      .pipe(
        tap(event => {
          if (event.type === HttpEventType.UploadProgress && event.total) {
            this.uploadProgress = Math.round(100 * event.loaded / event.total);
          }
        }),
        filter(event => event.type === HttpEventType.Response),
        map(event => event.body as Certification)
      )
      .subscribe({
        next: (certification) => {
          this.uploaded.emit(certification);
          this.toastService.success('Certification uploaded successfully');
          this.isUploading = false;
          this.close.emit();
        },
        error: (error) => {
          this.toastService.error('Failed to upload certification');
          console.error('Error uploading certification:', error);
          this.isUploading = false;
        }
      });
  }

  onCancel(): void {
    this.close.emit();
  }
}
```

---

## Services

### SpecialtyService

```typescript
@Injectable({ providedIn: 'root' })
export class SpecialtyService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/v1/specialties';

  getMySpecialties(): Observable<Specialty[]> {
    return this.http.get<Specialty[]>(`${this.apiUrl}/my-specialties`);
  }

  getSpecialtyCatalog(): Observable<SpecialtyCatalogItem[]> {
    return this.http.get<SpecialtyCatalogItem[]>(`${this.apiUrl}/catalog`);
  }

  addSpecialty(data: Partial<Specialty>): Observable<Specialty> {
    return this.http.post<Specialty>(this.apiUrl, data);
  }

  updateSpecialty(id: string, data: Partial<Specialty>): Observable<Specialty> {
    return this.http.patch<Specialty>(`${this.apiUrl}/${id}`, data);
  }

  deleteSpecialty(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateDisplayOrder(updates: Array<{ id: string; displayOrder: number }>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/reorder`, { updates });
  }
}
```

### CertificationService

```typescript
@Injectable({ providedIn: 'root' })
export class CertificationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/v1/certifications';

  uploadCertification(
    data: Partial<Certification>,
    file: File
  ): Observable<HttpEvent<Certification>> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('data', JSON.stringify(data));

    return this.http.post<Certification>(this.apiUrl, formData, {
      reportProgress: true,
      observe: 'events'
    });
  }

  getCertification(id: string): Observable<Certification> {
    return this.http.get<Certification>(`${this.apiUrl}/${id}`);
  }

  getCertificationsForSpecialty(specialtyId: string): Observable<Certification[]> {
    return this.http.get<Certification[]>(`${this.apiUrl}/specialty/${specialtyId}`);
  }

  deleteCertification(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  downloadCertificate(id: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${id}/download`, {
      responseType: 'blob'
    });
  }
}
```

---

## API Integration

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/specialties/my-specialties` | Get current professional's specialties |
| GET | `/api/v1/specialties/catalog` | Get specialty catalog items |
| POST | `/api/v1/specialties` | Add new specialty |
| PATCH | `/api/v1/specialties/:id` | Update specialty |
| DELETE | `/api/v1/specialties/:id` | Delete specialty |
| PUT | `/api/v1/specialties/reorder` | Update display order |
| POST | `/api/v1/certifications` | Upload certification |
| GET | `/api/v1/certifications/:id` | Get certification details |
| GET | `/api/v1/certifications/specialty/:specialtyId` | Get certifications for specialty |
| DELETE | `/api/v1/certifications/:id` | Delete certification |
| GET | `/api/v1/certifications/:id/download` | Download certificate file |

---

## User Interactions

### 1. Add Specialty from Catalog

**User Action:** Click "Add Specialty" button
**Flow:**
1. Modal opens with specialty catalog
2. User can search or browse by category
3. User clicks add (+) button on desired specialty
4. Specialty is added to their list
5. Modal closes
6. Success toast displayed

### 2. Add Custom Specialty

**User Action:** Click "Add Specialty" → "Custom" tab
**Flow:**
1. User switches to Custom tab in modal
2. User fills in specialty name (required)
3. User optionally adds description
4. User selects category
5. User sets years of experience
6. User clicks "Add Specialty"
7. Custom specialty is created and added to list
8. Modal closes
9. Success toast displayed

### 3. Reorder Specialties

**User Action:** Drag specialty item by drag handle
**Flow:**
1. User clicks and holds drag handle icon
2. User drags item to new position
3. Other items shift to make space
4. User releases mouse
5. Order is saved to backend
6. Success toast displayed

### 4. Update Years of Experience

**User Action:** Change value in years input field
**Flow:**
1. User types new value in years field
2. System debounces input (500ms)
3. Value is validated (0-50)
4. Update is sent to backend
5. Specialty is updated in list

### 5. Upload Certification

**User Action:** Click "Upload Certificate" button
**Flow:**
1. Certification upload modal opens
2. User fills in certification details
3. User selects file (PDF or image)
4. User clicks upload
5. Progress bar shows upload status
6. File is uploaded to server
7. Certification is associated with specialty
8. Modal closes
9. Success toast displayed
10. Certification status updated in list

### 6. Remove Specialty

**User Action:** Click delete/remove button
**Flow:**
1. Confirmation dialog appears
2. User confirms deletion
3. Specialty is removed from backend
4. Specialty is removed from list
5. Success toast displayed

---

## Validation Rules

### Specialty Validation

- **Name:** Required, max 100 characters
- **Description:** Optional, max 500 characters
- **Years of Experience:** Required, integer, min 0, max 50
- **Category:** Required, must be valid SpecialtyCategory enum value

### Certification Validation

- **Name:** Required, max 100 characters
- **Issuing Organization:** Required, max 100 characters
- **Issue Date:** Required, must be valid date, cannot be in future
- **Expiration Date:** Optional, must be after issue date if provided
- **File Type:** Must be PDF, JPG, JPEG, or PNG
- **File Size:** Maximum 5MB

---

## Error Handling

### Network Errors

- Display user-friendly toast message
- Log detailed error to console
- Maintain UI state (don't clear forms)
- Provide retry option where appropriate

### Validation Errors

- Show inline error messages on form fields
- Highlight invalid fields with error color
- Prevent form submission until valid
- Provide helpful error text

### Upload Errors

- Show error toast with specific message
- Reset upload progress
- Allow user to retry upload
- Validate file before upload attempt

---

## Accessibility

### Keyboard Navigation

- All interactive elements are keyboard accessible
- Tab order follows logical flow
- Modal can be closed with Escape key
- Drag-drop has keyboard alternative (arrow keys + modifier)

### Screen Reader Support

- All form fields have associated labels
- Error messages are announced
- Drag handle has aria-label "Reorder specialty"
- Modal has proper ARIA attributes
- Success/error toasts are announced

### Focus Management

- Focus moves to modal when opened
- Focus trapped within modal while open
- Focus returns to trigger element on close
- Focus visible on all interactive elements

---

## Performance Considerations

### Optimization Strategies

1. **Lazy Loading:** Modal components loaded on-demand
2. **Debouncing:** Search and years input debounced
3. **Virtual Scrolling:** Catalog list uses virtual scroll for large datasets
4. **Change Detection:** OnPush strategy for list items
5. **File Upload:** Chunked uploads for large files
6. **Caching:** Catalog data cached for session

### Loading States

- Show skeleton loaders while fetching specialties
- Display upload progress bar
- Disable buttons during async operations
- Show empty state when no specialties

---

## State Management

### Local Component State

- Specialties list
- Modal open/closed
- Selected tab
- Search filter
- Upload progress
- Loading states

### Backend Sync

- Optimistic UI updates where appropriate
- Rollback on error
- Refresh data after mutations
- Debounced auto-save for years field

---

## Testing Requirements

### Unit Tests

- Component initialization
- Form validation
- Service methods
- Error handling
- User interactions

### Integration Tests

- Add specialty from catalog
- Create custom specialty
- Reorder specialties
- Upload certification
- Remove specialty

### E2E Tests

- Complete add specialty flow
- Complete certification upload flow
- Drag-and-drop reordering
- Search and filter functionality

---

## Future Enhancements

### Phase 2

- Bulk specialty import
- Specialty recommendations based on profile
- Certification expiration notifications
- Public specialty verification badges

### Phase 3

- AI-powered specialty suggestions
- Video certification uploads
- Integration with third-party certification providers
- Specialty endorsements from customers

---

## Design Tokens Used

```scss
// Colors
--color-primary: #00529F
--color-accent: #FFD520
--color-success: #2E7D32
--color-warning: #ED6C02
--color-error: #D32F2F

// Spacing
--spacing-2: 8px
--spacing-3: 12px
--spacing-4: 16px
--spacing-6: 24px
--spacing-8: 32px

// Typography
--font-family-primary: 'Sharp Sans', 'Montserrat', sans-serif
--font-family-secondary: 'Lato', 'Roboto', sans-serif
--text-h2: 1.75rem
--text-h4: 1.25rem
--text-body-md: 1rem

// Border Radius
--radius-md: 8px
--radius-lg: 12px

// Shadows
--shadow-sm: 0 1px 3px rgba(0, 0, 0, 0.1)
--shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1)
--shadow-xl: 0 20px 25px rgba(0, 0, 0, 0.1)
--shadow-focus: 0 0 0 3px rgba(0, 82, 159, 0.4)
```

---

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile Safari (iOS 14+)
- Chrome Mobile (Android 10+)

---

## Dependencies

```json
{
  "dependencies": {
    "@angular/core": "^17.0.0",
    "@angular/common": "^17.0.0",
    "@angular/forms": "^17.0.0",
    "@angular/cdk": "^17.0.0",
    "rxjs": "^7.8.0"
  }
}
```

---

## Routing

```typescript
{
  path: 'specialties',
  component: SpecialtyManagementComponent,
  canActivate: [AuthGuard, ProfessionalGuard],
  data: {
    title: 'Manage Specialties',
    breadcrumb: 'Specialties'
  }
}
```
