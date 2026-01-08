# Specialty Management Interface Component Specification

## Component Overview

**Component ID**: REQ-SP-F007
**Component Name**: Specialty Management Interface
**Phase**: Phase 1
**Priority**: P0
**Status**: Design Complete

### Description

The Specialty Management Interface allows service professionals to manage their automotive specialties and certifications. Professionals can add specialties from a predefined catalog, create custom specialties, reorder them via drag-and-drop, and upload certification documents.

### Business Requirements

- Enable professionals to showcase their areas of expertise
- Provide a standardized catalog of automotive specialties
- Allow customization for unique or niche specialties
- Support certification document uploads for verification
- Enable professionals to prioritize specialties through ordering

---

## Component Architecture

### Framework Support

This component is designed to be implemented in both:
- **React** (with TypeScript)
- **Angular** (standalone components)

---

## Props/Inputs

### React Props Interface

```typescript
interface SpecialtyManagementProps {
  // Data
  currentSpecialties: Specialty[];
  availableSpecialties: SpecialtyCatalogItem[];
  certifications: Certification[];

  // Callbacks
  onAddSpecialties: (specialties: Specialty[]) => Promise<void>;
  onRemoveSpecialty: (specialtyId: string) => Promise<void>;
  onReorderSpecialties: (reorderedSpecialties: Specialty[]) => Promise<void>;
  onAddCertification: (file: File) => Promise<void>;
  onRemoveCertification: (certificationId: string) => Promise<void>;

  // Configuration
  maxSpecialties?: number; // Default: 10
  maxCertifications?: number; // Default: 20
  allowCustomSpecialties?: boolean; // Default: true

  // State
  isLoading?: boolean;
  error?: string | null;

  // Styling
  className?: string;
  theme?: 'light' | 'dark';
}
```

### Angular Component Inputs

```typescript
@Component({
  selector: 'app-specialty-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DragDropModule, ...],
  templateUrl: './specialty-management.component.html',
  styleUrls: ['./specialty-management.component.scss']
})
export class SpecialtyManagementComponent {
  // Data Inputs
  @Input() currentSpecialties: Specialty[] = [];
  @Input() availableSpecialties: SpecialtyCatalogItem[] = [];
  @Input() certifications: Certification[] = [];

  // Configuration Inputs
  @Input() maxSpecialties: number = 10;
  @Input() maxCertifications: number = 20;
  @Input() allowCustomSpecialties: boolean = true;

  // State Inputs
  @Input() isLoading: boolean = false;
  @Input() error: string | null = null;

  // Event Outputs
  @Output() addSpecialties = new EventEmitter<Specialty[]>();
  @Output() removeSpecialty = new EventEmitter<string>();
  @Output() reorderSpecialties = new EventEmitter<Specialty[]>();
  @Output() addCertification = new EventEmitter<File>();
  @Output() removeCertification = new EventEmitter<string>();
}
```

---

## Data Models

### Specialty

```typescript
interface Specialty {
  id: string;
  name: string;
  icon?: string; // Emoji or icon identifier
  yearsOfExperience: number;
  order: number; // Display order (0-based)
  isCustom: boolean; // Whether this is a custom specialty
  createdAt: Date;
  updatedAt: Date;
}
```

### SpecialtyCatalogItem

```typescript
interface SpecialtyCatalogItem {
  id: string;
  name: string;
  description: string;
  icon: string; // Emoji or icon identifier
  category: SpecialtyCategory;
  keywords: string[]; // For search
  isPopular?: boolean; // Featured/recommended
}

enum SpecialtyCategory {
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
```

### Certification

```typescript
interface Certification {
  id: string;
  name: string;
  fileName: string;
  fileUrl: string;
  fileSize: number; // In bytes
  mimeType: string;
  uploadedAt: Date;
  expirationDate?: Date | null; // Optional for certifications that expire
  isVerified?: boolean; // Admin verification status
}
```

### CustomSpecialtyInput

```typescript
interface CustomSpecialtyInput {
  name: string;
  yearsOfExperience: number;
  description?: string;
}
```

---

## Component Features

### 1. Current Specialties List

**Purpose**: Display and manage existing specialties

**Features**:
- List view of current specialties
- Drag handles for reordering
- Visual feedback during drag operations
- Remove button for each specialty
- Display icon, name, and years of experience
- Empty state when no specialties exist

**Drag-and-Drop Behavior**:
- Items are draggable via drag handle (not entire item)
- Visual indicators:
  - Drag handle shows grab cursor
  - Dragged item becomes semi-transparent (50% opacity)
  - Dragged item has blue background (--color-blue-50)
  - Drop zones are highlighted
- Reordering updates immediately in UI
- `onReorderSpecialties` callback fires after drop completes
- Smooth animations during reordering

**Empty State**:
```
Icon: 🎯 (large, gray)
Title: "No Specialties Added Yet"
Description: "Add specialties to showcase your expertise to potential customers"
Action Button: "Add Your First Specialty"
```

### 2. Add Specialty Modal

**Purpose**: Browse catalog and add new specialties

**Modal Structure**:
- Header with title and close button
- Search box for filtering catalog
- Scrollable catalog of available specialties
- Custom specialty form section
- Footer with Cancel and Add buttons

**Search Functionality**:
- Real-time filtering as user types
- Searches across:
  - Specialty name
  - Description
  - Keywords
- Case-insensitive matching
- Clear search button when text present
- No results state with helpful message

**Catalog Browser**:
- Grid/list of specialty cards
- Each card shows:
  - Icon
  - Name
  - Brief description
  - Checkbox for selection
- Multi-select capability
- Visual feedback on selection (blue background, checkmark)
- Scrollable area with fixed height
- Categories can be shown as sections (Phase 2)

**Selection Behavior**:
- Click anywhere on card to toggle selection
- Checkbox shows checkmark when selected
- Selected items have blue background and border
- Can select multiple items
- Selection persists during search

### 3. Custom Specialty Form

**Purpose**: Allow professionals to add unique specialties not in catalog

**Form Fields**:

| Field | Type | Validation | Required |
|-------|------|------------|----------|
| Specialty Name | Text | Max 100 chars, alphanumeric with spaces | Yes |
| Years of Experience | Number | Min: 0, Max: 99, Integer only | Yes |
| Description | Textarea | Max 500 chars (Phase 2) | No |

**Validation Rules**:
- Specialty name cannot duplicate existing specialty
- Years of experience must be realistic (0-99)
- Show inline validation errors
- Disable Add button until valid

**Behavior**:
- Located below catalog in modal
- Separated by divider line
- Can add custom specialty independent of catalog selections
- Custom specialty is added to selection when Add button clicked

### 4. Certification Upload

**Purpose**: Upload and manage certification documents

**Upload Features**:
- Drag-and-drop upload area
- Click to browse file picker
- Multiple file selection support
- Supported formats: PDF, JPG, JPEG, PNG
- Maximum file size: 5MB per file
- Maximum 20 certifications total

**Upload Area States**:

**Default State**:
```
Icon: 📄 (large)
Primary Text: "Upload Certification Documents"
Secondary Text: "PDF, JPG, or PNG (max 5MB)"
Background: Light gray dashed border
```

**Hover State**:
```
Border: Solid blue
Background: Light blue tint
Cursor: Pointer
```

**Uploading State**:
```
Show progress bar
Display file name being uploaded
Show percentage complete
Disable additional uploads
```

**Certification Display**:
- Grid layout (responsive)
- Each certification card shows:
  - Success icon (green checkmark)
  - Certification name (editable)
  - File name
  - Remove button
- Cards have light gray background
- Remove button shows confirmation dialog

**Validation**:
- Check file size before upload
- Verify file type
- Show error message for invalid files
- Prevent upload if at maximum

---

## User Interactions

### Adding Specialties from Catalog

1. User clicks "Add Specialty" button
2. Modal opens with catalog view
3. User searches or browses specialties
4. User clicks specialty cards to select (multi-select)
5. Selected items show checkmark and blue styling
6. User clicks "Add Selected" button
7. Modal closes
8. New specialties appear in list
9. Success message shown (toast/snackbar)

**Edge Cases**:
- If user selects specialty already in their list, show warning
- If user reaches maximum specialties, disable selection
- If no specialties selected and no custom specialty entered, disable Add button

### Adding Custom Specialty

1. User clicks "Add Specialty" button
2. Modal opens
3. User scrolls to "Add Custom Specialty" section
4. User enters specialty name and years of experience
5. Form validates in real-time
6. User clicks "Add Selected" button
7. Custom specialty is added to list
8. Modal closes
9. Success message shown

### Reordering Specialties

1. User hovers over drag handle (cursor changes to grab)
2. User clicks and holds drag handle
3. Item becomes semi-transparent and highlighted
4. User drags item to new position
5. Drop zones between items are highlighted
6. User releases mouse
7. Item moves to new position with animation
8. Order is saved (callback fired)

**Keyboard Alternative** (Accessibility):
1. User tabs to specialty item
2. User presses Space or Enter to enter drag mode
3. User uses Arrow keys to move item
4. User presses Space or Enter to drop
5. User presses Escape to cancel

### Removing Specialty

1. User clicks remove button (X) on specialty item
2. Confirmation dialog appears: "Remove [Specialty Name]?"
3. User confirms or cancels
4. If confirmed, specialty is removed with fade-out animation
5. List reorders to fill gap
6. Success message shown

### Uploading Certification

1. User clicks upload area or drags file over it
2. File picker opens (or file is dropped)
3. User selects file(s)
4. Files are validated
5. Upload progress shown
6. On success, certification card appears
7. Success message shown

**Validation Failures**:
- File too large: "File exceeds 5MB limit"
- Wrong format: "Only PDF, JPG, and PNG files are supported"
- Too many files: "Maximum 20 certifications allowed"

---

## State Management

### Component State

```typescript
interface SpecialtyManagementState {
  // Modal
  isModalOpen: boolean;

  // Search
  searchQuery: string;
  filteredSpecialties: SpecialtyCatalogItem[];

  // Selection
  selectedSpecialties: Set<string>; // IDs of selected catalog items

  // Custom Specialty Form
  customSpecialtyForm: {
    name: string;
    yearsOfExperience: number;
    errors: {
      name?: string;
      yearsOfExperience?: string;
    };
  };

  // Drag and Drop
  draggedItemId: string | null;
  dragOverItemId: string | null;

  // Upload
  uploadingFiles: File[];
  uploadProgress: Map<string, number>; // fileName -> percentage

  // UI State
  isProcessing: boolean;
  error: string | null;
  successMessage: string | null;
}
```

### State Actions

```typescript
// Modal Actions
openModal(): void
closeModal(): void

// Search Actions
setSearchQuery(query: string): void
filterSpecialties(query: string): void

// Selection Actions
toggleSpecialtySelection(id: string): void
clearSelection(): void

// Form Actions
updateCustomSpecialtyForm(field: string, value: any): void
validateCustomSpecialty(): boolean
resetCustomSpecialtyForm(): void

// Drag and Drop Actions
handleDragStart(id: string): void
handleDragOver(id: string): void
handleDrop(sourceId: string, targetId: string): void
handleDragEnd(): void

// CRUD Actions
addSelectedSpecialties(): Promise<void>
removeSpecialty(id: string): Promise<void>
reorderSpecialties(newOrder: Specialty[]): Promise<void>

// Certification Actions
uploadCertification(file: File): Promise<void>
removeCertification(id: string): Promise<void>
```

---

## Accessibility Requirements

### WCAG 2.1 AA Compliance

**Keyboard Navigation**:
- Tab order follows logical flow: Add button → Specialty items → Remove buttons → Upload area
- Modal opens with focus on close button or search field
- Escape key closes modal
- Enter/Space on catalog items toggles selection
- Arrow keys navigate between specialty items
- Custom keyboard drag-drop for specialty reordering

**Screen Reader Support**:
- Proper ARIA labels on all interactive elements
- `role="dialog"` and `aria-modal="true"` on modal
- `aria-label` on drag handles: "Drag to reorder [Specialty Name]"
- `aria-label` on remove buttons: "Remove [Specialty Name]"
- `aria-live="polite"` for success/error messages
- `aria-describedby` for form field errors
- Announce drag and drop actions: "Moved [Specialty] from position X to position Y"

**Focus Management**:
- Focus trap within modal when open
- Return focus to "Add Specialty" button on modal close
- Visible focus indicators on all interactive elements
- Focus moved to next item when specialty removed

**Color Contrast**:
- All text meets 4.5:1 contrast ratio
- Icon-only buttons have text alternatives
- Don't rely on color alone for selection state

**Touch Targets**:
- Minimum 44x44px touch targets for all buttons
- Adequate spacing between interactive elements
- Drag handles large enough for finger interaction

**Motion & Animation**:
- Respect `prefers-reduced-motion` media query
- Disable drag animations if motion preference set
- Provide instant feedback without transitions

### ARIA Attributes

```html
<!-- Modal -->
<div
  role="dialog"
  aria-modal="true"
  aria-labelledby="modal-title"
>
  <h2 id="modal-title">Add Specialties</h2>
</div>

<!-- Search -->
<input
  type="search"
  aria-label="Search specialties"
  role="searchbox"
/>

<!-- Specialty Item -->
<li
  draggable="true"
  aria-label="Engine Repair, 12 years of experience, position 1 of 4"
>
  <button
    aria-label="Drag to reorder Engine Repair"
    class="drag-handle"
  >
    ☰
  </button>
  <button
    aria-label="Remove Engine Repair specialty"
    class="remove-btn"
  >
    ✕
  </button>
</li>

<!-- Upload Area -->
<div
  role="button"
  tabindex="0"
  aria-label="Upload certification documents. Supported formats: PDF, JPG, PNG. Maximum size: 5MB"
>
  Upload Area
</div>

<!-- Live Region for Feedback -->
<div
  role="status"
  aria-live="polite"
  aria-atomic="true"
  class="sr-only"
>
  {successMessage || error}
</div>
```

---

## Styling Guidelines

### Design System Tokens

```css
/* Colors */
--specialty-primary: var(--color-blue-700); /* #00529F */
--specialty-primary-hover: var(--color-blue-800); /* #003366 */
--specialty-primary-light: var(--color-blue-100); /* #E3F2FD */
--specialty-selected: var(--color-blue-50); /* #F5F9FF */

/* Spacing */
--specialty-item-padding: var(--spacing-4); /* 16px */
--specialty-item-gap: var(--spacing-3); /* 12px */
--specialty-list-gap: var(--spacing-3); /* 12px */

/* Borders */
--specialty-border-radius: var(--radius-md); /* 8px */
--specialty-card-radius: var(--radius-lg); /* 12px */

/* Shadows */
--specialty-card-shadow: var(--shadow-sm);
--specialty-modal-shadow: var(--shadow-xl);
```

### Component Classes

```css
/* Main Container */
.specialty-management {
  max-width: 900px;
  padding: var(--spacing-8);
  background: var(--color-bg-primary);
  border-radius: var(--specialty-card-radius);
}

/* Specialty List */
.specialty-list {
  display: flex;
  flex-direction: column;
  gap: var(--specialty-list-gap);
}

/* Specialty Item */
.specialty-item {
  display: flex;
  align-items: center;
  gap: var(--specialty-item-gap);
  padding: var(--specialty-item-padding);
  border: 1px solid var(--color-gray-200);
  border-radius: var(--specialty-border-radius);
  transition: all 200ms ease;
}

.specialty-item:hover {
  box-shadow: var(--specialty-card-shadow);
}

.specialty-item.dragging {
  opacity: 0.5;
  background-color: var(--specialty-selected);
  border-color: var(--specialty-primary);
}

/* Drag Handle */
.drag-handle {
  cursor: grab;
  color: var(--color-gray-400);
  padding: var(--spacing-2);
}

.drag-handle:active {
  cursor: grabbing;
}

/* Modal */
.specialty-modal {
  max-width: 700px;
  max-height: 90vh;
  border-radius: var(--specialty-card-radius);
  box-shadow: var(--specialty-modal-shadow);
}
```

### Responsive Design

**Desktop (≥992px)**:
- Two-column certification grid
- Full modal width (700px)
- Hover states enabled

**Tablet (768px - 991px)**:
- Two-column certification grid
- Modal width: 90% viewport
- Hover states enabled

**Mobile (<768px)**:
- Single column certification grid
- Modal: Full width minus 32px padding
- Touch-friendly spacing increased
- Drag handles larger (48x48px minimum)
- Remove hover-only interactions

### Dark Mode Support (Phase 2)

```css
@media (prefers-color-scheme: dark) {
  .specialty-management {
    --specialty-bg: var(--color-gray-900);
    --specialty-text: var(--color-gray-100);
    --specialty-border: var(--color-gray-700);
  }
}
```

---

## Testing Requirements

### Unit Tests

**Component Rendering**:
- ✓ Renders with empty specialties list
- ✓ Renders with populated specialties
- ✓ Renders empty state correctly
- ✓ Renders certifications list
- ✓ Applies custom className prop
- ✓ Renders in loading state

**Modal Behavior**:
- ✓ Opens modal on "Add Specialty" click
- ✓ Closes modal on close button click
- ✓ Closes modal on Escape key
- ✓ Closes modal on overlay click
- ✓ Traps focus within modal
- ✓ Returns focus on close

**Search Functionality**:
- ✓ Filters catalog based on search query
- ✓ Shows no results message when appropriate
- ✓ Clears search on clear button click
- ✓ Case-insensitive search

**Selection**:
- ✓ Toggles selection on catalog item click
- ✓ Shows visual feedback for selected items
- ✓ Allows multiple selections
- ✓ Clears selection on modal close

**Custom Specialty Form**:
- ✓ Validates specialty name (required, max length)
- ✓ Validates years of experience (required, numeric, range)
- ✓ Shows inline validation errors
- ✓ Disables Add button when invalid
- ✓ Resets form on successful add
- ✓ Prevents duplicate specialty names

**Drag and Drop**:
- ✓ Initiates drag on drag handle mousedown
- ✓ Shows dragging state visual feedback
- ✓ Reorders items on drop
- ✓ Calls onReorderSpecialties with new order
- ✓ Cancels drag on Escape key
- ✓ Keyboard navigation for drag-drop (accessibility)

**CRUD Operations**:
- ✓ Calls onAddSpecialties with selected items
- ✓ Calls onRemoveSpecialty with correct ID
- ✓ Shows confirmation before removing
- ✓ Displays success message after add
- ✓ Displays error message on failure

**Certification Upload**:
- ✓ Accepts valid file formats (PDF, JPG, PNG)
- ✓ Rejects invalid file formats
- ✓ Rejects files over 5MB
- ✓ Shows upload progress
- ✓ Calls onAddCertification with file
- ✓ Shows certification card after upload

### Integration Tests

- ✓ Full user flow: Open modal → Search → Select → Add → Verify in list
- ✓ Full user flow: Add custom specialty → Verify in list
- ✓ Full user flow: Reorder specialties → Verify new order persists
- ✓ Full user flow: Upload certification → Verify in list
- ✓ Error handling: API failure shows error message
- ✓ Loading state: Shows skeleton/spinner during operations

### Accessibility Tests

- ✓ Keyboard navigation works for all interactions
- ✓ Screen reader announces all state changes
- ✓ Focus management is correct throughout
- ✓ ARIA attributes are present and correct
- ✓ Color contrast meets WCAG AA standards
- ✓ Touch targets meet minimum size requirements

### Visual Regression Tests

- ✓ Empty state appearance
- ✓ Populated list appearance
- ✓ Modal appearance
- ✓ Dragging state appearance
- ✓ Selected items appearance
- ✓ Error states appearance
- ✓ Mobile responsive layout
- ✓ Tablet responsive layout

### Performance Tests

- ✓ Renders 100+ catalog items without lag
- ✓ Search filters large catalog quickly (<100ms)
- ✓ Drag operations are smooth (60fps)
- ✓ File upload shows progress for large files
- ✓ Component mounts in <200ms

---

## API Integration

### Endpoints

**Get Available Specialties**:
```
GET /api/specialties/catalog
Response: SpecialtyCatalogItem[]
```

**Get Professional's Specialties**:
```
GET /api/professionals/{id}/specialties
Response: Specialty[]
```

**Add Specialties**:
```
POST /api/professionals/{id}/specialties
Body: {
  specialties: Array<{
    catalogId?: string,
    customName?: string,
    yearsOfExperience: number
  }>
}
Response: Specialty[]
```

**Remove Specialty**:
```
DELETE /api/professionals/{id}/specialties/{specialtyId}
Response: 204 No Content
```

**Reorder Specialties**:
```
PUT /api/professionals/{id}/specialties/order
Body: {
  specialtyIds: string[] // Array in new order
}
Response: Specialty[]
```

**Upload Certification**:
```
POST /api/professionals/{id}/certifications
Content-Type: multipart/form-data
Body: {
  file: File,
  name?: string
}
Response: Certification
```

**Delete Certification**:
```
DELETE /api/professionals/{id}/certifications/{certificationId}
Response: 204 No Content
```

### Error Handling

**Common Errors**:

| Status | Error | User Message |
|--------|-------|--------------|
| 400 | Invalid specialty data | "Please check your input and try again" |
| 409 | Duplicate specialty | "You already have this specialty" |
| 413 | File too large | "File size exceeds 5MB limit" |
| 415 | Unsupported file type | "Only PDF, JPG, and PNG files are supported" |
| 429 | Too many requests | "Please wait a moment and try again" |
| 500 | Server error | "Something went wrong. Please try again later" |

**Retry Logic**:
- Automatically retry failed requests (3 attempts)
- Exponential backoff between retries
- Show user-friendly error after final failure

---

## Implementation Notes

### React Implementation

**Dependencies**:
- `react-dnd` or `@dnd-kit/core` for drag-and-drop
- `react-hook-form` for form management
- `axios` or `fetch` for API calls
- `react-toastify` or custom toast for notifications

**Example Hook**:
```typescript
function useSpecialtyManagement(professionalId: string) {
  const [specialties, setSpecialties] = useState<Specialty[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const addSpecialties = async (newSpecialties: Specialty[]) => {
    setIsLoading(true);
    try {
      const response = await api.post(
        `/professionals/${professionalId}/specialties`,
        { specialties: newSpecialties }
      );
      setSpecialties([...specialties, ...response.data]);
      toast.success('Specialties added successfully');
    } catch (error) {
      toast.error('Failed to add specialties');
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  return { specialties, addSpecialties, isLoading };
}
```

### Angular Implementation

**Dependencies**:
- `@angular/cdk/drag-drop` for drag-and-drop
- `@angular/forms` (ReactiveFormsModule) for forms
- `HttpClient` for API calls
- Custom toast service for notifications

**Example Service**:
```typescript
@Injectable({ providedIn: 'root' })
export class SpecialtyService {
  constructor(private http: HttpClient) {}

  addSpecialties(
    professionalId: string,
    specialties: Specialty[]
  ): Observable<Specialty[]> {
    return this.http.post<Specialty[]>(
      `/api/professionals/${professionalId}/specialties`,
      { specialties }
    );
  }

  reorderSpecialties(
    professionalId: string,
    specialtyIds: string[]
  ): Observable<Specialty[]> {
    return this.http.put<Specialty[]>(
      `/api/professionals/${professionalId}/specialties/order`,
      { specialtyIds }
    );
  }
}
```

---

## Future Enhancements (Phase 2+)

### Phase 2
- Category grouping in specialty catalog
- Filter catalog by category
- Specialty descriptions on hover/expand
- Bulk upload certifications (multiple files at once)
- Certification expiration date tracking
- Certification renewal reminders
- Drag-and-drop for certification reordering

### Phase 3
- AI-powered specialty recommendations based on profile
- Specialty verification badges (admin approved)
- Specialty popularity indicators
- Related specialties suggestions
- Certification verification with third-party APIs
- Digital badge integration (Credly, Badgr)
- Skill level indicators (Beginner, Intermediate, Expert)

### Phase 4
- Social proof: Show how many professionals have each specialty
- Trending specialties indicator
- Specialty demand insights (analytics)
- Certification marketplace/training recommendations
- Export specialty resume (PDF)
- LinkedIn integration for certifications

---

## File Structure

### React
```
src/
  components/
    SpecialtyManagement/
      SpecialtyManagement.tsx
      SpecialtyManagement.module.css
      SpecialtyList.tsx
      SpecialtyItem.tsx
      AddSpecialtyModal.tsx
      SpecialtyCatalog.tsx
      CustomSpecialtyForm.tsx
      CertificationUpload.tsx
      CertificationList.tsx
      types.ts
      hooks/
        useSpecialtyManagement.ts
        useSpecialtyCatalog.ts
        useSpecialtyDragDrop.ts
      utils/
        validation.ts
        sorting.ts
      __tests__/
        SpecialtyManagement.test.tsx
        SpecialtyItem.test.tsx
        AddSpecialtyModal.test.tsx
```

### Angular
```
src/
  app/
    features/
      specialty-management/
        specialty-management.component.ts
        specialty-management.component.html
        specialty-management.component.scss
        specialty-management.component.spec.ts
        components/
          specialty-list/
          specialty-item/
          add-specialty-modal/
          specialty-catalog/
          custom-specialty-form/
          certification-upload/
          certification-list/
        services/
          specialty.service.ts
          specialty.service.spec.ts
        models/
          specialty.model.ts
          certification.model.ts
        validators/
          custom-specialty.validator.ts
```

---

## Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2026-01-08 | Design Team | Initial specification |

---

## References

- [Platform Style Guide](../../../specs/style-guide.md)
- [Service Professionals Frontend Requirements](../../../specs/03-service-professionals/frontend-requirements.md)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [Material Design - Drag and Drop](https://material.io/design/interaction/gestures.html#drag-swipe-scale)
- [CarMax Style Guide Compliance](../../../specs/style-guide-specs.md)
