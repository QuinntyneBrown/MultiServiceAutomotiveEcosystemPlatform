# Professional Referral Dashboard - Screenshots Documentation

**Page**: Professional Referral Dashboard
**Version**: 1.0.0
**Last Updated**: January 2026

---

## Overview

This document outlines the required screenshots for the Professional Referral Dashboard page documentation, including viewport sizes, states to capture, and naming conventions.

---

## Screenshot Requirements

### General Guidelines

**Tools**:
- Browser: Chrome (latest version) or Firefox
- Screen capture: Full-page screenshot capability
- Resolution: Retina/HiDPI (2x) preferred
- Format: PNG with transparency where applicable

**Naming Convention**:
```
professional-referral-dashboard-[view]-[state]-[viewport].png
```

Examples:
- `professional-referral-dashboard-main-default-desktop.png`
- `professional-referral-dashboard-modal-open-tablet.png`
- `professional-referral-dashboard-panel-hover-mobile.png`

---

## Required Screenshots

### 1. Full Page Views

#### 1.1 Desktop - Default State
**Filename**: `professional-referral-dashboard-full-default-desktop.png`
**Viewport**: 1920 x 1080px
**Description**: Full dashboard view showing all sections

**Capture**:
- ✅ Sidebar navigation (expanded)
- ✅ Top bar with notifications
- ✅ Page header with "Send New Referral" button
- ✅ All 4 quick stats cards
- ✅ Both referral panels (Sent & Received) side-by-side
- ✅ Quick actions panel at bottom
- ✅ Scroll to show all content

**States**:
- User: Logged in as "John Smith"
- Notifications: Badge showing "5"
- Stats populated with sample data
- Referral lists showing 5-7 items each

---

#### 1.2 Desktop - With Modal Open
**Filename**: `professional-referral-dashboard-modal-open-desktop.png`
**Viewport**: 1920 x 1080px
**Description**: Send Referral Modal open over dashboard

**Capture**:
- ✅ Darkened backdrop overlay
- ✅ Send Referral Modal centered
- ✅ All form fields visible
- ✅ Discount toggle in OFF state
- ✅ Close button visible

---

#### 1.3 Desktop - Modal with Discount Options
**Filename**: `professional-referral-dashboard-modal-discount-desktop.png`
**Viewport**: 1920 x 1080px
**Description**: Send Referral Modal with discount section expanded

**Capture**:
- ✅ Send Referral Modal
- ✅ Discount toggle in ON state (blue)
- ✅ Discount options section expanded and visible
- ✅ All discount fields shown (type, value, expiration)

---

#### 1.4 Tablet - Default State
**Filename**: `professional-referral-dashboard-full-default-tablet.png`
**Viewport**: 1024 x 768px
**Description**: Full dashboard on tablet with panels stacked

**Capture**:
- ✅ Hamburger menu icon (sidebar hidden)
- ✅ Top bar responsive layout
- ✅ Stats cards in 2x2 grid
- ✅ Sent referrals panel (full width)
- ✅ Received referrals panel (full width, stacked below)
- ✅ Quick actions panel

---

#### 1.5 Mobile - Default State
**Filename**: `professional-referral-dashboard-full-default-mobile.png`
**Viewport**: 375 x 812px (iPhone X)
**Description**: Full dashboard on mobile

**Capture**:
- ✅ Hamburger menu icon
- ✅ Compact top bar
- ✅ Stats cards in single column
- ✅ Sent referrals panel (full width)
- ✅ Received referrals panel (full width)
- ✅ Quick actions panel (stacked buttons)
- ✅ Full scroll capture

---

### 2. Component Close-ups

#### 2.1 Quick Stats Section
**Filename**: `professional-referral-dashboard-stats-default-desktop.png`
**Viewport**: Crop to stats section
**Description**: Close-up of all 4 stat cards

**Capture**:
- ✅ All 4 stat cards in grid layout
- ✅ Values, labels, icons clearly visible
- ✅ Trend indicators with percentages
- ✅ Proper spacing between cards

---

#### 2.2 Quick Stats - Hover State
**Filename**: `professional-referral-dashboard-stats-hover-desktop.png`
**Viewport**: Crop to stats section
**Description**: One stat card in hover state

**Capture**:
- ✅ One card elevated (shadow increased)
- ✅ Card slightly translated upward
- ✅ Other cards in default state for comparison

---

#### 2.3 Sent Referrals Panel
**Filename**: `professional-referral-dashboard-sent-panel-desktop.png`
**Viewport**: Crop to sent panel
**Description**: Sent referrals panel with various statuses

**Capture**:
- ✅ Panel header with title and count badge
- ✅ Filter tabs (All selected)
- ✅ Minimum 4 referral items showing different statuses:
  - Accepted (green border)
  - Pending (yellow border)
  - Declined (red border, reduced opacity)
- ✅ Action buttons visible
- ✅ Footer with "View All" link

---

#### 2.4 Sent Referrals Panel - Pending Filter
**Filename**: `professional-referral-dashboard-sent-panel-pending-desktop.png`
**Viewport**: Crop to sent panel
**Description**: Sent panel with "Pending" filter active

**Capture**:
- ✅ "Pending" tab button active (blue background)
- ✅ Only pending referrals visible
- ✅ Count badge updated
- ✅ "Send Reminder" buttons visible

---

#### 2.5 Received Referrals Panel
**Filename**: `professional-referral-dashboard-received-panel-desktop.png`
**Viewport**: Crop to received panel
**Description**: Received referrals panel with pending items

**Capture**:
- ✅ Panel header with title and count
- ✅ Filter tabs (Pending selected)
- ✅ Minimum 3 pending referrals showing:
  - High priority badge
  - Normal priority badge
  - Discount badges
  - Different service types
- ✅ Accept/Decline buttons visible
- ✅ Footer link

---

#### 2.6 Received Referrals Panel - Accepted Filter
**Filename**: `professional-referral-dashboard-received-panel-accepted-desktop.png`
**Viewport**: Crop to received panel
**Description**: Received panel with "Accepted" filter active

**Capture**:
- ✅ "Accepted" tab active
- ✅ Only accepted referrals shown
- ✅ Success badges visible
- ✅ "View Details" and "Contact Customer" buttons

---

#### 2.7 Referral Item - Hover State
**Filename**: `professional-referral-dashboard-item-hover-desktop.png`
**Viewport**: Crop to 1-2 items
**Description**: Referral item in hover state

**Capture**:
- ✅ One item with blue background (hover state)
- ✅ One item in default state for comparison
- ✅ All item details clearly visible

---

#### 2.8 Quick Actions Panel
**Filename**: `professional-referral-dashboard-quick-actions-desktop.png`
**Viewport**: Crop to quick actions section
**Description**: Quick actions panel with all buttons

**Capture**:
- ✅ Panel title
- ✅ All 3 action buttons:
  - Send New Referral (yellow icon)
  - View All Sent (blue icon)
  - View All Received (blue icon)
- ✅ Icons, titles, descriptions visible

---

#### 2.9 Quick Actions - Hover State
**Filename**: `professional-referral-dashboard-quick-actions-hover-desktop.png`
**Viewport**: Crop to quick actions section
**Description**: One action button in hover state

**Capture**:
- ✅ One button with blue background and border
- ✅ Other buttons in default state
- ✅ Hover effect clearly visible

---

### 3. Modal States

#### 3.1 Send Referral Modal - Empty
**Filename**: `professional-referral-dashboard-modal-empty-desktop.png`
**Viewport**: Modal only (cropped)
**Description**: Fresh modal with no data entered

**Capture**:
- ✅ Modal header with title and close button
- ✅ All form fields empty showing placeholders
- ✅ Discount toggle OFF
- ✅ Footer buttons (Cancel, Send Referral)
- ✅ No discount options visible

---

#### 3.2 Send Referral Modal - Filled
**Filename**: `professional-referral-dashboard-modal-filled-desktop.png`
**Viewport**: Modal only (cropped)
**Description**: Modal with all fields filled

**Capture**:
- ✅ All required fields populated with sample data:
  - Target Professional: "Elite Auto Body - Paint & Body Work"
  - Customer: "Sarah Williams - 2020 Honda Accord"
  - Service Needed: "Paint & Body Work"
  - Reason: Sample text
  - Priority: "High Priority"
  - Internal Notes: Sample text
- ✅ Discount toggle ON
- ✅ Discount options expanded
- ✅ Form ready to submit

---

#### 3.3 Send Referral Modal - Validation Errors
**Filename**: `professional-referral-dashboard-modal-errors-desktop.png`
**Viewport**: Modal only (cropped)
**Description**: Modal showing validation errors

**Capture**:
- ✅ Required fields empty or invalid
- ✅ Red borders on invalid fields
- ✅ Error messages below fields
- ✅ Submit button attempted but failed
- ✅ Highlight on first error field

---

#### 3.4 Send Referral Modal - Focus State
**Filename**: `professional-referral-dashboard-modal-focus-desktop.png`
**Viewport**: Modal only (cropped)
**Description**: Modal with input field focused

**Capture**:
- ✅ One input field focused
- ✅ Blue border and shadow on focused field
- ✅ Cursor visible in field
- ✅ Focus ring clearly visible

---

#### 3.5 Send Referral Modal - Discount Expanded
**Filename**: `professional-referral-dashboard-modal-discount-expanded-desktop.png`
**Viewport**: Modal only (cropped)
**Description**: Discount options section expanded

**Capture**:
- ✅ Toggle in ON position (blue)
- ✅ Discount type dropdown (Percentage selected)
- ✅ Discount value field (10)
- ✅ Expiration date picker
- ✅ Animation complete, fields visible

---

#### 3.6 Send Referral Modal - Mobile
**Filename**: `professional-referral-dashboard-modal-mobile.png`
**Viewport**: 375 x 812px
**Description**: Modal on mobile device

**Capture**:
- ✅ Modal fills most of screen
- ✅ All form fields stacked vertically
- ✅ Buttons stacked if needed
- ✅ Scrollable content
- ✅ Touch-friendly spacing

---

### 4. Interactive States

#### 4.1 Notification Badge
**Filename**: `professional-referral-dashboard-notification-badge-desktop.png`
**Viewport**: Crop to top bar
**Description**: Close-up of notification bell with badge

**Capture**:
- ✅ Bell icon
- ✅ Red badge with number "5"
- ✅ Badge positioned correctly

---

#### 4.2 Tab Filter - Active State
**Filename**: `professional-referral-dashboard-tabs-active-desktop.png`
**Viewport**: Crop to panel header
**Description**: Filter tabs with one active

**Capture**:
- ✅ Active tab: Blue background, white text
- ✅ Inactive tabs: White background, gray text with border
- ✅ Clear visual distinction

---

#### 4.3 Button States - Primary
**Filename**: `professional-referral-dashboard-button-primary-states-desktop.png`
**Viewport**: Component library style
**Description**: Primary button in all states

**Capture**:
Side by side comparison:
- ✅ Default state
- ✅ Hover state
- ✅ Active/pressed state
- ✅ Focus state (with ring)
- ✅ Disabled state

---

#### 4.4 Badge Variants
**Filename**: `professional-referral-dashboard-badges-desktop.png`
**Viewport**: Component library style
**Description**: All badge color variants

**Capture**:
All badges in a row:
- ✅ Success (green) - "Accepted"
- ✅ Warning (yellow) - "Pending"
- ✅ Error (red) - "Declined"
- ✅ Primary (blue) - "High Priority"
- ✅ Secondary (gray) - "Normal"
- ✅ Accent (yellow) - "10% Discount"

---

### 5. Responsive Views

#### 5.1 Mobile - Stats Cards
**Filename**: `professional-referral-dashboard-stats-mobile.png`
**Viewport**: 375 x 812px (crop to stats)
**Description**: Stats cards in mobile single-column layout

**Capture**:
- ✅ All 4 cards stacked vertically
- ✅ Full width cards
- ✅ Proper spacing between cards
- ✅ All content readable

---

#### 5.2 Mobile - Sent Panel
**Filename**: `professional-referral-dashboard-sent-panel-mobile.png`
**Viewport**: 375 x 812px (crop to panel)
**Description**: Sent referrals panel on mobile

**Capture**:
- ✅ Header with wrapped title and count
- ✅ Filter tabs responsive layout
- ✅ Referral items stacked
- ✅ Action buttons stacked or horizontal scroll
- ✅ Touch-friendly sizing

---

#### 5.3 Mobile - Quick Actions
**Filename**: `professional-referral-dashboard-quick-actions-mobile.png`
**Viewport**: 375 x 812px (crop to panel)
**Description**: Quick actions on mobile

**Capture**:
- ✅ Action buttons full width
- ✅ Icons and text clearly visible
- ✅ Proper spacing for touch targets
- ✅ All 3 buttons visible

---

#### 5.4 Tablet - Two Column Stats
**Filename**: `professional-referral-dashboard-stats-tablet.png`
**Viewport**: 1024 x 768px (crop to stats)
**Description**: Stats in 2x2 grid on tablet

**Capture**:
- ✅ 2 columns, 2 rows
- ✅ Proper spacing
- ✅ Cards maintain aspect ratio
- ✅ Readable at tablet size

---

### 6. Edge Cases & Empty States

#### 6.1 Empty Sent Referrals
**Filename**: `professional-referral-dashboard-sent-empty-desktop.png`
**Viewport**: Crop to sent panel
**Description**: Sent panel with no referrals

**Capture**:
- ✅ Panel header
- ✅ Empty state icon (gray circle with icon)
- ✅ Empty state title: "No sent referrals"
- ✅ Empty state description
- ✅ CTA button: "Send Your First Referral"

---

#### 6.2 Empty Received Referrals
**Filename**: `professional-referral-dashboard-received-empty-desktop.png`
**Viewport**: Crop to received panel
**Description**: Received panel with no referrals

**Capture**:
- ✅ Panel header
- ✅ Empty state icon
- ✅ Empty state title: "No received referrals"
- ✅ Empty state description
- ✅ Helpful message about how referrals work

---

#### 6.3 Loading State
**Filename**: `professional-referral-dashboard-loading-desktop.png`
**Viewport**: 1920 x 1080px
**Description**: Dashboard loading state

**Capture**:
- ✅ Skeleton screens for stats cards
- ✅ Loading indicators in panels
- ✅ Shimmer/pulse animation (if possible)
- ✅ Overall loading state

---

#### 6.4 Error State
**Filename**: `professional-referral-dashboard-error-desktop.png`
**Viewport**: Crop to one panel
**Description**: Error state when data fails to load

**Capture**:
- ✅ Error icon (red)
- ✅ Error message: "Failed to load referrals"
- ✅ Retry button
- ✅ Helpful error description

---

### 7. Accessibility Features

#### 7.1 Focus States
**Filename**: `professional-referral-dashboard-focus-states-desktop.png`
**Viewport**: Component examples
**Description**: Various components with keyboard focus

**Capture**:
Side by side examples:
- ✅ Button with focus ring
- ✅ Input field with focus ring
- ✅ Link with focus outline
- ✅ Card/action item with focus state

---

#### 7.2 High Contrast Mode
**Filename**: `professional-referral-dashboard-high-contrast-desktop.png`
**Viewport**: 1920 x 1080px
**Description**: Dashboard in high contrast mode

**Capture**:
- ✅ Full page in high contrast
- ✅ All text clearly readable
- ✅ Borders and separators visible
- ✅ Interactive elements distinguishable

---

### 8. Print View

#### 8.1 Print Layout
**Filename**: `professional-referral-dashboard-print.png`
**Viewport**: A4 page size
**Description**: Print-friendly version of dashboard

**Capture**:
- ✅ Stats summary table
- ✅ Referral lists formatted for print
- ✅ No unnecessary decorations
- ✅ Black and white friendly
- ✅ Page breaks appropriate

---

## Screenshot Checklist

Before submitting screenshots, verify:

### Quality
- [ ] Images are clear and sharp (2x resolution)
- [ ] No pixelation or blur
- [ ] Colors are accurate
- [ ] Text is readable at all sizes
- [ ] Consistent browser chrome (or cropped)

### Content
- [ ] Sample data is realistic and professional
- [ ] No placeholder/lorem ipsum text
- [ ] Dates are reasonable (not 1970 or far future)
- [ ] Numbers make sense in context
- [ ] No debug/development artifacts visible

### Consistency
- [ ] Same user shown across all screenshots
- [ ] Consistent data between related views
- [ ] Matching visual style
- [ ] Same browser and version used
- [ ] Consistent zoom level

### Coverage
- [ ] All required screenshots captured
- [ ] All states documented
- [ ] Responsive views included
- [ ] Interactive states shown
- [ ] Edge cases covered

---

## Annotation Guidelines

If annotating screenshots:

### Tools
- Use Figma, Sketch, or similar
- Alternatively use Skitch or Snagit

### Annotation Style
- **Callouts**: Red boxes with numbers
- **Arrows**: Red arrows pointing to features
- **Text**: Sans-serif font, 14-16px, black on white background
- **Highlights**: Yellow transparent overlay for emphasis
- **Blur**: Blur any sensitive/test data if needed

### Common Annotations
1. "Primary CTA - Opens send referral modal"
2. "Real-time status indicator - updates automatically"
3. "Click to accept referral"
4. "Filters update list without page reload"
5. "Keyboard accessible - Tab to navigate"

---

## File Organization

```
screenshots/
├── desktop/
│   ├── full-page/
│   │   ├── professional-referral-dashboard-full-default-desktop.png
│   │   ├── professional-referral-dashboard-modal-open-desktop.png
│   │   └── professional-referral-dashboard-modal-discount-desktop.png
│   ├── components/
│   │   ├── professional-referral-dashboard-stats-default-desktop.png
│   │   ├── professional-referral-dashboard-sent-panel-desktop.png
│   │   └── professional-referral-dashboard-received-panel-desktop.png
│   ├── modals/
│   │   ├── professional-referral-dashboard-modal-empty-desktop.png
│   │   ├── professional-referral-dashboard-modal-filled-desktop.png
│   │   └── professional-referral-dashboard-modal-errors-desktop.png
│   └── states/
│       ├── professional-referral-dashboard-loading-desktop.png
│       └── professional-referral-dashboard-error-desktop.png
├── tablet/
│   ├── professional-referral-dashboard-full-default-tablet.png
│   └── professional-referral-dashboard-stats-tablet.png
├── mobile/
│   ├── professional-referral-dashboard-full-default-mobile.png
│   ├── professional-referral-dashboard-modal-mobile.png
│   └── professional-referral-dashboard-stats-mobile.png
└── annotated/
    └── [annotated versions of key screenshots]
```

---

## Screenshot Delivery

### Format
- **Primary**: PNG format, optimized
- **Alternative**: WebP for documentation sites
- **Size**: Optimize to < 500KB per image
- **Naming**: Follow convention strictly

### Metadata
Include a `screenshot-manifest.json`:

```json
{
  "page": "professional-referral-dashboard",
  "version": "1.0.0",
  "date": "2026-01-08",
  "screenshots": [
    {
      "filename": "professional-referral-dashboard-full-default-desktop.png",
      "viewport": "1920x1080",
      "device": "desktop",
      "state": "default",
      "description": "Full dashboard view showing all sections",
      "annotations": false
    },
    {
      "filename": "professional-referral-dashboard-modal-open-desktop.png",
      "viewport": "1920x1080",
      "device": "desktop",
      "state": "modal-open",
      "description": "Send Referral Modal open over dashboard",
      "annotations": false
    }
  ]
}
```

---

## Testing Views for Screenshots

### Browser DevTools Settings

**Chrome DevTools**:
1. Open DevTools (F12)
2. Toggle Device Toolbar (Ctrl+Shift+M)
3. Select viewport:
   - Desktop: Responsive, set to 1920x1080
   - Tablet: iPad Pro (1024x768)
   - Mobile: iPhone X (375x812)
4. Ensure "Device pixel ratio" is set to 2
5. Disable browser cache
6. Hard refresh (Ctrl+Shift+R)

**Firefox DevTools**:
1. Open DevTools (F12)
2. Click Responsive Design Mode (Ctrl+Shift+M)
3. Select device or enter custom dimensions
4. Set DPR to 2
5. Hard refresh

### Screenshot Capture

**Full Page**:
- Chrome: DevTools > More tools > Capture full size screenshot
- Firefox: Right-click > Take a Screenshot > Save full page
- Extension: Full Page Screen Capture

**Specific Area**:
- Use built-in snipping tool
- Mac: Cmd+Shift+4
- Windows: Win+Shift+S
- Linux: Gnome Screenshot or similar

---

## Review Checklist

Before final submission, review each screenshot:

### Visual Quality
- [ ] Sharp and clear at 100% zoom
- [ ] No compression artifacts
- [ ] Proper colors (not washed out or oversaturated)
- [ ] Consistent lighting/rendering

### Accuracy
- [ ] Matches current design spec
- [ ] Uses correct fonts
- [ ] Uses correct colors from design system
- [ ] Spacing matches specification
- [ ] No design inconsistencies

### Context
- [ ] Screenshot tells a clear story
- [ ] Purpose is obvious
- [ ] Labeled clearly
- [ ] Organized logically

### Completeness
- [ ] All required views captured
- [ ] All states documented
- [ ] No missing screenshots
- [ ] Manifest file included

---

## Version History

| Version | Date | Changes | Author |
|---------|------|---------|--------|
| 1.0.0 | 2026-01-08 | Initial screenshot documentation | Design Team |

---

## Questions or Issues?

For questions about screenshot requirements or to report issues with this documentation:

- **Design Team**: design@autopro.platform
- **Project Lead**: project-lead@autopro.platform
- **Documentation**: docs@autopro.platform
