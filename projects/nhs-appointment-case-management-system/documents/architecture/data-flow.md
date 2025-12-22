# Data Flow: Appointment CRUD

## Create Appointment (POST /appointments)
1. Client submits request payload
2. API validates invariants (e.g. scheduled time must be in the future)
3. API persists entity using DbContext
4. API returns 201 Created

```mermaid
sequenceDiagram
  participant C as Client
  participant A as NhsPortal.Api
  participant D as NhsDbContext

  C->>A: POST /appointments
  A->>A: Validate ScheduledAt >= now
  A->>D: Add + SaveChangesAsync
  D-->>A: Appointment(Id)
  A-->>C: 201 Created + body
