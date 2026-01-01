# Appointment CRUD – Sequence

```mermaid
sequenceDiagram
Client->>API: Create Appointment
API->>Validator: Validate request
API->>ApplicationService: CreateAsync
ApplicationService->>DbContext: Save Appointment
ApplicationService->>AuditLog: Write CREATE entry
API->>Client: 201 Created