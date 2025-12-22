# ADR-0001: Architecture Style for NHS Portal

## Status
Accepted

## Context
The system must support change tolerance, clear boundaries, and testable business behaviour, and is expected to evolve toward persistence, integrations, and governance constraints.

## Decision
Adopt a layered approach to keep domain rules independent from transport and infrastructure concerns.

## Consequences
### Positive
- Better testability of business rules
- Reduced coupling to persistence/integration choices
- Clearer governance enforcement points

### Negative
- More structure than a single minimal API project
- Requires discipline to maintain boundaries
