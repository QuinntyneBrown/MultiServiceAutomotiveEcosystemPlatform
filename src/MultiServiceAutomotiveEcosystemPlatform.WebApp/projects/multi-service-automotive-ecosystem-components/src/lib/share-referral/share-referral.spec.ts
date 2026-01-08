import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ShareReferral, UserInfo } from './share-referral';
import { vi } from 'vitest';

describe('ShareReferral', () => {
  let component: ShareReferral;
  let fixture: ComponentFixture<ShareReferral>;

  const mockUser: UserInfo = {
    id: '123',
    firstName: 'John',
    lastName: 'Doe',
    email: 'john@example.com'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShareReferral, ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ShareReferral);
    component = fixture.componentInstance;
    component.referralCode = 'ABC123';
    component.user = mockUser;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Initialization', () => {
    it('should initialize with copy-link tab by default', () => {
      expect(component.activeTab()).toBe('copy-link');
    });

    it('should generate referral URL if not provided', () => {
      expect(component.referralUrl).toBe('https://autoservice.com/r/ABC123');
    });

    it('should use provided referral URL', () => {
      component.referralUrl = 'https://custom.com/ref/XYZ';
      component.ngOnInit();
      expect(component.referralUrl).toBe('https://custom.com/ref/XYZ');
    });

    it('should initialize email form with default values', () => {
      expect(component.emailForm.value.subject).toContain('John invited you');
      expect(component.emailForm.value.message).toContain('AutoService Platform');
    });

    it('should initialize SMS form with default values', () => {
      expect(component.smsForm.value.message).toContain('AutoService');
    });
  });

  describe('Tab Navigation', () => {
    it('should switch to email tab', () => {
      component.switchTab('email');
      expect(component.activeTab()).toBe('email');
    });

    it('should switch to sms tab', () => {
      component.switchTab('sms');
      expect(component.activeTab()).toBe('sms');
    });

    it('should switch to qr-code tab', () => {
      component.switchTab('qr-code');
      expect(component.activeTab()).toBe('qr-code');
    });

    it('should reset copied state when switching tabs', () => {
      component.isCopied.set(true);
      component.switchTab('email');
      expect(component.isCopied()).toBe(false);
    });
  });

  describe('Copy Link', () => {
    it('should copy link to clipboard', async () => {
      const mockClipboard = {
        writeText: vi.fn().mockResolvedValue(undefined)
      };
      Object.assign(navigator, { clipboard: mockClipboard });

      await component.copyLink();

      expect(mockClipboard.writeText).toHaveBeenCalledWith(component.referralUrl);
      expect(component.isCopied()).toBe(true);
    });

    it('should emit shareSuccess on successful copy', async () => {
      const mockClipboard = {
        writeText: vi.fn().mockResolvedValue(undefined)
      };
      Object.assign(navigator, { clipboard: mockClipboard });

      const successSpy = vi.spyOn(component.shareSuccess, 'emit');

      await component.copyLink();

      expect(successSpy).toHaveBeenCalledWith(expect.objectContaining({
        method: 'copy-link'
      }));
    });

    it('should emit shareError on clipboard failure', async () => {
      const mockClipboard = {
        writeText: vi.fn().mockRejectedValue(new Error('Failed'))
      };
      Object.assign(navigator, { clipboard: mockClipboard });

      const errorSpy = vi.spyOn(component.shareError, 'emit');

      await component.copyLink();

      expect(errorSpy).toHaveBeenCalledWith(expect.objectContaining({
        method: 'copy-link',
        error: expect.objectContaining({ code: 'SERVICE_UNAVAILABLE' })
      }));
    });
  });

  describe('Email Form', () => {
    it('should validate email recipients', () => {
      const control = component.emailForm.get('recipients');
      control?.setValue('invalid');
      expect(control?.valid).toBeFalsy();

      control?.setValue('valid@example.com');
      expect(control?.valid).toBeTruthy();
    });

    it('should accept multiple comma-separated emails', () => {
      const control = component.emailForm.get('recipients');
      control?.setValue('one@example.com, two@example.com');
      expect(control?.valid).toBeTruthy();
    });

    it('should validate message max length', () => {
      const control = component.emailForm.get('message');
      control?.setValue('a'.repeat(2001));
      expect(control?.hasError('maxlength')).toBeTruthy();
    });

    it('should not submit invalid form', () => {
      const successSpy = vi.spyOn(component.shareSuccess, 'emit');
      component.emailForm.get('recipients')?.setValue('');
      component.sendEmail();
      expect(successSpy).not.toHaveBeenCalled();
    });

    it('should emit success on valid email submit', async () => {
      const successSpy = vi.spyOn(component.shareSuccess, 'emit');
      component.emailForm.patchValue({
        recipients: 'test@example.com',
        message: 'Test message'
      });

      await component.sendEmail();
      await new Promise(resolve => setTimeout(resolve, 1100));

      expect(successSpy).toHaveBeenCalledWith(expect.objectContaining({
        method: 'email',
        metadata: expect.objectContaining({ recipientCount: 1 })
      }));
    });

    it('should count multiple recipients correctly', async () => {
      const successSpy = vi.spyOn(component.shareSuccess, 'emit');
      component.emailForm.patchValue({
        recipients: 'one@example.com, two@example.com, three@example.com',
        message: 'Test'
      });

      await component.sendEmail();
      await new Promise(resolve => setTimeout(resolve, 1100));

      expect(successSpy).toHaveBeenCalledWith(expect.objectContaining({
        metadata: expect.objectContaining({ recipientCount: 3 })
      }));
    });
  });

  describe('SMS Form', () => {
    it('should validate phone number format', () => {
      const control = component.smsForm.get('phoneNumber');
      control?.setValue('1234567890');
      expect(control?.valid).toBeFalsy();

      control?.setValue('(555) 123-4567');
      expect(control?.valid).toBeTruthy();
    });

    it('should format phone number as user types', () => {
      const event = {
        target: { value: '5551234567' }
      } as unknown as Event;

      component.formatPhoneNumber(event);
      expect(component.smsForm.value.phoneNumber).toBe('(555) 123-4567');
    });

    it('should validate message max length for SMS', () => {
      const control = component.smsForm.get('message');
      control?.setValue('a'.repeat(161));
      expect(control?.hasError('maxlength')).toBeTruthy();
    });

    it('should not submit invalid SMS form', () => {
      const successSpy = vi.spyOn(component.shareSuccess, 'emit');
      component.smsForm.get('phoneNumber')?.setValue('');
      component.sendSms();
      expect(successSpy).not.toHaveBeenCalled();
    });
  });

  describe('Social Sharing', () => {
    beforeEach(() => {
      vi.spyOn(window, 'open').mockImplementation(() => null);
    });

    it('should open Facebook share dialog', () => {
      component.shareSocial('facebook');
      expect(window.open).toHaveBeenCalledWith(
        expect.stringContaining('facebook.com/sharer'),
        '_blank',
        expect.any(String)
      );
    });

    it('should open Twitter share dialog', () => {
      component.shareSocial('twitter');
      expect(window.open).toHaveBeenCalledWith(
        expect.stringContaining('twitter.com/intent/tweet'),
        '_blank',
        expect.any(String)
      );
    });

    it('should open WhatsApp share dialog', () => {
      component.shareSocial('whatsapp');
      expect(window.open).toHaveBeenCalledWith(
        expect.stringContaining('wa.me'),
        '_blank',
        expect.any(String)
      );
    });

    it('should open LinkedIn share dialog', () => {
      component.shareSocial('linkedin');
      expect(window.open).toHaveBeenCalledWith(
        expect.stringContaining('linkedin.com/sharing'),
        '_blank',
        expect.any(String)
      );
    });

    it('should emit shareSuccess for social share', () => {
      const successSpy = vi.spyOn(component.shareSuccess, 'emit');
      component.shareSocial('facebook');
      expect(successSpy).toHaveBeenCalledWith(expect.objectContaining({
        method: 'facebook',
        metadata: expect.objectContaining({ platform: 'facebook' })
      }));
    });
  });

  describe('QR Code', () => {
    it('should generate QR code URL', () => {
      expect(component.qrCodeDataUrl()).toContain('api.qrserver.com');
    });

    it('should download QR code as PNG', () => {
      const link = document.createElement('a');
      vi.spyOn(document, 'createElement').mockReturnValue(link);
      const clickSpy = vi.spyOn(link, 'click');

      component.downloadQR('png');

      expect(link.download).toContain('.png');
      expect(clickSpy).toHaveBeenCalled();
    });

    it('should download QR code as SVG', () => {
      const link = document.createElement('a');
      vi.spyOn(document, 'createElement').mockReturnValue(link);
      const clickSpy = vi.spyOn(link, 'click');

      component.downloadQR('svg');

      expect(link.download).toContain('.svg');
      expect(clickSpy).toHaveBeenCalled();
    });

    it('should emit shareSuccess on QR download', () => {
      const successSpy = vi.spyOn(component.shareSuccess, 'emit');
      vi.spyOn(document, 'createElement').mockReturnValue({
        href: '',
        download: '',
        click: () => {}
      } as any);

      component.downloadQR('png');

      expect(successSpy).toHaveBeenCalledWith(expect.objectContaining({
        method: 'qr-code'
      }));
    });
  });

  describe('Modal Controls', () => {
    it('should emit closeModal on close', () => {
      const closeSpy = vi.spyOn(component.closeModal, 'emit');
      component.close();
      expect(closeSpy).toHaveBeenCalled();
    });

    it('should close on Escape key', () => {
      const closeSpy = vi.spyOn(component.closeModal, 'emit');
      const event = new KeyboardEvent('keydown', { key: 'Escape' });
      component.onKeydown(event);
      expect(closeSpy).toHaveBeenCalled();
    });

    it('should not close on other keys', () => {
      const closeSpy = vi.spyOn(component.closeModal, 'emit');
      const event = new KeyboardEvent('keydown', { key: 'Enter' });
      component.onKeydown(event);
      expect(closeSpy).not.toHaveBeenCalled();
    });
  });

  describe('Character Counts', () => {
    it('should return email character count', () => {
      component.emailForm.get('message')?.setValue('Hello World');
      expect(component.emailCharCount).toBe(11);
    });

    it('should return SMS character count', () => {
      component.smsForm.get('message')?.setValue('Short message');
      expect(component.smsCharCount).toBe(13);
    });
  });

  describe('Error Messages', () => {
    it('should return email error for required field', () => {
      const control = component.emailForm.get('recipients');
      control?.setValue('');
      control?.markAsTouched();
      expect(component.getEmailError('recipients')).toBe('This field is required');
    });

    it('should return email error for invalid format', () => {
      const control = component.emailForm.get('recipients');
      control?.setValue('invalid');
      control?.markAsTouched();
      expect(component.getEmailError('recipients')).toContain('valid email');
    });

    it('should return SMS error for invalid phone', () => {
      const control = component.smsForm.get('phoneNumber');
      control?.setValue('123');
      control?.markAsTouched();
      expect(component.getSmsError('phoneNumber')).toContain('Format');
    });
  });

  describe('Component Inputs', () => {
    it('should accept defaultTab input', () => {
      component.defaultTab = 'email';
      component.ngOnInit();
      expect(component.activeTab()).toBe('email');
    });

    it('should accept branding input', () => {
      component.branding = { discount: '20%' };
      expect(component.branding?.discount).toBe('20%');
    });

    it('should accept enableAnalytics input', () => {
      component.enableAnalytics = false;
      expect(component.enableAnalytics).toBe(false);
    });
  });

  describe('Loading State', () => {
    it('should manage loading state during email send', () => {
      expect(component.isLoading()).toBe(false);

      component.emailForm.patchValue({
        recipients: 'test@example.com',
        message: 'Test'
      });

      component.sendEmail();
      expect(component.isLoading()).toBe(true);
    });
  });
});
