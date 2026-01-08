import type { Meta, StoryObj } from '@storybook/angular';
import { argsToTemplate } from '@storybook/angular';
import { CustomerLogin } from './customer-login';

/**
 * Customer Login Component (CM-F002)
 * 
 * A secure login page allowing customers to authenticate using email/password.
 * 
 * ## Features
 * - Email and password authentication
 * - Remember me functionality
 * - Password visibility toggle
 * - Forgot password link
 * - Optional social login
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
const meta: Meta<CustomerLogin> = {
  title: 'Components/Authentication/CustomerLogin',
  component: CustomerLogin,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
    docs: {
      description: {
        component: 'A comprehensive login form with validation, error handling, and social login options.',
      },
    },
  },
  argTypes: {
    redirectUrl: {
      control: 'text',
      description: 'URL to redirect after successful login',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    enableSocialLogin: {
      control: 'boolean',
      description: 'Enable social login buttons',
      table: {
        type: { summary: 'boolean' },
        defaultValue: { summary: 'false' },
      },
    },
    enableRememberMe: {
      control: 'boolean',
      description: 'Show remember me checkbox',
      table: {
        type: { summary: 'boolean' },
        defaultValue: { summary: 'true' },
      },
    },
    prefillEmail: {
      control: 'text',
      description: 'Pre-fill the email field',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    successMessage: {
      control: 'text',
      description: 'Display a success message banner',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    socialProviders: {
      control: 'object',
      description: 'Array of social login providers',
      table: {
        type: { summary: 'string[]' },
        defaultValue: { summary: '[]' },
      },
    },
    loginSuccess: {
      action: 'loginSuccess',
      description: 'Emitted when login is successful',
      table: {
        type: { summary: 'EventEmitter<AuthenticatedUser>' },
      },
    },
    loginError: {
      action: 'loginError',
      description: 'Emitted when login fails',
      table: {
        type: { summary: 'EventEmitter<AuthError>' },
      },
    },
  },
};

export default meta;
type Story = StoryObj<CustomerLogin>;

/**
 * Default login form with standard options
 */
export const Default: Story = {
  args: {
    enableRememberMe: true,
    enableSocialLogin: false,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};

/**
 * Login form with pre-filled email (e.g., after registration)
 */
export const WithPrefillEmail: Story = {
  args: {
    prefillEmail: 'user@example.com',
    enableRememberMe: true,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};

/**
 * Login form with success message (e.g., after password reset)
 */
export const WithSuccessMessage: Story = {
  args: {
    successMessage: 'Password reset successful! Please sign in with your new password.',
    enableRememberMe: true,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};

/**
 * Login form with social login options
 */
export const WithSocialLogin: Story = {
  args: {
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook', 'apple'],
    enableRememberMe: true,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};

/**
 * Login form without remember me option
 */
export const WithoutRememberMe: Story = {
  args: {
    enableRememberMe: false,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};

/**
 * Mobile view - demonstrates responsive design
 */
export const Mobile: Story = {
  args: {
    enableRememberMe: true,
    enableSocialLogin: true,
    socialProviders: ['google', 'facebook'],
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};

/**
 * Complete example with all features enabled
 */
export const Complete: Story = {
  args: {
    redirectUrl: '/dashboard',
    enableSocialLogin: true,
    enableRememberMe: true,
    socialProviders: ['google', 'facebook'],
    prefillEmail: 'demo@example.com',
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-login ${argsToTemplate(args)}></ms-customer-login>`,
  }),
};
