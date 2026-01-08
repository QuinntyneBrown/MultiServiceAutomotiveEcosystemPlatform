import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ProfileManagement, Profile } from './profile-management';
import { vi } from 'vitest';

describe('ProfileManagement', () => {
  let component: ProfileManagement;
  let fixture: ComponentFixture<ProfileManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileManagement, ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    component.ngOnDestroy();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
    it('should initialize with basic tab active', () => {
      expect(component.activeTab()).toBe('basic');
    });

    it('should initialize in create mode by default', () => {
      expect(component.mode).toBe('create');
    });

    it('should initialize form with empty values', () => {
      expect(component.profileForm.value.businessName).toBe('');
      expect(component.profileForm.value.personalName).toBe('');
    });

    it('should have one phone number by default', () => {
      expect(component.phoneNumbers.length).toBe(1);
    });

    it('should have one email by default', () => {
      expect(component.emails.length).toBe(1);
    });

    it('should populate form when initialData is provided', () => {
      component.initialData = {
        businessName: 'Test Auto Shop',
        businessType: 'auto-repair',
        personalName: 'John Doe'
      };
      component.ngOnInit();

      expect(component.profileForm.value.businessName).toBe('Test Auto Shop');
      expect(component.profileForm.value.personalName).toBe('John Doe');
    });
  });

  describe('Tab Navigation', () => {
    it('should switch to about tab', () => {
      component.switchTab('about');
      expect(component.activeTab()).toBe('about');
    });

    it('should switch to contact tab', () => {
      component.switchTab('contact');
      expect(component.activeTab()).toBe('contact');
    });

    it('should switch to location tab', () => {
      component.switchTab('location');
      expect(component.activeTab()).toBe('location');
    });

    it('should switch to media tab', () => {
      component.switchTab('media');
      expect(component.activeTab()).toBe('media');
    });
  });

  describe('Form Validation', () => {
    it('should require business name', () => {
      const control = component.profileForm.get('businessName');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate business name min length', () => {
      const control = component.profileForm.get('businessName');
      control?.setValue('A');
      expect(control?.hasError('minlength')).toBeTruthy();
    });

    it('should require personal name', () => {
      const control = component.profileForm.get('personalName');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should require bio', () => {
      const control = component.profileForm.get('bio');
      expect(control?.valid).toBeFalsy();
      expect(control?.hasError('required')).toBeTruthy();
    });

    it('should validate bio minimum length', () => {
      const control = component.profileForm.get('bio');
      control?.setValue('Short bio');
      expect(control?.hasError('minlength')).toBeTruthy();

      control?.setValue('a'.repeat(50));
      expect(control?.hasError('minlength')).toBeFalsy();
    });

    it('should validate website URL pattern', () => {
      const control = component.profileForm.get('website');
      control?.setValue('invalid-url');
      expect(control?.hasError('pattern')).toBeTruthy();

      control?.setValue('https://www.example.com');
      expect(control?.valid).toBeTruthy();
    });

    it('should validate ZIP code pattern', () => {
      const control = component.profileForm.get('address.zipCode');
      control?.setValue('123');
      expect(control?.hasError('pattern')).toBeTruthy();

      control?.setValue('12345');
      expect(control?.valid).toBeTruthy();
    });
  });

  describe('Phone Numbers', () => {
    it('should add phone number', () => {
      component.addPhone();
      expect(component.phoneNumbers.length).toBe(2);
    });

    it('should not add more than 5 phone numbers', () => {
      for (let i = 0; i < 10; i++) {
        component.addPhone();
      }
      expect(component.phoneNumbers.length).toBe(5);
    });

    it('should remove phone number', () => {
      component.addPhone();
      expect(component.phoneNumbers.length).toBe(2);
      component.removePhone(1);
      expect(component.phoneNumbers.length).toBe(1);
    });

    it('should not remove last phone number', () => {
      component.removePhone(0);
      expect(component.phoneNumbers.length).toBe(1);
    });

    it('should format phone number', () => {
      const event = {
        target: { value: '5551234567' }
      } as unknown as Event;

      component.formatPhoneNumber(event, 0);
      expect(component.phoneNumbers.at(0).value.number).toBe('(555) 123-4567');
    });
  });

  describe('Emails', () => {
    it('should add email', () => {
      component.addEmail();
      expect(component.emails.length).toBe(2);
    });

    it('should not add more than 5 emails', () => {
      for (let i = 0; i < 10; i++) {
        component.addEmail();
      }
      expect(component.emails.length).toBe(5);
    });

    it('should remove email', () => {
      component.addEmail();
      expect(component.emails.length).toBe(2);
      component.removeEmail(1);
      expect(component.emails.length).toBe(1);
    });

    it('should not remove last email', () => {
      component.removeEmail(0);
      expect(component.emails.length).toBe(1);
    });
  });

  describe('Save Draft', () => {
    it('should not save draft without business name', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.saveDraft();
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('should save draft with business name', async () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.profileForm.patchValue({ businessName: 'Test Shop' });

      await component.saveDraft();

      expect(saveSpy).toHaveBeenCalledWith(expect.objectContaining({
        isDraft: true,
        profile: expect.objectContaining({ status: 'draft' })
      }));
    });

    it('should update save status', async () => {
      component.profileForm.patchValue({ businessName: 'Test Shop' });

      const promise = component.saveDraft();
      expect(component.isSavingDraft()).toBe(true);

      await promise;
      expect(component.autoSaveStatus()).toBe('saved');
    });
  });

  describe('Publish', () => {
    it('should not publish invalid form', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.publish();
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('should publish valid form', async () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      fillValidForm(component);

      await component.publish();

      expect(saveSpy).toHaveBeenCalledWith(expect.objectContaining({
        isDraft: false,
        profile: expect.objectContaining({ status: 'published' })
      }));
    });

    it('should mark all fields as touched on invalid submit', () => {
      component.publish();
      expect(component.profileForm.get('businessName')?.touched).toBeTruthy();
    });
  });

  describe('Preview', () => {
    it('should emit preview event', () => {
      const previewSpy = vi.spyOn(component.preview, 'emit');
      component.profileForm.patchValue({ businessName: 'Test' });
      component.onPreview();
      expect(previewSpy).toHaveBeenCalled();
    });
  });

  describe('Cancel', () => {
    it('should emit cancel event', () => {
      const cancelSpy = vi.spyOn(component.cancel, 'emit');
      component.onCancel();
      expect(cancelSpy).toHaveBeenCalled();
    });
  });

  describe('File Upload', () => {
    it('should handle profile photo selection', () => {
      const mockFile = new File(['test'], 'test.jpg', { type: 'image/jpeg' });
      const event = {
        target: { files: [mockFile] }
      } as unknown as Event;

      const reader = new FileReader();
      vi.spyOn(window, 'FileReader').mockImplementation(() => ({
        readAsDataURL: function() {
          this.result = 'data:image/jpeg;base64,test';
          this.onload?.();
        },
        result: ''
      } as any));

      component.onFileSelected(event, 'profilePhoto');
    });

    it('should remove file', () => {
      component.profileForm.patchValue({
        profilePhoto: { url: 'test.jpg', fileName: 'test.jpg', fileSize: 1000, mimeType: 'image/jpeg' }
      });

      component.removeFile('profilePhoto');
      expect(component.profileForm.get('profilePhoto')?.value).toBeNull();
    });
  });

  describe('Error Messages', () => {
    it('should return error for required field', () => {
      const control = component.profileForm.get('businessName');
      control?.markAsTouched();
      expect(component.getError('businessName')).toBe('This field is required');
    });

    it('should return null for valid field', () => {
      const control = component.profileForm.get('businessName');
      control?.setValue('Valid Business Name');
      expect(component.getError('businessName')).toBeNull();
    });
  });

  describe('Bio Character Count', () => {
    it('should return bio character count', () => {
      component.profileForm.patchValue({ bio: 'Hello World' });
      expect(component.bioCharCount).toBe(11);
    });

    it('should return 0 for empty bio', () => {
      expect(component.bioCharCount).toBe(0);
    });
  });

  describe('Business Types', () => {
    it('should have all business types', () => {
      expect(component.businessTypes.length).toBeGreaterThan(0);
    });

    it('should include auto-repair type', () => {
      const autoRepair = component.businessTypes.find(t => t.value === 'auto-repair');
      expect(autoRepair?.label).toBe('Auto Repair Shop');
    });
  });

  describe('States List', () => {
    it('should have 50 states', () => {
      expect(component.states.length).toBe(50);
    });
  });
});

function fillValidForm(component: ProfileManagement) {
  component.profileForm.patchValue({
    businessName: 'Test Auto Shop',
    businessType: 'auto-repair',
    personalName: 'John Doe',
    bio: 'This is a detailed description of our auto repair shop with more than fifty characters to pass validation.',
    address: {
      street: '123 Main Street',
      city: 'Springfield',
      state: 'IL',
      zipCode: '62701'
    }
  });

  component.phoneNumbers.at(0).patchValue({
    number: '(555) 123-4567'
  });

  component.emails.at(0).patchValue({
    email: 'test@example.com'
  });
}
