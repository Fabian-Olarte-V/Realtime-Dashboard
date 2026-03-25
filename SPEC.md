## Realtime Dashboard

### Purpose

Realtime Dashboard started as a practice project around a simple ticket queue, authentication, and near real-time updates. The idea was to build something small enough to finish, but still rich enough to apply patterns that are common in day-to-day product work.

It was mainly a way to practice:

- Angular with feature-based structure
- NgRx for state management
- RxJS for async flows and polling
- JWT authentication and route protection
- ASP.NET Core with layered architecture
- PostgreSQL integration
- Dockerized local setup

### General idea

The application represents a ticket queue that authenticated users can review and operate from a dashboard.

Main flow:

1. A user logs in or signs up.
2. The user enters the queue dashboard.
3. The dashboard displays tickets with search, filtering, and sorting.
4. The user can open a ticket detail panel.
5. Depending on role and ownership, the user can assign a ticket to self or complete it.
6. The queue refreshes periodically to reflect recent changes.

### Roles

- `ADMIN`
- `AGENT`

### Ticket statuses

- `NEW`
- `IN_PROGRESS`
- `DONE`
- `FAILED`

### Core rules

- Only authenticated users can access the dashboard
- `ADMIN` and `AGENT` can view the queue
- Ticket changes use optimistic concurrency
- Expired tickets can be marked as failed automatically

### Implemented screens

- Auth screen with login and signup
- Queue dashboard with search, filter, sort, ticket list, and detail panel

### Current scope

Included in the project:

- authentication with JWT
- protected frontend routes
- queue browsing and selection
- ticket actions
- periodic polling for changes
- backend API with PostgreSQL
- Docker Compose environment

### Notes

- `README.md` should be the main operational document for setup, commands, Docker usage, and technical details
- `SPEC.md` is intentionally lightweight and only keeps the project purpose and functional idea
