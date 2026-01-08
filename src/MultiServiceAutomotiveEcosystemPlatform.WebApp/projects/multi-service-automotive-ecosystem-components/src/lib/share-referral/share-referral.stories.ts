import type { Meta, StoryObj } from '@storybook/angular';
import { argsToTemplate } from '@storybook/angular';
import { ShareReferral, UserInfo } from './share-referral';

/**
 * Share Referral Interface Component (REQ-RF-F002)
 *
 * A comprehensive modal for sharing referral links through multiple channels.
 *
 * ## Features
 * - Copy link with one click
 * - Email sharing with message preview
 * - SMS sharing with character count
 * - Social media sharing (Facebook, Twitter, WhatsApp, LinkedIn)
 * - QR code generation and download
 * - Phone number auto-formatting
 * - Real-time validation
 *
 * ## Accessibility
 * - WCAG 2.1 AA compliant
 * - Keyboard navigation with Escape to close
 * - ARIA labels for all tabs and buttons
 * - Focus management within modal
 */
const meta: Meta<ShareReferral> = {
  title: 'Components/Referral/ShareReferral',
  component: ShareReferral,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
    docs: {
      description: {
        component: 'A modal interface for sharing referral links through multiple channels including copy link, email, SMS, social media, and QR codes.',
      },
    },
  },
  argTypes: {
    referralCode: {
      control: 'text',
      description: 'Unique referral code for the user',
      table: {
        type: { summary: 'string' },
      },
    },
    referralUrl: {
      control: 'text',
      description: 'Full referral URL (auto-generated if not provided)',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    user: {
      control: 'object',
      description: 'User information for personalization',
      table: {
        type: { summary: 'UserInfo' },
      },
    },
    defaultTab: {
      control: 'select',
      options: ['copy-link', 'email', 'sms', 'social', 'qr-code'],
      description: 'Default tab to display when modal opens',
      table: {
        type: { summary: 'ShareMethod' },
        defaultValue: { summary: 'copy-link' },
      },
    },
    branding: {
      control: 'object',
      description: 'Custom branding options',
      table: {
        type: { summary: 'BrandingOptions' },
        defaultValue: { summary: 'undefined' },
      },
    },
    enableAnalytics: {
      control: 'boolean',
      description: 'Enable analytics tracking',
      table: {
        type: { summary: 'boolean' },
        defaultValue: { summary: 'true' },
      },
    },
    closeModal: {
      action: 'closeModal',
      description: 'Emitted when modal is closed',
      table: {
        type: { summary: 'EventEmitter<void>' },
      },
    },
    shareSuccess: {
      action: 'shareSuccess',
      description: 'Emitted when share is successful',
      table: {
        type: { summary: 'EventEmitter<ShareSuccessEvent>' },
      },
    },
    shareError: {
      action: 'shareError',
      description: 'Emitted when share fails',
      table: {
        type: { summary: 'EventEmitter<ShareErrorEvent>' },
      },
    },
  },
};

export default meta;
type Story = StoryObj<ShareReferral>;

const defaultUser: UserInfo = {
  id: '123',
  firstName: 'John',
  lastName: 'Doe',
  email: 'john.doe@example.com'
};

/**
 * Default view showing the copy link tab
 */
export const Default: Story = {
  args: {
    referralCode: 'ABC123XYZ',
    user: defaultUser,
    defaultTab: 'copy-link',
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * Email sharing tab with form
 */
export const EmailTab: Story = {
  args: {
    referralCode: 'ABC123XYZ',
    user: defaultUser,
    defaultTab: 'email',
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * SMS sharing tab with phone input
 */
export const SmsTab: Story = {
  args: {
    referralCode: 'ABC123XYZ',
    user: defaultUser,
    defaultTab: 'sms',
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * Social media sharing tab with platform buttons
 */
export const SocialTab: Story = {
  args: {
    referralCode: 'ABC123XYZ',
    user: defaultUser,
    defaultTab: 'social' as any,
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * QR code tab with download options
 */
export const QrCodeTab: Story = {
  args: {
    referralCode: 'ABC123XYZ',
    user: defaultUser,
    defaultTab: 'qr-code',
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * With custom branding and discount
 */
export const WithBranding: Story = {
  args: {
    referralCode: 'PROMO20',
    user: defaultUser,
    defaultTab: 'copy-link',
    branding: {
      discount: '20%',
      accentColor: '#FF5722',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * With custom referral URL
 */
export const WithCustomUrl: Story = {
  args: {
    referralCode: 'CUSTOM',
    referralUrl: 'https://myshop.com/invite/CUSTOM',
    user: defaultUser,
    defaultTab: 'copy-link',
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * Mobile viewport demonstration
 */
export const Mobile: Story = {
  args: {
    referralCode: 'ABC123XYZ',
    user: defaultUser,
    defaultTab: 'copy-link',
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};

/**
 * Professional user sharing
 */
export const ProfessionalUser: Story = {
  args: {
    referralCode: 'PROREF001',
    user: {
      id: '456',
      firstName: 'Mike',
      lastName: 'Smith',
      email: 'mike.smith@autorepair.com'
    } as UserInfo,
    defaultTab: 'email',
    branding: {
      discount: '25%',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-share-referral ${argsToTemplate(args)}></ms-share-referral>`,
  }),
};
