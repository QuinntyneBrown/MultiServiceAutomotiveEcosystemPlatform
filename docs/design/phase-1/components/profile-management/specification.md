# Profile Management Interface - Component Specification

**Component ID:** REQ-SP-F006
**Component Name:** Profile Management Interface
**Phase:** Phase 1
**Priority:** P0 (Critical)
**Status:** Design Complete
**Last Updated:** January 2026

---

## Table of Contents

1. [Component Overview](#component-overview)
2. [User Stories](#user-stories)
3. [Component Architecture](#component-architecture)
4. [Props & Inputs](#props--inputs)
5. [Form Sections](#form-sections)
6. [Validation Rules](#validation-rules)
7. [File Upload Handling](#file-upload-handling)
8. [State Management](#state-management)
9. [API Integration](#api-integration)
10. [Accessibility Requirements](#accessibility-requirements)
11. [Testing Requirements](#testing-requirements)
12. [Performance Considerations](#performance-considerations)

---

## Component Overview

### Purpose

The Profile Management Interface enables service professionals to create, edit, and manage their business profiles on the Multi-Service Automotive Platform. This component provides a comprehensive form with multiple sections for entering business information, contact details, location data, and media uploads.

### Key Features

- **Multi-section Form:** Organized into logical tabs (Basic Information, About/Bio, Contact Information, Location, Media)
- **Real-time Validation:** Instant feedback on form field errors
- **File Upload:** Support for profile photo, cover photo, and business logo
- **Draft Saving:** Auto-save and manual save draft capabilities
- **Preview Mode:** Preview profile before publishing
- **Responsive Design:** Works seamlessly on desktop, tablet, and mobile devices

### Business Requirements

- Service professionals must be able to create complete business profiles
- Profiles must include required information (business name, type, contact info, location)
- Support for multiple contact methods (phone numbers, emails)
- Image uploads for visual branding
- Draft state for incomplete profiles
- Published state for live, searchable profiles

---

## User Stories

### Primary User Stories

**US-PM-001:** As a service professional, I want to create a new business profile so that customers can find and contact me.

**US-PM-002:** As a service professional, I want to edit my existing profile information so that I can keep my details up to date.

**US-PM-003:** As a service professional, I want to save my profile as a draft so that I can complete it later.

**US-PM-004:** As a service professional, I want to preview my profile before publishing so that I can ensure it looks professional.

**US-PM-005:** As a service professional, I want to upload photos and a logo so that my profile is visually appealing.

**US-PM-006:** As a service professional, I want to add multiple contact methods so that customers can reach me in their preferred way.

### Acceptance Criteria

- All required fields must be validated before profile can be published
- Character limits must be enforced on text fields
- Image uploads must validate file type, size, and dimensions
- Draft profiles must auto-save every 30 seconds
- Published profiles must be immediately visible in search results
- Form must be keyboard accessible and screen reader compatible

---

## Component Architecture

### Technology Stack

**React Implementation:**
```typescript
interface ProfileManagementProps {
  profileId?: string;
  mode: 'create' | 'edit';
  onSave: (profile: Profile, isDraft: boolean) => Promise<void>;
  onCancel: () => void;
  onPreview: (profile: Profile) => void;
  initialData?: Partial<Profile>;
  autoSaveInterval?: number; // milliseconds, default 30000
}
```

**Angular Implementation:**
```typescript
@Component({
  selector: 'app-profile-management',
  templateUrl: './profile-management.component.html',
  styleUrls: ['./profile-management.component.scss']
})
export class ProfileManagementComponent implements OnInit, OnDestroy {
  @Input() profileId?: string;
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() initialData?: Partial<Profile>;
  @Input() autoSaveInterval: number = 30000;

  @Output() save = new EventEmitter<{profile: Profile, isDraft: boolean}>();
  @Output() cancel = new EventEmitter<void>();
  @Output() preview = new EventEmitter<Profile>();
}
```

### Component Structure

```
ProfileManagement/
├── components/
│   ├── BasicInformationSection.tsx
│   ├── AboutBioSection.tsx
│   ├── ContactInformationSection.tsx
│   ├── LocationSection.tsx
│   ├── MediaSection.tsx
│   ├── TabNavigation.tsx
│   ├── FormActions.tsx
│   └── ImageUploader.tsx
├── hooks/
│   ├── useProfileForm.ts
│   ├── useAutoSave.ts
│   ├── useImageUpload.ts
│   └── useFormValidation.ts
├── types/
│   └── profile.types.ts
├── utils/
│   ├── validation.ts
│   └── imageProcessing.ts
└── ProfileManagement.tsx
```

---

## Props & Inputs

### React Props Definition

```typescript
interface Profile {
  id?: string;
  // Basic Information
  businessName: string;
  businessType: BusinessType;
  personalName: string;
  professionalTitle?: string;

  // About & Bio
  bio: string;
  yearsInBusiness?: number;
  numberOfEmployees?: number;

  // Contact Information
  phoneNumbers: ContactPhone[];
  emails: ContactEmail[];
  website?: string;

  // Location
  address: Address;
  serviceRadius?: ServiceRadius;

  // Media
  profilePhoto?: MediaFile;
  coverPhoto?: MediaFile;
  logo?: MediaFile;

  // Meta
  status: 'draft' | 'published' | 'archived';
  createdAt?: Date;
  updatedAt?: Date;
  publishedAt?: Date;
}

interface ContactPhone {
  id: string;
  type: 'mobile' | 'business' | 'fax';
  number: string;
  isPrimary: boolean;
}

interface ContactEmail {
  id: string;
  type: 'primary' | 'billing' | 'support';
  email: string;
  isPrimary: boolean;
}

interface Address {
  street: string;
  addressLine2?: string;
  city: string;
  province: string;
  postalCode: string;
  country?: string;
}

interface ServiceRadius {
  value: number;
  unit: 'miles' | 'km';
}

interface MediaFile {
  id?: string;
  url: string;
  fileName: string;
  fileSize: number;
  mimeType: string;
  width?: number;
  height?: number;
}

type BusinessType =
  | 'auto-repair'
  | 'body-shop'
  | 'detailing'
  | 'towing'
  | 'tire-shop'
  | 'parts-supplier'
  | 'dealer'
  | 'mobile-mechanic'
  | 'other';
```

### Angular Input/Output Definition

```typescript
export interface ProfileManagementInputs {
  profileId?: string;
  mode: 'create' | 'edit';
  initialData?: Partial<Profile>;
  autoSaveInterval?: number;
}

export interface ProfileManagementOutputs {
  save: {profile: Profile, isDraft: boolean};
  cancel: void;
  preview: Profile;
}
```

---

## Form Sections

### Section 1: Basic Information

**Fields:**
- **Business Name** (required, text, max 100 chars)
- **Business Type** (required, select)
- **Your Full Name** (required, text, max 100 chars)
- **Professional Title** (optional, text, max 100 chars)

**Validation:**
- Business name must be unique (async validation)
- All required fields must be filled
- Character limits enforced

**Tab Order:** 1

---

### Section 2: About & Bio

**Fields:**
- **Business Description** (required, textarea, max 2000 chars)
- **Years in Business** (optional, number, min 0, max 200)
- **Number of Employees** (optional, number, min 1, max 10000)

**Features:**
- Real-time character counter
- Warning at 90% capacity (1800 chars)
- Error at 100% capacity (2000 chars)

**Tab Order:** 2

---

### Section 3: Contact Information

**Fields:**
- **Phone Numbers** (at least 1 required, multi-input)
  - Type: mobile, business, fax
  - Number: formatted phone number
  - Can add/remove up to 5 phone numbers

- **Email Addresses** (at least 1 required, multi-input)
  - Type: primary, billing, support
  - Email: validated email format
  - Can add/remove up to 5 emails

- **Website** (optional, URL format)

**Validation:**
- At least one phone number required
- At least one email required
- Phone numbers must be valid US/Canada format
- Emails must be valid format
- Website must be valid URL (https:// preferred)

**Tab Order:** 3

---

### Section 4: Location

**Fields:**
- **Street Address** (required, text, max 200 chars)
- **Address Line 2** (optional, text, max 100 chars)
- **City** (required, text, max 100 chars)
- **Province** (required, select)
- **Postal Code** (required, text, 6-7 chars)
- **Service Radius** (optional)
  - Value: number, min 0, max 500
  - Unit: miles or kilometers

**Features:**
- Address autocomplete/validation (Google Places API)
- Map preview of location (optional)
- Service radius visualization

**Tab Order:** 4

---

### Section 5: Media

**Fields:**
- **Profile Photo** (optional, image upload)
  - Recommended: Square, min 400x400px
  - Max file size: 5MB
  - Formats: JPG, PNG, WebP

- **Cover Photo** (optional, image upload)
  - Recommended: 1200x400px (3:1 ratio)
  - Max file size: 10MB
  - Formats: JPG, PNG, WebP

- **Business Logo** (optional, image upload)
  - Recommended: Square, min 200x200px
  - Max file size: 5MB
  - Formats: JPG, PNG, SVG, WebP

**Features:**
- Drag-and-drop upload
- Image preview
- Crop/resize tool
- Remove uploaded image

**Tab Order:** 5

---

## Validation Rules

### Field-Level Validation

**Text Fields:**
```typescript
const textFieldValidation = {
  businessName: {
    required: true,
    minLength: 2,
    maxLength: 100,
    pattern: /^[a-zA-Z0-9\s\-'&.,]+$/,
    asyncValidation: checkBusinessNameUnique
  },
  personalName: {
    required: true,
    minLength: 2,
    maxLength: 100,
    pattern: /^[a-zA-Z\s\-']+$/
  },
  professionalTitle: {
    required: false,
    maxLength: 100
  },
  bio: {
    required: true,
    minLength: 50,
    maxLength: 2000
  }
};
```

**Contact Validation:**
```typescript
const contactValidation = {
  phone: {
    pattern: /^\(\d{3}\)\s\d{3}-\d{4}$/,
    message: 'Phone must be in format: (555) 123-4567'
  },
  email: {
    pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
    message: 'Please enter a valid email address'
  },
  website: {
    pattern: /^https?:\/\/.+\..+$/,
    message: 'Please enter a valid URL'
  }
};
```

**Address Validation:**
```typescript
const addressValidation = {
  street: { required: true, maxLength: 200 },
  city: { required: true, maxLength: 100 },
  province: { required: true },
  postalCode: {
    required: true,
    pattern: /^[ABCEGHJ-NPRSTVXY]\d[ABCEGHJ-NPRSTV-Z][ -]?\d[ABCEGHJ-NPRSTV-Z]\d$/i,
    message: 'Postal code must be in format: A1A 1A1'
  }
};
```

### Form-Level Validation

**Publish Requirements:**
- All required fields must be completed
- At least one phone number
- At least one email address
- Valid address (street, city, province, postal code)
- Bio minimum 50 characters

**Draft Requirements:**
- Business name (minimum requirement for draft)
- No other required validations for draft save

---

## File Upload Handling

### Upload Flow

```typescript
interface ImageUploadConfig {
  maxFileSize: number; // bytes
  allowedTypes: string[];
  minDimensions?: { width: number; height: number };
  recommendedDimensions?: { width: number; height: number };
  aspectRatio?: number;
}

const uploadConfigs = {
  profilePhoto: {
    maxFileSize: 5 * 1024 * 1024, // 5MB
    allowedTypes: ['image/jpeg', 'image/png', 'image/webp'],
    minDimensions: { width: 400, height: 400 },
    recommendedDimensions: { width: 800, height: 800 },
    aspectRatio: 1
  },
  coverPhoto: {
    maxFileSize: 10 * 1024 * 1024, // 10MB
    allowedTypes: ['image/jpeg', 'image/png', 'image/webp'],
    minDimensions: { width: 1200, height: 400 },
    recommendedDimensions: { width: 1200, height: 400 },
    aspectRatio: 3
  },
  logo: {
    maxFileSize: 5 * 1024 * 1024, // 5MB
    allowedTypes: ['image/jpeg', 'image/png', 'image/svg+xml', 'image/webp'],
    minDimensions: { width: 200, height: 200 },
    recommendedDimensions: { width: 400, height: 400 },
    aspectRatio: 1
  }
};
```

### Upload Process

1. **Client-Side Validation:**
   - Check file type
   - Check file size
   - Read image dimensions
   - Validate dimensions meet minimum requirements

2. **Image Processing:**
   - Generate thumbnail preview
   - Optional: Client-side resize/compress
   - Optional: Crop tool for aspect ratio

3. **Upload to Server:**
   - Multipart form upload
   - Progress indicator
   - Cancel upload capability

4. **Server-Side Processing:**
   - Virus scan
   - Re-validate file type and size
   - Generate optimized versions (thumbnails, WebP)
   - Store in CDN/S3

5. **Error Handling:**
   - Display user-friendly error messages
   - Retry mechanism for failed uploads
   - Rollback on complete form failure

### Upload Component Interface

```typescript
interface ImageUploaderProps {
  config: ImageUploadConfig;
  value?: MediaFile;
  onChange: (file: MediaFile | null) => void;
  onError: (error: string) => void;
  label: string;
  description?: string;
  showCropTool?: boolean;
}
```

---

## State Management

### Draft vs. Published States

**State Diagram:**
```
[New] → [Draft] → [Published] → [Archived]
           ↓          ↑
           └──────────┘
           (can toggle)
```

**State Definitions:**

```typescript
enum ProfileStatus {
  DRAFT = 'draft',
  PUBLISHED = 'published',
  ARCHIVED = 'archived'
}

interface ProfileState {
  data: Profile;
  status: ProfileStatus;
  isDirty: boolean;
  isSaving: boolean;
  lastSaved?: Date;
  errors: ValidationErrors;
  validationState: 'idle' | 'validating' | 'valid' | 'invalid';
}
```

### Auto-Save Behavior

**Implementation:**
```typescript
const useAutoSave = (
  formData: Profile,
  saveCallback: (data: Profile, isDraft: boolean) => Promise<void>,
  interval: number = 30000
) => {
  const [lastSaved, setLastSaved] = useState<Date | null>(null);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    const timer = setInterval(async () => {
      if (hasChanges(formData) && !isSaving) {
        setIsSaving(true);
        try {
          await saveCallback(formData, true); // Always draft for auto-save
          setLastSaved(new Date());
        } catch (error) {
          console.error('Auto-save failed:', error);
        } finally {
          setIsSaving(false);
        }
      }
    }, interval);

    return () => clearInterval(timer);
  }, [formData, interval, isSaving]);

  return { lastSaved, isSaving };
};
```

**Auto-Save Rules:**
- Trigger every 30 seconds (configurable)
- Only save if form has changes (isDirty)
- Only save if minimum requirements met (business name)
- Always save as draft, never auto-publish
- Show "Saving..." indicator
- Show "Last saved: X minutes ago"
- Disable during active save operation

### Manual Save Operations

**Save Draft:**
- User clicks "Save Draft" button
- Validates minimum requirements (business name only)
- Saves with status = 'draft'
- Shows success message
- Resets isDirty flag

**Publish:**
- User clicks "Publish Profile" button
- Validates all required fields
- If validation fails, show errors and prevent publish
- If validation passes, save with status = 'published'
- Show success message with confirmation
- Redirect to profile view (optional)

**Preview:**
- User clicks "Preview" button
- No save operation
- Opens profile in preview mode (read-only view)
- Can return to edit mode

---

## API Integration

### Endpoints

**Create Profile:**
```typescript
POST /api/v1/profiles
Headers: Authorization: Bearer {token}
Body: Profile
Response: { id: string, status: string, profile: Profile }
```

**Update Profile:**
```typescript
PUT /api/v1/profiles/{profileId}
Headers: Authorization: Bearer {token}
Body: Profile
Response: { id: string, status: string, profile: Profile }
```

**Get Profile:**
```typescript
GET /api/v1/profiles/{profileId}
Headers: Authorization: Bearer {token}
Response: Profile
```

**Upload Image:**
```typescript
POST /api/v1/profiles/{profileId}/media
Headers:
  Authorization: Bearer {token}
  Content-Type: multipart/form-data
Body: FormData { file: File, type: 'profile' | 'cover' | 'logo' }
Response: MediaFile
```

**Validate Business Name:**
```typescript
POST /api/v1/profiles/validate/business-name
Headers: Authorization: Bearer {token}
Body: { businessName: string, excludeProfileId?: string }
Response: { isAvailable: boolean, suggestions?: string[] }
```

### Error Handling

**API Error Responses:**
```typescript
interface APIError {
  code: string;
  message: string;
  field?: string;
  details?: any;
}

// Example Error Codes
const ERROR_CODES = {
  VALIDATION_ERROR: 'VALIDATION_ERROR',
  DUPLICATE_BUSINESS_NAME: 'DUPLICATE_BUSINESS_NAME',
  UNAUTHORIZED: 'UNAUTHORIZED',
  FILE_TOO_LARGE: 'FILE_TOO_LARGE',
  INVALID_FILE_TYPE: 'INVALID_FILE_TYPE',
  SERVER_ERROR: 'SERVER_ERROR'
};
```

**Client Error Handling:**
- Show field-level errors for validation failures
- Show toast/alert for network errors
- Retry mechanism for transient failures
- Offline detection and queue actions

---

## Accessibility Requirements

### WCAG 2.1 AA Compliance

**Keyboard Navigation:**
- All form fields accessible via Tab/Shift+Tab
- Tab order follows logical reading order
- Enter key submits form
- Escape key cancels/closes modals
- Arrow keys navigate within select dropdowns

**Screen Reader Support:**
- All form fields have associated labels
- Required fields indicated with aria-required="true"
- Error messages announced with aria-live regions
- Form sections have proper heading hierarchy (h2, h3)
- Image upload areas have descriptive aria-labels

**Focus Management:**
- Visible focus indicators on all interactive elements
- Focus ring: 3px solid rgba(0, 82, 159, 0.4)
- Focus trapped within modal dialogs
- Focus returned to trigger element on modal close

**Color Contrast:**
- Text meets 4.5:1 contrast ratio
- Error messages meet 4.5:1 contrast ratio
- Focus indicators meet 3:1 contrast ratio

**Error Handling:**
- Error messages clear and descriptive
- Errors announced to screen readers
- Errors visible without color alone
- Error summary at top of form (optional)

### ARIA Attributes

```html
<!-- Required Field -->
<label for="business-name">
  Business Name
  <span aria-label="required">*</span>
</label>
<input
  id="business-name"
  type="text"
  aria-required="true"
  aria-invalid="false"
  aria-describedby="business-name-helper business-name-error"
/>
<span id="business-name-helper" class="form-helper">
  This is the name customers will see
</span>
<span id="business-name-error" class="form-error" role="alert" aria-live="polite">
  <!-- Error message appears here -->
</span>

<!-- File Upload -->
<div
  role="button"
  tabindex="0"
  aria-label="Upload profile photo. Recommended size: 400 by 400 pixels"
  onkeydown="handleKeyPress"
>
  <input type="file" id="profile-photo" class="visually-hidden" />
</div>
```

---

## Testing Requirements

### Unit Tests

**Components to Test:**
- BasicInformationSection
- AboutBioSection
- ContactInformationSection
- LocationSection
- MediaSection
- ImageUploader
- Form validation utilities
- Auto-save hook

**Test Cases:**
```typescript
describe('ProfileManagement', () => {
  describe('Form Validation', () => {
    it('should validate required fields', () => {});
    it('should validate email format', () => {});
    it('should validate phone number format', () => {});
    it('should validate URL format', () => {});
    it('should enforce character limits', () => {});
    it('should validate image dimensions', () => {});
    it('should validate image file size', () => {});
  });

  describe('Auto-Save', () => {
    it('should auto-save after 30 seconds', () => {});
    it('should not auto-save if no changes', () => {});
    it('should not auto-save during active save', () => {});
    it('should show saving indicator', () => {});
  });

  describe('State Management', () => {
    it('should mark form as dirty on change', () => {});
    it('should save as draft', () => {});
    it('should publish profile', () => {});
    it('should prevent publish with validation errors', () => {});
  });

  describe('Image Upload', () => {
    it('should validate file type', () => {});
    it('should validate file size', () => {});
    it('should generate preview', () => {});
    it('should handle upload errors', () => {});
  });
});
```

### Integration Tests

**User Flows to Test:**
1. Create new profile from scratch
2. Edit existing profile
3. Save draft and resume later
4. Upload images
5. Add/remove multiple phone numbers
6. Add/remove multiple emails
7. Preview profile
8. Publish profile
9. Handle validation errors
10. Auto-save functionality

### E2E Tests

**Critical Paths:**
```typescript
describe('Profile Management E2E', () => {
  it('should complete full profile creation flow', async () => {
    // Navigate to profile creation
    // Fill all required fields
    // Upload images
    // Save draft
    // Preview profile
    // Publish profile
    // Verify profile is published
  });

  it('should edit existing profile', async () => {
    // Navigate to profile edit
    // Modify fields
    // Save changes
    // Verify changes persisted
  });

  it('should handle auto-save', async () => {
    // Start creating profile
    // Make changes
    // Wait for auto-save
    // Verify draft saved
  });
});
```

### Accessibility Tests

**Tools:**
- axe-core automated testing
- Manual keyboard navigation testing
- Screen reader testing (NVDA, JAWS, VoiceOver)

**Test Cases:**
- All form fields keyboard accessible
- Proper heading hierarchy
- ARIA attributes correct
- Focus management working
- Error announcements working
- Color contrast meets standards

---

## Performance Considerations

### Optimization Strategies

**1. Form Performance:**
- Debounce validation (300ms)
- Memoize expensive computations
- Virtual scrolling for long lists (if applicable)
- Lazy load tab content

**2. Image Optimization:**
- Client-side image compression
- Lazy load image previews
- Generate WebP versions
- Use progressive JPEGs
- Implement CDN caching

**3. Auto-Save Optimization:**
- Only send changed fields to API
- Debounce save operations
- Queue saves if offline
- Implement optimistic updates

**4. Bundle Size:**
- Code splitting by tab
- Lazy load image processing libraries
- Tree-shake unused utilities
- Use dynamic imports

### Performance Metrics

**Target Metrics:**
- Initial load: < 2 seconds
- Time to interactive: < 3 seconds
- Form field response: < 100ms
- Image upload: < 5 seconds (5MB file)
- Auto-save: < 1 second
- Largest Contentful Paint: < 2.5s
- First Input Delay: < 100ms
- Cumulative Layout Shift: < 0.1

---

## Dependencies

### External Libraries

**React Implementation:**
```json
{
  "react": "^18.2.0",
  "react-hook-form": "^7.45.0",
  "yup": "^1.2.0",
  "@tanstack/react-query": "^4.29.0",
  "axios": "^1.4.0",
  "react-dropzone": "^14.2.0",
  "react-image-crop": "^10.1.0"
}
```

**Angular Implementation:**
```json
{
  "@angular/core": "^16.0.0",
  "@angular/forms": "^16.0.0",
  "rxjs": "^7.8.0",
  "ngx-dropzone": "^3.1.0",
  "ngx-image-cropper": "^7.0.0"
}
```

### Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile: iOS 14+, Android 10+

---

## Design Assets

### Mockups
- Desktop: `/docs/design/phase-1/components/profile-management/mockup.html`
- Screenshot: `/docs/design/phase-1/components/profile-management/screenshot.svg`

### Related Components
- Professional Profile View (REQ-SP-F004)
- Professional Directory (REQ-SP-F003)
- Customer Profile Management (Similar pattern)

### Style Guide Reference
- Platform Style Guide: `/docs/specs/style-guide.md`
- Primary Color: #00529F
- Accent Color: #FFD520
- Font Primary: Montserrat
- Font Secondary: Lato

---

## Notes & Considerations

### Future Enhancements

1. **Business Hours Management:** Add section for managing business hours
2. **Service Areas Map:** Interactive map for defining service areas
3. **Certifications & Awards:** Dedicated section for credentials
4. **Portfolio/Gallery:** Multiple images showcasing work
5. **Team Members:** Add team member profiles
6. **Social Media Links:** Integration with social platforms
7. **Payment Methods:** Accepted payment methods
8. **Languages Spoken:** Multi-language support indicator

### Known Limitations

- Maximum 5 phone numbers per profile
- Maximum 5 email addresses per profile
- Maximum file sizes: 5MB (photos), 10MB (cover)
- No video upload support in Phase 1
- No bulk import/export in Phase 1

### Security Considerations

- All uploads scanned for malware
- Profile data encrypted at rest
- API requests authenticated via JWT
- Rate limiting on API endpoints
- CSRF protection on form submissions
- XSS protection on user-generated content

---

## Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | January 2026 | Design Team | Initial specification |

---

## Approval

**Product Owner:** _____________________ Date: _________

**Engineering Lead:** _____________________ Date: _________

**Design Lead:** _____________________ Date: _________
