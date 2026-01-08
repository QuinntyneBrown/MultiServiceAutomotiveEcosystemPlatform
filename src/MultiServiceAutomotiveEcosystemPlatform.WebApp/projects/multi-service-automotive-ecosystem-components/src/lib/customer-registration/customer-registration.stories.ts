import type { Meta, StoryObj } from '@storybook/angular';
import { argsToTemplate } from '@storybook/angular';
import { CustomerRegistration } from './customer-registration';

/**
 * Customer Registration Component (CM-F001)
 * 
 * A comprehensive registration form allowing customers to create accounts on the platform.
 * 
 * ## Features
 * - Email and password registration
 * - Name and phone validation
 * - Password strength indicator
 * - Referral code integration
 * - Social login support
 * - Marketing consent
 * - Terms acceptance
 * - Form validation
 * - Loading states
 * - Error handling
 * 
 * ## Accessibility
 * - WCAG 2.1 AA compliant
 * - Keyboard navigation support
 * - Screen reader friendly
 * - Focus management
 * - ARIA labels
 */
const meta: Meta<CustomerRegistration> = {
  title: 'Components/Authentication/CustomerRegistration',
  component: CustomerRegistration,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
    docs: {
      description: {
        component: 'A comprehensive registration form with validation, password strength indicator, and social login options.',
      },
    },
  },
  argTypes: {
    referralCode: {
      control: 'text',
      description: 'Pre-filled referral code from URL',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    referrer: {
      control: 'object',
      description: 'Referrer information to display',
      table: {
        type: { summary: '{ name: string; discount?: string }' },
        defaultValue: { summary: 'undefined' },
      },
    },
    socialProviders: {
      control: 'object',
      description: 'Available social login providers',
      table: {
        type: { summary: "('google' | 'facebook' | 'apple')[]" },
        defaultValue: { summary: "['google', 'facebook']" },
      },
    },
    redirectUrl: {
      control: 'text',
      description: 'Custom redirect URL after registration',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    enableSocialLogin: {
      control: 'boolean',
      description: 'Enable/disable social login',
      table: {
        type: { summary: 'boolean' },
        defaultValue: { summary: 'true' },
      },
    },
    registrationSuccess: {
      action: 'registrationSuccess',
      description: 'Emitted when registration is successful',
      table: {
        type: { summary: 'EventEmitter<RegisteredUser>' },
      },
    },
    registrationError: {
      action: 'registrationError',
      description: 'Emitted when registration fails',
      table: {
        type: { summary: 'EventEmitter<RegistrationError>' },
      },
    },
  },
};

export default meta;
type Story = StoryObj<CustomerRegistration>;

/**
 * Default registration form with all fields empty.
 * This is the standard state when a user first lands on the registration page.
 */
export const Default: Story = {
  args: {
    enableSocialLogin: false,
  },
};

/**
 * Registration form with social login options enabled.
 * Shows Google and Facebook login buttons above the registration form.
 */
export const WithSocialLogin: Story = {
  args: {
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook'],
  },
};

/**
 * Registration form with all social login providers.
 * Displays Google, Facebook, and Apple login options.
 */
export const AllSocialProviders: Story = {
  args: {
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook', 'apple'],
  },
};

/**
 * Registration form with referral banner displayed.
 * Shows when a user arrives via a referral link with a valid code.
 */
export const WithReferral: Story = {
  args: {
    referralCode: 'REFER123',
    referrer: {
      name: 'John Doe',
      discount: '10%',
    },
    enableSocialLogin: false,
  },
};

/**
 * Registration form with referral and social login.
 * Combines referral benefits with social login options.
 */
export const WithReferralAndSocial: Story = {
  args: {
    referralCode: 'REFER123',
    referrer: {
      name: 'Sarah Johnson',
      discount: '$20',
    },
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook'],
  },
};

/**
 * Mobile view of the registration form.
 * Demonstrates responsive design on smaller screens.
 */
export const Mobile: Story = {
  args: {
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook'],
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1',
    },
  },
};

/**
 * Tablet view of the registration form.
 * Shows form layout on medium-sized screens.
 */
export const Tablet: Story = {
  args: {
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook'],
    referrer: {
      name: 'Mike Wilson',
      discount: '15%',
    },
  },
  parameters: {
    viewport: {
      defaultViewport: 'tablet',
    },
  },
};

/**
 * Registration form with Google login only.
 * For implementations that only support Google authentication.
 */
export const GoogleOnly: Story = {
  args: {
    enableSocialLogin: true,
    socialProviders: ['google'],
  },
};

/**
 * Registration form without any social login options.
 * Traditional email/password registration only.
 */
export const EmailOnly: Story = {
  args: {
    enableSocialLogin: false,
  },
};

/**
 * Form in loading state during submission.
 * Note: This story demonstrates the loading state visually.
 * In a real scenario, this would be triggered by form submission.
 */
export const LoadingState: Story = {
  args: {
    enableSocialLogin: false,
  },
  play: async ({ canvasElement }) => {
    // This would normally be handled by the component during submission
    // For demo purposes, we're just showing the form as-is
  },
};
