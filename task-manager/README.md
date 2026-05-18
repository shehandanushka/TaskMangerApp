# Task Manager

A modern Angular 21 task management application with authentication, task CRUD, and server-side rendering support.

## Key Features

- Standalone Angular components
- Angular Material UI
- Login flow with Basic Authentication header support
- Task list with search, filter, sorting and CRUD actions
- Task create / edit dialogs and delete confirmation
- Server-side rendering (SSR) support via `@angular/ssr`

## Requirements

- Node.js 20+ (recommended)
- npm 11+ (project uses `npm@11.14.1`)
- Local backend API running at `https://localhost:44399/api`

## Backend API Expectations

This frontend expects the following backend endpoints:

- `POST /api/auth/login` – authenticate user credentials
- `GET /api/tasks` – list tasks
- `GET /api/tasks/:id` – get a single task
- `POST /api/tasks` – create a task
- `PUT /api/tasks/:id` – update a task
- `PATCH /api/tasks/:id/toggle` – toggle completed state
- `DELETE /api/tasks/:id` – delete a task

The frontend sends an `Authorization: Basic <credentials>` header for authenticated requests after login.

## Setup

Install dependencies:

```bash
npm install
```

## Development

Start the development server:

```bash
npm start
```

Open the browser at:

```text
http://localhost:4200
```

## Build

Build the application for production and SSR:

```bash
npm run build
```

This outputs the browser and server bundles to `dist/task-manager`.

## Run SSR Server

After building, start the SSR server:

```bash
npm run serve:ssr:task-manager
```

Then open:

```text
http://localhost:4000
```

## Testing

Run unit tests:

```bash
npm test
```

## Project Structure

- `src/app/app.component.ts` – root standalone component
- `src/app/app.config.ts` – application providers and HTTP configuration
- `src/app/app.routes.ts` – route definitions
- `src/app/core/services/auth.service.ts` – auth storage and headers
- `src/app/core/services/task.service.ts` – task API service
- `src/app/features/auth/login.component.ts` – login page
- `src/app/features/tasks/tasks.component.ts` – task dashboard
- `src/app/features/tasks/task-list/` – task listing and filters
- `src/app/features/tasks/task-form/` – task add/edit dialog
- `src/app/shared/confirm-dialog.component.ts` – reusable confirmation dialog

## Notes

- The app uses `provideHttpClient()` from Angular 21.
- Angular Material components are used for layout, dialogs, tables, and forms.
- Authentication credentials are stored in `localStorage` and included in all task requests.
- If the backend API is not available at `https://localhost:44399/api`, update `src/app/core/services/task.service.ts` and `src/app/core/services/auth.service.ts`.

## Troubleshooting

- If login fails, verify the backend `POST /api/auth/login` endpoint and credentials.
- If task loading fails, verify the tasks API endpoint and CORS configuration.
- If SSR server does not start, ensure `npm run build` completes successfully and `dist/task-manager/server/server.mjs` exists.
