import type { Meta, StoryObj } from '@storybook/angular';
import { SpecialtyManagement, Specialty, SpecialtyCatalogItem, Certification, SpecialtyCategory } from './specialty-management';

const mockCatalogItems: SpecialtyCatalogItem[] = [
  {
    id: 'cat-engine',
    name: 'Engine Repair',
    description: 'Complete engine diagnostics, repair, and rebuilding services',
    icon: '🔧',
    category: SpecialtyCategory.ENGINE,
    keywords: ['motor', 'engine', 'pistons', 'cylinders'],
    isPopular: true
  },
  {
    id: 'cat-brakes',
    name: 'Brake Service',
    description: 'Brake pad replacement, rotor resurfacing, and brake system repairs',
    icon: '🛑',
    category: SpecialtyCategory.BRAKES,
    keywords: ['brakes', 'pads', 'rotors', 'calipers'],
    isPopular: true
  },
  {
    id: 'cat-electrical',
    name: 'Electrical Systems',
    description: 'Wiring, batteries, alternators, and electrical diagnostics',
    icon: '⚡',
    category: SpecialtyCategory.ELECTRICAL,
    keywords: ['wiring', 'battery', 'alternator', 'starter']
  },
  {
    id: 'cat-transmission',
    name: 'Transmission Service',
    description: 'Manual and automatic transmission repair and maintenance',
    icon: '⚙️',
    category: SpecialtyCategory.TRANSMISSION,
    keywords: ['transmission', 'gearbox', 'clutch']
  },
  {
    id: 'cat-hvac',
    name: 'AC & Heating',
    description: 'Air conditioning repair, heating system service, and climate control',
    icon: '❄️',
    category: SpecialtyCategory.HVAC,
    keywords: ['ac', 'air conditioning', 'heating', 'climate']
  },
  {
    id: 'cat-suspension',
    name: 'Suspension & Steering',
    description: 'Shocks, struts, alignment, and steering system repairs',
    icon: '🎯',
    category: SpecialtyCategory.SUSPENSION,
    keywords: ['suspension', 'shocks', 'struts', 'alignment']
  },
  {
    id: 'cat-diagnostic',
    name: 'Computer Diagnostics',
    description: 'OBD-II scanning, ECU programming, and electronic troubleshooting',
    icon: '💻',
    category: SpecialtyCategory.DIAGNOSTIC,
    keywords: ['diagnostic', 'scan', 'obd', 'computer'],
    isPopular: true
  },
  {
    id: 'cat-exhaust',
    name: 'Exhaust Systems',
    description: 'Muffler replacement, catalytic converters, and exhaust repairs',
    icon: '💨',
    category: SpecialtyCategory.EXHAUST,
    keywords: ['exhaust', 'muffler', 'catalytic converter']
  },
  {
    id: 'cat-maintenance',
    name: 'Preventive Maintenance',
    description: 'Oil changes, fluid checks, filter replacements, and tune-ups',
    icon: '🛢️',
    category: SpecialtyCategory.MAINTENANCE,
    keywords: ['oil change', 'maintenance', 'tune-up', 'filter']
  },
  {
    id: 'cat-body',
    name: 'Body Work',
    description: 'Dent repair, paint touch-ups, and minor collision repairs',
    icon: '🎨',
    category: SpecialtyCategory.BODY,
    keywords: ['body', 'dent', 'paint', 'collision']
  },
  {
    id: 'cat-classic',
    name: 'Classic Car Restoration',
    description: 'Vintage vehicle restoration and specialty parts sourcing',
    icon: '🚗',
    category: SpecialtyCategory.SPECIALTY_VEHICLES,
    keywords: ['classic', 'vintage', 'restoration', 'antique']
  },
  {
    id: 'cat-hybrid',
    name: 'Hybrid & Electric Vehicles',
    description: 'Battery service, electric motor repair, and hybrid systems',
    icon: '🔋',
    category: SpecialtyCategory.SPECIALTY_VEHICLES,
    keywords: ['hybrid', 'electric', 'ev', 'battery']
  }
];

const mockSpecialties: Specialty[] = [
  {
    id: 'spec-1',
    name: 'Engine Repair',
    icon: '🔧',
    yearsOfExperience: 12,
    order: 0,
    isCustom: false,
    createdAt: new Date('2024-01-15'),
    updatedAt: new Date('2024-01-15')
  },
  {
    id: 'spec-2',
    name: 'Brake Service',
    icon: '🛑',
    yearsOfExperience: 10,
    order: 1,
    isCustom: false,
    createdAt: new Date('2024-02-20'),
    updatedAt: new Date('2024-02-20')
  },
  {
    id: 'spec-3',
    name: 'Computer Diagnostics',
    icon: '💻',
    yearsOfExperience: 8,
    order: 2,
    isCustom: false,
    createdAt: new Date('2024-03-10'),
    updatedAt: new Date('2024-03-10')
  },
  {
    id: 'spec-4',
    name: 'European Vehicle Specialist',
    yearsOfExperience: 15,
    order: 3,
    isCustom: true,
    createdAt: new Date('2024-04-01'),
    updatedAt: new Date('2024-04-01')
  }
];

const mockCertifications: Certification[] = [
  {
    id: 'cert-1',
    name: 'ASE Master Technician',
    fileName: 'ase-master-cert.pdf',
    fileUrl: '/files/ase-master-cert.pdf',
    fileSize: 1245184,
    mimeType: 'application/pdf',
    uploadedAt: new Date('2024-01-10'),
    expirationDate: new Date('2026-01-10'),
    isVerified: true
  },
  {
    id: 'cert-2',
    name: 'BMW Certified Technician',
    fileName: 'bmw-certification.pdf',
    fileUrl: '/files/bmw-certification.pdf',
    fileSize: 892416,
    mimeType: 'application/pdf',
    uploadedAt: new Date('2024-03-15'),
    isVerified: true
  },
  {
    id: 'cert-3',
    name: 'EPA 609 Certification',
    fileName: 'epa-609.jpg',
    fileUrl: '/files/epa-609.jpg',
    fileSize: 456789,
    mimeType: 'image/jpeg',
    uploadedAt: new Date('2024-05-20'),
    isVerified: false
  }
];

const meta: Meta<SpecialtyManagement> = {
  title: 'Components/SpecialtyManagement',
  component: SpecialtyManagement,
  tags: ['autodocs'],
  parameters: {
    layout: 'padded',
    docs: {
      description: {
        component: `
The Specialty Management Interface allows service professionals to manage their automotive specialties and certifications.

## Features
- Add specialties from a predefined catalog
- Create custom specialties with years of experience
- Drag-and-drop reordering of specialties
- Upload certification documents (PDF, JPG, PNG)
- Search and filter specialty catalog
- Maximum limits for specialties and certifications

## Usage
\`\`\`html
<ms-specialty-management
  [currentSpecialties]="specialties"
  [availableSpecialties]="catalog"
  [certifications]="certifications"
  [maxSpecialties]="10"
  [maxCertifications]="20"
  (addSpecialties)="onAddSpecialties($event)"
  (removeSpecialty)="onRemoveSpecialty($event)"
  (reorderSpecialties)="onReorderSpecialties($event)"
  (addCertification)="onAddCertification($event)"
  (removeCertification)="onRemoveCertification($event)"
/>
\`\`\`
        `
      }
    }
  },
  argTypes: {
    currentSpecialties: {
      description: 'Array of currently added specialties',
      control: 'object'
    },
    availableSpecialties: {
      description: 'Catalog of available specialties to choose from',
      control: 'object'
    },
    certifications: {
      description: 'Array of uploaded certifications',
      control: 'object'
    },
    maxSpecialties: {
      description: 'Maximum number of specialties allowed',
      control: { type: 'number', min: 1, max: 50 }
    },
    maxCertifications: {
      description: 'Maximum number of certifications allowed',
      control: { type: 'number', min: 1, max: 50 }
    },
    allowCustomSpecialties: {
      description: 'Whether to allow creating custom specialties',
      control: 'boolean'
    },
    isLoading: {
      description: 'Show loading state',
      control: 'boolean'
    },
    error: {
      description: 'Error message to display',
      control: 'text'
    },
    addSpecialties: {
      description: 'Event emitted when specialties are added',
      action: 'addSpecialties'
    },
    removeSpecialty: {
      description: 'Event emitted when a specialty is removed',
      action: 'removeSpecialty'
    },
    reorderSpecialties: {
      description: 'Event emitted when specialties are reordered',
      action: 'reorderSpecialties'
    },
    addCertification: {
      description: 'Event emitted when a certification file is uploaded',
      action: 'addCertification'
    },
    removeCertification: {
      description: 'Event emitted when a certification is removed',
      action: 'removeCertification'
    }
  }
};

export default meta;
type Story = StoryObj<SpecialtyManagement>;

export const EmptyState: Story = {
  args: {
    currentSpecialties: [],
    availableSpecialties: mockCatalogItems,
    certifications: [],
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    docs: {
      description: {
        story: 'Initial state when no specialties or certifications have been added. Shows empty state with call-to-action.'
      }
    }
  }
};

export const WithSpecialties: Story = {
  args: {
    currentSpecialties: mockSpecialties,
    availableSpecialties: mockCatalogItems,
    certifications: [],
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    docs: {
      description: {
        story: 'Showing a list of specialties with drag-and-drop reordering capability. Note the custom specialty badge.'
      }
    }
  }
};

export const WithCertifications: Story = {
  args: {
    currentSpecialties: mockSpecialties.slice(0, 2),
    availableSpecialties: mockCatalogItems,
    certifications: mockCertifications,
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    docs: {
      description: {
        story: 'Profile with both specialties and uploaded certifications. Verified certifications show a badge.'
      }
    }
  }
};

export const FullProfile: Story = {
  args: {
    currentSpecialties: mockSpecialties,
    availableSpecialties: mockCatalogItems,
    certifications: mockCertifications,
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    docs: {
      description: {
        story: 'Complete professional profile with multiple specialties and certifications.'
      }
    }
  }
};

export const LoadingState: Story = {
  args: {
    currentSpecialties: [],
    availableSpecialties: [],
    certifications: [],
    maxSpecialties: 10,
    maxCertifications: 20,
    isLoading: true
  },
  parameters: {
    docs: {
      description: {
        story: 'Loading state shown while fetching specialties data.'
      }
    }
  }
};

export const NearMaxLimit: Story = {
  args: {
    currentSpecialties: [
      ...mockSpecialties,
      {
        id: 'spec-5',
        name: 'Transmission Service',
        icon: '⚙️',
        yearsOfExperience: 6,
        order: 4,
        isCustom: false,
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 'spec-6',
        name: 'AC & Heating',
        icon: '❄️',
        yearsOfExperience: 5,
        order: 5,
        isCustom: false,
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 'spec-7',
        name: 'Suspension & Steering',
        icon: '🎯',
        yearsOfExperience: 7,
        order: 6,
        isCustom: false,
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 'spec-8',
        name: 'Exhaust Systems',
        icon: '💨',
        yearsOfExperience: 4,
        order: 7,
        isCustom: false,
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 'spec-9',
        name: 'Preventive Maintenance',
        icon: '🛢️',
        yearsOfExperience: 8,
        order: 8,
        isCustom: false,
        createdAt: new Date(),
        updatedAt: new Date()
      }
    ],
    availableSpecialties: mockCatalogItems,
    certifications: mockCertifications,
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    docs: {
      description: {
        story: 'Profile with 9 of 10 maximum specialties. Shows limit indicator and remaining capacity.'
      }
    }
  }
};

export const WithError: Story = {
  args: {
    currentSpecialties: mockSpecialties.slice(0, 2),
    availableSpecialties: mockCatalogItems,
    certifications: [],
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false,
    error: 'Failed to load specialties. Please try again.'
  },
  parameters: {
    docs: {
      description: {
        story: 'Displaying an error message when something goes wrong.'
      }
    }
  }
};

export const CustomSpecialtiesDisabled: Story = {
  args: {
    currentSpecialties: mockSpecialties.slice(0, 2),
    availableSpecialties: mockCatalogItems,
    certifications: [],
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: false,
    isLoading: false
  },
  parameters: {
    docs: {
      description: {
        story: 'Modal with custom specialty form disabled. Users can only select from the catalog.'
      }
    }
  }
};

export const Mobile: Story = {
  args: {
    currentSpecialties: mockSpecialties,
    availableSpecialties: mockCatalogItems,
    certifications: mockCertifications,
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1'
    },
    docs: {
      description: {
        story: 'Mobile responsive layout with single-column certification grid and larger touch targets.'
      }
    }
  }
};

export const Tablet: Story = {
  args: {
    currentSpecialties: mockSpecialties,
    availableSpecialties: mockCatalogItems,
    certifications: mockCertifications,
    maxSpecialties: 10,
    maxCertifications: 20,
    allowCustomSpecialties: true,
    isLoading: false
  },
  parameters: {
    viewport: {
      defaultViewport: 'tablet'
    },
    docs: {
      description: {
        story: 'Tablet viewport showing responsive layout adjustments.'
      }
    }
  }
};
