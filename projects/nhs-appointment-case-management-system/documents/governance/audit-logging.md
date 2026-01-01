# Audit Logging

## Purpose
Audit logging provides traceability, accountability, and operational assurance in a regulated environment such as the NHS.

## Events Logged
The system records the following events:
- CREATE (appointment created)
- UPDATE (appointment modified)
- DELETE (appointment removed)
- STATUS_CHANGE (explicit lifecycle transitions)

## Captured Fields
Each audit record includes:
- Actor (user or system identity)
- Timestamp (UTC)
- Correlation ID (for cross-service traceability)
- Entity type and entity identifier
- Action performed

## Data Protection & PII Policy
Audit logs must never store:
- Patient notes
- NHS numbers
- Clinical content
- Full request payloads

Only minimal metadata required for traceability is recorded.

## Operational Intent
Audit logs support:
- Incident investigation
- Change traceability
- Regulatory assurance

Audit logging is designed to maximise observability while minimising data exposure.
