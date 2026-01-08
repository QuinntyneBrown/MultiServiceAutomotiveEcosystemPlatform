# Customer Create/Edit Form Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-CM-F008` |
| **Component Name** | CustomerForm |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Form Component |

## Description

A comprehensive form component for creating and editing customer records in the professional interface. Supports all customer fields including contact information, address, preferences, and internal notes with robust validation and user-friendly input patterns.

## User Stories

1. **As a service professional**, I want to quickly add new customers, so I can track their service history and preferences.
2. **As a professional**, I want to edit customer information, so I can keep records up-to-date.
3. **As a professional**, I want validation feedback, so I can ensure all required information is complete.
4. **As a professional**, I want to add tags to customers, so I can organize and categorize them effectively.

## Component Structure

```
CustomerForm/
├── index.ts                         # Public exports
├── CustomerForm.tsx                 # Main component (React)
├── CustomerForm.component.ts        # Main component (Angular)
├── CustomerForm.module.scss         # Styles
├── CustomerForm.test.tsx            # Unit tests
├── hooks/
│   ├── useCustomerForm.ts          # Form state management
│   └── useAddressValidation.ts     # Address validation logic
├── components/
│   ├── ContactSection/             # Contact info section
│   ├── AddressSection/             # Address fields section
│   ├── PreferencesSection/         # Preferences section
│   ├── NotesSection/               # Notes and tags section
│   └── TagsInput/                  # Tags input component
└── validation/
    └── customerSchema.ts           # Form validation rules
```

## Props / Inputs

### React

```typescript
interface CustomerFormProps {
  /** Customer data for editing (undefined for create) */
  customer?: Customer;

  /** Form mode */
  mode: 'create' | 'edit';

  /** Callback on successful save */
  onSave?: (customer: Customer) => void;

  /** Callback on cancel */
  onCancel?: () => void;

  /** Callback on form error */
  onError?: (error: FormError) => void;

  /** Show duplicate warning */
  duplicateWarning?: {
    email?: string;
    phone?: string;
    customerId?: string;
  };

  /** Enable auto-save (for edit mode) */
  autoSave?: boolean;

  /** Initial form values */
  initialValues?: Partial<CustomerFormValues>;
}

interface Customer {
  id?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: {
    street: string;
    street2?: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
  };
  preferences: {
    contactMethod: 'email' | 'phone' | 'sms';
    preferredTime?: 'morning' | 'afternoon' | 'evening' | null;
  };
  notes?: string;
  tags?: string[];
  createdAt?: string;
  updatedAt?: string;
}

interface FormError {
  field?: string;
  code: string;
  message: string;
}
```

### Angular

```typescript
@Component({
  selector: 'app-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.scss']
})
export class CustomerFormComponent {
  @Input() customer?: Customer;
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() duplicateWarning?: DuplicateWarning;
  @Input() autoSave = false;

  @Output() save = new EventEmitter<Customer>();
  @Output() cancel = new EventEmitter<void>();
  @Output() formError = new EventEmitter<FormError>();
}
```

## Form Fields

### Contact Information

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| firstName | text | Yes | 2-50 characters, letters, spaces, hyphens, apostrophes |
| lastName | text | Yes | 2-50 characters, letters, spaces, hyphens, apostrophes |
| email | email | Yes | Valid email format, unique check |
| phone | tel | Yes | Valid US phone format (XXX) XXX-XXXX |

### Address

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| street | text | Yes | 5-100 characters |
| street2 | text | No | Max 100 characters |
| city | text | Yes | 2-50 characters |
| province | select | Yes | Valid Canadian province code |
| postalCode | text | Yes | Canadian postal code format (A1A 1A1) |
| country | select | No | Default: CA |

### Preferences

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| contactMethod | radio | No | email, phone, or sms (default: email) |
| preferredTime | select | No | morning, afternoon, evening, or null |

### Notes and Tags

| Field | Type | Required | Validation |
|-------|------|----------|------------|
| notes | textarea | No | Max 1000 characters |
| tags | array | No | Max 20 tags, each 2-30 characters |

## State Management

```typescript
interface CustomerFormState {
  // Form values
  values: {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    address: {
      street: string;
      street2: string;
      city: string;
      province: string;
      postalCode: string;
      country: string;
    };
    preferences: {
      contactMethod: 'email' | 'phone' | 'sms';
      preferredTime: string | null;
    };
    notes: string;
    tags: string[];
  };

  // Validation errors
  errors: {
    [key: string]: string | undefined;
  };

  // Touched fields
  touched: {
    [key: string]: boolean;
  };

  // Form state
  isSubmitting: boolean;
  isValid: boolean;
  isDirty: boolean;
  submitError: string | null;

  // Duplicate check
  duplicateCheck: {
    checking: boolean;
    found: boolean;
    existingCustomerId?: string;
  };

  // Auto-save
  autoSaveStatus: 'idle' | 'saving' | 'saved' | 'error';
  lastSaved?: Date;
}
```

## Visual Specifications

### Layout

- **Form Container**: Max width 1200px, centered
- **Card Padding**: 32px (desktop), 24px (mobile)
- **Section Gap**: 32px between sections
- **Field Gap**: 24px between fields
- **Border Radius**: 12px (card), 8px (inputs)

### Colors

| Element | Token | Hex |
|---------|-------|-----|
| Card Background | --color-white | #FFFFFF |
| Input Border | --color-gray-300 | #CCCCCC |
| Input Border Focus | --color-primary | #00529F |
| Input Border Error | --color-error | #D32F2F |
| Label Text | --color-gray-800 | #333333 |
| Helper Text | --color-gray-600 | #666666 |
| Error Text | --color-error | #D32F2F |
| Info Banner BG | --color-info-light | #E1F5FE |
| Info Banner Border | --color-info | #0288D1 |
| Tag Background | --color-primary-light | #E3F2FD |
| Tag Text | --color-primary | #00529F |

### Typography

| Element | Font | Size | Weight |
|---------|------|------|--------|
| Page Title | Montserrat | 32px | 700 |
| Section Title | Montserrat | 20px | 600 |
| Section Subtitle | Lato | 14px | 400 |
| Form Labels | Lato | 14px | 600 |
| Input Text | Lato | 16px | 400 |
| Helper Text | Lato | 12px | 400 |
| Error Text | Lato | 12px | 400 |
| Button Text | Lato | 16px | 600 |

### Input Field States

```scss
// Default
.form-input {
  height: 48px;
  border: 1px solid var(--color-gray-300);
  background: var(--color-white);
  border-radius: 8px;
  padding: 0 16px;
}

// Focus
.form-input:focus {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px rgba(0, 82, 159, 0.4);
}

// Error
.form-input--error {
  border-color: var(--color-error);
}

.form-input--error:focus {
  box-shadow: 0 0 0 3px rgba(211, 47, 47, 0.4);
}

// Disabled
.form-input:disabled {
  background: var(--color-gray-100);
  cursor: not-allowed;
  color: var(--color-gray-600);
}
```

## Tags Input Component

### Behavior

- Tags displayed as pills with remove button
- Click on container to focus input
- Press Enter or comma to add tag
- Press Backspace on empty input to remove last tag
- Prevent duplicate tags
- Maximum 20 tags per customer

### Tag Styling

```scss
.tag {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 4px 12px;
  background-color: var(--color-primary-light);
  color: var(--color-primary);
  border-radius: 16px;
  font-size: 14px;
}

.tag-remove {
  cursor: pointer;
  background: none;
  border: none;
  color: inherit;
  padding: 0;
}
```

## Validation Rules

```typescript
const customerFormSchema = {
  firstName: {
    required: 'First name is required',
    minLength: { value: 2, message: 'First name must be at least 2 characters' },
    maxLength: { value: 50, message: 'First name must be less than 50 characters' },
    pattern: {
      value: /^[A-Za-z\s\-']+$/,
      message: 'First name can only contain letters, spaces, hyphens, and apostrophes'
    }
  },
  lastName: {
    required: 'Last name is required',
    minLength: { value: 2, message: 'Last name must be at least 2 characters' },
    maxLength: { value: 50, message: 'Last name must be less than 50 characters' },
    pattern: {
      value: /^[A-Za-z\s\-']+$/,
      message: 'Last name can only contain letters, spaces, hyphens, and apostrophes'
    }
  },
  email: {
    required: 'Email is required',
    pattern: {
      value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
      message: 'Please enter a valid email address'
    },
    validate: {
      unique: async (value) => {
        const exists = await checkEmailExists(value);
        return !exists || 'This email is already registered';
      }
    }
  },
  phone: {
    required: 'Phone number is required',
    pattern: {
      value: /^\(\d{3}\) \d{3}-\d{4}$/,
      message: 'Please enter a valid phone number in format (XXX) XXX-XXXX'
    }
  },
  'address.street': {
    required: 'Street address is required',
    minLength: { value: 5, message: 'Street address must be at least 5 characters' },
    maxLength: { value: 100, message: 'Street address must be less than 100 characters' }
  },
  'address.city': {
    required: 'City is required',
    minLength: { value: 2, message: 'City must be at least 2 characters' },
    maxLength: { value: 50, message: 'City must be less than 50 characters' }
  },
  'address.province': {
    required: 'Province is required'
  },
  'address.postalCode': {
    required: 'Postal code is required',
    pattern: {
      value: /^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$/,
      message: 'Postal code must be in format A1A 1A1'
    }
  },
  notes: {
    maxLength: { value: 1000, message: 'Notes must be less than 1000 characters' }
  },
  tags: {
    validate: {
      maxTags: (value) => !value || value.length <= 20 || 'Maximum 20 tags allowed',
      tagLength: (value) => {
        if (!value) return true;
        return value.every(tag => tag.length >= 2 && tag.length <= 30)
          || 'Each tag must be between 2-30 characters';
      }
    }
  }
};
```

## Phone Number Formatting

Automatically format phone input as user types:

```typescript
function formatPhoneNumber(value: string): string {
  const cleaned = value.replace(/\D/g, '');
  const match = cleaned.match(/^(\d{0,3})(\d{0,3})(\d{0,4})$/);

  if (!match) return value;

  const [, area, prefix, line] = match;

  if (line) {
    return `(${area}) ${prefix}-${line}`;
  } else if (prefix) {
    return `(${area}) ${prefix}`;
  } else if (area) {
    return `(${area}`;
  }

  return cleaned;
}
```

## API Integration

### Create Customer Endpoint

```typescript
// POST /api/v1/customers
interface CreateCustomerRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: {
    street: string;
    street2?: string;
    city: string;
    province: string;
    postalCode: string;
    country: string;
  };
  preferences: {
    contactMethod: 'email' | 'phone' | 'sms';
    preferredTime?: string;
  };
  notes?: string;
  tags?: string[];
}

interface CreateCustomerResponse {
  success: boolean;
  data?: {
    customerId: string;
    customer: Customer;
  };
  error?: {
    code: string;
    message: string;
    field?: string;
  };
}
```

### Update Customer Endpoint

```typescript
// PUT /api/v1/customers/:id
interface UpdateCustomerRequest {
  // Same as CreateCustomerRequest
}

interface UpdateCustomerResponse {
  success: boolean;
  data?: {
    customer: Customer;
    updatedAt: string;
  };
  error?: {
    code: string;
    message: string;
    field?: string;
  };
}
```

### Duplicate Check Endpoint

```typescript
// GET /api/v1/customers/check-duplicate?email=...&phone=...
interface DuplicateCheckResponse {
  exists: boolean;
  customerId?: string;
  matchedOn?: 'email' | 'phone' | 'both';
}
```

## Accessibility Requirements

### WCAG 2.1 AA Compliance

- [ ] All form fields have associated labels
- [ ] Error messages linked to fields via aria-describedby
- [ ] Focus visible on all interactive elements
- [ ] Color is not the only indicator of errors
- [ ] Form can be completed with keyboard only
- [ ] Required fields indicated in label text (not just asterisk)

### Screen Reader Support

```html
<div class="form-group" role="group" aria-labelledby="firstName-label">
  <label id="firstName-label" for="firstName" class="form-label">
    First Name <span aria-hidden="true">*</span>
    <span class="sr-only">(required)</span>
  </label>
  <input
    type="text"
    id="firstName"
    name="firstName"
    aria-required="true"
    aria-invalid="false"
    aria-describedby="firstName-helper firstName-error"
  />
  <span id="firstName-helper" class="form-helper">
    Enter customer's first name
  </span>
  <span id="firstName-error" class="form-error" role="alert" aria-live="polite">
    <!-- Error message appears here -->
  </span>
</div>
```

### Focus Management

- Auto-focus first field on page load (for create mode)
- Move focus to first error on validation failure
- Announce form submission status
- Trap focus in duplicate warning modal (if shown)

## Responsive Behavior

### Desktop (≥768px)

- Two-column layout for name fields, email/phone, city/state, zip/country
- Full width for street address and textarea
- Buttons aligned to right
- Maximum container width: 1200px

### Mobile (<768px)

- Single-column layout for all fields
- Stacked buttons (Save on top, Cancel below)
- Full-width buttons
- Reduced padding: 24px instead of 32px
- Larger touch targets (48px minimum)

## Error Handling

### Client-Side Errors

| Error | Message | Behavior |
|-------|---------|----------|
| Required field empty | "[Field] is required" | Show on blur |
| Invalid email | "Please enter a valid email address" | Show on blur |
| Invalid phone | "Please enter a valid phone number" | Show on blur |
| Invalid postal code | "Postal code must be in format A1A 1A1" | Show on blur |
| Max length exceeded | "[Field] must be less than [X] characters" | Show while typing |

### Server-Side Errors

| Error Code | Message | Behavior |
|------------|---------|----------|
| EMAIL_EXISTS | "This email is already registered" | Show below email field, offer to view existing customer |
| PHONE_EXISTS | "This phone number is already registered" | Show below phone field |
| VALIDATION_ERROR | Field-specific message | Show below relevant field |
| SERVER_ERROR | "Unable to save customer. Please try again." | Show alert banner |
| NETWORK_ERROR | "Connection lost. Changes will be saved when reconnected." | Show alert, enable retry |

## Duplicate Warning

When duplicate email or phone is detected:

```typescript
interface DuplicateWarningModal {
  show: boolean;
  message: string;
  existingCustomer: {
    id: string;
    name: string;
    email?: string;
    phone?: string;
  };
  actions: [
    {
      label: 'View Existing Customer';
      onClick: () => void;
    },
    {
      label: 'Save Anyway';
      onClick: () => void;
      warning: true;
    },
    {
      label: 'Cancel';
      onClick: () => void;
    }
  ];
}
```

## Auto-Save Feature (Edit Mode)

- Save changes automatically after 3 seconds of inactivity
- Show "Saving..." indicator
- Show "Saved at [time]" on success
- Prevent navigation if save in progress
- Handle conflicts (show warning if customer was modified by another user)

```typescript
const autoSave = useAutoSave({
  onSave: async (values) => {
    await updateCustomer(customerId, values);
  },
  delay: 3000,
  enabled: mode === 'edit' && autoSaveEnabled
});
```

## Analytics Events

```typescript
// Track form usage
trackEvent('customer_form_opened', { mode: 'create' | 'edit' });
trackEvent('customer_form_field_focused', { field: string });
trackEvent('customer_form_validation_error', { field: string, error: string });
trackEvent('customer_form_submitted', { mode: 'create' | 'edit' });
trackEvent('customer_form_success', {
  mode: 'create' | 'edit',
  customerId: string,
  hasTags: boolean,
  hasNotes: boolean
});
trackEvent('customer_form_error', {
  mode: 'create' | 'edit',
  errorCode: string
});
trackEvent('customer_form_cancelled', { mode: 'create' | 'edit', isDirty: boolean });
trackEvent('duplicate_customer_warning_shown', { matchedOn: 'email' | 'phone' });
```

## Testing Requirements

### Unit Tests

```typescript
describe('CustomerForm', () => {
  it('renders all form sections', () => {});
  it('validates required fields on blur', () => {});
  it('validates email format', () => {});
  it('validates phone format', () => {});
  it('formats phone number as user types', () => {});
  it('validates postal code format', () => {});
  it('validates name character restrictions', () => {});
  it('validates max length for notes', () => {});
  it('adds and removes tags correctly', () => {});
  it('prevents duplicate tags', () => {});
  it('enforces maximum tag count', () => {});
  it('submits form with valid data (create)', () => {});
  it('submits form with valid data (edit)', () => {});
  it('displays server errors correctly', () => {});
  it('shows duplicate warning when applicable', () => {});
  it('auto-saves in edit mode', () => {});
  it('confirms navigation when form is dirty', () => {});
  it('is accessible (axe audit)', () => {});
});
```

### E2E Tests

```typescript
describe('Customer Form Flow', () => {
  it('creates a new customer successfully', () => {});
  it('edits existing customer successfully', () => {});
  it('validates all required fields', () => {});
  it('handles duplicate email error', () => {});
  it('handles duplicate phone error', () => {});
  it('adds and removes tags', () => {});
  it('cancels form and confirms if dirty', () => {});
  it('auto-saves changes in edit mode', () => {});
  it('works correctly on mobile viewport', () => {});
});
```

### Visual Regression Tests

- Compare component screenshots at different viewport sizes
- Test all validation states
- Test with different data (long names, many tags, etc.)

## Dependencies

- Form validation library (react-hook-form / Angular Reactive Forms)
- Phone number formatting library (libphonenumber-js)
- Address validation service (optional for Phase 3)
- State/province lookup data

## Related Components

- `Header` - Platform header
- `Button` - Shared button component
- `TextField` - Shared input component
- `Select` - Shared select dropdown component
- `TextArea` - Shared textarea component
- `RadioGroup` - Shared radio button component
- `TagsInput` - Custom tags input component
- `Alert` - Error/success messages
- `Modal` - Duplicate warning modal

## Performance Considerations

1. **Debounce Validation**
   - Debounce async validation (email uniqueness) by 500ms
   - Validate synchronous rules on blur

2. **Optimize Re-renders**
   - Use field-level state management
   - Memoize validation functions
   - Lazy load address autocomplete (Phase 3)

3. **Bundle Size**
   - Code-split phone formatting library
   - Lazy load state/province data

## Security Considerations

1. **Data Sanitization**
   - Sanitize all inputs before submission
   - Prevent XSS in notes field
   - Validate all inputs server-side

2. **PII Protection**
   - Encrypt customer data at rest
   - Mask sensitive data in logs
   - Require authentication to access form

3. **CSRF Protection**
   - Include CSRF token in form submission
   - Validate token on server

## Future Enhancements (Post-Phase 1)

- Address autocomplete with Google Places API
- Upload customer profile photo
- Multiple phone numbers
- Custom fields per professional
- Import customer from vCard
- Duplicate customer merge functionality
- Vehicle information section
- Service history preview (read-only)

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-08 | Initial specification |
