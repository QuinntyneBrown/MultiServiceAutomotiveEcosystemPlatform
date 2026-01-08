# Customer Referral Dashboard - Screenshots & Visual Documentation

## Overview

This document provides visual documentation and descriptions of the Customer Referral Dashboard interface components and layouts.

---

## Page Layout

### Desktop View (≥992px)

```
┌─────────────────────────────────────────────────────────────────────┐
│ HEADER                                                              │
│ [Logo] AutoPro Network    Dashboard | Vehicles | History |         │
│                           REFERRALS (active) | Rewards     [Avatar] │
└─────────────────────────────────────────────────────────────────────┘
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│  My Referrals                                                       │
│  Share your referral code and earn rewards when friends join        │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ YOUR REFERRAL CODE                                            │ │
│  │ Share this code with friends and family                       │ │
│  │                                                               │ │
│  │  ┌─────────────────────────────┐         ┌─────────────────┐│ │
│  │  │ Referral Code               │         │                 ││ │
│  │  │ JDOE2026                    │         │   [QR Code]     ││ │
│  │  │ autopro.network/join/JDOE...│         │                 ││ │
│  │  │                             │         │  Scan to Share  ││ │
│  │  │ [Copy Link] [Email] [SMS]   │         └─────────────────┘│ │
│  │  │ [Facebook Share]            │                            │ │
│  │  └─────────────────────────────┘                            │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐             │
│  │  [👥]    │ │  [✓]     │ │  [⏰]    │ │  [$]     │             │
│  │   12     │ │   7      │ │   3      │ │  $350    │             │
│  │ Total    │ │ Success  │ │ Pending  │ │ Rewards  │             │
│  │ Sent     │ │ Converts │ │ Referrals│ │ Earned   │             │
│  │ +3 month │ │ 58% rate │ │ Awaiting │ │ +$100 mo │             │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘             │
│                                                                     │
│  ┌─────────────────────────────────┐ ┌─────────────────────────┐  │
│  │ My Referrals          View All →│ │ Rewards Summary         │  │
│  ├─────────────────────────────────┤ ├─────────────────────────┤  │
│  │ ┌─────────────────────────────┐ │ │ Available Balance       │  │
│  │ │ Sarah Johnson      [Rewarded]│ │ │     $200                │  │
│  │ │ sarah.j@email.com            │ │ │  Ready to use           │  │
│  │ │ 📅 Dec 15 | 💰 $50 earned   │ │ │                         │  │
│  │ │ ●━●━●━●━● (Timeline)         │ │ │ Earned: $350            │  │
│  │ └─────────────────────────────┘ │ │ Pending: $150           │  │
│  │                                 │ │ Redeemed: $150          │  │
│  │ ┌─────────────────────────────┐ │ │                         │  │
│  │ │ Mike Peterson    [Converted] │ │ │ [Redeem Rewards]        │  │
│  │ │ mike.p@email.com             │ │ │ (Phase 3)               │  │
│  │ │ 📅 Jan 2 | 💰 $50 pending   │ │ └─────────────────────────┘  │
│  │ │ ●━●━●━○━○ (Timeline)         │ │                             │
│  │ └─────────────────────────────┘ │ ┌─────────────────────────┐  │
│  │                                 │ │ Invite a Friend         │  │
│  │ ┌─────────────────────────────┐ │ ├─────────────────────────┤  │
│  │ │ Lisa Anderson      [Pending] │ │ │ Friend's Name *         │  │
│  │ │ lisa.a@email.com             │ │ │ [____________]          │  │
│  │ │ 📅 Jan 5 | ⏰ Exp Feb 5     │ │ │                         │  │
│  │ │ ●━○━○━○━○ (Timeline)         │ │ │ Friend's Email *        │  │
│  │ └─────────────────────────────┘ │ │ [____________]          │  │
│  │                                 │ │                         │  │
│  └─────────────────────────────────┘ │ Friend's Phone          │  │
│                                      │ [____________]          │  │
│                                      │                         │  │
│                                      │ Personal Message        │  │
│                                      │ [____________]          │  │
│                                      │ [____________]          │  │
│                                      │                         │  │
│                                      │ [Send Invitation]       │  │
│                                      └─────────────────────────┘  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
                                        [Toast: Link copied! ✓]
```

---

## Component Breakdowns

### 1. Referral Code Card

**Visual Hierarchy:**
```
┌─────────────────────────────────────────────────────────────┐
│ YOUR REFERRAL CODE                                          │
│ Share this code with friends and family                     │
│                                                             │
│ ┌────────────────────────┐              ┌────────────────┐ │
│ │ Referral Code          │              │                │ │
│ │                        │              │  ████████████  │ │
│ │     JDOE2026           │              │  ████████████  │ │
│ │                        │              │  ████████████  │ │
│ │ autopro.network/join/  │              │  ████████████  │ │
│ │ JDOE2026               │              │                │ │
│ │                        │              │  Scan to Share │ │
│ │ [Copy Link] [Email]    │              └────────────────┘ │
│ │ [SMS] [Facebook Share] │                                 │
│ └────────────────────────┘                                 │
└─────────────────────────────────────────────────────────────┘
```

**Design Specifications:**
- Background: Linear gradient 135deg, #00529F to #003366
- Text: White (#FFFFFF)
- Code Display:
  - Background: rgba(255, 255, 255, 0.15)
  - Border: 2px dashed rgba(255, 255, 255, 0.3)
  - Border radius: 8px
  - Padding: 24px
- Code Text:
  - Font: Monospace
  - Size: 36px (2.25rem)
  - Weight: Bold (700)
  - Letter spacing: 0.05em
- Share Buttons:
  - Height: 40px
  - Padding: 12px 20px
  - Background: rgba(255, 255, 255, 0.2)
  - Border: 1px solid rgba(255, 255, 255, 0.3)
  - Copy Link (Primary): Background #FFD520, text #001F3F
- QR Code:
  - Size: 160x160px
  - Background: White
  - Border radius: 4px
  - Padding: 16px

**States:**
- Hover on buttons: Background rgba(255, 255, 255, 0.3), translateY(-2px)
- Active/Click: Scale(0.98)

---

### 2. Statistics Cards

**Individual Card Layout:**
```
┌──────────────────────┐
│  ┌────┐              │
│  │ 👥 │         12   │
│  └────┘              │
│                      │
│  Total Referrals     │
│  Sent                │
│                      │
│  ↑ +3 this month     │
└──────────────────────┘
```

**Design Specifications:**
- Card Size: Minimum 240px wide, auto height
- Background: White (#FFFFFF)
- Border radius: 12px
- Box shadow: 0 1px 3px rgba(0, 0, 0, 0.1)
- Padding: 24px
- Hover: translateY(-2px), shadow increase

**Icon Styles:**
- Size: 48x48px
- Border radius: 8px
- Icon size: 24x24px inside
- Colors:
  - Blue: Background #E3F2FD, Icon #00529F
  - Green: Background #E8F5E9, Icon #2E7D32
  - Yellow: Background #FFF2CC, Icon #B8860B
  - Gray: Background #F2F2F2, Icon #4D4D4D

**Typography:**
- Value: 36px (2.25rem), bold, #1A1A1A
- Label: 14px (0.875rem), regular, #666666
- Change: 12px (0.75rem), color-coded

**Grid Behavior:**
- Desktop (≥992px): 4 columns
- Tablet (768-991px): 2 columns
- Mobile (<768px): 1 column
- Gap: 24px

---

### 3. Referral List Item

**Layout:**
```
┌─────────────────────────────────────────────────────┐
│ Sarah Johnson                          [Rewarded ●] │
│ sarah.j@email.com                                   │
│                                                     │
│ 📅 Joined Dec 15, 2025    💰 $50 earned            │
│                                                     │
│ ─────────────────────────────────────────────────── │
│                                                     │
│ ●━━━●━━━●━━━●━━━●                                  │
│ Sent Opened SignUp Service Reward                  │
│  ✓    ✓     ✓      ✓       ✓                       │
└─────────────────────────────────────────────────────┘
```

**Design Specifications:**
- Background: White
- Border: 1px solid #E5E5E5
- Border radius: 8px
- Padding: 20px
- Hover: Border color #00529F, box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1)

**Status Badges:**
- Height: 24px
- Padding: 4px 12px
- Border radius: 9999px (pill)
- Font: 12px, semibold
- Colors:
  - Pending: Background #FFF3E0, Text #ED6C02
  - Converted: Background #E8F5E9, Text #2E7D32
  - Rewarded: Background #E3F2FD, Text #00529F
  - Expired: Background #F2F2F2, Text #666666

---

### 4. Status Timeline

**Visual Design:**
```
●━━━━●━━━━●━━━━○━━━━○
✓     ✓     ✓

Sent  Opened  SignUp  Service  Reward
```

**Step States:**

**Completed:**
- Circle: 24px diameter, Background #2E7D32, Border 3px white
- Checkmark: White, 12px
- Line: 2px height, #2E7D32
- Label: #1A1A1A, semibold

**Active:**
- Circle: 24px diameter, Background #00529F
- Glow: 0 0 0 4px rgba(0, 82, 159, 0.2)
- Line: 2px height, #CCCCCC
- Label: #1A1A1A, semibold

**Pending:**
- Circle: 24px diameter, Background #CCCCCC
- Line: 2px height, #CCCCCC
- Label: #666666, medium

**Responsive:**
- Desktop: All steps visible inline
- Mobile: Horizontal scroll, minimum step width 80px

---

### 5. Rewards Summary

**Layout:**
```
┌─────────────────────┐
│ Available Balance   │
│                     │
│      $200           │
│   Ready to use      │
└─────────────────────┘

┌─────────────────────┐
│ ✓ Earned    $350    │
│ ⏰ Pending   $150    │
│ ↓ Redeemed  $150    │
└─────────────────────┘

┌─────────────────────┐
│  Redeem Rewards     │
│    (Phase 3)        │
└─────────────────────┘
```

**Design Specifications:**

**Balance Section:**
- Background: Linear gradient 135deg, #FFF8E1 to #FFF2CC
- Border radius: 8px
- Padding: 24px
- Text align: center
- Value: 48px (3rem), bold, #001F3F

**Breakdown:**
- Row height: Auto with 16px gap
- Border bottom: 1px solid #E5E5E5 (except last)
- Label: 14px, #4D4D4D, with 16px icon
- Value: 16px, semibold, #1A1A1A
- Pending value color: #ED6C02

**CTA Button:**
- Width: 100%
- Height: 48px
- Background: #00529F (disabled: #CCCCCC)
- Color: White (disabled: #808080)
- Border radius: 8px
- Font: 16px, semibold

---

### 6. Invitation Form

**Layout:**
```
┌─────────────────────────────────┐
│ Invite a Friend                 │
├─────────────────────────────────┤
│                                 │
│ Friend's Name *                 │
│ [________________________]      │
│                                 │
│ Friend's Email *                │
│ [________________________]      │
│                                 │
│ Friend's Phone (optional)       │
│ [________________________]      │
│                                 │
│ Personal Message (optional)     │
│ [________________________]      │
│ [________________________]      │
│ [________________________]      │
│                                 │
│ ┌─────────────────────────────┐ │
│ │    Send Invitation          │ │
│ └─────────────────────────────┘ │
└─────────────────────────────────┘
```

**Field Specifications:**
- Label: 14px, #4D4D4D, margin-bottom 8px
- Input height: 48px (single line), 80px (textarea)
- Padding: 12px 16px
- Border: 1px solid #999999
- Border radius: 8px
- Background: White
- Font: 16px, Lato

**States:**
- Focus: Border #00529F, box-shadow 0 0 0 3px rgba(0, 82, 159, 0.4)
- Error: Border #D32F2F, box-shadow 0 0 0 3px rgba(211, 47, 47, 0.4)
- Disabled: Background #F2F2F2, text #808080

**Gap Between Fields:** 20px

---

## Mobile Views

### Mobile Layout (<768px)

```
┌─────────────────────────────┐
│ ☰  AutoPro Network    [JD] │
└─────────────────────────────┘
┌─────────────────────────────┐
│ My Referrals                │
│ Share your referral code... │
│                             │
│ ┌─────────────────────────┐ │
│ │ YOUR REFERRAL CODE      │ │
│ │                         │ │
│ │ Referral Code           │ │
│ │    JDOE2026             │ │
│ │ autopro.network/join... │ │
│ │                         │ │
│ │   ████████████          │ │
│ │   ████████████          │ │
│ │   Scan to Share         │ │
│ │                         │ │
│ │ [Copy Link]             │ │
│ │ [Email] [SMS]           │ │
│ │ [Facebook Share]        │ │
│ └─────────────────────────┘ │
│                             │
│ ┌─────────────────────────┐ │
│ │  👥                     │ │
│ │  12                     │ │
│ │  Total Referrals Sent   │ │
│ │  +3 this month          │ │
│ └─────────────────────────┘ │
│                             │
│ ┌─────────────────────────┐ │
│ │  ✓                      │ │
│ │  7                      │ │
│ │  Successful Conversions │ │
│ │  58% conversion rate    │ │
│ └─────────────────────────┘ │
│                             │
│ (More stats...)             │
│                             │
│ ┌─────────────────────────┐ │
│ │ My Referrals  View All →│ │
│ ├─────────────────────────┤ │
│ │ Sarah Johnson [Rewarded]│ │
│ │ sarah.j@email.com       │ │
│ │ Dec 15 | $50 earned     │ │
│ │                         │ │
│ │ Timeline (scroll →)     │ │
│ │ ●━●━●━●━●               │ │
│ └─────────────────────────┘ │
│                             │
│ (More referrals...)         │
│                             │
│ ┌─────────────────────────┐ │
│ │ Rewards Summary         │ │
│ │                         │ │
│ │ Available Balance       │ │
│ │      $200               │ │
│ └─────────────────────────┘ │
│                             │
│ ┌─────────────────────────┐ │
│ │ Invite a Friend         │ │
│ │ (Form fields...)        │ │
│ └─────────────────────────┘ │
└─────────────────────────────┘
```

**Mobile Adaptations:**
1. Hamburger menu replaces navigation
2. Single column layout throughout
3. QR code moves below referral code (centered)
4. Share buttons stack/wrap
5. Statistics cards full width
6. Timeline scrolls horizontally
7. Form fields remain full width
8. Increased touch targets (minimum 44x44px)

---

## Color Palette Reference

### Primary Colors
```
Blue (#00529F)    ████████  Primary actions, headers
Yellow (#FFD520)  ████████  Accent, rewards, CTAs
```

### Status Colors
```
Success (#2E7D32) ████████  Converted, completed
Warning (#ED6C02) ████████  Pending, attention needed
Error (#D32F2F)   ████████  Errors, expired
Info (#0288D1)    ████████  Informational
```

### Neutral Colors
```
Gray 900 (#1A1A1A) ████████ Primary text
Gray 700 (#4D4D4D) ████████ Secondary text
Gray 500 (#808080) ████████ Disabled text
Gray 300 (#CCCCCC) ████████ Borders, dividers
Gray 100 (#F2F2F2) ████████ Light backgrounds
White (#FFFFFF)    ████████ Card backgrounds
```

### Background Tints
```
Blue 100 (#E3F2FD)   ████████ Light blue backgrounds
Yellow 200 (#FFF8E1) ████████ Light yellow backgrounds
Success 100 (#E8F5E9) ████████ Success backgrounds
Warning 100 (#FFF3E0) ████████ Warning backgrounds
```

---

## Typography Scale

### Headings
```
Page Title (36px)      My Referrals
Section Title (20px)   My Referrals
Card Title (18px)      YOUR REFERRAL CODE
```

### Body Text
```
Body Large (18px)      Share your referral code and earn...
Body Medium (16px)     Default text, form inputs
Body Small (14px)      Labels, metadata
Caption (12px)         Badges, helper text
Micro (10px)          Timeline labels
```

### Special
```
Display Code (36px)    JDOE2026
Display Value (48px)   $200
Stat Value (36px)      12
```

---

## Interactive States

### Button States

**Copy Link (Primary):**
```
Default:  [Copy Link]  bg:#FFD520 text:#001F3F
Hover:    [Copy Link]  bg:#FFDD4B shadow:md translateY(-1px)
Active:   [Copy Link]  bg:#DAA520 scale(0.98)
Focus:    [Copy Link]  ring:blue-4px
```

**Secondary Buttons:**
```
Default:  [Email]  bg:rgba(255,255,255,0.2) text:white
Hover:    [Email]  bg:rgba(255,255,255,0.3) translateY(-2px)
Active:   [Email]  bg:rgba(255,255,255,0.15)
```

**CTA Button:**
```
Default:  [Send Invitation]  bg:#00529F text:white
Hover:    [Send Invitation]  bg:#003366 shadow:md
Disabled: [Send Invitation]  bg:#CCCCCC text:#808080
```

### Card States
```
Default:  shadow:sm
Hover:    shadow:md translateY(-2px)
```

### Referral Item States
```
Default:  border:#E5E5E5
Hover:    border:#00529F shadow:sm
Active:   bg:#F5F9FF
```

---

## Spacing Reference

### Component Spacing
- Page padding: 32px (desktop), 24px (tablet), 16px (mobile)
- Section gap: 32px
- Card padding: 24px
- Card gap: 24px
- Form field gap: 20px
- Button gap: 12px
- Icon spacing: 8px

### Grid Gaps
- Statistics grid: 24px
- Dashboard grid: 24px
- Mobile grid: 16px

---

## Animation & Transitions

### Hover Animations
```css
transition: all 0.2s ease;
transform: translateY(-2px);
box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
```

### Button Click
```css
transform: scale(0.98);
transition: transform 0.1s ease;
```

### Toast Slide In
```css
@keyframes slideIn {
  from {
    transform: translateX(400px);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}
animation: slideIn 0.3s ease;
```

### Loading States
- Skeleton screens for data loading
- Pulse animation on loading elements
- Spinner for form submissions

---

## Accessibility Features

### Visual Indicators
- Focus rings: 3px blue outline on all interactive elements
- Status badges use icons + color + text
- High contrast text (4.5:1 minimum)
- Touch targets minimum 44x44px

### Screen Reader Text
```html
<span class="sr-only">Your referral code is</span>
<div aria-label="JDOE2026">JDOE2026</div>
```

### Keyboard Navigation
- Tab order follows visual hierarchy
- Skip links for main content
- Arrow keys for timeline navigation
- Enter/Space for button activation

---

## Edge Cases & Empty States

### No Referrals Sent
```
┌─────────────────────────────────┐
│                                 │
│         ╭─────╮                 │
│         │ 👥  │                 │
│         ╰─────╯                 │
│                                 │
│   No Referrals Yet              │
│                                 │
│   Share your code to start      │
│   earning rewards!              │
│                                 │
│   [Copy Referral Link]          │
│                                 │
└─────────────────────────────────┘
```

### All Referrals Expired
```
All 3 referrals have expired.
Send new invitations to continue earning!
```

### Network Error
```
┌─────────────────────────────────┐
│   ⚠️                            │
│   Unable to load referrals      │
│                                 │
│   Please check your connection  │
│   and try again.                │
│                                 │
│   [Retry]                       │
└─────────────────────────────────┘
```

---

## Toast Notifications

### Success Toast
```
┌──────────────────────────────┐
│ ✓  Link copied to clipboard! │
└──────────────────────────────┘
bg: #2E7D32
text: white
position: bottom-right
duration: 3s
```

### Error Toast
```
┌──────────────────────────────┐
│ ✗  Failed to send invitation │
└──────────────────────────────┘
bg: #D32F2F
text: white
position: bottom-right
duration: 5s
```

---

## Loading States

### Skeleton Screens
- Statistics cards: Gray pulse rectangles
- Referral list: Gray pulse cards
- Rewards: Gray pulse for amounts

### Spinner
- Size: 24px
- Color: #00529F
- Position: Center of loading area
- Animation: Rotate 360deg, 0.8s infinite

---

## Print Styles

When printing the page:
- Hide: Header navigation, share buttons, forms
- Show: Referral code enlarged, QR code prominent
- Black & white friendly: Replace colors with patterns
- Single column layout
- Page breaks between sections

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | January 2026 | Initial visual documentation |
