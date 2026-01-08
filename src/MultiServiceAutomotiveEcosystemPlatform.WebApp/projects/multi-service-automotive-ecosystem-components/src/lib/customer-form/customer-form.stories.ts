import type { Meta, StoryObj } from '@storybook/angular';
import { argsToTemplate } from '@storybook/angular';
import { CustomerForm, Customer } from './customer-form';

/**
 * Customer Create/Edit Form Component (REQ-CM-F008)
 *
 * A comprehensive form for creating and editing customer records.
 *
 * ## Features
 * - Contact information fields with validation
 * - Address section with state dropdown
 * - Preferences for contact method and time
 * - Tags system for customer categorization
 * - Notes field for internal comments
 * - Phone number auto-formatting
 * - Real-time validation feedback
 * - Auto-save support (edit mode)
 *
 * ## Accessibility
 * - WCAG 2.1 AA compliant
 * - All fields have associated labels
 * - Error messages linked via aria-describedby
 * - Keyboard navigation support
 */
const meta: Meta<CustomerForm> = {
  title: 'Components/Forms/CustomerForm',
  component: CustomerForm,
  tags: ['autodocs'],
  parameters: {
    layout: 'padded',
    docs: {
      description: {
        component: 'A comprehensive form for creating and editing customer records with validation, tags, and preferences.',
      },
    },
  },
  argTypes: {
    mode: {
      control: 'radio',
      options: ['create', 'edit'],
      description: 'Form mode - create new or edit existing customer',
      table: {
        type: { summary: "'create' | 'edit'" },
        defaultValue: { summary: 'create' },
      },
    },
    customer: {
      control: 'object',
      description: 'Customer data for editing (undefined for create mode)',
      table: {
        type: { summary: 'Customer' },
        defaultValue: { summary: 'undefined' },
      },
    },
    autoSave: {
      control: 'boolean',
      description: 'Enable auto-save functionality (edit mode only)',
      table: {
        type: { summary: 'boolean' },
        defaultValue: { summary: 'false' },
      },
    },
    duplicateWarning: {
      control: 'object',
      description: 'Show duplicate customer warning',
      table: {
        type: { summary: 'DuplicateWarning' },
        defaultValue: { summary: 'undefined' },
      },
    },
    save: {
      action: 'save',
      description: 'Emitted when form is submitted successfully',
      table: {
        type: { summary: 'EventEmitter<Customer>' },
      },
    },
    cancel: {
      action: 'cancel',
      description: 'Emitted when cancel button is clicked',
      table: {
        type: { summary: 'EventEmitter<void>' },
      },
    },
    formError: {
      action: 'formError',
      description: 'Emitted when a form error occurs',
      table: {
        type: { summary: 'EventEmitter<FormError>' },
      },
    },
  },
};

export default meta;
type Story = StoryObj<CustomerForm>;

/**
 * Default create mode - empty form for adding a new customer
 */
export const CreateMode: Story = {
  args: {
    mode: 'create',
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * Edit mode with pre-populated customer data
 */
export const EditMode: Story = {
  args: {
    mode: 'edit',
    customer: {
      id: '123',
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      phone: '(555) 123-4567',
      address: {
        street: '123 Main Street',
        street2: 'Apt 4B',
        city: 'Springfield',
        state: 'IL',
        zipCode: '62701',
        country: 'US',
      },
      preferences: {
        contactMethod: 'email',
        preferredTime: 'morning',
      },
      notes: 'VIP customer, prefers email communication. Has a fleet of 5 vehicles.',
      tags: ['VIP', 'Fleet', 'Preferred'],
    } as Customer,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * Edit mode with auto-save enabled
 */
export const WithAutoSave: Story = {
  args: {
    mode: 'edit',
    autoSave: true,
    customer: {
      id: '456',
      firstName: 'Jane',
      lastName: 'Smith',
      email: 'jane.smith@example.com',
      phone: '(555) 987-6543',
      address: {
        street: '456 Oak Avenue',
        city: 'Chicago',
        state: 'IL',
        zipCode: '60601',
        country: 'US',
      },
      preferences: {
        contactMethod: 'phone',
        preferredTime: 'afternoon',
      },
    } as Customer,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * With duplicate warning banner displayed
 */
export const WithDuplicateWarning: Story = {
  args: {
    mode: 'create',
    duplicateWarning: {
      email: 'existing@example.com',
      customerId: '789',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * Edit mode with many tags
 */
export const WithManyTags: Story = {
  args: {
    mode: 'edit',
    customer: {
      id: '321',
      firstName: 'Bob',
      lastName: 'Wilson',
      email: 'bob.wilson@example.com',
      phone: '(555) 456-7890',
      address: {
        street: '789 Pine Road',
        city: 'Austin',
        state: 'TX',
        zipCode: '78701',
        country: 'US',
      },
      preferences: {
        contactMethod: 'sms',
      },
      tags: [
        'VIP', 'Fleet', 'Commercial', 'Priority', 'Discount',
        'Wholesale', 'Regular', 'Premium', 'Gold', 'Referral'
      ],
    } as Customer,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * Mobile viewport demonstration
 */
export const Mobile: Story = {
  args: {
    mode: 'create',
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * Tablet viewport demonstration
 */
export const Tablet: Story = {
  args: {
    mode: 'create',
  },
  parameters: {
    viewport: {
      defaultViewport: 'tablet',
    },
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};

/**
 * Customer with long notes
 */
export const WithLongNotes: Story = {
  args: {
    mode: 'edit',
    customer: {
      id: '999',
      firstName: 'Alice',
      lastName: 'Johnson',
      email: 'alice@example.com',
      phone: '(555) 111-2222',
      address: {
        street: '100 Corporate Drive',
        city: 'Denver',
        state: 'CO',
        zipCode: '80202',
        country: 'US',
      },
      preferences: {
        contactMethod: 'email',
      },
      notes: `This is a long customer note that demonstrates how the notes field handles longer text content.

The customer has been with us since 2020 and has multiple vehicles in their household. They prefer morning appointments and usually request the same technician (Mike) for all their services.

Special considerations:
- Always send appointment reminders via email
- They have a preferred parking spot at the front
- Their daughter also brings her car here
- They referred several other customers to us

Payment notes:
- Has a corporate account
- Net 30 terms approved
- Credit limit: $5,000`,
    } as Customer,
  },
  render: (args) => ({
    props: args,
    template: `<ms-customer-form ${argsToTemplate(args)}></ms-customer-form>`,
  }),
};
