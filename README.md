# Realtime Dashboard

Realtime Dashboard is a small full-stack project built around a ticket queue workflow. I used it as a practical exercise to work on frontend state management, backend API design, authentication, and synchronization of changing data in a setup that feels close to a real application.

The stack combines Angular, NgRx, RxJS, ASP.NET Core, PostgreSQL, and Docker. The result is a protected dashboard where users can sign in, review tickets, take actions on them, and see changes reflected through periodic polling.

## What I applied in this project

- Feature-based Angular structure
- NgRx with actions, reducers, selectors, and effects
- RxJS polling and request coordination without overlapping calls
- JWT authentication and route protection
- Role-based authorization in the API
- Optimistic concurrency with `expectedVersion`
- EF Core migrations with PostgreSQL
- Background processing for overdue tickets
- Docker Compose for local full-stack execution

## Core functionality

- Login and signup with `username` and password
- Protected `/queue` route
- Ticket list with search, filtering, and sorting
- Ticket detail panel
- Assign ticket to current user
- Complete ticket
- Delta polling against `/api/tickets/changes`
- Automatic transition of overdue tickets to `FAILED`

## Roles and statuses

### Roles

- `ADMIN`
- `AGENT`

### Ticket statuses

- `NEW`
- `IN_PROGRESS`
- `DONE`
- `FAILED`

## Project structure

```text
realtime-dashboard/
|- docker-compose.yml
|- .env
|- SPEC.md
|- Frontend/
|- Backend/
```

### Backend layers

- `Api`
- `Application`
- `Domain`
- `Infrastructure`
- `Testing`

### Frontend areas

- `auth`
- `queue`
- `core`
- `shared`

## Run with Docker Compose

### Requirements

- Docker
- Docker Compose

### Start

```bash
docker compose up --build
```

### Services

- Frontend: `http://localhost`
- Backend API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- Health check: `http://localhost:8080/api/health`
- PostgreSQL: `localhost:5432`

### Environment values

```env
DB_HOST=postgresdb
DB_PORT=5432
DB_NAME=realtime-db
DB_USER=admin
DB_PASSWORD=admin
```

## Seed data

On backend startup:

- migrations are applied
- base users are created if they do not exist
- existing ticket records are removed
- 500 sample tickets are generated again

Initial credentials:

- Admin: `admin` / `admin123`
- Agent: `agent` / `agent123`

## Run locally

### Backend

Requirements:

- .NET SDK 9
- PostgreSQL 15

Run:

```bash
dotnet run --project Backend/Api/Api.csproj
```

### Frontend

Requirements:

- Node.js 22
- npm 11

Run:

```bash
cd Frontend
npm ci
npm start
```

Frontend dev URL:

- `http://localhost:4200`

Configured API base URL:

- `http://localhost:8080/api`

## Main endpoints

- `POST /api/auth/login`
- `POST /api/auth/signup`
- `GET /api/tickets`
- `GET /api/tickets/changes`
- `PUT /api/tickets/{id}/assign`
- `PUT /api/tickets/{id}/complete`
- `GET /api/users`
- `GET /api/health`

## Technical notes

- Polling runs every `5000 ms`
- Polling uses `exhaustMap` to avoid overlapping requests
- The backend wraps successful responses in a standard response envelope
- CORS is open for local development
- Swagger is enabled

## Testing

- Frontend: Vitest
- Backend: xUnit + Moq
