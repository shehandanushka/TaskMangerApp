# TaskManagerAPI

## Overview

TaskManagerAPI is a .NET 10 Web API for task management. It stores tasks and users in SQL Server and uses basic authentication for protected task endpoints.

## Current Database Configuration

- Database name: `TaskManagerDB`
- Connection string location:
  - `appsettings.json`
  - `appsettings.Development.json`
- Current SQL Server connection string:
  - `Server=DESKTOP-7S3IV45\\SQLEXPRESS;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True;`

> If your SQL Server instance name differs, update the `Server` value in the connection string files.

## Prerequisites

- .NET 10 SDK
- SQL Server or SQL Server Express
- A compatible IDE or editor such as Visual Studio, VS Code, or Rider

## Setup

1. Open the project folder in your editor.
2. Confirm the connection string in `appsettings.json` and `appsettings.Development.json`.
3. If needed, update the SQL Server instance name.

## Database and Migrations

This project automatically applies pending migrations on startup:

- `AppDbContext` is configured to use SQL Server
- On application startup, `db.Database.Migrate();` runs and applies migrations
- Seed data is configured in `AppDbContext.OnModelCreating`

Seeded records:

- User: `admin` / `admin123`
- Sample tasks: two initial tasks

## Authentication

- Uses Basic Authentication via `BasicAuthHandler`
- Protected endpoints require an `Authorization` header with Basic credentials
- Valid login route: `POST /api/auth/login`

### Sample login request

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

## Running the API

From the project folder, run:

```bash
dotnet run
```

The API will start and, in development, expose Swagger at:

```text
https://localhost:{port}/
```

## API Endpoints

### Authentication

- `POST /api/auth/login`

### Task management

- `GET /api/tasks`
- `GET /api/tasks/{id}`
- `POST /api/tasks`
- `PUT /api/tasks/{id}`
- `PATCH /api/tasks/{id}/toggle`
- `DELETE /api/tasks/{id}`

## Notes

- The backend is already using `TaskManagerDB` as the database name.
- The project builds successfully with `dotnet build`.
- Update the SQL Server connection string if you are not using `DESKTOP-7S3IV45\\SQLEXPRESS`.
