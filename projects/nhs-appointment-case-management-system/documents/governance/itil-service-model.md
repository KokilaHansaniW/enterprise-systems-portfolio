# ITIL Service Model Alignment

## Service View
- **Service**: NHS Appointment & Case Management Portal
- **Service Requests**: Create, reschedule, cancel appointments
- **Incidents**: Portal outages, booking failures, degraded performance
- **Problems**: Recurring incidents caused by underlying defects
- **Changes**: Modifications affecting workflows, persistence, or integrations

## Change Enablement Approach
- Changes are delivered via feature branches and pull requests
- Pull requests require review and documentation where applicable
- Testing and validation are expected before merge
- Release governance is introduced incrementally as the system evolves

## Operational Alignment
The system supports ITIL principles through:
- Incident traceability via correlation IDs
- Change accountability via versioned services
- Service transparency through structured audit logs