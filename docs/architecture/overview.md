# System Architecture Overview

## Architecture Style
The system is designed as a microservice-based architecture using Clean Architecture principles.

Each service:
- Is independently deployable
- Owns its own data
- Exposes functionality via HTTP APIs and events

## Core Components
- API Gateway – entry point for clients
- Identity Service – authentication and authorization
- Task Service – core business logic
- Supporting services (Project, Notification, Audit)

## Communication
- Synchronous: REST (HTTP)
- Asynchronous: Message broker (events)

## Environment
- Local development using Docker and Docker Compose
- Cloud-agnostic design