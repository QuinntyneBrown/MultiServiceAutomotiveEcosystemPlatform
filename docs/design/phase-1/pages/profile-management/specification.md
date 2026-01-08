# Profile Management - Angular Implementation Specification

## Overview
Profile Management interface for professionals to edit and manage their business profiles, including basic information, bio, contact details, location, and media uploads.

**Requirement ID**: REQ-SP-F006
**Phase**: Phase 1
**Priority**: P0
**Route**: `/dashboard/profile/edit`

---

## Component Architecture

### Main Component
**Path**: `src/app/features/professional/profile-management/profile-management.component.ts`

```typescript
@Component({
  selector: 'app-profile-management',
  templateUrl: './profile-management.component.html',
  styleUrls: ['./profile-management.component.scss']
})
export class ProfileManagementComponent implements OnInit, OnDestroy {
  // Component implementation
}
```

### Child Components

1. **BasicInfoFormComponent**
   - Path: `src/app/features/professional/profile-management/forms/basic-info-form/`
   - Purpose: Business name, personal name, title, business type
   - Inputs: `[profile]`, `[validationErrors]`
   - Outputs: `(formChange)`, `(formValid)`

2. **BioFormComponent**
   - Path: `src/app/features/professional/profile-management/forms/bio-form/`
   - Purpose: Rich text bio editor with preview
   - Inputs: `[bio]`, `[maxLength]`
   - Outputs: `(bioChange)`, `(bioValid)`
   - Features: Character counter, live preview, rich text toolbar

3. **ContactFormComponent**
   - Path: `src/app/features/professional/profile-management/forms/contact-form/`
   - Purpose: Phone numbers, email, website
   - Inputs: `[contactInfo]`
   - Outputs: `(contactChange)`, `(contactValid)`
   - Features: Multiple phone numbers, phone formatting, email validation

4. **LocationFormComponent**
   - Path: `src/app/features/professional/profile-management/forms/location-form/`
   - Purpose: Address, map preview, service radius
   - Inputs: `[address]`, `[serviceRadius]`
   - Outputs: `(locationChange)`, `(locationValid)`, `(addressValidated)`
   - Features: Address autocomplete, map integration, radius slider

5. **MediaUploadFormComponent**
   - Path: `src/app/features/professional/profile-management/forms/media-upload-form/`
   - Purpose: Profile photo, cover photo, logo uploads
   - Inputs: `[profilePhoto]`, `[coverPhoto]`, `[logo]`
   - Outputs: `(mediaChange)`, `(uploadProgress)`
   - Features: Image preview, crop tool, drag-drop upload

### Shared Components

1. **ImageCropperModalComponent**
   - Path: `src/app/shared/components/image-cropper-modal/`
   - Purpose: Modal for cropping uploaded images
   - Library: `ngx-image-cropper` or similar
   - Inputs: `[imageFile]`, `[aspectRatio]`, `[cropperSettings]`
   - Outputs: `(imageCropped)`, `(modalClosed)`

2. **RichTextEditorComponent**
   - Path: `src/app/shared/components/rich-text-editor/`
   - Purpose: WYSIWYG editor for bio
   - Library: `ngx-quill` or `angular-editor`
   - Inputs: `[content]`, `[maxLength]`, `[placeholder]`
   - Outputs: `(contentChange)`, `(charCountChange)`

3. **ServiceRadiusSliderComponent**
   - Path: `src/app/shared/components/service-radius-slider/`
   - Purpose: Custom slider for service radius
   - Inputs: `[value]`, `[min]`, `[max]`, `[step]`
   - Outputs: `(valueChange)`

---

## Data Models

### ProfileFormData Interface
```typescript
export interface ProfileFormData {
  basic: BasicInfo;
  bio: BioInfo;
  contact: ContactInfo;
  location: LocationInfo;
  media: MediaInfo;
  metadata: ProfileMetadata;
}

export interface BasicInfo {
  businessName: string;
  firstName: string;
  lastName: string;
  title: string;
  businessType: BusinessType;
}

export interface BioInfo {
  description: string;
  yearsInBusiness: number;
  employeeCount: string;
}

export interface ContactInfo {
  phones: PhoneNumber[];
  email: string;
  website?: string;
  socialLinks?: SocialLinks;
}

export interface PhoneNumber {
  id: string;
  type: 'mobile' | 'office' | 'fax';
  number: string;
  isPrimary: boolean;
}

export interface LocationInfo {
  street: string;
  street2?: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  serviceRadius: number; // in miles
  coordinates?: {
    lat: number;
    lng: number;
  };
}

export interface MediaInfo {
  profilePhoto?: ImageUpload;
  coverPhoto?: ImageUpload;
  logo?: ImageUpload;
}

export interface ImageUpload {
  id: string;
  url: string;
  fileName: string;
  fileSize: number;
  mimeType: string;
  uploadedAt: Date;
  croppedData?: CropData;
}

export interface CropData {
  x: number;
  y: number;
  width: number;
  height: number;
  rotate: number;
  scaleX: number;
  scaleY: number;
}

export interface ProfileMetadata {
  completionPercentage: number;
  isPublished: boolean;
  isDraft: boolean;
  lastSaved: Date;
  lastPublished?: Date;
}

export enum BusinessType {
  MECHANIC = 'mechanic',
  BODY_SHOP = 'body-shop',
  DETAILING = 'detailing',
  TOWING = 'towing',
  TIRE_SHOP = 'tire-shop',
  DEALER = 'dealer',
  OTHER = 'other'
}
```

---

## Services

### ProfileService
**Path**: `src/app/core/services/profile.service.ts`

```typescript
@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private apiUrl = environment.apiUrl + '/api/professionals';

  constructor(private http: HttpClient) {}

  // Get current professional's profile
  getProfile(): Observable<ProfileFormData> {
    return this.http.get<ProfileFormData>(`${this.apiUrl}/me/profile`);
  }

  // Save profile as draft
  saveDraft(profile: ProfileFormData): Observable<ProfileFormData> {
    return this.http.put<ProfileFormData>(
      `${this.apiUrl}/me/profile/draft`,
      profile
    );
  }

  // Publish profile changes
  publishProfile(profile: ProfileFormData): Observable<ProfileFormData> {
    return this.http.put<ProfileFormData>(
      `${this.apiUrl}/me/profile/publish`,
      profile
    );
  }

  // Calculate profile completion percentage
  calculateCompletion(profile: ProfileFormData): number {
    // Implementation logic
  }

  // Validate address
  validateAddress(address: Partial<LocationInfo>): Observable<AddressValidation> {
    return this.http.post<AddressValidation>(
      `${this.apiUrl}/validate-address`,
      address
    );
  }

  // Get geocode coordinates
  geocodeAddress(address: string): Observable<Coordinates> {
    return this.http.get<Coordinates>(
      `${this.apiUrl}/geocode?address=${encodeURIComponent(address)}`
    );
  }
}
```

### MediaUploadService
**Path**: `src/app/core/services/media-upload.service.ts`

```typescript
@Injectable({
  providedIn: 'root'
})
export class MediaUploadService {
  private apiUrl = environment.apiUrl + '/api/media';

  constructor(private http: HttpClient) {}

  // Upload image file
  uploadImage(
    file: File,
    category: 'profile' | 'cover' | 'logo',
    cropData?: CropData
  ): Observable<ImageUpload> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('category', category);
    if (cropData) {
      formData.append('cropData', JSON.stringify(cropData));
    }

    return this.http.post<ImageUpload>(
      `${this.apiUrl}/upload`,
      formData,
      {
        reportProgress: true,
        observe: 'events'
      }
    ).pipe(
      map(event => this.getEventMessage(event)),
      filter(message => message !== null)
    );
  }

  // Delete image
  deleteImage(imageId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${imageId}`);
  }

  // Validate image file
  validateImageFile(file: File): { valid: boolean; error?: string } {
    const maxSize = 5 * 1024 * 1024; // 5MB
    const allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];

    if (!allowedTypes.includes(file.type)) {
      return {
        valid: false,
        error: 'Invalid file type. Please upload JPG, PNG, or WebP.'
      };
    }

    if (file.size > maxSize) {
      return {
        valid: false,
        error: 'File size exceeds 5MB limit.'
      };
    }

    return { valid: true };
  }

  private getEventMessage(event: HttpEvent<any>) {
    // Handle upload progress events
  }
}
```

### AutoSaveService
**Path**: `src/app/core/services/auto-save.service.ts`

```typescript
@Injectable({
  providedIn: 'root'
})
export class AutoSaveService {
  private saveDelay = 2000; // 2 seconds
  private saveSubject = new Subject<any>();

  constructor() {
    this.setupAutoSave();
  }

  private setupAutoSave() {
    this.saveSubject.pipe(
      debounceTime(this.saveDelay),
      distinctUntilChanged(),
      switchMap(data => this.performSave(data))
    ).subscribe();
  }

  queueSave(data: any) {
    this.saveSubject.next(data);
  }

  private performSave(data: any): Observable<any> {
    // Actual save logic
    return of(data);
  }
}
```

---

## Form Management

### Reactive Forms Setup

```typescript
export class ProfileManagementComponent implements OnInit {
  profileForm: FormGroup;
  activeTab: string = 'basic';
  saveStatus: 'saved' | 'saving' | 'error' = 'saved';
  completionPercentage: number = 0;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private autoSaveService: AutoSaveService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.buildForm();
    this.loadProfile();
    this.setupAutoSave();
    this.setupFormValidation();
  }

  private buildForm() {
    this.profileForm = this.fb.group({
      basic: this.fb.group({
        businessName: ['', [Validators.required, Validators.maxLength(100)]],
        firstName: ['', [Validators.required, Validators.maxLength(50)]],
        lastName: ['', [Validators.required, Validators.maxLength(50)]],
        title: ['', Validators.maxLength(100)],
        businessType: ['', Validators.required]
      }),
      bio: this.fb.group({
        description: ['', [Validators.required, Validators.maxLength(1000)]],
        yearsInBusiness: [0, [Validators.min(0), Validators.max(100)]],
        employeeCount: ['']
      }),
      contact: this.fb.group({
        phones: this.fb.array([this.createPhoneFormGroup()]),
        email: ['', [Validators.required, Validators.email]],
        website: ['', Validators.pattern(/^https?:\/\/.+/)]
      }),
      location: this.fb.group({
        street: ['', Validators.required],
        street2: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipCode: ['', [Validators.required, Validators.pattern(/^\d{5}$/)]],
        country: ['US'],
        serviceRadius: [25, [Validators.min(0), Validators.max(100)]]
      }),
      media: this.fb.group({
        profilePhoto: [null],
        coverPhoto: [null],
        logo: [null]
      })
    });
  }

  private createPhoneFormGroup(): FormGroup {
    return this.fb.group({
      id: [this.generateId()],
      type: ['mobile'],
      number: ['', [Validators.required, this.phoneValidator]],
      isPrimary: [false]
    });
  }

  get phonesArray(): FormArray {
    return this.profileForm.get('contact.phones') as FormArray;
  }

  addPhone() {
    this.phonesArray.push(this.createPhoneFormGroup());
  }

  removePhone(index: number) {
    if (this.phonesArray.length > 1) {
      this.phonesArray.removeAt(index);
    }
  }

  private phoneValidator(control: AbstractControl): ValidationErrors | null {
    const phoneRegex = /^\(\d{3}\) \d{3}-\d{4}$/;
    return phoneRegex.test(control.value) ? null : { invalidPhone: true };
  }

  private setupAutoSave() {
    this.profileForm.valueChanges.pipe(
      debounceTime(2000),
      distinctUntilChanged()
    ).subscribe(value => {
      this.saveStatus = 'saving';
      this.autoSaveService.queueSave(value);
      this.updateCompletion();
    });
  }

  private updateCompletion() {
    this.completionPercentage = this.profileService.calculateCompletion(
      this.profileForm.value
    );
  }

  saveDraft() {
    if (this.profileForm.valid) {
      this.profileService.saveDraft(this.profileForm.value).subscribe({
        next: (profile) => {
          this.saveStatus = 'saved';
          this.snackBar.open('Draft saved successfully', 'Close', {
            duration: 3000
          });
        },
        error: (error) => {
          this.saveStatus = 'error';
          this.snackBar.open('Error saving draft', 'Close', {
            duration: 3000
          });
        }
      });
    }
  }

  publishProfile() {
    if (this.profileForm.valid) {
      this.profileService.publishProfile(this.profileForm.value).subscribe({
        next: (profile) => {
          this.snackBar.open('Profile published successfully', 'Close', {
            duration: 3000
          });
          this.router.navigate(['/dashboard/profile']);
        },
        error: (error) => {
          this.snackBar.open('Error publishing profile', 'Close', {
            duration: 3000
          });
        }
      });
    } else {
      this.markFormGroupTouched(this.profileForm);
      this.snackBar.open('Please fix validation errors', 'Close', {
        duration: 3000
      });
    }
  }

  previewProfile() {
    window.open('/preview/profile', '_blank');
  }

  switchTab(tab: string) {
    this.activeTab = tab;
  }

  private markFormGroupTouched(formGroup: FormGroup | FormArray) {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control.markAsTouched();
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markFormGroupTouched(control);
      }
    });
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.profileForm.dirty && this.saveStatus !== 'saved') {
      return confirm('You have unsaved changes. Do you want to leave?');
    }
    return true;
  }
}
```

---

## Image Upload Implementation

### MediaUploadFormComponent

```typescript
@Component({
  selector: 'app-media-upload-form',
  templateUrl: './media-upload-form.component.html'
})
export class MediaUploadFormComponent {
  @Input() profilePhoto: ImageUpload;
  @Input() coverPhoto: ImageUpload;
  @Input() logo: ImageUpload;
  @Output() mediaChange = new EventEmitter<MediaInfo>();

  uploadProgress: { [key: string]: number } = {};
  showCropperModal = false;
  currentImage: File;
  currentCategory: 'profile' | 'cover' | 'logo';
  cropperSettings: any;

  constructor(
    private mediaService: MediaUploadService,
    private snackBar: MatSnackBar
  ) {}

  onFileSelected(event: Event, category: 'profile' | 'cover' | 'logo') {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];

      const validation = this.mediaService.validateImageFile(file);
      if (!validation.valid) {
        this.snackBar.open(validation.error, 'Close', { duration: 3000 });
        return;
      }

      this.currentImage = file;
      this.currentCategory = category;
      this.setCropperSettings(category);
      this.showCropperModal = true;
    }
  }

  private setCropperSettings(category: string) {
    switch (category) {
      case 'profile':
        this.cropperSettings = {
          aspectRatio: 1,
          resizeToWidth: 400,
          cropperMinWidth: 200
        };
        break;
      case 'cover':
        this.cropperSettings = {
          aspectRatio: 21 / 9,
          resizeToWidth: 1920,
          cropperMinWidth: 800
        };
        break;
      case 'logo':
        this.cropperSettings = {
          aspectRatio: 1,
          resizeToWidth: 300,
          cropperMinWidth: 150
        };
        break;
    }
  }

  onImageCropped(cropData: CropData) {
    this.showCropperModal = false;
    this.uploadImage(this.currentImage, this.currentCategory, cropData);
  }

  private uploadImage(file: File, category: string, cropData: CropData) {
    this.uploadProgress[category] = 0;

    this.mediaService.uploadImage(file, category as any, cropData).subscribe({
      next: (event) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadProgress[category] = Math.round(
            (100 * event.loaded) / event.total
          );
        } else if (event instanceof HttpResponse) {
          const uploadedImage = event.body as ImageUpload;
          this.updateMedia(category, uploadedImage);
          delete this.uploadProgress[category];
          this.snackBar.open('Image uploaded successfully', 'Close', {
            duration: 3000
          });
        }
      },
      error: (error) => {
        delete this.uploadProgress[category];
        this.snackBar.open('Upload failed', 'Close', { duration: 3000 });
      }
    });
  }

  deleteImage(category: 'profile' | 'cover' | 'logo') {
    const image = this[`${category}Photo`] as ImageUpload;
    if (image && confirm('Are you sure you want to delete this image?')) {
      this.mediaService.deleteImage(image.id).subscribe({
        next: () => {
          this.updateMedia(category, null);
          this.snackBar.open('Image deleted', 'Close', { duration: 3000 });
        },
        error: () => {
          this.snackBar.open('Delete failed', 'Close', { duration: 3000 });
        }
      });
    }
  }

  private updateMedia(category: string, image: ImageUpload) {
    const mediaInfo: MediaInfo = {
      profilePhoto: this.profilePhoto,
      coverPhoto: this.coverPhoto,
      logo: this.logo
    };
    mediaInfo[`${category}Photo`] = image;
    this.mediaChange.emit(mediaInfo);
  }
}
```

---

## Validation Rules

### Form Field Validation

| Field | Rules | Error Messages |
|-------|-------|----------------|
| Business Name | Required, Max 100 chars | "Business name is required" |
| First Name | Required, Max 50 chars | "First name is required" |
| Last Name | Required, Max 50 chars | "Last name is required" |
| Title | Max 100 chars | - |
| Business Type | Required | "Please select a business type" |
| Description | Required, Max 1000 chars | "Description is required" |
| Email | Required, Valid email | "Valid email required" |
| Phone | Required, Format: (555) 123-4567 | "Invalid phone format" |
| Website | Valid URL (optional) | "Must be a valid URL" |
| Street | Required | "Street address is required" |
| City | Required | "City is required" |
| State | Required | "State is required" |
| ZIP Code | Required, 5 digits | "5-digit ZIP required" |

### Custom Validators

```typescript
export class ProfileValidators {
  static phoneNumber(control: AbstractControl): ValidationErrors | null {
    const phoneRegex = /^\(\d{3}\) \d{3}-\d{4}$/;
    return phoneRegex.test(control.value) ? null : { invalidPhone: true };
  }

  static url(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;
    const urlRegex = /^https?:\/\/.+\..+/;
    return urlRegex.test(control.value) ? null : { invalidUrl: true };
  }

  static zipCode(control: AbstractControl): ValidationErrors | null {
    const zipRegex = /^\d{5}$/;
    return zipRegex.test(control.value) ? null : { invalidZip: true };
  }

  static bioMaxLength(max: number) {
    return (control: AbstractControl): ValidationErrors | null => {
      const textContent = stripHtml(control.value);
      return textContent.length <= max ? null : { maxLengthExceeded: true };
    };
  }
}

function stripHtml(html: string): string {
  const tmp = document.createElement('DIV');
  tmp.innerHTML = html;
  return tmp.textContent || tmp.innerText || '';
}
```

---

## State Management

### Profile State (NgRx - Optional)

```typescript
// profile.state.ts
export interface ProfileState {
  profile: ProfileFormData | null;
  loading: boolean;
  saving: boolean;
  error: string | null;
  completionPercentage: number;
  hasUnsavedChanges: boolean;
}

// profile.actions.ts
export const loadProfile = createAction('[Profile] Load Profile');
export const loadProfileSuccess = createAction(
  '[Profile] Load Profile Success',
  props<{ profile: ProfileFormData }>()
);
export const saveDraft = createAction(
  '[Profile] Save Draft',
  props<{ profile: ProfileFormData }>()
);
export const publishProfile = createAction(
  '[Profile] Publish Profile',
  props<{ profile: ProfileFormData }>()
);
export const updateCompletion = createAction(
  '[Profile] Update Completion',
  props<{ percentage: number }>()
);

// profile.reducer.ts
export const profileReducer = createReducer(
  initialState,
  on(loadProfile, state => ({ ...state, loading: true })),
  on(loadProfileSuccess, (state, { profile }) => ({
    ...state,
    profile,
    loading: false
  })),
  on(saveDraft, state => ({ ...state, saving: true })),
  // ... more reducers
);

// profile.effects.ts
@Injectable()
export class ProfileEffects {
  loadProfile$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadProfile),
      switchMap(() =>
        this.profileService.getProfile().pipe(
          map(profile => loadProfileSuccess({ profile })),
          catchError(error => of(loadProfileFailure({ error })))
        )
      )
    )
  );

  constructor(
    private actions$: Actions,
    private profileService: ProfileService
  ) {}
}
```

---

## Routing & Guards

### Route Configuration

```typescript
const routes: Routes = [
  {
    path: 'dashboard/profile',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'edit',
        component: ProfileManagementComponent,
        canDeactivate: [UnsavedChangesGuard]
      },
      {
        path: '',
        component: ProfileViewComponent
      }
    ]
  }
];
```

### Unsaved Changes Guard

```typescript
@Injectable({
  providedIn: 'root'
})
export class UnsavedChangesGuard implements CanDeactivate<ProfileManagementComponent> {
  canDeactivate(
    component: ProfileManagementComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    return component.canDeactivate();
  }
}
```

---

## Integration Points

### Third-Party Libraries

1. **Rich Text Editor**
   - Library: `ngx-quill` or `@angular/cdk`
   - NPM: `npm install ngx-quill quill`
   - Configuration: Quill toolbar with bold, italic, underline, lists, links

2. **Image Cropper**
   - Library: `ngx-image-cropper`
   - NPM: `npm install ngx-image-cropper`
   - Features: Crop, zoom, rotate

3. **Address Autocomplete**
   - Library: Google Places API or `ngx-google-places-autocomplete`
   - Configuration: API key in environment

4. **Map Display**
   - Library: `@angular/google-maps`
   - NPM: `npm install @angular/google-maps`
   - Features: Display location pin

### API Endpoints

```typescript
// GET - Load profile
GET /api/professionals/me/profile
Response: ProfileFormData

// PUT - Save draft
PUT /api/professionals/me/profile/draft
Body: ProfileFormData
Response: ProfileFormData

// PUT - Publish profile
PUT /api/professionals/me/profile/publish
Body: ProfileFormData
Response: ProfileFormData

// POST - Upload image
POST /api/media/upload
Body: FormData (file, category, cropData)
Response: ImageUpload

// DELETE - Delete image
DELETE /api/media/:imageId
Response: void

// POST - Validate address
POST /api/professionals/validate-address
Body: Partial<LocationInfo>
Response: AddressValidation

// GET - Geocode address
GET /api/professionals/geocode?address=...
Response: Coordinates
```

---

## Accessibility Requirements

### WCAG 2.1 AA Compliance

1. **Keyboard Navigation**
   - All form fields accessible via Tab
   - Tab navigation follows logical order
   - Escape key closes modals

2. **ARIA Labels**
   - All form inputs have proper labels
   - Error messages linked via `aria-describedby`
   - Modal has `role="dialog"` and `aria-modal="true"`

3. **Focus Management**
   - Focus trapped in modal when open
   - Focus returns to trigger element on close
   - Visible focus indicators on all interactive elements

4. **Screen Reader Support**
   - Form validation errors announced
   - Image upload status announced
   - Auto-save status announced

### Implementation

```html
<!-- Example: Accessible form field -->
<div class="form-field">
  <label for="businessName" class="form-field__label">
    Business Name
    <span aria-label="required">*</span>
  </label>
  <input
    id="businessName"
    type="text"
    class="form-field__input"
    [class.form-field__input--error]="hasError('businessName')"
    formControlName="businessName"
    aria-required="true"
    [attr.aria-invalid]="hasError('businessName')"
    [attr.aria-describedby]="hasError('businessName') ? 'businessName-error' : null"
  >
  <div
    *ngIf="hasError('businessName')"
    id="businessName-error"
    class="form-field__error"
    role="alert"
  >
    <svg aria-hidden="true">...</svg>
    Business name is required
  </div>
</div>
```

---

## Performance Optimization

### Lazy Loading

```typescript
// Feature module lazy loaded
const routes: Routes = [
  {
    path: 'profile',
    loadChildren: () => import('./profile-management/profile-management.module')
      .then(m => m.ProfileManagementModule)
  }
];
```

### Image Optimization

1. **Client-side compression** before upload
2. **Progressive upload** with chunks for large files
3. **Thumbnail generation** on server
4. **CDN delivery** for uploaded images

### Change Detection

```typescript
@Component({
  selector: 'app-media-upload-form',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MediaUploadFormComponent {
  // Use OnPush for better performance
}
```

### Debouncing

```typescript
// Auto-save debounced
this.profileForm.valueChanges.pipe(
  debounceTime(2000),
  distinctUntilChanged()
).subscribe(value => this.autoSave(value));

// Address validation debounced
this.locationForm.get('street').valueChanges.pipe(
  debounceTime(500),
  switchMap(value => this.validateAddress(value))
).subscribe(result => this.handleValidation(result));
```

---

## Testing Strategy

### Unit Tests

```typescript
describe('ProfileManagementComponent', () => {
  let component: ProfileManagementComponent;
  let fixture: ComponentFixture<ProfileManagementComponent>;
  let profileService: jasmine.SpyObj<ProfileService>;

  beforeEach(async () => {
    const profileServiceSpy = jasmine.createSpyObj('ProfileService', [
      'getProfile',
      'saveDraft',
      'publishProfile'
    ]);

    await TestBed.configureTestingModule({
      declarations: [ProfileManagementComponent],
      imports: [ReactiveFormsModule, HttpClientTestingModule],
      providers: [
        { provide: ProfileService, useValue: profileServiceSpy }
      ]
    }).compileComponents();

    profileService = TestBed.inject(ProfileService) as jasmine.SpyObj<ProfileService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load profile on init', () => {
    const mockProfile: ProfileFormData = { /* ... */ };
    profileService.getProfile.and.returnValue(of(mockProfile));

    component.ngOnInit();

    expect(profileService.getProfile).toHaveBeenCalled();
    expect(component.profileForm.value).toEqual(mockProfile);
  });

  it('should validate required fields', () => {
    const businessNameControl = component.profileForm.get('basic.businessName');
    businessNameControl.setValue('');

    expect(businessNameControl.valid).toBeFalse();
    expect(businessNameControl.hasError('required')).toBeTrue();
  });

  it('should add phone number', () => {
    const initialLength = component.phonesArray.length;
    component.addPhone();

    expect(component.phonesArray.length).toBe(initialLength + 1);
  });

  it('should not remove last phone number', () => {
    while (component.phonesArray.length > 1) {
      component.removePhone(0);
    }
    component.removePhone(0);

    expect(component.phonesArray.length).toBe(1);
  });
});
```

### E2E Tests

```typescript
describe('Profile Management E2E', () => {
  it('should complete profile workflow', () => {
    cy.visit('/dashboard/profile/edit');

    // Fill basic info
    cy.get('#businessName').type('Smith Auto Repair');
    cy.get('#firstName').type('John');
    cy.get('#lastName').type('Smith');

    // Switch to bio tab
    cy.get('[data-tab="about"]').click();
    cy.get('.rich-text-content').type('Professional auto repair...');

    // Upload profile photo
    cy.get('[data-tab="media"]').click();
    cy.get('input[type="file"]').first().selectFile('fixtures/profile.jpg');
    cy.get('.btn--primary').contains('Apply').click();

    // Save draft
    cy.get('.btn--secondary').contains('Save Draft').click();
    cy.get('.snackbar').should('contain', 'Draft saved');

    // Publish
    cy.get('.btn--primary').contains('Publish').click();
    cy.url().should('include', '/dashboard/profile');
  });
});
```

---

## Error Handling

### Error Messages

```typescript
export const PROFILE_ERROR_MESSAGES = {
  LOAD_FAILED: 'Failed to load profile. Please try again.',
  SAVE_FAILED: 'Failed to save changes. Please try again.',
  PUBLISH_FAILED: 'Failed to publish profile. Please try again.',
  UPLOAD_FAILED: 'Image upload failed. Please try again.',
  VALIDATION_FAILED: 'Please fix all validation errors before saving.',
  NETWORK_ERROR: 'Network error. Please check your connection.',
  UNAUTHORIZED: 'You are not authorized to perform this action.'
};
```

### Error Handling Service

```typescript
@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {
  handleError(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return PROFILE_ERROR_MESSAGES.NETWORK_ERROR;
    } else if (error.status === 401 || error.status === 403) {
      return PROFILE_ERROR_MESSAGES.UNAUTHORIZED;
    } else if (error.status === 400) {
      return error.error?.message || PROFILE_ERROR_MESSAGES.VALIDATION_FAILED;
    } else {
      return error.error?.message || PROFILE_ERROR_MESSAGES.SAVE_FAILED;
    }
  }
}
```

---

## Mobile Responsiveness

### Breakpoints

- **Mobile**: < 576px
- **Tablet**: 576px - 991px
- **Desktop**: > 991px

### Mobile Adaptations

1. **Tabbed Navigation**: Horizontal scrolling tabs on mobile
2. **Stacked Layout**: Two-column forms become single column
3. **Touch Targets**: Minimum 48px tap targets
4. **Modal Behavior**: Full-screen modals on mobile
5. **Image Upload**: Camera access on mobile devices

```typescript
// Mobile camera access
onMobileImageCapture(event: Event, category: string) {
  const input = event.target as HTMLInputElement;
  input.setAttribute('capture', 'environment');
  input.click();
}
```

---

## Security Considerations

### Input Sanitization

```typescript
import { DomSanitizer } from '@angular/platform-browser';

constructor(private sanitizer: DomSanitizer) {}

sanitizeBio(html: string): SafeHtml {
  return this.sanitizer.sanitize(SecurityContext.HTML, html);
}
```

### File Upload Security

1. **File type validation** (client and server)
2. **File size limits** (5MB max)
3. **Virus scanning** on server
4. **Secure file storage** with unique IDs
5. **CORS configuration** for CDN

### CSRF Protection

```typescript
// HttpClient automatically includes CSRF token
this.http.post(url, data, {
  withCredentials: true
});
```

---

## Deployment Checklist

- [ ] Environment variables configured
- [ ] API endpoints verified
- [ ] Image upload CDN configured
- [ ] Google Maps API key added
- [ ] Error tracking enabled (Sentry)
- [ ] Analytics tracking added
- [ ] Performance monitoring enabled
- [ ] Accessibility audit passed
- [ ] Security audit passed
- [ ] E2E tests passing
- [ ] Documentation updated
