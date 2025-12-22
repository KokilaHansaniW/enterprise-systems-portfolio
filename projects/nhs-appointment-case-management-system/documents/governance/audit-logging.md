# Audit Logging (Design Notes)

## What to log
- Appointment create/update/delete
- Status transitions
- Validation failures (non-sensitive)
- Exceptions with correlation id

## Data protection
- Avoid logging sensitive identifiers in plaintext
- Prefer metadata and identifiers over full payloads
- Use structured logging to control fields

## Operational intent
Logs must support incident response and traceability without unnecessary data exposure.
