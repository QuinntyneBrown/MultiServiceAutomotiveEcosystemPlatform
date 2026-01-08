# Send Professional Referral Form Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-RF-F006` |
| **Component Name** | SendProfessionalReferralForm |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

A comprehensive form for service professionals to refer their customers to other professionals in the network. Includes customer selection, professional search, service details, priority levels, and contextual notes for the receiving professional.

## User Stories

1. **As a professional**, I want to refer my customer to another specialist, so they receive the specific service they need.
2. **As a professional**, I want to search for receiving professionals by specialty, so I can find the right match.
3. **As a professional**, I want to include context and notes, so the receiving professional understands the situation.
4. **As a professional**, I want to set priority levels, so urgent cases are handled appropriately.

## Component Structure

```
SendProfessionalReferralForm/
├── index.ts                              # Public exports
├── SendProfessionalReferralForm.tsx      # Main component (React)
├── SendProfessionalReferralForm.component.ts  # Main component (Angular)
├── SendProfessionalReferralForm.module.scss   # Styles
├── SendProfessionalReferralForm.test.tsx      # Unit tests
├── components/
│   ├── CustomerSelector/                 # Customer selection widget
│   ├── ProfessionalSearch/               # Professional search with autocomplete
│   ├── VehicleSelector/                  # Vehicle selection (optional)
│   ├── ServiceTypeSelect/                # Service type dropdown
│   ├── PrioritySelector/                 # Priority level selection
│   └── ReferralNotes/                    # Notes text areas
├── hooks/
│   ├── useProfessionalSearch.ts          # Professional search logic
│   ├── useCustomerSearch.ts              # Customer search logic
│   └── useReferralForm.ts                # Form state management
└── types/
    └── referral-form.types.ts            # Type definitions
```

## Props / Inputs

### React

```typescript
interface SendProfessionalReferralFormProps {
  /** Pre-selected customer ID */
  customerId?: string;

  /** Pre-selected professional ID */
  professionalId?: string;

  /** Draft referral to resume editing */
  draftId?: string;

  /** Callback when referral is sent */
  onSend?: (referral: ProfessionalReferral) => void;

  /** Callback when form is cancelled */
  onCancel?: () => void;

  /** Callback when draft is saved */
  onSaveDraft?: (draft: ReferralDraft) => void;

  /** Available service types */
  serviceTypes?: ServiceType[];

  /** Enable vehicle selection */
  showVehicleSelector?: boolean;
}
```

### Angular

```typescript
@Component({
  selector: 'app-send-professional-referral-form',
  templateUrl: './send-professional-referral-form.component.html',
  styleUrls: ['./send-professional-referral-form.component.scss']
})
export class SendProfessionalReferralFormComponent {
  @Input() customerId?: string;
  @Input() professionalId?: string;
  @Input() draftId?: string;
  @Input() serviceTypes: ServiceType[] = [];
  @Input() showVehicleSelector = true;

  @Output() send = new EventEmitter<ProfessionalReferral>();
  @Output() cancel = new EventEmitter<void>();
  @Output() saveDraft = new EventEmitter<ReferralDraft>();
}
```

## Data Models

### Professional Referral

```typescript
interface ProfessionalReferral {
  id: string;
  sendingProfessionalId: string;
  receivingProfessionalId: string;
  customerId: string;
  vehicleId?: string;
  serviceType: string;
  estimatedValue?: number;
  priority: 'normal' | 'high' | 'urgent';
  reason: string;
  notes?: string;
  status: 'pending' | 'accepted' | 'declined' | 'completed';
  createdAt: Date;
}

interface ReferralDraft {
  id: string;
  formData: Partial<ProfessionalReferralFormData>;
  lastSavedAt: Date;
}

interface ProfessionalReferralFormData {
  customerId: string;
  receivingProfessionalId: string;
  vehicleId?: string;
  serviceType: string;
  estimatedValue?: number;
  priority: 'normal' | 'high' | 'urgent';
  reason: string;
  notes?: string;
}

interface ServiceType {
  id: string;
  name: string;
  description?: string;
  category: string;
}

interface ProfessionalSearchResult {
  id: string;
  businessName: string;
  ownerName: string;
  avatar?: string;
  specialties: string[];
  location: {
    city: string;
    state: string;
  };
  rating: number;
  reviewCount: number;
  isNetworkPartner: boolean;
}
```

## State Management

```typescript
interface ReferralFormState {
  // Form data
  formData: ProfessionalReferralFormData;

  // Customer
  selectedCustomer: Customer | null;
  customerSearchQuery: string;
  customerSearchResults: Customer[];

  // Professional
  selectedProfessional: ProfessionalSearchResult | null;
  professionalSearchQuery: string;
  professionalSearchResults: ProfessionalSearchResult[];
  isSearchingProfessionals: boolean;

  // Vehicle
  selectedVehicle: Vehicle | null;
  availableVehicles: Vehicle[];

  // Form state
  isSubmitting: boolean;
  isSavingDraft: boolean;
  isDirty: boolean;

  // Validation
  errors: Record<string, string>;
  touched: Record<string, boolean>;

  // Draft
  draftId: string | null;
  lastSavedAt: Date | null;
}
```

## Visual Specifications

### Layout

- **Max Width**: 900px centered
- **Sections**: Card-based with clear separation
- **Spacing**: 24px between sections, 16px within sections

### Form Sections

| Section | Content |
|---------|---------|
| Info Alert | Explanation of professional referral feature |
| Customer Information | Selected customer card, vehicle selector |
| Receiving Professional | Search autocomplete, selected professional card |
| Referral Details | Service type, estimated value, priority, reason, notes |
| Actions | Cancel, Save Draft, Send Referral buttons |

### Customer Selector Card

```
┌─────────────────────────────────────────────────────────────┐
│  ┌──────┐                                                   │
│  │  JD  │  John Doe                              [Change]   │
│  └──────┘  john.doe@email.com | (555) 123-4567              │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Professional Search Results

```
┌─────────────────────────────────────────────────────────────┐
│ 🔍 Search by name, specialty, or location...                │
├─────────────────────────────────────────────────────────────┤
│  ┌──────┐  Body Shop Pro                                    │
│  │  BS  │  Springfield, IL | 4.8 rating                     │
│  └──────┘  [Collision Repair] [Paint]                       │
├─────────────────────────────────────────────────────────────┤
│  ┌──────┐  AutoBody Excellence                              │
│  │  AB  │  Springfield, IL | 4.6 rating                     │
│  └──────┘  [Collision Repair] [Dent Removal]                │
└─────────────────────────────────────────────────────────────┘
```

### Priority Options

| Priority | Icon | Border Color | Background |
|----------|------|--------------|------------|
| Normal | Activity | --color-primary (selected) | --color-primary-light |
| High | Clock | --color-warning | --color-warning-light |
| Urgent | Warning Triangle | --color-error | --color-error-light |

### Button States

| Button | Variant | Condition |
|--------|---------|-----------|
| Cancel | Secondary | Always enabled |
| Save Draft | Secondary | Enabled when form is dirty |
| Send Referral | Primary | Enabled when form is valid |

## Form Validation

### Required Fields

```typescript
const validationRules = {
  customerId: {
    required: 'Customer is required',
  },
  receivingProfessionalId: {
    required: 'Receiving professional is required',
  },
  serviceType: {
    required: 'Service type is required',
  },
  reason: {
    required: 'Reason for referral is required',
    minLength: { value: 20, message: 'Please provide more detail (min 20 characters)' },
    maxLength: { value: 1000, message: 'Reason is too long (max 1000 characters)' },
  },
  estimatedValue: {
    min: { value: 0, message: 'Value must be positive' },
    pattern: { value: /^\d+(\.\d{2})?$/, message: 'Invalid currency format' },
  },
  notes: {
    maxLength: { value: 2000, message: 'Notes are too long (max 2000 characters)' },
  },
};
```

### Validation Logic

```typescript
const validateForm = (data: ProfessionalReferralFormData): ValidationResult => {
  const errors: Record<string, string> = {};

  if (!data.customerId) {
    errors.customerId = 'Please select a customer';
  }

  if (!data.receivingProfessionalId) {
    errors.receivingProfessionalId = 'Please select a receiving professional';
  }

  if (!data.serviceType) {
    errors.serviceType = 'Please select a service type';
  }

  if (!data.reason || data.reason.trim().length < 20) {
    errors.reason = 'Please provide more detail about the referral reason';
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
};
```

## Professional Search

### Search Behavior

- **Debounce**: 300ms
- **Minimum characters**: 2
- **Fields searched**: business name, owner name, specialties, location
- **Max results**: 10

### Search API

```typescript
// GET /api/v1/professionals/search
interface ProfessionalSearchRequest {
  query: string;
  specialty?: string;
  location?: {
    lat: number;
    lng: number;
    radius: number; // miles
  };
  excludeIds?: string[]; // Exclude self
  limit?: number;
}

interface ProfessionalSearchResponse {
  data: ProfessionalSearchResult[];
  meta: {
    totalCount: number;
    hasMore: boolean;
  };
}
```

### Search Hook

```typescript
const useProfessionalSearch = (options: {
  excludeIds?: string[];
  debounceMs?: number;
}) => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState<ProfessionalSearchResult[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const debouncedSearch = useMemo(
    () => debounce(async (searchQuery: string) => {
      if (searchQuery.length < 2) {
        setResults([]);
        return;
      }

      setIsLoading(true);
      try {
        const response = await api.searchProfessionals({
          query: searchQuery,
          excludeIds: options.excludeIds,
        });
        setResults(response.data);
      } finally {
        setIsLoading(false);
      }
    }, options.debounceMs || 300),
    [options.excludeIds, options.debounceMs]
  );

  useEffect(() => {
    debouncedSearch(query);
  }, [query, debouncedSearch]);

  return { query, setQuery, results, isLoading };
};
```

## API Integration

### Create Referral

```typescript
// POST /api/v1/referrals/professional
interface CreateProfessionalReferralRequest {
  customerId: string;
  receivingProfessionalId: string;
  vehicleId?: string;
  serviceType: string;
  estimatedValue?: number;
  priority: 'normal' | 'high' | 'urgent';
  reason: string;
  notes?: string;
}

interface CreateProfessionalReferralResponse {
  data: ProfessionalReferral;
  message: string;
}
```

### Save Draft

```typescript
// POST /api/v1/referrals/drafts
// PUT /api/v1/referrals/drafts/:id
interface SaveReferralDraftRequest {
  formData: Partial<ProfessionalReferralFormData>;
}

interface SaveReferralDraftResponse {
  data: ReferralDraft;
}
```

### Get Draft

```typescript
// GET /api/v1/referrals/drafts/:id
interface GetReferralDraftResponse {
  data: ReferralDraft;
}
```

## Auto-Save Draft

```typescript
const useAutoSaveDraft = (
  formData: ProfessionalReferralFormData,
  isDirty: boolean,
  draftId: string | null
) => {
  const [lastSaved, setLastSaved] = useState<Date | null>(null);

  useEffect(() => {
    if (!isDirty) return;

    const autoSaveTimer = setTimeout(async () => {
      try {
        const response = draftId
          ? await api.updateDraft(draftId, { formData })
          : await api.createDraft({ formData });
        setLastSaved(new Date());
      } catch (error) {
        console.error('Auto-save failed:', error);
      }
    }, 5000); // Auto-save after 5 seconds of inactivity

    return () => clearTimeout(autoSaveTimer);
  }, [formData, isDirty, draftId]);

  return { lastSaved };
};
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] Form fields have associated labels
- [ ] Required fields are marked with aria-required
- [ ] Error messages are linked with aria-describedby
- [ ] Search autocomplete has proper ARIA roles
- [ ] Priority options use radio group pattern
- [ ] Loading states are announced

### Keyboard Navigation

- Tab through all form fields
- Arrow keys within priority selection
- Enter to select from autocomplete
- Escape to close autocomplete dropdown

### Screen Reader Support

```html
<div role="combobox" aria-expanded="true" aria-haspopup="listbox">
  <input
    type="text"
    aria-autocomplete="list"
    aria-controls="professional-results"
    aria-describedby="professional-search-hint"
  />
  <ul id="professional-results" role="listbox">
    <li role="option" aria-selected="false">...</li>
  </ul>
</div>
<p id="professional-search-hint" class="sr-only">
  Search for professionals by name, specialty, or location
</p>
```

## Responsive Behavior

### Desktop (≥1200px)
- Form fields in 2-column grid where appropriate
- Priority options side by side

### Tablet (768px - 1199px)
- Single column layout
- Priority options may wrap to 2 per row

### Mobile (<768px)
- Full width form fields
- Priority options stack vertically
- Sticky submit button at bottom

## Testing Requirements

### Unit Tests

```typescript
describe('SendProfessionalReferralForm', () => {
  it('renders form with all sections', () => {});
  it('validates required fields', () => {});
  it('searches professionals on input', () => {});
  it('selects professional from search results', () => {});
  it('allows priority selection', () => {});
  it('submits form with valid data', () => {});
  it('shows validation errors', () => {});
  it('auto-saves draft', () => {});
  it('loads existing draft', () => {});
  it('handles cancel action', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### Integration Tests

```typescript
describe('Professional Referral Flow', () => {
  it('completes full referral flow', () => {});
  it('searches and selects professional', () => {});
  it('saves and resumes draft', () => {});
  it('handles API errors gracefully', () => {});
});
```

## Dependencies

- Form library (React Hook Form / Angular Reactive Forms)
- Customer search component
- Professional search component
- Vehicle selector component
- Text area with character count
- Priority radio group
- Toast notifications

## Related Components

- `ProfessionalReferralDashboard` - Overview of sent/received referrals
- `ReceivedReferralsManagement` - Manage incoming referrals
- `CustomerSelector` - Reusable customer picker
- `ProfessionalSearch` - Reusable professional search

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-07 | Initial specification |
