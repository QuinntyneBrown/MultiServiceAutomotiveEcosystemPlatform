import type { Meta, StoryObj } from '@storybook/angular';
import { argsToTemplate } from '@storybook/angular';
import { ProfileManagement, Profile } from './profile-management';

/**
 * Profile Management Interface Component (REQ-SP-F006)
 *
 * A comprehensive form for service professionals to manage their business profiles.
 *
 * ## Features
 * - Multi-section tabbed form
 * - Basic information (business name, type, owner)
 * - About section with bio and company details
 * - Contact information (multiple phones/emails)
 * - Location with address and service radius
 * - Media uploads (profile photo, cover, logo)
 * - Auto-save functionality
 * - Draft and publish modes
 *
 * ## Accessibility
 * - WCAG 2.1 AA compliant
 * - Keyboard navigation between tabs
 * - Form field labels and error messages
 * - Focus management
 */
const meta: Meta<ProfileManagement> = {
  title: 'Components/Profile/ProfileManagement',
  component: ProfileManagement,
  tags: ['autodocs'],
  parameters: {
    layout: 'padded',
    docs: {
      description: {
        component: 'A comprehensive form for service professionals to create and manage their business profiles with multiple sections for basic info, contact details, location, and media uploads.',
      },
    },
  },
  argTypes: {
    mode: {
      control: 'radio',
      options: ['create', 'edit'],
      description: 'Form mode - create new or edit existing profile',
      table: {
        type: { summary: "'create' | 'edit'" },
        defaultValue: { summary: 'create' },
      },
    },
    profileId: {
      control: 'text',
      description: 'Profile ID for editing',
      table: {
        type: { summary: 'string' },
        defaultValue: { summary: 'undefined' },
      },
    },
    initialData: {
      control: 'object',
      description: 'Initial data for editing',
      table: {
        type: { summary: 'Partial<Profile>' },
        defaultValue: { summary: 'undefined' },
      },
    },
    autoSaveInterval: {
      control: 'number',
      description: 'Auto-save interval in milliseconds',
      table: {
        type: { summary: 'number' },
        defaultValue: { summary: '30000' },
      },
    },
    save: {
      action: 'save',
      description: 'Emitted when profile is saved',
      table: {
        type: { summary: 'EventEmitter<{profile: Profile, isDraft: boolean}>' },
      },
    },
    cancel: {
      action: 'cancel',
      description: 'Emitted when cancelled',
      table: {
        type: { summary: 'EventEmitter<void>' },
      },
    },
    preview: {
      action: 'preview',
      description: 'Emitted when preview is requested',
      table: {
        type: { summary: 'EventEmitter<Profile>' },
      },
    },
  },
};

export default meta;
type Story = StoryObj<ProfileManagement>;

/**
 * Create mode - empty form for new profile
 */
export const CreateMode: Story = {
  args: {
    mode: 'create',
  },
  render: (args) => ({
    props: args,
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
  }),
};

/**
 * Edit mode with existing profile data
 */
export const EditMode: Story = {
  args: {
    mode: 'edit',
    profileId: '123',
    initialData: {
      businessName: 'Mike\'s Auto Repair',
      businessType: 'auto-repair',
      personalName: 'Mike Johnson',
      professionalTitle: 'Owner & Lead Technician',
      bio: 'Mike\'s Auto Repair has been serving the Springfield community for over 15 years. We specialize in all makes and models, offering everything from routine maintenance to complex engine repairs. Our team of certified technicians is dedicated to providing honest, reliable service at fair prices.',
      yearsInBusiness: 15,
      numberOfEmployees: 8,
      phoneNumbers: [
        { id: '1', type: 'business', number: '(555) 123-4567', isPrimary: true },
        { id: '2', type: 'mobile', number: '(555) 987-6543', isPrimary: false }
      ],
      emails: [
        { id: '1', type: 'primary', email: 'info@mikesauto.com', isPrimary: true },
        { id: '2', type: 'support', email: 'service@mikesauto.com', isPrimary: false }
      ],
      website: 'https://www.mikesautorepair.com',
      address: {
        street: '456 Main Street',
        city: 'Springfield',
        province: 'ON',
        postalCode: 'K1A 0B1',
        country: 'CA'
      },
      serviceRadius: {
        value: 25,
        unit: 'miles'
      }
    } as Partial<Profile>,
  },
  render: (args) => ({
    props: args,
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
  }),
};

/**
 * With auto-save enabled
 */
export const WithAutoSave: Story = {
  args: {
    mode: 'edit',
    profileId: '456',
    autoSaveInterval: 10000,
    initialData: {
      businessName: 'Quick Lube Express',
      businessType: 'detailing',
      personalName: 'Sarah Williams',
      bio: 'Quick Lube Express provides fast, professional oil changes and basic maintenance services. Our drive-through service gets you back on the road in under 15 minutes!',
    } as Partial<Profile>,
  },
  render: (args) => ({
    props: args,
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
  }),
};

/**
 * Body shop profile example
 */
export const BodyShopProfile: Story = {
  args: {
    mode: 'edit',
    profileId: '789',
    initialData: {
      businessName: 'Premier Collision Center',
      businessType: 'body-shop',
      personalName: 'Robert Chen',
      professionalTitle: 'Certified Collision Repair Specialist',
      bio: 'Premier Collision Center is an I-CAR Gold certified body shop with over 20 years of experience restoring vehicles to their pre-accident condition. We work with all major insurance companies and offer lifetime warranties on all repairs.',
      yearsInBusiness: 20,
      numberOfEmployees: 15,
      website: 'https://www.premiercollision.com',
      address: {
        street: '789 Industrial Boulevard',
        addressLine2: 'Building C',
        city: 'Chicago',
        province: 'QC',
        postalCode: 'H2Y 1C6',
        country: 'CA'
      }
    } as Partial<Profile>,
  },
  render: (args) => ({
    props: args,
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
  }),
};

/**
 * Mobile mechanic profile
 */
export const MobileMechanic: Story = {
  args: {
    mode: 'edit',
    profileId: '101',
    initialData: {
      businessName: 'On-The-Go Auto Repair',
      businessType: 'mobile-mechanic',
      personalName: 'David Martinez',
      professionalTitle: 'ASE Certified Mobile Mechanic',
      bio: 'Bringing the shop to you! I provide convenient mobile auto repair services in the greater Austin area. From oil changes to brake repairs, I come to your home or office.',
      yearsInBusiness: 5,
      numberOfEmployees: 1,
      serviceRadius: {
        value: 50,
        unit: 'miles'
      }
    } as Partial<Profile>,
  },
  render: (args) => ({
    props: args,
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
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
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
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
    template: `<ms-profile-management ${argsToTemplate(args)}></ms-profile-management>`,
  }),
};
