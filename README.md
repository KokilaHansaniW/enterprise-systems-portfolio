# Enterprise Systems Portfolio

This repository contains a curated portfolio of software engineering work focused on the design and delivery of **enterprise-grade systems** in **regulated and large-scale environments**, including healthcare, public services, finance, and cloud-native platforms.

The emphasis is on **architecture, domain modelling, system boundaries, and delivery discipline** rather than isolated code samples.

---

## What This Portfolio Demonstrates

- Clear modelling of real-world business domains  
- Explicit architectural boundaries and separation of concerns  
- Change-tolerant design using dependency inversion  
- API-first thinking and integration readiness  
- Testing focused on business rules and correctness  
- Security, auditability, and governance awareness  
- Professional delivery workflow (branches, commits, pull requests)

---

## Portfolio Projects

#1 NHS Appointment & Case Management Platform

A healthcare-oriented system designed to model appointment scheduling and case management workflows, reflecting governance and operational constraints typical of large public organisations.

**Key characteristics**
- Domain-first design (patients, appointments, billing)
- Clean separation between domain, application, and infrastructure
- Testable business logic independent of frameworks
- Consideration of audit trails and data protection

**Location**
  Live demo: https://...
  Code: projects/nhs-appointment-case-management/

.
├── projects/
│   └── nhs-appointment-case-management/
│       ├── README.md
│       ├── system/
│       │   ├── backend/
│       │   └── frontend/             
│       ├── architecture/
│       │   ├── diagrams/
│       │   └── decisions/             (Architecture Decision Records)
│       ├── governance/
│       │   ├── security.md
│       │   ├── privacy-and-gdpr.md
│       │   └── audit-and-logging.md
│       └── evidence/
│           ├── day-by-day/
│           └── screenshots/
└── docs/
    ├── engineering-standards.md
    ├── commit-and-pr-rules.md
    └── interview-notes.md



Each project documents **context, architectural intent, and trade-offs**.

---

## Architectural Approach

Architecture choices are **context-driven**, balancing maintainability, risk, and long-term ownership.  
The focus is on protecting core business logic from infrastructure and external change.

---

## How to Review

- Start with each project’s README for context
- Review architecture diagrams and decision notes
- Inspect commit history for delivery discipline

---

## Contact

- GitHub: https://github.com/KokilaHansaniW/
- Linkedin: www.linkedin.com/in/kokilahansani
