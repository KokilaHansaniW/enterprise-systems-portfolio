# Clean Architecture Overview

This system follows Clean Architecture principles to ensure:
- Separation of concerns
- Testability
- Replaceable infrastructure
- Compliance with regulated environments (e.g. NHS)

Dependencies always point inwards.

```mermaid
flowchart LR
    API[API / Presentation]
    APP[Application / Use Cases]
    DOMAIN[Domain / Business Rules]
    INFRA[Infrastructure / Persistence & Audit]

    API --> APP
    APP --> DOMAIN
    INFRA --> APP