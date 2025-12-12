NHS Appointment & Case Management System

Enterprise-grade .NET 8 platform for regulated healthcare workflows
Designed & implemented with architecture, governance, and public-sector compliance as first-class constraints.

TL;DR

Tech: .NET 8 Minimal API, EF Core, Clean Architecture, Azure-ready, CI/CD, structured logging
Domain: NHS-style appointment, case, and workflow management
Compliance: ITIL-aligned service modelling, WCAG 2.2 considerations, GDPR-aware logging, audit traceability
Core Value: A production-ready backend demonstrating how to engineer safe, traceable, defensible digital services for healthcare and other regulated sectors.

1. Overview

This system models a real NHS-style digital service responsible for patient appointment scheduling, case tracking, clinician workflows, and end-to-end operational visibility.

It is engineered with the same architectural constraints used across UK public-sector systems:

Clinical governance requirements

Auditability and traceability of all user actions

Data protection obligations (GDPR/UK GDPR)

Accessibility requirements (WCAG 2.2 AA)

Service reliability and operational resilience

The result is a backend platform that demonstrates senior engineering discipline, compliance-aware design, and scalable architectural patterns appropriate for NHS Trusts, Councils, Civil Service, and regulated industries (finance, insurance, legal, etc.).

2. Technology Stack
Backend

    .NET 8 / ASP.NET Core Minimal API

    Entity Framework Core (InMemory for development)

    xUnit for automated testing

    Structured logging (Microsoft.Extensions.Logging)

    Configuration via appsettings.json

Architecture & Governance

    Clean Architecture principles

    Domain-Driven abstractions

    ITIL-aligned modelling of incidents vs service requests

    WCAG 2.2 accessibility considerations (in UI layer, planned)

    GDPR-aware request handling & audit logging strategy

Cloud & DevOps

    Azure App Service-ready

    Containerisation support (Docker) — planned

    GitHub Actions CI pipeline (.github/workflows) — in progress

Planned Enhancements

    FluentValidation

    Azure Service Bus integration

    Role-based access and authentication via Azure AD

    React/Blazor Admin UI (WCAG 2.2 AA compliant)

3. System Architecture

The solution follows a Clean Architecture layout to ensure:

Business rules remain independent of technical frameworks

Infrastructure can change without affecting domain logic

The system is easier to test, maintain, extend, and audit

system/
 ├── backend/
 │   └── nhs-portal/
 │       ├── NhsPortal.Api/        # HTTP layer – endpoints, routing, DI setup
 │       ├── NhsPortal.Domain/     # Core logic – entities, value objects, policies
 │       ├── NhsPortal.Infrastructure/ (planned)
 │       └── NhsPortal.Tests/      # Automated test suite
 └── frontend/ (planned)

Key Layers
NhsPortal.Api

Minimal API endpoints

Dependency injection setup

Request/response pipeline

Health checks and logging

NhsPortal.Domain

Appointment and patient entities

Domain events (later)

Validation and business rules

Interfaces for infrastructure abstractions

Infrastructure (planned)

EF Core DbContext

Persistence strategy

External service integrations (email, Service Bus, audit log storage)

Tests

Unit tests validating domain logic

Integration tests validating endpoint behaviour

4. Running the API Locally

This project is intentionally simple to start — allowing any reviewer to run and inspect it immediately.

1. Navigate to the API project
cd system/backend/nhs-portal/NhsPortal.Api

2. Run the API
dotnet run


You should see something like:

Now listening on: http://localhost:5170
Application started. Press Ctrl+C to shut down.

3. Test the health endpoint

Open in browser or Postman:

GET http://localhost:5170/health


Expected output:

200 OK
"Healthy"

4. Stop the API

Press:

Ctrl + C

5. Architecture Documents & Governance Evidence

This project includes a dedicated documentation set under /documents:

Architecture

Located in:

documents/architecture/


Contains:

Architecture rationale

High-level diagrams (C4, flow, deployment)

Trade-off decisions

Governance

Located in:

documents/governance/


Includes:

GDPR impact considerations

ITIL service modelling

WCAG 2.2 accessibility requirements (for UI layer)

Audit logging strategy

6. Why Clean Architecture?

Clean Architecture was selected because it is the most appropriate architecture for regulated domains where:

auditability

traceability

change isolation

risk minimisation

maintainability

long-term extensibility

are paramount.

It is superior to microservices at this stage because:

Microservices introduce operational complexity (orchestration, distributed tracing, network overhead) WITHOUT delivering value for a single cohesive domain.

Clean Architecture keeps the domain pure and the system testable.

Once the domain stabilises, individual capabilities could be extracted into microservices safely.

This mirrors how NHS and financial sector organisations evolve systems in real life.

7. Status & Roadmap
Delivered

Solution structure

Minimal API with health endpoint

Domain layer scaffolding

Documentation folder structure

Governance baseline notes

In Progress

Appointment booking endpoints

Domain validation rules

Tests

Planned

React/Blazor Admin UI

Azure deployment

Audit logging pipeline

Event-driven notifications

RAG-based clinical-policy assistant (AI microservice)

8. Contact

If reviewing this project and you want a walkthrough, architecture explanation, or a discussion about applying these patterns in real NHS or regulated environments, feel free to reach out.