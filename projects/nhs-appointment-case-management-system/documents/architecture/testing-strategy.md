# Testing Strategy

## Goals
- Prove critical behaviour for the vertical slice
- Prevent regression in appointment lifecycle rules
- Maintain contract stability for API consumers

## Current scope
- Liveness: API starts and responds on `/health`
- Appointment CRUD behaviour (create, list, get-by-id, update, delete)
- Validation: reject appointments scheduled in the past

## Next scope
- Domain unit tests for invariants and lifecycle transitions
- Integration tests with a real database provider (Azure SQL)
- OpenAPI contract checks in CI
