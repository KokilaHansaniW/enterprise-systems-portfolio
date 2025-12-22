# Domain Governance

## Purpose
Domain governance defines controls that prevent invalid state and reduce operational risk.

## Appointment workflow governance
Status-controlled lifecycle supports correctness, traceability, and operational clarity.

## Minimum audit fields (boundary enforced)
- Timestamp (UTC)
- Action type (Create/Update/Delete/StatusChange)
- Entity identifier
- Actor identity (placeholder until auth is implemented)
- Correlation/Trace Id

## Rationale
In regulated environments, correctness and traceability must be systematic rather than ad-hoc.
