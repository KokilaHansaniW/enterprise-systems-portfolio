# Architecture Rationale

## Context
This system targets regulated, high-assurance environments where behaviour must be verifiable, change must be controlled, and auditability must be explicit.

## Architectural approach
The solution is structured into clear boundaries:

- **Domain**: entities, value objects, invariants, and business rules
- **API**: HTTP endpoints and request/response contracts
- **Persistence**: EF Core used to support a working vertical slice

The intent is to protect core business behaviour from infrastructure volatility and enable predictable evolution.

## Why this approach
- **Change isolation**: persistence/integration changes should not force domain rewrites
- **Testability**: business rules can be validated independently of HTTP/database
- **Governance readiness**: consistent enforcement points for audit and policy
- **Operational clarity**: boundaries support faster incident triage and ownership clarity

## Why not microservices initially
Microservices add distributed complexity (network reliability, versioned contracts, monitoring overhead). The initial focus is a stable, auditable vertical slice with clean boundaries. Decomposition is a later optimisation once domain boundaries and integration points mature.

## Compliance considerations (high level)
- Auditability (who/what/when)
- Data protection by design (minimise exposure of sensitive data)
- Traceability via commits, pull requests, and decision records (ADRs)
