# Professional Profile Page - ASCII Wireframes

## Overview

This document contains ASCII wireframes for the Professional Profile page, showing the layout and component hierarchy for desktop, tablet, and mobile viewports.

---

## Desktop View (1200px+)

```
┌─────────────────────────────────────────────────────────────────────────────────────┐
│                              PUBLIC NAVIGATION BAR                                  │
│  [Logo] AutoPro Network    [Find Pros] [How It Works] [About]  [Sign In] [Join]   │
└─────────────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────────────┐
│                                                                                     │
│                        COVER PHOTO / HERO BACKGROUND                                │
│                     (Linear Gradient: Blue 700 → Blue 900)                         │
│                                                                                     │
└─────────────────────────────────────────────────────────────────────────────────────┘
     │
     │  (Profile avatar overlaps cover photo)
     ▼
┌─────────────────────────────────────────────────────────────────────────────────────┐
│                                                                                     │
│  ┌────────────┐  ┌──────────────────────────────────────────┐  ┌──────────────┐   │
│  │            │  │  AutoPro Mechanics                        │  │ [📞] [✉️]     │   │
│  │  PROFILE   │  │  John Smith, ASE Certified Master Tech    │  │              │   │
│  │  AVATAR    │  │                                           │  │ [Send        │   │
│  │  160x160   │  │  [🔧 Mechanic] [✓ Verified Professional] │  │  Inquiry]    │   │
│  │            │  │                                           │  │              │   │
│  │    [✓]     │  │  ★★★★★ 5.0 (127 reviews)                 │  └──────────────┘   │
│  └────────────┘  └──────────────────────────────────────────┘                      │
│                                                                                     │
└─────────────────────────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────┬─────────────────────────────────────────┐
│                                           │                                         │
│  MAIN CONTENT AREA                        │  SIDEBAR (360px)                        │
│  (Flexible width)                         │  (Sticky positioned)                    │
│                                           │                                         │
│  ┌─────────────────────────────────────┐  │  ┌───────────────────────────────────┐ │
│  │ [👤] About                          │  │  │ [📞] Contact Information         │ │
│  │─────────────────────────────────────│  │  │───────────────────────────────────│ │
│  │                                     │  │  │ 📞 Phone                          │ │
│  │  Welcome to AutoPro Mechanics...    │  │  │ (555) 123-4567                    │ │
│  │  Bio text paragraph 1...            │  │  │                                   │ │
│  │                                     │  │  │ ✉️ Email                          │ │
│  │  Bio text paragraph 2...            │  │  │ info@autopromechanics.com        │ │
│  │                                     │  │  │                                   │ │
│  │  ┌──────────┐ ┌──────────┐ ┌──────┐│  │  │ 📍 Address                        │ │
│  │  │[📅] 15+  │ │[📍] 20   │ │[👥]  ││  │  │ 123 Main Street                   │ │
│  │  │Years     │ │Mile Rad  │ │5 Tech││  │  │ Springfield, IL 62701             │ │
│  │  └──────────┘ └──────────┘ └──────┘│  │  │                                   │ │
│  └─────────────────────────────────────┘  │  │ 🔗 Website                        │ │
│                                           │  │ autopromechanics.com              │ │
│  ┌─────────────────────────────────────┐  │  └───────────────────────────────────┘ │
│  │ [🔧] Specialties & Services         │  │                                         │
│  │─────────────────────────────────────│  │  ┌───────────────────────────────────┐ │
│  │                                     │  │  │ [🕐] Business Hours              │ │
│  │  ┌─────────┐ ┌─────────┐ ┌────────┐│  │  │───────────────────────────────────│ │
│  │  │[⚙️]     │ │[🔄]     │ │[🛑]    ││  │  │ Monday    8:00 AM - 6:00 PM      │ │
│  │  │Engine   │ │Transm.  │ │Brakes  ││  │  │                     • Open Now    │ │
│  │  │Repair   │ │Service  │ │Systems ││  │  │ Tuesday   8:00 AM - 6:00 PM      │ │
│  │  └─────────┘ └─────────┘ └────────┘│  │  │ Wednesday 8:00 AM - 6:00 PM      │ │
│  │                                     │  │  │ Thursday  8:00 AM - 6:00 PM      │ │
│  │  ┌─────────┐ ┌─────────┐ ┌────────┐│  │  │ Friday    8:00 AM - 6:00 PM      │ │
│  │  │[⚡]     │ │[❄️]     │ │[🔍]    ││  │  │ Saturday  9:00 AM - 3:00 PM      │ │
│  │  │Electric │ │A/C &    │ │Diagnos.││  │  │ Sunday    Closed                 │ │
│  │  │Systems  │ │Heating  │ │        ││  │  └───────────────────────────────────┘ │
│  │  └─────────┘ └─────────┘ └────────┘│  │                                         │
│  │                                     │  │  ┌───────────────────────────────────┐ │
│  │  Certifications                     │  │  │ [📍] Location                    │ │
│  │                                     │  │  │───────────────────────────────────│ │
│  │  ┌───────────────────────────────┐  │  │  │                                   │ │
│  │  │[🛡️] ASE Master Technician    │  │  │  │  ┌─────────────────────────────┐ │ │
│  │  │     National Institute for... │  │  │  │  │                             │ │ │
│  │  │     Certified Since 2015      │  │  │  │  │      MAP PLACEHOLDER        │ │ │
│  │  └───────────────────────────────┘  │  │  │  │                             │ │ │
│  │                                     │  │  │  └─────────────────────────────┘ │ │
│  │  ┌───────────────────────────────┐  │  │  │                                   │ │
│  │  │[🎖️] State Emission Inspector │  │  │  │  123 Main Street                  │ │
│  │  │     Department of Motor...    │  │  │  │  Springfield, IL 62701            │ │
│  │  │     Licensed 2018 - Present   │  │  │  │                                   │ │
│  │  └───────────────────────────────┘  │  │  │  [Get Directions →]               │ │
│  │                                     │  │  └───────────────────────────────────┘ │
│  │  ┌───────────────────────────────┐  │  │                                         │
│  │  │[✓] Hybrid Vehicle Specialist │  │  │  ┌───────────────────────────────────┐ │
│  │  │     Automotive Training...    │  │  │  │ [💬] Send an Inquiry             │ │
│  │  │     Certified Since 2020      │  │  │  │───────────────────────────────────│ │
│  │  └───────────────────────────────┘  │  │  │                                   │ │
│  └─────────────────────────────────────┘  │  │  Your Name *                      │ │
│                                           │  │  [____________________________]   │ │
│  ┌─────────────────────────────────────┐  │  │                                   │ │
│  │ [🖼️] Photo Gallery                 │  │  │  Email Address *                  │ │
│  │─────────────────────────────────────│  │  │  [____________________________]   │ │
│  │                                     │  │  │                                   │ │
│  │    ┌─────────────────────────┐      │  │  │  Phone Number                     │ │
│  │    │                         │      │  │  │  [____________________________]   │ │
│  │    │   [🖼️] Photo Gallery    │      │  │  │                                   │ │
│  │    │   Coming Soon           │      │  │  │  Message *                        │ │
│  │    │                         │      │  │  │  [____________________________]   │ │
│  │    │   Phase 2 Feature       │      │  │  │  [____________________________]   │ │
│  │    │                         │      │  │  │  [____________________________]   │ │
│  │    └─────────────────────────┘      │  │  │  [____________________________]   │ │
│  │                                     │  │  │                                   │ │
│  └─────────────────────────────────────┘  │  │  [Send Inquiry →]                 │ │
│                                           │  └───────────────────────────────────┘ │
│  ┌─────────────────────────────────────┐  │                                         │
│  │ [⭐] Customer Reviews               │  │                                         │
│  │─────────────────────────────────────│  │                                         │
│  │                                     │  │                                         │
│  │    ┌─────────────────────────┐      │  │                                         │
│  │    │                         │      │  │                                         │
│  │    │   [⭐] Reviews           │      │  │                                         │
│  │    │   Coming Soon           │      │  │                                         │
│  │    │                         │      │  │                                         │
│  │    │   Phase 3 Feature       │      │  │                                         │
│  │    │                         │      │  │                                         │
│  │    └─────────────────────────┘      │  │                                         │
│  │                                     │  │                                         │
│  └─────────────────────────────────────┘  │                                         │
│                                           │                                         │
└───────────────────────────────────────────┴─────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────────────┐
│                                    FOOTER                                           │
│                                                                                     │
│  AutoPro Network          For Professionals     For Customers      Company         │
│  ───────────────          ─────────────────     ──────────────     ─────────       │
│  Connecting auto          Join Network          Find Pros          About Us        │
│  professionals with       Dashboard             How It Works       Contact         │
│  customers...             Pricing               Referrals          Privacy         │
│                           Resources             FAQs               Terms           │
│                                                                                     │
│  ─────────────────────────────────────────────────────────────────────────────────  │
│                        © 2026 AutoPro Network. All rights reserved.                │
└─────────────────────────────────────────────────────────────────────────────────────┘
```

---

## Tablet View (768px - 991px)

```
┌─────────────────────────────────────────────────────────┐
│         PUBLIC NAVIGATION BAR                           │
│  [Logo] AutoPro  [Find] [How] [About] [Sign In] [Join] │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                                                         │
│              COVER PHOTO / HERO BACKGROUND              │
│                                                         │
└─────────────────────────────────────────────────────────┘
           │
           ▼
┌─────────────────────────────────────────────────────────┐
│  ┌──────────┐  ┌────────────────────┐  ┌──────────────┐│
│  │ PROFILE  │  │ AutoPro Mechanics  │  │ [📞] [✉️]     ││
│  │ AVATAR   │  │ John Smith, ASE... │  │              ││
│  │ 120x120  │  │                    │  │ [Send        ││
│  │   [✓]    │  │ [Mechanic] [✓]     │  │  Inquiry]    ││
│  └──────────┘  │ ★★★★★ 5.0 (127)    │  └──────────────┘│
│                └────────────────────┘                   │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│  MAIN CONTENT (Full width, sidebar below)               │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [👤] About                                        │  │
│  │───────────────────────────────────────────────────│  │
│  │ Bio text...                                       │  │
│  │                                                   │  │
│  │ ┌──────────┐ ┌──────────┐ ┌──────────┐          │  │
│  │ │15+ Years │ │20 Miles  │ │5 Techs   │          │  │
│  │ └──────────┘ └──────────┘ └──────────┘          │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [🔧] Specialties & Services                       │  │
│  │───────────────────────────────────────────────────│  │
│  │ [Grid of specialties - 2 columns on tablet]      │  │
│  │ [Certifications list]                             │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [🖼️] Photo Gallery (Phase 2)                     │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [⭐] Customer Reviews (Phase 3)                   │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│  SIDEBAR SECTIONS (Full width, stacked)                 │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [📞] Contact Information                          │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [🕐] Business Hours                               │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [📍] Location + Map                               │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
│  ┌───────────────────────────────────────────────────┐  │
│  │ [💬] Send an Inquiry                              │  │
│  │ [Inquiry Form]                                    │  │
│  └───────────────────────────────────────────────────┘  │
│                                                         │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                        FOOTER                           │
└─────────────────────────────────────────────────────────┘
```

---

## Mobile View (< 768px)

```
┌───────────────────────────────────────┐
│  [☰] AutoPro Network      [Join]     │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│                                       │
│        COVER PHOTO (200px)            │
│                                       │
└───────────────────────────────────────┘
              │
              ▼
┌───────────────────────────────────────┐
│         ┌──────────┐                  │
│         │ PROFILE  │                  │
│         │ AVATAR   │                  │
│         │ 120x120  │                  │
│         │   [✓]    │                  │
│         └──────────┘                  │
│                                       │
│      AutoPro Mechanics                │
│   John Smith, ASE Certified           │
│                                       │
│    [🔧 Mechanic/Repair Shop]          │
│    [✓ Verified Professional]          │
│                                       │
│      ★★★★★ 5.0 (127 reviews)          │
│                                       │
│  ┌─────────┐ ┌────────┐ ┌──────────┐ │
│  │  [📞]   │ │  [✉️]  │ │   Send   │ │
│  │  Call   │ │ Email  │ │ Inquiry  │ │
│  └─────────┘ └────────┘ └──────────┘ │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [👤] About                            │
│───────────────────────────────────────│
│                                       │
│ Welcome to AutoPro Mechanics...       │
│ Bio text paragraph 1...               │
│                                       │
│ Bio text paragraph 2...               │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [📅] 15+ Years in Business        │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [📍] 20 Mile Service Radius       │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [👥] 5 Technicians                │ │
│ └───────────────────────────────────┘ │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [🔧] Specialties & Services           │
│───────────────────────────────────────│
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [⚙️] Engine Repair                │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [🔄] Transmission Service         │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [🛑] Brake Systems                │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [🔍] Diagnostics                  │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [⚡] Electrical Systems            │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [❄️] A/C & Heating                │ │
│ └───────────────────────────────────┘ │
│                                       │
│ Certifications                        │
│ ──────────────                        │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [🛡️] ASE Master Technician       │ │
│ │ National Institute for...         │ │
│ │ Certified Since 2015              │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [🎖️] State Emission Inspector    │ │
│ │ Department of Motor Vehicles      │ │
│ │ Licensed 2018 - Present           │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │ [✓] Hybrid Vehicle Specialist    │ │
│ │ Automotive Training Institute     │ │
│ │ Certified Since 2020              │ │
│ └───────────────────────────────────┘ │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [🖼️] Photo Gallery                   │
│───────────────────────────────────────│
│                                       │
│   ┌───────────────────────────┐       │
│   │                           │       │
│   │  Photo Gallery            │       │
│   │  Coming Soon              │       │
│   │                           │       │
│   │  Phase 2 Feature          │       │
│   │                           │       │
│   └───────────────────────────┘       │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [⭐] Customer Reviews                 │
│───────────────────────────────────────│
│                                       │
│   ┌───────────────────────────┐       │
│   │                           │       │
│   │  Customer Reviews         │       │
│   │  Coming Soon              │       │
│   │                           │       │
│   │  Phase 3 Feature          │       │
│   │                           │       │
│   └───────────────────────────┘       │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [📞] Contact Information              │
│───────────────────────────────────────│
│                                       │
│ 📞 Phone                              │
│ (555) 123-4567                        │
│ [Call Now →]                          │
│                                       │
│ ✉️ Email                              │
│ info@autopromechanics.com             │
│ [Send Email →]                        │
│                                       │
│ 📍 Address                            │
│ 123 Main Street                       │
│ Springfield, IL 62701                 │
│                                       │
│ 🔗 Website                            │
│ autopromechanics.com                  │
│ [Visit Website →]                     │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [🕐] Business Hours                   │
│───────────────────────────────────────│
│                                       │
│ Monday                                │
│ 8:00 AM - 6:00 PM • Open Now          │
│                                       │
│ Tuesday                               │
│ 8:00 AM - 6:00 PM                     │
│                                       │
│ Wednesday                             │
│ 8:00 AM - 6:00 PM                     │
│                                       │
│ Thursday                              │
│ 8:00 AM - 6:00 PM                     │
│                                       │
│ Friday                                │
│ 8:00 AM - 6:00 PM                     │
│                                       │
│ Saturday                              │
│ 9:00 AM - 3:00 PM                     │
│                                       │
│ Sunday                                │
│ Closed                                │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [📍] Location                         │
│───────────────────────────────────────│
│                                       │
│ ┌───────────────────────────────────┐ │
│ │                                   │ │
│ │        MAP PLACEHOLDER            │ │
│ │                                   │ │
│ └───────────────────────────────────┘ │
│                                       │
│ 123 Main Street                       │
│ Springfield, IL 62701                 │
│                                       │
│ [Get Directions →]                    │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│ [💬] Send an Inquiry                  │
│───────────────────────────────────────│
│                                       │
│ Your Name *                           │
│ ┌───────────────────────────────────┐ │
│ │                                   │ │
│ └───────────────────────────────────┘ │
│                                       │
│ Email Address *                       │
│ ┌───────────────────────────────────┐ │
│ │                                   │ │
│ └───────────────────────────────────┘ │
│                                       │
│ Phone Number                          │
│ ┌───────────────────────────────────┐ │
│ │                                   │ │
│ └───────────────────────────────────┘ │
│                                       │
│ Message *                             │
│ ┌───────────────────────────────────┐ │
│ │                                   │ │
│ │                                   │ │
│ │                                   │ │
│ │                                   │ │
│ └───────────────────────────────────┘ │
│                                       │
│ ┌───────────────────────────────────┐ │
│ │      [Send Inquiry →]             │ │
│ └───────────────────────────────────┘ │
│                                       │
└───────────────────────────────────────┘

┌───────────────────────────────────────┐
│              FOOTER                   │
│                                       │
│ AutoPro Network                       │
│ ───────────────                       │
│ For Professionals                     │
│ For Customers                         │
│ Company                               │
│                                       │
│ © 2026 AutoPro Network                │
└───────────────────────────────────────┘
```

---

## Component Hierarchy

```
ProfessionalProfileComponent
├── PublicNavbarComponent
├── ProfileHeroSection
│   ├── CoverPhotoComponent
│   ├── ProfileAvatarComponent
│   │   └── VerifiedBadgeComponent
│   ├── ProfileHeaderComponent
│   │   ├── BusinessNameDisplay
│   │   ├── ProfessionalNameDisplay
│   │   ├── BadgeGroupComponent
│   │   │   ├── BusinessTypeBadge
│   │   │   └── VerifiedBadge
│   │   └── RatingDisplayComponent
│   └── ActionButtonsComponent
│       ├── CallButton
│       ├── EmailButton
│       └── InquiryButton
├── ProfileMainContent
│   ├── AboutSectionComponent
│   │   ├── BioTextDisplay
│   │   └── StatsGridComponent
│   │       ├── YearsInBusinessStat
│   │       ├── ServiceAreaStat
│   │       └── TeamSizeStat
│   ├── SpecialtiesSectionComponent
│   │   ├── SpecialtiesGridComponent
│   │   │   └── SpecialtyCardComponent (x6)
│   │   └── CertificationsListComponent
│   │       └── CertificationCardComponent (x3)
│   ├── GallerySectionComponent (Phase 2)
│   │   └── GalleryPlaceholderComponent
│   └── ReviewsSectionComponent (Phase 3)
│       └── ReviewsPlaceholderComponent
└── ProfileSidebarComponent
    ├── ContactInfoCardComponent
    │   └── ContactItemComponent (x4)
    ├── BusinessHoursCardComponent
    │   └── HoursItemComponent (x7)
    ├── LocationCardComponent
    │   ├── MapComponent
    │   └── DirectionsButton
    └── InquiryFormCardComponent
        ├── FormFieldComponent (x4)
        ├── ValidationMessageComponent
        └── SubmitButtonComponent
└── PublicFooterComponent
```

---

## Interaction States

### Hero Section

**Desktop:**
```
┌──────────────────────────────────────────────────────┐
│  ┌──────┐  AutoPro Mechanics                         │
│  │ IMG  │  John Smith, ASE Certified                 │
│  │ [✓]  │  [Mechanic] [Verified]                     │
│  └──────┘  ★★★★★ 5.0 (127)                          │
│                                                       │
│             ┌─────┐ ┌─────┐ ┌──────────────┐         │
│             │ 📞  │ │ ✉️  │ │ Send Inquiry │         │
│             └─────┘ └─────┘ └──────────────┘         │
│                                                       │
│             HOVER STATES:                             │
│             ┌─────────┐                               │
│             │ 📞 Call │  (Blue background)            │
│             └─────────┘                               │
│                                                       │
│             ┌──────────────────────┐                  │
│             │ Send Inquiry →       │  (Yellow bg)     │
│             └──────────────────────┘                  │
└──────────────────────────────────────────────────────┘
```

**Mobile:**
```
┌─────────────────────────────┐
│      ┌──────────┐           │
│      │   IMG    │           │
│      │   [✓]    │           │
│      └──────────┘           │
│                             │
│   AutoPro Mechanics         │
│   John Smith, ASE...        │
│                             │
│ ┌──────┐┌──────┐┌─────────┐│
│ │ 📞   ││ ✉️   ││  Send   ││
│ │ Call ││Email ││ Inquiry ││
│ └──────┘└──────┘└─────────┘│
│                             │
│ MOBILE BUTTONS FULL WIDTH:  │
│ ┌─────────────────────────┐ │
│ │      📞 Call Now        │ │
│ └─────────────────────────┘ │
│ ┌─────────────────────────┐ │
│ │      ✉️ Send Email      │ │
│ └─────────────────────────┘ │
│ ┌─────────────────────────┐ │
│ │   💬 Send Inquiry →     │ │
│ └─────────────────────────┘ │
└─────────────────────────────┘
```

---

### Inquiry Form States

**Default State:**
```
┌────────────────────────────────┐
│ [💬] Send an Inquiry           │
│────────────────────────────────│
│                                │
│ Your Name *                    │
│ [__________________________]   │
│                                │
│ Email Address *                │
│ [__________________________]   │
│                                │
│ Phone Number                   │
│ [__________________________]   │
│                                │
│ Message *                      │
│ [__________________________]   │
│ [__________________________]   │
│ [__________________________]   │
│                                │
│ [Send Inquiry →]               │
└────────────────────────────────┘
```

**Validation Error State:**
```
┌────────────────────────────────┐
│ [💬] Send an Inquiry           │
│────────────────────────────────│
│                                │
│ Your Name *                    │
│ [__________________________]   │
│ ⚠️ Name is required            │
│                                │
│ Email Address *                │
│ [john@example______________]   │
│ ⚠️ Please enter valid email    │
│                                │
│ Message *                      │
│ [__________________________]   │
│ ⚠️ Message must be 10+ chars   │
│                                │
│ [Send Inquiry →]               │
└────────────────────────────────┘
```

**Loading State:**
```
┌────────────────────────────────┐
│ [💬] Send an Inquiry           │
│────────────────────────────────│
│                                │
│ Your Name *                    │
│ [John Doe__________________]   │
│                                │
│ Email Address *                │
│ [john@example.com__________]   │
│                                │
│ Phone Number                   │
│ [(555) 123-4567____________]   │
│                                │
│ Message *                      │
│ [I need help with...       ]   │
│ [                          ]   │
│                                │
│ [⏳ Sending...]                │
│  (Disabled, spinner animation) │
└────────────────────────────────┘
```

**Success State:**
```
┌────────────────────────────────┐
│ [💬] Send an Inquiry           │
│────────────────────────────────│
│                                │
│ ┌──────────────────────────┐   │
│ │ ✅ Inquiry sent!         │   │
│ │ We'll respond shortly.   │   │
│ └──────────────────────────┘   │
│                                │
│ Your Name *                    │
│ [__________________________]   │
│                                │
│ Email Address *                │
│ [__________________________]   │
│                                │
│ Phone Number                   │
│ [__________________________]   │
│                                │
│ Message *                      │
│ [__________________________]   │
│ [__________________________]   │
│                                │
│ [Send Inquiry →]               │
└────────────────────────────────┘
```

---

## Responsive Behavior Summary

| Element | Desktop (>992px) | Tablet (768-991px) | Mobile (<768px) |
|---------|------------------|--------------------|--------------------|
| Layout | 2-column (content + sidebar) | Single column (sidebar below) | Single column |
| Cover Height | 300px | 250px | 200px |
| Avatar Size | 160x160px | 120x120px | 120x120px |
| Specialties Grid | 3 columns | 2 columns | 1 column |
| Sidebar | Sticky (360px) | Full width, stacked | Full width, stacked |
| Contact Buttons | Icon buttons + CTA | Icon buttons + CTA | Full-width stacked |
| Business Hours | Table format | Table format | Stacked list |
| Map Height | 200px | 180px | 160px |

---

## Loading States

### Skeleton Loader

```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│         ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓                   │
│         (Shimmer animation on cover photo)              │
│                                                         │
└─────────────────────────────────────────────────────────┘
     │
     ▼
┌─────────────────────────────────────────────────────────┐
│  ┌────────┐  ┌─────────────────────┐  ┌──────────┐     │
│  │▓▓▓▓▓▓▓▓│  │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │  │ ▓▓▓▓▓▓▓  │     │
│  │▓▓▓▓▓▓▓▓│  │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓   │  │ ▓▓▓▓▓▓▓  │     │
│  │▓▓▓▓▓▓▓▓│  │                     │  │          │     │
│  │▓▓▓▓▓▓▓▓│  │ ▓▓▓▓▓  ▓▓▓▓▓       │  │ ▓▓▓▓▓▓▓  │     │
│  └────────┘  │ ▓▓▓▓▓▓▓▓▓▓▓▓▓      │  └──────────┘     │
│              └─────────────────────┘                    │
└─────────────────────────────────────────────────────────┘

┌──────────────────────────┬──────────────────────────────┐
│ ┌────────────────────┐   │ ┌────────────────────────┐   │
│ │ ▓▓▓▓▓▓▓▓           │   │ │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓       │   │
│ │                    │   │ │                        │   │
│ │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │   │ │ ▓▓▓▓▓▓▓▓▓▓▓▓          │   │
│ │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │   │ │ ▓▓▓▓▓▓▓▓▓▓▓▓          │   │
│ │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │   │ │                        │   │
│ └────────────────────┘   │ └────────────────────────┘   │
│                          │                              │
│ (Content skeleton)       │ (Sidebar skeleton)           │
└──────────────────────────┴──────────────────────────────┘

(All skeleton elements have shimmer animation)
```

---

## Error States

### Profile Not Found

```
┌─────────────────────────────────────────────────────┐
│                                                     │
│                      [❌]                           │
│                                                     │
│               Professional Not Found                │
│                                                     │
│         The profile you're looking for              │
│         doesn't exist or has been removed.          │
│                                                     │
│         ┌───────────────────────────┐               │
│         │ ← Back to Directory       │               │
│         └───────────────────────────┘               │
│                                                     │
└─────────────────────────────────────────────────────┘
```

### Network Error

```
┌─────────────────────────────────────────────────────┐
│                                                     │
│                      [⚠️]                           │
│                                                     │
│               Connection Error                      │
│                                                     │
│         Unable to load professional profile.        │
│         Please check your connection.               │
│                                                     │
│         ┌───────────────────────────┐               │
│         │    🔄 Try Again           │               │
│         └───────────────────────────┘               │
│                                                     │
└─────────────────────────────────────────────────────┘
```

---

## Accessibility Annotations

### Screen Reader Flow

```
1. "Navigation landmark"
   - "Link: AutoPro Network home"
   - "Link: Find Professionals"
   - "Button: Sign In"

2. "Main content"
   - "Heading level 1: AutoPro Mechanics"
   - "Verified Professional badge"
   - "Rating: 5.0 out of 5 stars, 127 reviews"
   - "Button: Call phone number (555) 123-4567"
   - "Button: Send email to info@autopromechanics.com"
   - "Button: Send inquiry"

3. "About section"
   - "Heading level 2: About"
   - [Bio content read naturally]
   - "15 plus years in business"
   - "20 mile service radius"
   - "5 technicians on team"

4. "Specialties section"
   - "Heading level 2: Specialties and Services"
   - "Specialty: Engine Repair"
   - "Specialty: Transmission Service"
   - [etc...]

5. "Complementary sidebar"
   - "Contact Information"
   - "Business Hours: Currently open, Monday 8 AM to 6 PM"
   - "Location: 123 Main Street, Springfield, Illinois"
   - "Form: Send an inquiry"

6. "Footer landmark"
   - [Footer content]
```

### Keyboard Navigation

```
Tab Order:
1. Skip to main content (hidden, shows on focus)
2. Navigation links
3. Sign In button
4. Join Network button
5. Call button
6. Email button
7. Send Inquiry button
8. Contact form (if scrolled to inquiry button)
9. Footer links
```

---

## Print Styles

When user prints the page:

```
┌─────────────────────────────────────────────────┐
│  AutoPro Mechanics                              │
│  John Smith, ASE Certified Master Technician    │
│  ★★★★★ 5.0 (127 reviews)                       │
│  Verified Professional                          │
├─────────────────────────────────────────────────┤
│  About                                          │
│  [Bio text printed in full]                     │
│                                                 │
│  Contact Information                            │
│  Phone: (555) 123-4567                          │
│  Email: info@autopromechanics.com               │
│  Address: 123 Main Street, Springfield, IL      │
│  Website: autopromechanics.com                  │
│                                                 │
│  Business Hours                                 │
│  Monday: 8:00 AM - 6:00 PM                      │
│  [Full week schedule]                           │
│                                                 │
│  Specialties                                    │
│  • Engine Repair                                │
│  • Transmission Service                         │
│  [All specialties listed]                       │
│                                                 │
│  Certifications                                 │
│  • ASE Master Technician (Since 2015)           │
│  [All certifications listed]                    │
└─────────────────────────────────────────────────┘

(Navigation, footer, and interactive elements hidden)
(QR code with profile URL added at bottom)
```
