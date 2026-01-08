import { Component, Input, Output, EventEmitter, signal, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';

export type BusinessType = 'auto-repair' | 'body-shop' | 'detailing' | 'towing' | 'tire-shop' | 'parts-supplier' | 'dealer' | 'mobile-mechanic' | 'other';

export interface ContactPhone {
  id: string;
  type: 'mobile' | 'business' | 'fax';
  number: string;
  isPrimary: boolean;
}

export interface ContactEmail {
  id: string;
  type: 'primary' | 'billing' | 'support';
  email: string;
  isPrimary: boolean;
}

export interface Address {
  street: string;
  addressLine2?: string;
  city: string;
  state: string;
  zipCode: string;
  country?: string;
}

export interface ServiceRadius {
  value: number;
  unit: 'miles' | 'km';
}

export interface MediaFile {
  id?: string;
  url: string;
  fileName: string;
  fileSize: number;
  mimeType: string;
  width?: number;
  height?: number;
}

export interface Profile {
  id?: string;
  businessName: string;
  businessType: BusinessType;
  personalName: string;
  professionalTitle?: string;
  bio: string;
  yearsInBusiness?: number;
  numberOfEmployees?: number;
  phoneNumbers: ContactPhone[];
  emails: ContactEmail[];
  website?: string;
  address: Address;
  serviceRadius?: ServiceRadius;
  profilePhoto?: MediaFile;
  coverPhoto?: MediaFile;
  logo?: MediaFile;
  status: 'draft' | 'published' | 'archived';
  createdAt?: Date;
  updatedAt?: Date;
  publishedAt?: Date;
}

/**
 * Profile Management Interface Component (REQ-SP-F006)
 *
 * A comprehensive form for service professionals to create and manage their business profiles.
 * Includes multiple sections for basic info, contact details, location, and media uploads.
 *
 * @example
 * ```html
 * <ms-profile-management
 *   [mode]="'create'"
 *   (save)="handleSave($event)"
 *   (preview)="handlePreview($event)">
 * </ms-profile-management>
 * ```
 */
@Component({
  selector: 'ms-profile-management',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile-management.html',
  styleUrl: './profile-management.scss',
})
export class ProfileManagement implements OnInit, OnDestroy {
  /** Profile ID for editing */
  @Input() profileId?: string;

  /** Form mode */
  @Input() mode: 'create' | 'edit' = 'create';

  /** Initial data for editing */
  @Input() initialData?: Partial<Profile>;

  /** Auto-save interval in milliseconds */
  @Input() autoSaveInterval = 30000;

  @Output() save = new EventEmitter<{ profile: Profile; isDraft: boolean }>();
  @Output() cancel = new EventEmitter<void>();
  @Output() preview = new EventEmitter<Profile>();

  profileForm!: FormGroup;
  activeTab = signal<'basic' | 'about' | 'contact' | 'location' | 'media'>('basic');
  isSubmitting = signal(false);
  isSavingDraft = signal(false);
  autoSaveStatus = signal<'idle' | 'saving' | 'saved' | 'error'>('idle');
  lastSaved = signal<Date | null>(null);

  private autoSaveTimer?: ReturnType<typeof setInterval>;

  readonly businessTypes: { value: BusinessType; label: string }[] = [
    { value: 'auto-repair', label: 'Auto Repair Shop' },
    { value: 'body-shop', label: 'Body Shop' },
    { value: 'detailing', label: 'Detailing Service' },
    { value: 'towing', label: 'Towing Service' },
    { value: 'tire-shop', label: 'Tire Shop' },
    { value: 'parts-supplier', label: 'Parts Supplier' },
    { value: 'dealer', label: 'Dealership' },
    { value: 'mobile-mechanic', label: 'Mobile Mechanic' },
    { value: 'other', label: 'Other' }
  ];

  readonly states = [
    { code: 'AL', name: 'Alabama' }, { code: 'AK', name: 'Alaska' },
    { code: 'AZ', name: 'Arizona' }, { code: 'AR', name: 'Arkansas' },
    { code: 'CA', name: 'California' }, { code: 'CO', name: 'Colorado' },
    { code: 'CT', name: 'Connecticut' }, { code: 'DE', name: 'Delaware' },
    { code: 'FL', name: 'Florida' }, { code: 'GA', name: 'Georgia' },
    { code: 'HI', name: 'Hawaii' }, { code: 'ID', name: 'Idaho' },
    { code: 'IL', name: 'Illinois' }, { code: 'IN', name: 'Indiana' },
    { code: 'IA', name: 'Iowa' }, { code: 'KS', name: 'Kansas' },
    { code: 'KY', name: 'Kentucky' }, { code: 'LA', name: 'Louisiana' },
    { code: 'ME', name: 'Maine' }, { code: 'MD', name: 'Maryland' },
    { code: 'MA', name: 'Massachusetts' }, { code: 'MI', name: 'Michigan' },
    { code: 'MN', name: 'Minnesota' }, { code: 'MS', name: 'Mississippi' },
    { code: 'MO', name: 'Missouri' }, { code: 'MT', name: 'Montana' },
    { code: 'NE', name: 'Nebraska' }, { code: 'NV', name: 'Nevada' },
    { code: 'NH', name: 'New Hampshire' }, { code: 'NJ', name: 'New Jersey' },
    { code: 'NM', name: 'New Mexico' }, { code: 'NY', name: 'New York' },
    { code: 'NC', name: 'North Carolina' }, { code: 'ND', name: 'North Dakota' },
    { code: 'OH', name: 'Ohio' }, { code: 'OK', name: 'Oklahoma' },
    { code: 'OR', name: 'Oregon' }, { code: 'PA', name: 'Pennsylvania' },
    { code: 'RI', name: 'Rhode Island' }, { code: 'SC', name: 'South Carolina' },
    { code: 'SD', name: 'South Dakota' }, { code: 'TN', name: 'Tennessee' },
    { code: 'TX', name: 'Texas' }, { code: 'UT', name: 'Utah' },
    { code: 'VT', name: 'Vermont' }, { code: 'VA', name: 'Virginia' },
    { code: 'WA', name: 'Washington' }, { code: 'WV', name: 'West Virginia' },
    { code: 'WI', name: 'Wisconsin' }, { code: 'WY', name: 'Wyoming' }
  ];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.initializeForm();

    if (this.initialData) {
      this.populateForm(this.initialData);
    }

    if (this.mode === 'edit' && this.autoSaveInterval > 0) {
      this.startAutoSave();
    }
  }

  ngOnDestroy() {
    if (this.autoSaveTimer) {
      clearInterval(this.autoSaveTimer);
    }
  }

  private initializeForm() {
    this.profileForm = this.fb.group({
      businessName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      businessType: ['', [Validators.required]],
      personalName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      professionalTitle: ['', [Validators.maxLength(100)]],
      bio: ['', [Validators.required, Validators.minLength(50), Validators.maxLength(2000)]],
      yearsInBusiness: [null, [Validators.min(0), Validators.max(200)]],
      numberOfEmployees: [null, [Validators.min(1), Validators.max(10000)]],
      phoneNumbers: this.fb.array([this.createPhoneGroup()]),
      emails: this.fb.array([this.createEmailGroup()]),
      website: ['', [Validators.pattern(/^https?:\/\/.+\..+$/)]],
      address: this.fb.group({
        street: ['', [Validators.required, Validators.maxLength(200)]],
        addressLine2: ['', [Validators.maxLength(100)]],
        city: ['', [Validators.required, Validators.maxLength(100)]],
        state: ['', [Validators.required]],
        zipCode: ['', [Validators.required, Validators.pattern(/^\d{5}(-\d{4})?$/)]],
        country: ['US']
      }),
      serviceRadius: this.fb.group({
        value: [null, [Validators.min(0), Validators.max(500)]],
        unit: ['miles']
      }),
      profilePhoto: [null],
      coverPhoto: [null],
      logo: [null]
    });
  }

  private createPhoneGroup(): FormGroup {
    return this.fb.group({
      id: [this.generateId()],
      type: ['business'],
      number: ['', [Validators.required, Validators.pattern(/^\(\d{3}\) \d{3}-\d{4}$/)]],
      isPrimary: [false]
    });
  }

  private createEmailGroup(): FormGroup {
    return this.fb.group({
      id: [this.generateId()],
      type: ['primary'],
      email: ['', [Validators.required, Validators.email]],
      isPrimary: [false]
    });
  }

  private generateId(): string {
    return Math.random().toString(36).substring(2, 9);
  }

  private populateForm(data: Partial<Profile>) {
    this.profileForm.patchValue({
      businessName: data.businessName,
      businessType: data.businessType,
      personalName: data.personalName,
      professionalTitle: data.professionalTitle,
      bio: data.bio,
      yearsInBusiness: data.yearsInBusiness,
      numberOfEmployees: data.numberOfEmployees,
      website: data.website,
      address: data.address,
      serviceRadius: data.serviceRadius,
      profilePhoto: data.profilePhoto,
      coverPhoto: data.coverPhoto,
      logo: data.logo
    });

    if (data.phoneNumbers?.length) {
      const phonesArray = this.profileForm.get('phoneNumbers') as FormArray;
      phonesArray.clear();
      data.phoneNumbers.forEach(phone => {
        const group = this.createPhoneGroup();
        group.patchValue(phone);
        phonesArray.push(group);
      });
    }

    if (data.emails?.length) {
      const emailsArray = this.profileForm.get('emails') as FormArray;
      emailsArray.clear();
      data.emails.forEach(email => {
        const group = this.createEmailGroup();
        group.patchValue(email);
        emailsArray.push(group);
      });
    }
  }

  private startAutoSave() {
    this.autoSaveTimer = setInterval(() => {
      if (this.profileForm.dirty && !this.isSubmitting()) {
        this.saveDraft();
      }
    }, this.autoSaveInterval);
  }

  get phoneNumbers(): FormArray {
    return this.profileForm.get('phoneNumbers') as FormArray;
  }

  get emails(): FormArray {
    return this.profileForm.get('emails') as FormArray;
  }

  addPhone() {
    if (this.phoneNumbers.length < 5) {
      this.phoneNumbers.push(this.createPhoneGroup());
    }
  }

  removePhone(index: number) {
    if (this.phoneNumbers.length > 1) {
      this.phoneNumbers.removeAt(index);
    }
  }

  addEmail() {
    if (this.emails.length < 5) {
      this.emails.push(this.createEmailGroup());
    }
  }

  removeEmail(index: number) {
    if (this.emails.length > 1) {
      this.emails.removeAt(index);
    }
  }

  formatPhoneNumber(event: Event, index: number) {
    const input = event.target as HTMLInputElement;
    let value = input.value.replace(/\D/g, '');

    if (value.length >= 10) {
      value = value.substring(0, 10);
    }

    if (value.length >= 6) {
      input.value = `(${value.substring(0, 3)}) ${value.substring(3, 6)}-${value.substring(6)}`;
    } else if (value.length >= 3) {
      input.value = `(${value.substring(0, 3)}) ${value.substring(3)}`;
    } else if (value.length > 0) {
      input.value = `(${value}`;
    }

    this.phoneNumbers.at(index).patchValue({ number: input.value });
  }

  switchTab(tab: 'basic' | 'about' | 'contact' | 'location' | 'media') {
    this.activeTab.set(tab);
  }

  async saveDraft() {
    const businessName = this.profileForm.get('businessName')?.value;
    if (!businessName) return;

    this.isSavingDraft.set(true);
    this.autoSaveStatus.set('saving');

    try {
      const formValue = this.profileForm.value;
      const profile: Profile = {
        ...formValue,
        id: this.profileId,
        status: 'draft',
        phoneNumbers: this.phoneNumbers.value,
        emails: this.emails.value
      };

      this.save.emit({ profile, isDraft: true });
      this.autoSaveStatus.set('saved');
      this.lastSaved.set(new Date());
      this.profileForm.markAsPristine();
    } catch {
      this.autoSaveStatus.set('error');
    } finally {
      this.isSavingDraft.set(false);
    }
  }

  async publish() {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      this.highlightFirstError();
      return;
    }

    this.isSubmitting.set(true);

    try {
      const formValue = this.profileForm.value;
      const profile: Profile = {
        ...formValue,
        id: this.profileId,
        status: 'published',
        phoneNumbers: this.phoneNumbers.value,
        emails: this.emails.value,
        publishedAt: new Date()
      };

      this.save.emit({ profile, isDraft: false });
    } finally {
      this.isSubmitting.set(false);
    }
  }

  private highlightFirstError() {
    const controls = this.profileForm.controls;
    for (const key of Object.keys(controls)) {
      if (controls[key].invalid) {
        if (key === 'businessName' || key === 'businessType' || key === 'personalName' || key === 'professionalTitle') {
          this.activeTab.set('basic');
        } else if (key === 'bio' || key === 'yearsInBusiness' || key === 'numberOfEmployees') {
          this.activeTab.set('about');
        } else if (key === 'phoneNumbers' || key === 'emails' || key === 'website') {
          this.activeTab.set('contact');
        } else if (key === 'address') {
          this.activeTab.set('location');
        }
        break;
      }
    }
  }

  onPreview() {
    const formValue = this.profileForm.value;
    const profile: Profile = {
      ...formValue,
      id: this.profileId,
      status: 'draft',
      phoneNumbers: this.phoneNumbers.value,
      emails: this.emails.value
    };

    this.preview.emit(profile);
  }

  onCancel() {
    this.cancel.emit();
  }

  onFileSelected(event: Event, type: 'profilePhoto' | 'coverPhoto' | 'logo') {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        const mediaFile: MediaFile = {
          url: reader.result as string,
          fileName: file.name,
          fileSize: file.size,
          mimeType: file.type
        };
        this.profileForm.patchValue({ [type]: mediaFile });
      };
      reader.readAsDataURL(file);
    }
  }

  removeFile(type: 'profilePhoto' | 'coverPhoto' | 'logo') {
    this.profileForm.patchValue({ [type]: null });
  }

  getError(controlPath: string): string | null {
    const control = this.profileForm.get(controlPath);
    if (!control?.touched || !control?.errors) return null;

    if (control.hasError('required')) return 'This field is required';
    if (control.hasError('minlength')) return 'Value is too short';
    if (control.hasError('maxlength')) return 'Value is too long';
    if (control.hasError('email')) return 'Please enter a valid email';
    if (control.hasError('pattern')) {
      if (controlPath.includes('phone')) return 'Format: (555) 123-4567';
      if (controlPath.includes('zipCode')) return 'Format: 12345 or 12345-6789';
      if (controlPath.includes('website')) return 'Please enter a valid URL';
      return 'Invalid format';
    }
    if (control.hasError('min')) return 'Value is too low';
    if (control.hasError('max')) return 'Value is too high';
    return null;
  }

  get bioCharCount(): number {
    return this.profileForm.get('bio')?.value?.length || 0;
  }
}
