# ADR-0001: Architecture Style Selection

## Status
Accepted

## Context
This system supports appointment management in a healthcare context where
auditability, testability, and separation of concerns are essential.

## Decision
The system follows Clean Architecture principles:
- Domain-centric design
- Application services orchestrate use cases
- Infrastructure concerns isolated

## Alternatives Considered
- Layered architecture
- Monolithic MVC

## Consequences
- Higher initial structure cost
- Strong governance alignment
- Easier testing and regulatory compliance