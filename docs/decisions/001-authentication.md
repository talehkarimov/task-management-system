# 001 – Authentication Strategy

## Context
The system requires user authentication and must support both traditional and modern login approaches.

## Decision
We will support:
- Local credentials-based authentication (username/email + password)
- External authentication via Google OAuth (OpenID Connect)

Authentication will be handled by a dedicated Identity Service.

## Consequences
- Identity logic is isolated from business services
- System can be extended with additional OAuth providers
- API Gateway will validate tokens for downstream services
