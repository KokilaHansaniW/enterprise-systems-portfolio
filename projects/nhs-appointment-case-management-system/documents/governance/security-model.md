# Security Model

## Roles

### Admin
- Full visibility of appointments
- Administrative delete and override operations
- Access to audit logs

### Clinician
- Create appointments
- View and update own appointments
- Cannot access audit logs or system configuration

## Least Privilege
Roles are restricted to the minimum permissions required.
No role has unrestricted system access.

## Authentication
In a production NHS environment:
- Authentication would be provided by **Microsoft Entra ID / NHS Identity**
- Role claims would be enforced at API boundary

## Traceability
All requests carry a **Correlation ID** for end-to-end tracing across:
- API
- Application services
- Audit logs
