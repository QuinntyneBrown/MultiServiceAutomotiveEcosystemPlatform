# Professional Dashboard Home Component

## Component Overview

| Property | Value |
|----------|-------|
| **Component ID** | `REQ-SP-F005` |
| **Component Name** | ProfessionalDashboardHome |
| **Phase** | Phase 1 |
| **Priority** | P0 (Critical) |
| **Type** | Page Component |

## Description

The Professional Dashboard Home is the primary landing page for service professionals after login. It provides an at-a-glance overview of their business metrics, recent activity, and quick access to common actions. The dashboard includes customer statistics, inquiry management, referral tracking, and profile completion status.

## User Stories

1. **As a service professional**, I want to see my key business metrics at a glance, so I can understand my business performance.
2. **As a busy professional**, I want to see recent customer activities and inquiries, so I can respond promptly.
3. **As a professional**, I want quick access to common actions, so I can efficiently manage my workflow.
4. **As a new user**, I want to see my profile completion status, so I know what steps I need to complete.
5. **As a professional**, I want to track referrals I've sent and received, so I can monitor my network activity.

## Component Structure

```
ProfessionalDashboardHome/
├── index.ts                          # Public exports
├── ProfessionalDashboard.tsx         # Main component (React)
├── ProfessionalDashboard.component.ts # Main component (Angular)
├── ProfessionalDashboard.module.scss # Styles
├── ProfessionalDashboard.test.tsx    # Unit tests
├── components/
│   ├── Sidebar/                      # Navigation sidebar
│   ├── TopBar/                       # Top navigation bar
│   ├── WelcomeBanner/                # Welcome banner with profile completion
│   ├── StatsCard/                    # Reusable stat card
│   ├── ActivityFeed/                 # Recent activities list
│   └── QuickActions/                 # Quick action buttons
├── hooks/
│   ├── useDashboardData.ts           # Dashboard data fetching
│   └── useProfileCompletion.ts       # Profile completion logic
└── services/
    └── dashboardService.ts           # Dashboard API calls
```

## Props / Inputs

### React

```typescript
interface ProfessionalDashboardHomeProps {
  /** Current authenticated user */
  user: ProfessionalUser;

  /** Initial dashboard data (for SSR) */
  initialData?: DashboardData;

  /** Callback when navigation item is clicked */
  onNavigate?: (route: string) => void;

  /** Callback when quick action is triggered */
  onQuickAction?: (action: QuickActionType) => void;

  /** Enable/disable real-time updates */
  enableRealTimeUpdates?: boolean;

  /** Refresh interval in milliseconds (default: 30000) */
  refreshInterval?: number;
}

interface ProfessionalUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  businessName?: string;
  profileImageUrl?: string;
  profileCompletion: number;
}

interface DashboardData {
  stats: DashboardStats;
  recentActivity: Activity[];
  notifications: Notification[];
  profileCompletion: ProfileCompletion;
}

interface DashboardStats {
  totalCustomers: number;
  customersTrend: TrendData;
  newInquiries: number;
  inquiriesTrend: TrendData;
  pendingReferrals: number;
  monthlyReferrals: number;
  referralsTrend: TrendData;
}

interface TrendData {
  value: number;
  percentage: number;
  direction: 'up' | 'down' | 'neutral';
  period: string;
}

interface Activity {
  id: string;
  type: 'customer' | 'inquiry' | 'referral' | 'review' | 'update';
  title: string;
  description: string;
  timestamp: Date;
  status?: 'new' | 'pending' | 'completed';
  metadata?: Record<string, any>;
}

interface ProfileCompletion {
  percentage: number;
  missingFields: string[];
  completedSections: string[];
  nextSteps: string[];
}

type QuickActionType = 'add_customer' | 'send_referral' | 'view_inquiries' | 'update_profile';
```

### Angular

```typescript
@Component({
  selector: 'app-professional-dashboard-home',
  templateUrl: './professional-dashboard-home.component.html',
  styleUrls: ['./professional-dashboard-home.component.scss']
})
export class ProfessionalDashboardHomeComponent implements OnInit, OnDestroy {
  @Input() user!: ProfessionalUser;
  @Input() initialData?: DashboardData;
  @Input() enableRealTimeUpdates = true;
  @Input() refreshInterval = 30000;

  @Output() navigate = new EventEmitter<string>();
  @Output() quickAction = new EventEmitter<QuickActionType>();

  dashboardData$: Observable<DashboardData>;
  isLoading = false;
  error: string | null = null;
}
```

## Dashboard Widgets

### 1. Sidebar Navigation

```typescript
interface SidebarConfig {
  items: NavigationItem[];
  activeRoute: string;
  collapsed?: boolean;
}

interface NavigationItem {
  id: string;
  label: string;
  icon: string;
  route: string;
  badge?: number;
  isActive: boolean;
}

const defaultNavigationItems: NavigationItem[] = [
  { id: 'dashboard', label: 'Dashboard', icon: 'dashboard', route: '/dashboard', isActive: true },
  { id: 'customers', label: 'Customers', icon: 'people', route: '/customers', isActive: false },
  { id: 'referrals', label: 'Referrals', icon: 'handshake', route: '/referrals', isActive: false },
  { id: 'inquiries', label: 'Inquiries', icon: 'message', route: '/inquiries', badge: 8, isActive: false },
  { id: 'profile', label: 'My Profile', icon: 'person', route: '/profile', isActive: false }
];
```

### 2. Welcome Banner

```typescript
interface WelcomeBannerProps {
  userName: string;
  greeting?: string;
  profileCompletion: ProfileCompletion;
  onCompleteProfile: () => void;
}

interface ProfileCompletion {
  percentage: number;
  missingFields: string[];
  nextSteps: string[];
}
```

### 3. Stats Cards

```typescript
interface StatCard {
  id: string;
  title: string;
  value: number;
  icon: string;
  iconColor: 'blue' | 'green' | 'orange' | 'purple';
  trend?: TrendData;
  onClick?: () => void;
}

const defaultStatCards: StatCard[] = [
  {
    id: 'total_customers',
    title: 'Total Customers',
    value: 142,
    icon: 'people',
    iconColor: 'blue',
    trend: { value: 12, percentage: 12, direction: 'up', period: 'last month' }
  },
  {
    id: 'new_inquiries',
    title: 'New Inquiries',
    value: 8,
    icon: 'message',
    iconColor: 'green',
    trend: { value: 3, percentage: 0, direction: 'up', period: 'this week' }
  },
  {
    id: 'pending_referrals',
    title: 'Pending Referrals',
    value: 5,
    icon: 'handshake',
    iconColor: 'orange'
  },
  {
    id: 'monthly_referrals',
    title: 'This Month\'s Referrals',
    value: 23,
    icon: 'send',
    iconColor: 'purple',
    trend: { value: 8, percentage: 8, direction: 'up', period: 'last month' }
  }
];
```

### 4. Activity Feed

```typescript
interface ActivityFeedProps {
  activities: Activity[];
  maxItems?: number;
  onViewAll: () => void;
  onActivityClick?: (activity: Activity) => void;
}

interface Activity {
  id: string;
  type: 'customer' | 'inquiry' | 'referral' | 'review' | 'update';
  title: string;
  description: string;
  timestamp: Date;
  status?: 'new' | 'pending' | 'completed';
  user?: {
    name: string;
    avatar?: string;
  };
  metadata?: {
    customerId?: string;
    inquiryId?: string;
    referralId?: string;
    rating?: number;
  };
}

// Activity type icons and colors
const activityConfig = {
  customer: { icon: 'people', color: '#E3F2FD', iconColor: '#00529F' },
  inquiry: { icon: 'message', color: '#E1F5FE', iconColor: '#0288D1' },
  referral: { icon: 'handshake', color: '#F3E5F5', iconColor: '#7B1FA2' },
  review: { icon: 'star', color: '#FFF3E0', iconColor: '#ED6C02' },
  update: { icon: 'notification', color: '#E8F5E9', iconColor: '#2E7D32' }
};
```

### 5. Quick Actions Panel

```typescript
interface QuickAction {
  id: QuickActionType;
  title: string;
  description: string;
  icon: string;
  route?: string;
  onClick?: () => void;
}

const defaultQuickActions: QuickAction[] = [
  {
    id: 'add_customer',
    title: 'Add Customer',
    description: 'Register a new customer',
    icon: 'person_add',
    route: '/customers/new'
  },
  {
    id: 'send_referral',
    title: 'Send Referral',
    description: 'Refer to another professional',
    icon: 'send',
    route: '/referrals/send'
  },
  {
    id: 'view_inquiries',
    title: 'View Inquiries',
    description: 'Check pending inquiries',
    icon: 'inbox',
    route: '/inquiries'
  },
  {
    id: 'update_profile',
    title: 'Update Profile',
    description: 'Edit your business info',
    icon: 'edit',
    route: '/profile/edit'
  }
];
```

## Visual Specifications

### Layout Structure

```
┌─────────────────────────────────────────────────────────┐
│ Sidebar (260px)  │ Main Content                         │
│                  │ ┌─────────────────────────────────┐  │
│ ┌──────────┐     │ │ Top Bar                        │  │
│ │ Logo     │     │ └─────────────────────────────────┘  │
│ └──────────┘     │                                      │
│                  │ ┌─────────────────────────────────┐  │
│ • Dashboard*     │ │ Welcome Banner                 │  │
│ • Customers      │ │ Profile Completion: 75%        │  │
│ • Referrals      │ └─────────────────────────────────┘  │
│ • Inquiries [8]  │                                      │
│ • My Profile     │ ┌───┐ ┌───┐ ┌───┐ ┌───┐            │
│                  │ │142│ │ 8 │ │ 5 │ │23 │ Stats      │
│                  │ └───┘ └───┘ └───┘ └───┘            │
│                  │                                      │
│                  │ ┌──────────────┐ ┌──────────┐       │
│                  │ │ Activity Feed│ │ Quick    │       │
│                  │ │              │ │ Actions  │       │
│                  │ │              │ │          │       │
│                  │ └──────────────┘ └──────────┘       │
└─────────────────────────────────────────────────────────┘
```

### Spacing & Dimensions

| Element | Value |
|---------|-------|
| Sidebar Width | 260px |
| Main Content Max Width | 1400px |
| Content Padding | 24px |
| Stats Card Gap | 24px |
| Activity Feed Width | 2fr (66% of grid) |
| Quick Actions Width | 1fr (33% of grid) |
| Card Border Radius | 12px |
| Button Border Radius | 8px |
| Avatar Size | 40px |

### Colors

| Element | Token | Hex |
|---------|-------|-----|
| Sidebar Background | --color-blue-700 (gradient) | #00529F → #003366 |
| Background | --color-gray-50 | #FAFAFA |
| Card Background | --color-white | #FFFFFF |
| Welcome Banner | --color-primary (gradient) | #00529F → #0066CC |
| Primary Text | --color-gray-900 | #1A1A1A |
| Secondary Text | --color-gray-600 | #666666 |
| Progress Bar Fill | --color-accent | #FFD520 |
| Success Color | --color-success | #2E7D32 |
| Warning Color | --color-warning | #ED6C02 |
| Badge New | --color-success-light | #E8F5E9 |
| Badge Pending | --color-warning-light | #FFF3E0 |

### Typography

| Element | Font | Size | Weight | Color |
|---------|------|------|--------|-------|
| Page Title | Montserrat | 24px | 700 | #1A1A1A |
| Welcome Heading | Montserrat | 28px | 700 | #FFFFFF |
| Section Title | Montserrat | 20px | 700 | #1A1A1A |
| Stat Value | Montserrat | 32px | 700 | #1A1A1A |
| Stat Label | Lato | 14px | 400 | #666666 |
| Activity Title | Lato | 14px | 600 | #1A1A1A |
| Activity Description | Lato | 13px | 400 | #666666 |
| Activity Time | Lato | 12px | 400 | #999999 |
| Quick Action Title | Lato | 14px | 600 | #1A1A1A |
| Quick Action Description | Lato | 12px | 400 | #666666 |
| Nav Item | Lato | 15px | 500 | rgba(255,255,255,0.8) |
| Nav Item Active | Lato | 15px | 600 | #FFFFFF |

## State Management

### Dashboard State

```typescript
interface DashboardState {
  // Data
  stats: DashboardStats | null;
  activities: Activity[];
  notifications: Notification[];
  profileCompletion: ProfileCompletion | null;

  // UI State
  isLoading: boolean;
  error: string | null;
  lastUpdated: Date | null;
  activeNavItem: string;

  // Filters
  activityFilter: ActivityType | 'all';
  dateRange: DateRange;

  // Real-time
  connectionStatus: 'connected' | 'disconnected' | 'reconnecting';
  unreadCount: number;
}

interface DateRange {
  start: Date;
  end: Date;
}
```

### Actions

```typescript
type DashboardAction =
  | { type: 'FETCH_DASHBOARD_START' }
  | { type: 'FETCH_DASHBOARD_SUCCESS'; payload: DashboardData }
  | { type: 'FETCH_DASHBOARD_ERROR'; payload: string }
  | { type: 'UPDATE_STATS'; payload: Partial<DashboardStats> }
  | { type: 'ADD_ACTIVITY'; payload: Activity }
  | { type: 'MARK_ACTIVITY_READ'; payload: string }
  | { type: 'SET_ACTIVITY_FILTER'; payload: ActivityType | 'all' }
  | { type: 'SET_DATE_RANGE'; payload: DateRange }
  | { type: 'UPDATE_PROFILE_COMPLETION'; payload: number }
  | { type: 'SET_ACTIVE_NAV'; payload: string };
```

## API Integration

### Get Dashboard Data

```typescript
// GET /api/v1/professionals/{professionalId}/dashboard
interface GetDashboardRequest {
  professionalId: string;
  dateRange?: {
    start: string; // ISO date
    end: string;   // ISO date
  };
}

interface GetDashboardResponse {
  success: boolean;
  data: {
    stats: DashboardStats;
    recentActivity: Activity[];
    notifications: Notification[];
    profileCompletion: ProfileCompletion;
    lastUpdated: string;
  };
}
```

### Get Activity Feed

```typescript
// GET /api/v1/professionals/{professionalId}/activities
interface GetActivitiesRequest {
  professionalId: string;
  type?: ActivityType | 'all';
  limit?: number;
  offset?: number;
  startDate?: string;
  endDate?: string;
}

interface GetActivitiesResponse {
  success: boolean;
  data: {
    activities: Activity[];
    total: number;
    hasMore: boolean;
  };
}
```

### Get Statistics

```typescript
// GET /api/v1/professionals/{professionalId}/stats
interface GetStatsRequest {
  professionalId: string;
  period?: 'day' | 'week' | 'month' | 'year';
}

interface GetStatsResponse {
  success: boolean;
  data: DashboardStats;
}
```

### Get Profile Completion

```typescript
// GET /api/v1/professionals/{professionalId}/profile-completion
interface GetProfileCompletionResponse {
  success: boolean;
  data: {
    percentage: number;
    completedSections: string[];
    missingFields: {
      section: string;
      field: string;
      label: string;
      required: boolean;
    }[];
    nextSteps: string[];
  };
}
```

## Real-Time Updates

### WebSocket Integration

```typescript
interface WebSocketConfig {
  url: string;
  reconnectInterval: number;
  maxReconnectAttempts: number;
}

// WebSocket message types
type WebSocketMessage =
  | { type: 'NEW_INQUIRY'; payload: Activity }
  | { type: 'NEW_CUSTOMER'; payload: Activity }
  | { type: 'REFERRAL_UPDATE'; payload: Activity }
  | { type: 'STAT_UPDATE'; payload: Partial<DashboardStats> }
  | { type: 'NOTIFICATION'; payload: Notification };

// WebSocket hook
const useWebSocket = (config: WebSocketConfig) => {
  const [connectionStatus, setConnectionStatus] = useState<'connected' | 'disconnected' | 'reconnecting'>('disconnected');
  const [messages, setMessages] = useState<WebSocketMessage[]>([]);

  // Implementation details...

  return { connectionStatus, messages, send };
};
```

## Responsive Behavior

### Desktop (≥1200px)
- 4-column stats grid
- 2-column content grid (Activity Feed: 2fr, Quick Actions: 1fr)
- Full sidebar visible
- All elements at full width

### Tablet (768px - 1199px)
- 2-column stats grid (2x2)
- 2-column content grid maintained
- Sidebar remains visible
- Reduced padding

### Mobile (<768px)
- Single column layout
- Stats cards stacked vertically
- Activity feed and quick actions stacked
- Collapsible sidebar (hamburger menu)
- Larger touch targets (48px minimum)
- Welcome banner text wraps

## Accessibility Requirements

### WCAG 2.1 AA Compliance

```typescript
const accessibilityFeatures = {
  semanticHTML: true,
  ariaLabels: true,
  keyboardNavigation: true,
  focusManagement: true,
  colorContrast: 'AAA', // Exceeds AA requirements
  screenReaderSupport: true
};
```

### Keyboard Navigation

| Action | Shortcut |
|--------|----------|
| Navigate stats cards | Tab / Shift+Tab |
| Navigate sidebar | Arrow keys |
| Open quick action | Enter / Space |
| View activity details | Enter |
| Refresh dashboard | Ctrl+R |

### Screen Reader Support

```html
<!-- Sidebar -->
<nav aria-label="Main navigation">
  <ul role="menu">
    <li role="menuitem" aria-current="page">Dashboard</li>
    <li role="menuitem">
      Customers
    </li>
    <li role="menuitem">
      Inquiries
      <span aria-label="8 new inquiries" class="badge">8</span>
    </li>
  </ul>
</nav>

<!-- Stats Card -->
<div
  role="article"
  aria-label="Total customers statistic"
  tabindex="0"
>
  <h3 id="stat-title">Total Customers</h3>
  <p aria-describedby="stat-title">142</p>
  <p aria-live="polite">12% increase from last month</p>
</div>

<!-- Activity Feed -->
<section aria-label="Recent activity">
  <h2>Recent Activity</h2>
  <ul role="feed" aria-busy="false">
    <li role="article" aria-posinset="1" aria-setsize="6">
      <h3>New customer added</h3>
      <p>Sarah Johnson requested brake service</p>
      <time datetime="2026-01-08T10:00:00Z">2 hours ago</time>
    </li>
  </ul>
</section>

<!-- Quick Actions -->
<section aria-label="Quick actions">
  <h2>Quick Actions</h2>
  <div role="group">
    <button
      aria-label="Add new customer"
      aria-describedby="action-1-desc"
    >
      <span id="action-1-desc">Register a new customer</span>
    </button>
  </div>
</section>
```

## Performance Considerations

### Data Fetching Strategy

```typescript
// Initial load: Fetch critical data
const initialLoad = async () => {
  const [stats, activities] = await Promise.all([
    fetchStats(),
    fetchActivities({ limit: 10 })
  ]);

  return { stats, activities };
};

// Background refresh: Update non-critical data
const backgroundRefresh = async () => {
  const [profileCompletion, notifications] = await Promise.all([
    fetchProfileCompletion(),
    fetchNotifications()
  ]);

  return { profileCompletion, notifications };
};
```

### Caching Strategy

```typescript
const cacheConfig = {
  stats: { ttl: 300000, staleWhileRevalidate: true }, // 5 minutes
  activities: { ttl: 60000, staleWhileRevalidate: true }, // 1 minute
  profileCompletion: { ttl: 3600000 }, // 1 hour
};
```

### Optimization Techniques

1. **Lazy Loading**: Load activity details on demand
2. **Virtual Scrolling**: For long activity lists
3. **Debounced Updates**: Batch real-time updates
4. **Memoization**: Cache computed values
5. **Code Splitting**: Separate bundles for dashboard widgets

## Analytics Events

```typescript
// Page view
trackEvent('professional_dashboard_viewed', {
  professionalId: user.id,
  profileCompletion: profileCompletion.percentage
});

// Stat card interaction
trackEvent('dashboard_stat_clicked', {
  statType: 'total_customers',
  value: 142
});

// Activity interaction
trackEvent('dashboard_activity_clicked', {
  activityType: 'customer',
  activityId: activity.id
});

// Quick action
trackEvent('dashboard_quick_action_clicked', {
  actionType: 'add_customer'
});

// Navigation
trackEvent('dashboard_navigation_clicked', {
  destination: 'customers'
});

// Profile completion
trackEvent('dashboard_complete_profile_clicked', {
  currentCompletion: 75
});
```

## Testing Requirements

### Unit Tests

```typescript
describe('ProfessionalDashboardHome', () => {
  describe('Rendering', () => {
    it('renders all dashboard sections', () => {});
    it('displays user welcome message', () => {});
    it('shows correct stats values', () => {});
    it('renders activity feed items', () => {});
    it('displays quick action buttons', () => {});
  });

  describe('Stats Cards', () => {
    it('formats stat values correctly', () => {});
    it('displays trend indicators', () => {});
    it('shows positive trend in green', () => {});
    it('shows negative trend in red', () => {});
    it('handles missing trend data', () => {});
  });

  describe('Activity Feed', () => {
    it('displays activities in chronological order', () => {});
    it('shows activity icons based on type', () => {});
    it('formats timestamps correctly', () => {});
    it('displays activity badges', () => {});
    it('handles empty activity list', () => {});
  });

  describe('Profile Completion', () => {
    it('displays correct completion percentage', () => {});
    it('updates progress bar width', () => {});
    it('shows complete profile link', () => {});
    it('hides when profile is 100% complete', () => {});
  });

  describe('Real-time Updates', () => {
    it('connects to WebSocket on mount', () => {});
    it('updates stats on WebSocket message', () => {});
    it('adds new activity on WebSocket message', () => {});
    it('reconnects on connection loss', () => {});
  });

  describe('Accessibility', () => {
    it('passes axe accessibility audit', () => {});
    it('has proper ARIA labels', () => {});
    it('supports keyboard navigation', () => {});
    it('announces live region updates', () => {});
  });
});
```

### Integration Tests

```typescript
describe('Dashboard Integration', () => {
  it('loads dashboard data on mount', () => {});
  it('refreshes data on interval', () => {});
  it('navigates to customers page', () => {});
  it('navigates to quick action routes', () => {});
  it('filters activities by type', () => {});
  it('handles API errors gracefully', () => {});
  it('retries failed requests', () => {});
});
```

### E2E Tests

```typescript
describe('Professional Dashboard Flow', () => {
  it('displays dashboard after login', () => {});
  it('shows personalized welcome message', () => {});
  it('updates stats in real-time', () => {});
  it('navigates to customer detail from activity', () => {});
  it('creates new customer from quick action', () => {});
  it('completes profile from banner link', () => {});
  it('responds to inquiry from activity feed', () => {});
});
```

## Error Handling

### Error States

```typescript
interface ErrorState {
  type: 'NETWORK_ERROR' | 'SERVER_ERROR' | 'UNAUTHORIZED' | 'NOT_FOUND';
  message: string;
  retryable: boolean;
  action?: {
    label: string;
    handler: () => void;
  };
}

const errorMessages = {
  NETWORK_ERROR: {
    title: 'Connection Error',
    message: 'Unable to load dashboard data. Please check your internet connection.',
    retryable: true
  },
  SERVER_ERROR: {
    title: 'Server Error',
    message: 'We\'re having trouble loading your dashboard. Please try again.',
    retryable: true
  },
  UNAUTHORIZED: {
    title: 'Session Expired',
    message: 'Your session has expired. Please log in again.',
    retryable: false
  }
};
```

### Fallback UI

```typescript
const DashboardError: React.FC<{ error: ErrorState }> = ({ error }) => (
  <div className="dashboard-error">
    <h2>{error.message}</h2>
    {error.retryable && (
      <button onClick={error.action?.handler}>
        Retry
      </button>
    )}
  </div>
);
```

## Dependencies

- React 18+ or Angular 15+
- Chart library (for future trend visualizations)
- WebSocket client library
- Date formatting library (date-fns or moment)
- State management (Redux/NgRx or Context API)

## Related Components

- `Sidebar` - Shared navigation sidebar
- `TopBar` - Shared top navigation
- `StatCard` - Reusable stat card component
- `ActivityItem` - Activity feed item component
- `QuickActionButton` - Quick action button component
- `Badge` - Status badge component
- `ProgressBar` - Progress indicator component

## Future Enhancements

1. **Charts & Visualizations**: Add trend charts for stats over time
2. **Customizable Dashboard**: Allow users to rearrange widgets
3. **Export Data**: Export dashboard data to PDF/CSV
4. **Advanced Filtering**: Filter activities by date range, type, status
5. **Dark Mode**: Support dark theme preference
6. **Notification Center**: Expanded notification management
7. **Dashboard Templates**: Industry-specific dashboard layouts

## Changelog

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2026-01-08 | Initial specification |
