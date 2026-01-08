# Phase 1 User Flow Diagrams

This directory contains PlantUML user flow diagrams for all Phase 1 features of the Multi-Service Automotive Ecosystem Platform.

## Overview

Phase 1 focuses on the MVP - Referral Core, establishing the foundational referral tracking system that enables professionals to refer customers to each other and customers to refer friends.

## Diagram Index

### Customer Flows

| # | Diagram | Description | Related Requirements |
|---|---------|-------------|---------------------|
| 01 | [Customer Registration](./customer-registration/customer-registration.png) | Customer self-registration with email verification | REQ-CM-F001 |
| 02 | [Customer Login](./customer-login/customer-login.png) | Secure authentication with session management | REQ-CM-F002 |
| 03 | [Forgot Password](./forgot-password/forgot-password.png) | Password reset with secure token flow | REQ-CM-F002 |
| 04 | [Customer Referral Sharing](./customer-referral-sharing/customer-referral-sharing.png) | Share referrals via email, SMS, social, QR | REQ-RF-F001, REQ-RF-F002, REQ-RF-F003 |
| 05 | [Referral Status Tracking](./referral-status-tracking/referral-status-tracking.png) | Track referral status and rewards | REQ-RF-F004 |

### Professional Flows

| # | Diagram | Description | Related Requirements |
|---|---------|-------------|---------------------|
| 06 | [Professional Profile Setup](./professional-profile-setup/professional-profile-setup.png) | Profile creation with media and specialties | REQ-SP-F006, REQ-SP-F007 |
| 07 | [Professional Directory Browse](./professional-directory-browse/professional-directory-browse.png) | Browse and filter professional directory | REQ-SP-F001, REQ-SP-F002 |
| 08 | [Professional Dashboard](./professional-dashboard/professional-dashboard.png) | Dashboard home with stats and quick actions | REQ-SP-F005 |
| 09 | [Professional Send Referral](./professional-send-referral/professional-send-referral.png) | Refer customers to colleague professionals | REQ-RF-F005, REQ-RF-F006 |
| 10 | [Professional Receive Referral](./professional-receive-referral/professional-receive-referral.png) | Accept or decline referrals from colleagues | REQ-RF-F007, REQ-RF-F008, REQ-RF-F009 |
| 12 | [Customer Management](./customer-management/customer-management.png) | View, add, and manage customers | REQ-CM-F006, REQ-CM-F007, REQ-CM-F008 |

### Public/Landing Flows

| # | Diagram | Description | Related Requirements |
|---|---------|-------------|---------------------|
| 11 | [Referral Landing Page](./referral-landing-page/referral-landing-page.png) | New visitor arriving via referral link | REQ-RF-F015, REQ-RF-F016 |

## File Structure

```
phase-1/
├── README.md                              # This file
├── customer-registration/
│   ├── customer-registration.puml         # PlantUML source
│   └── customer-registration.png          # Rendered diagram
├── customer-login/
│   ├── customer-login.puml
│   └── customer-login.png
├── forgot-password/
│   ├── forgot-password.puml
│   └── forgot-password.png
├── customer-referral-sharing/
│   ├── customer-referral-sharing.puml
│   └── customer-referral-sharing.png
├── referral-status-tracking/
│   ├── referral-status-tracking.puml
│   └── referral-status-tracking.png
├── professional-profile-setup/
│   ├── professional-profile-setup.puml
│   └── professional-profile-setup.png
├── professional-directory-browse/
│   ├── professional-directory-browse.puml
│   └── professional-directory-browse.png
├── professional-dashboard/
│   ├── professional-dashboard.puml
│   └── professional-dashboard.png
├── professional-send-referral/
│   ├── professional-send-referral.puml
│   └── professional-send-referral.png
├── professional-receive-referral/
│   ├── professional-receive-referral.puml
│   └── professional-receive-referral.png
├── referral-landing-page/
│   ├── referral-landing-page.puml
│   └── referral-landing-page.png
└── customer-management/
	├── customer-management.puml
	└── customer-management.png
```

## Viewing Diagrams

### PNG Images
The rendered PNG images can be viewed directly in any image viewer or web browser.

### PlantUML Source
The `.puml` files contain the PlantUML source code. To regenerate the diagrams:

```bash
# Render a single diagram
plantuml -tpng filename.puml

# Render all diagrams in directory
plantuml -tpng *.puml
```

### Online Editor
You can also paste the PlantUML source into [PlantUML Online Editor](http://www.plantuml.com/plantuml/uml/) to view and edit.

## Diagram Conventions

- **Blue activities**: Standard user/system actions
- **Orange diamonds**: Decision points
- **Green activities**: Section headers/milestones
- **Notes**: Additional context and field details
- **Fork/Join**: Parallel or optional actions

## Related Documentation

- [Roadmap](../../specs/ROADMAP.md) - Phase 1 requirements list
- [Customer Management Specs](../../specs/02-customer-management/frontend-requirements.md)
- [Service Professionals Specs](../../specs/03-service-professionals/frontend-requirements.md)
- [Referral System Specs](../../specs/04-referral-system/frontend-requirements.md)
