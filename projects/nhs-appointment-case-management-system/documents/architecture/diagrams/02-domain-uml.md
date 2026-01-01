# Domain Model (UML – Simplified)

```mermaid
classDiagram
Appointment --> Patient
Appointment --> Clinician

class Appointment {
  Id
  ScheduledAtUtc
  Status
}

class Patient {
  Id
}

class Clinician {
  Id
}