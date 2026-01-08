import type { Meta, StoryObj } from '@storybook/angular';
import { TenantNotFound } from './tenant-not-found';

/**
 * Tenant Not Found Page Component (MT-F009)
 * 
 * A dedicated error page displayed when a user attempts to access a tenant subdomain 
 * that doesn't exist or is no longer active.
 * 
 * ## Features
 * - Clear error messaging
 * - Animated error icon
 * - Action buttons for navigation
 * - Help section with suggestions
 * - Support link
 * - Responsive design
 * 
 * ## Accessibility
 * - WCAG 2.1 AA compliant
 * - Semantic HTML structure
 * - Keyboard navigation support
 * - Focus management
 */
const meta: Meta<TenantNotFound> = {
  title: 'Components/Error Pages/TenantNotFound',
  component: TenantNotFound,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
    docs: {
      description: {
        component: 'An error page for when a tenant subdomain cannot be found, providing helpful guidance and navigation options.',
      },
    },
  },
  argTypes: {
    attemptedSubdomain: {
      control: 'text',
      description: 'The invalid subdomain that was accessed',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    mainPlatformUrl: {
      control: 'text',
      description: 'URL to redirect to main platform',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'https://www.platform.com' },
      },
    },
    supportUrl: {
      control: 'text',
      description: 'URL for support contact',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'https://www.platform.com/contact' },
      },
    },
    customMessage: {
      control: 'text',
      description: 'Custom error message (optional)',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    navigateToMain: {
      action: 'navigateToMain',
      description: 'Emitted when user clicks main platform button',
      table: {
        type: { summary: 'EventEmitter<void>' },
      },
    },
    contactSupport: {
      action: 'contactSupport',
      description: 'Emitted when user clicks support button',
      table: {
        type: { summary: 'EventEmitter<void>' },
      },
    },
  },
};

export default meta;
type Story = StoryObj<TenantNotFound>;

/**
 * Default error page with no subdomain specified.
 * Shows the generic error message.
 */
export const Default: Story = {
  args: {},
};

/**
 * Error page with a specific subdomain that was attempted.
 * The message includes the attempted subdomain name.
 */
export const WithAttemptedSubdomain: Story = {
  args: {
    attemptedSubdomain: 'invalid-shop',
  },
};

/**
 * Error page with a custom error message.
 * Useful for specific error scenarios.
 */
export const WithCustomMessage: Story = {
  args: {
    customMessage: 'This network has been deactivated by the owner. Please contact support for more information.',
  },
};

/**
 * Error page with custom URLs for navigation.
 * Demonstrates customizable platform and support URLs.
 */
export const WithCustomUrls: Story = {
  args: {
    attemptedSubdomain: 'old-network',
    mainPlatformUrl: 'https://custom-platform.com',
    supportUrl: 'https://custom-platform.com/support',
  },
};

/**
 * Complete example with all props configured.
 * Shows how the component looks with all customizations.
 */
export const FullyCustomized: Story = {
  args: {
    attemptedSubdomain: 'closed-business',
    customMessage: 'This automotive network is no longer active. The business has closed or moved to a different platform.',
    mainPlatformUrl: 'https://automotive-hub.com',
    supportUrl: 'https://automotive-hub.com/contact',
  },
};

/**
 * Mobile view of the error page.
 * Demonstrates responsive design on smaller screens.
 */
export const Mobile: Story = {
  args: {
    attemptedSubdomain: 'invalid-network',
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1',
    },
  },
};

/**
 * Tablet view of the error page.
 * Shows the layout on medium-sized screens.
 */
export const Tablet: Story = {
  args: {
    attemptedSubdomain: 'old-shop',
    customMessage: 'This network has been moved to a new location.',
  },
  parameters: {
    viewport: {
      defaultViewport: 'tablet',
    },
  },
};

/**
 * Minimal configuration with just the subdomain.
 * Clean, simple error page.
 */
export const Minimal: Story = {
  args: {
    attemptedSubdomain: 'example',
  },
};
