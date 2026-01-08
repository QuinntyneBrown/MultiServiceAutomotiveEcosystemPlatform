# Phase 1 User Flow Diagrams

This directory contains PlantUML user flow diagrams for all Phase 1 features of the Multi-Service Automotive Ecosystem Platform.

## Overview

Phase 1 focuses on the MVP - Referral Core, establishing the foundational referral tracking system that enables professionals to refer customers to each other and customers to refer friends.

## Diagram Index

### Customer Flows

| # | Diagram | Description | Related Requirements |
|---|---------|-------------|---------------------|
| 01 | [Customer Registration](./01-customer-registration-flow.png) | Customer self-registration with email verification | REQ-CM-F001 |
| 02 | [Customer Login](./02-customer-login-flow.png) | Secure authentication with session management | REQ-CM-F002 |
| 03 | [Forgot Password](./03-forgot-password-flow.png) | Password reset with secure token flow | REQ-CM-F002 |
| 04 | [Customer Referral Sharing](./04-customer-referral-sharing-flow.png) | Share referrals via email, SMS, social, QR | REQ-RF-F001, REQ-RF-F002, REQ-RF-F003 |
| 05 | [Referral Status Tracking](./05-referral-status-tracking-flow.png) | Track referral status and rewards | REQ-RF-F004 |

### Professional Flows

| # | Diagram | Description | Related Requirements |
|---|---------|-------------|---------------------|
| 06 | [Professional Profile Setup](./06-professional-profile-setup-flow.png) | Profile creation with media and specialties | REQ-SP-F006, REQ-SP-F007 |
| 07 | [Professional Directory Browse](./07-professional-directory-browse-flow.png) | Browse and filter professional directory | REQ-SP-F001, REQ-SP-F002 |
| 08 | [Professional Dashboard](./08-professional-dashboard-flow.png) | Dashboard home with stats and quick actions | REQ-SP-F005 |
| 09 | [Professional Send Referral](./09-professional-send-referral-flow.png) | Refer customers to colleague professionals | REQ-RF-F005, REQ-RF-F006 |
| 10 | [Professional Receive Referral](./10-professional-receive-referral-flow.png) | Accept or decline referrals from colleagues | REQ-RF-F007, REQ-RF-F008, REQ-RF-F009 |
| 12 | [Customer Management](./12-customer-management-flow.png) | View, add, and manage customers | REQ-CM-F006, REQ-CM-F007, REQ-CM-F008 |

### Public/Landing Flows

| # | Diagram | Description | Related Requirements |
|---|---------|-------------|---------------------|
| 11 | [Referral Landing Page](./11-referral-landing-page-flow.png) | New visitor arriving via referral link | REQ-RF-F015, REQ-RF-F016 |

## File Structure

```
phase-1/
├── README.md                              # This file
├── 01-customer-registration-flow.puml     # PlantUML source
├── 01-customer-registration-flow.png      # Rendered diagram
├── 02-customer-login-flow.puml
├── 02-customer-login-flow.png
├── 03-forgot-password-flow.puml
├── 03-forgot-password-flow.png
├── 04-customer-referral-sharing-flow.puml
├── 04-customer-referral-sharing-flow.png
├── 05-referral-status-tracking-flow.puml
├── 05-referral-status-tracking-flow.png
├── 06-professional-profile-setup-flow.puml
├── 06-professional-profile-setup-flow.png
├── 07-professional-directory-browse-flow.puml
├── 07-professional-directory-browse-flow.png
├── 08-professional-dashboard-flow.puml
├── 08-professional-dashboard-flow.png
├── 09-professional-send-referral-flow.puml
├── 09-professional-send-referral-flow.png
├── 10-professional-receive-referral-flow.puml
├── 10-professional-receive-referral-flow.png
├── 11-referral-landing-page-flow.puml
├── 11-referral-landing-page-flow.png
├── 12-customer-management-flow.puml
└── 12-customer-management-flow.png
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
