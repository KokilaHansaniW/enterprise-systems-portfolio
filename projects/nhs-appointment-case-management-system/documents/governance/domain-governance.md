# Domain Governance

## Purpose
Domain governance defines rules and constraints that prevent invalid state transitions and reduce operational risk.

## Governance Principles
- Business rules are enforced in the domain layer
- Infrastructure cannot alter domain behaviour
- Application services orchestrate but do not bypass domain rules

## Appointment Lifecycle Governance
Appointments follow a controlled status lifecycle to ensure:
- Correctness of state transitions
- Traceability of changes
- Operational clarity

## Minimum Audit Boundary
All domain state changes must be traceable with at least:
- Timestamp (UTC)
- Action type (Create, Update, Delete, StatusChange)
- Entity identifier
- Actor identity (placeholder until authentication is integrated)
- Correlation / Trace ID

## Rationale
In regulated systems, correctness and traceability must be systematic rather than ad-hoc. Domain governance ensures consistent behaviour regardless of infrastructure or interface changes.