# ITIL Service Model Mapping

## Service view
- **Service**: NHS Portal (Appointment & Case Management capability)
- **Service Request**: create/reschedule/cancel appointment
- **Incident**: portal outage, booking failures, degraded performance
- **Problem**: recurring incidents caused by underlying defect
- **Change**: modification affecting workflow, persistence, or integrations

## Change enablement approach
- Delivered via feature branches + pull requests
- PRs require documentation and tests where applicable
- Release governance added incrementally as the system evolves
