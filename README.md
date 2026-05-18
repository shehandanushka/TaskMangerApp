# Task Manager Application - Full Stack Setup Guide

A complete full-stack task management application with a **Angular 21 frontend** and **.NET 10 backend API**, featuring authentication, CRUD operations, and SQL Server database integration.

---

## 📋 Table of Contents

1. [Project Overview](#project-overview)
2. [Tech Stack](#tech-stack)
3. [Project Structure](#project-structure)
4. [Prerequisites](#prerequisites)
5. [Database Setup](#database-setup)
6. [Backend Setup & Running](#backend-setup--running)
7. [Frontend Setup & Running](#frontend-setup--running)
8. [API Documentation](#api-documentation)
9. [Database Schema & Queries](#database-schema--queries)
10. [Authentication](#authentication)
11. [Development Workflow](#development-workflow)
12. [Troubleshooting](#troubleshooting)

---

## 🎯 Project Overview

This is a full-stack task management application with the following capabilities:

- **User Authentication**: Basic authentication with username/password
- **Task Management**: Create, Read, Update, Delete (CRUD) tasks
- **Task Filtering**: Search, filter by status, and sort tasks
- **Responsive UI**: Modern Angular Material Design interface
- **Real-time Updates**: Server-side rendering support
- **RESTful API**: Complete REST API for all operations

---

## 🛠 Tech Stack

### Frontend
- **Angular 21.2** - Latest standalone components
- **Angular Material** - UI component library
- **TypeScript 5.9** - Type-safe JavaScript
- **RxJS** - Reactive programming
- **Angular SSR** - Server-side rendering support
- **Node.js 20+** - Runtime environment
- **npm 11.14.1** - Package manager

### Backend
- **.NET 10** - Web API framework
- **Entity Framework Core 10** - ORM for database access
- **SQL Server** - Database (or SQL Server Express)
- **Swagger/OpenAPI** - API documentation
- **Basic Authentication** - Authentication scheme
- **CORS** - Cross-Origin Resource Sharing

---

## 📁 Project Structure

```
Task Manager app/
├── task-manager/                    # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/               # Core services, models, guards
│   │   │   │   ├── models/
│   │   │   │   └── services/
│   │   │   ├── features/           # Feature modules (auth, tasks)
│   │   │   │   ├── auth/
│   │   │   │   └── tasks/
│   │   │   ├── guards/             # Route guards
│   │   │   └── shared/             # Shared components
│   │   ├── main.ts                 # Application entry point
│   │   ├── main.server.ts          # Server-side rendering entry
│   │   ├── styles.scss             # Global styles
│   │   └── index.html              # HTML template
│   ├── angular.json                # Angular CLI configuration
│   ├── package.json                # Dependencies
│   └── tsconfig.json               # TypeScript configuration
│
└── TaskManagerAPI/                  # .NET Backend
    ├── Program.cs                  # Application entry point
    ├── appsettings.json            # Configuration (Production)
    ├── appsettings.Development.json # Configuration (Development)
    ├── Controllers/                # API endpoints
    │   ├── AuthController.cs
    │   └── TasksController.cs
    ├── Services/                   # Business logic
    │   ├── AuthService.cs
    │   └── TaskService.cs
    ├── Repositories/               # Data access
    │   ├── TaskRepository.cs
    │   └── UserRepository.cs
    ├── Models/                     # Domain models
    │   ├── TaskItem.cs
    │   ├── User.cs
    │   └── DTOs.cs
    ├── Data/                       # Database context
    │   └── AppDbContext.cs
    ├── Migrations/                 # Database migrations
    │   ├── 20260516082324_InitialCreate.cs
    │   └── 20260516084402_Task.cs
    ├── Auth/                       # Authentication
    │   └── BasicAuthHandler.cs
    └── TaskManagerAPI.csproj       # Project file
```

---

## ✅ Prerequisites

### Required Software

1. **Node.js & npm**
   - Download: https://nodejs.org/ (v20+)
   - Check: `node --version` and `npm --version`

2. **.NET 10 SDK**
   - Download: https://dotnet.microsoft.com/download/dotnet
   - Check: `dotnet --version`

3. **SQL Server or SQL Server Express**
   - Download: https://www.microsoft.com/sql-server/sql-server-downloads
   - Or use: Local DB (included with Visual Studio)

4. **Code Editor** (one of)
   - Visual Studio Code
   - Visual Studio 2024+
   - JetBrains Rider

### System Requirements

- **Windows 10/11** (or WSL2 for other OS)
- **8 GB RAM minimum** (16 GB recommended)
- **500 MB free disk space** for dependencies

---

## 🗄️ Database Setup

### Step 1: Verify SQL Server Connection String

Open `TaskManagerAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-7S3IV45\\SQLEXPRESS;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Important**: If your SQL Server instance name differs, update the `Server` value.

#### Find Your SQL Server Instance Name:
- **SQL Server Express**: Usually `SQLEXPRESS` or `YourComputerName\SQLEXPRESS`
- **SQL Server Developer Edition**: Often `MSSQLSERVER` or `YourComputerName\MSSQLSERVER`
- **Local DB**: `(localdb)\MSSQLLocalDB`

To find your instance:
```bash
# PowerShell
Get-CimInstance -Class Win32_Service | Where-Object {$_.Name -match "SQL"}
```

### Step 2: Configure Connection String (if needed)

If your SQL Server instance differs, update the connection string in BOTH files:
- `TaskManagerAPI/appsettings.json`
- `TaskManagerAPI/appsettings.Development.json`

Example:
```json
"DefaultConnection": "Server=YourServerName\\SQLEXPRESS;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

### Step 3: Apply Migrations Automatically

The application automatically applies migrations on startup. The database and tables will be created when you first run the backend.

#### Alternative: Manual Migration (if needed)

```bash
cd TaskManagerAPI
dotnet ef database update
```

### Step 4: Verify Database Creation

After running the backend for the first time, verify the database was created:

**Option 1: SQL Server Management Studio (SSMS)**
1. Connect to your SQL Server instance
2. Expand "Databases"
3. Look for `TaskManagerDB`

**Option 2: SQL Query**
```sql
SELECT name FROM sys.databases WHERE name = 'TaskManagerDB';
```

### Step 5: Seed Data

The database includes seed data that's automatically applied:

**Users Table:**
| Id | Username | Password  |
|----|----------|-----------|
| 1  | admin    | admin123  |

**Tasks Table:**
| Id | Title                   | Description                   | Priority | IsCompleted | CreatedAt              |
|----|-------------------------|-------------------------------|----------|-------------|------------------------|
| 1  | Complete project setup  | Set up .NET and Angular       | High     | false       | 2024-01-01 00:00:00    |
| 2  | Design database schema  | Create tables for tasks/users | High     | true        | 2024-01-02 00:00:00    |

---

## 🚀 Backend Setup & Running

### Step 1: Navigate to Backend Directory

```bash
cd TaskManagerAPI
```

### Step 2: Restore NuGet Packages

```bash
dotnet restore
```

### Step 3: Build the Project

```bash
dotnet build
```

### Step 4: Run the Application

```bash
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:44399
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Step 5: Access Swagger Documentation

Open your browser and navigate to:
```
https://localhost:44399/swagger/index.html
```

Here you can:
- View all API endpoints
- Test endpoints directly
- See request/response schemas
- Copy curl commands

### Backend Running on

- **HTTPS**: `https://localhost:44399`
- **HTTP**: `http://localhost:5265`
- **Swagger UI**: `https://localhost:44399/swagger/index.html`

### Step 6: Stop the Application

Press `Ctrl+C` in the terminal

---

## 🎨 Frontend Setup & Running

### Step 1: Navigate to Frontend Directory

```bash
cd task-manager
```

### Step 2: Install Dependencies

```bash
npm install
```

This installs all packages listed in `package.json`, including:
- Angular 21.2
- Angular Material
- TypeScript 5.9
- And other dependencies

### Step 3: Start Development Server

```bash
npm start
```

**Expected Output:**
```
✔ Compiled successfully.
✔ Build complete.

Initial Chunk Files | Names         | Size
main.js             | main         | 500 kB |

Application bundle generation complete. [30.123 seconds]

Watch mode enabled. Watching for file changes in the workspace directory.

NOTE: Raw file changes will trigger a rebuild. Your browser should refresh automatically when files are updated.

Application is running at 'http://localhost:4200'.
```

### Step 4: Access the Application

Open your browser and navigate to:
```
http://localhost:4200
```

### Step 5: Test the Application

1. **Login**
   - Username: `admin`
   - Password: `admin123`

2. **View Tasks**: See the list of seeded tasks

3. **Create Task**: Click "+" button to add a new task

4. **Edit Task**: Click on a task to edit it

5. **Toggle Completion**: Click the checkbox to mark as complete

6. **Delete Task**: Click delete icon to remove a task

### Step 6: Stop Development Server

Press `Ctrl+C` in the terminal

### Build for Production (Optional)

```bash
npm run build
```

Output is in `dist/task-manager/` directory.

---

## 📡 API Documentation

### Base URL

```
https://localhost:44399/api
```

### Authentication

All endpoints except `/auth/login` require Basic Authentication header:

```
Authorization: Basic base64(username:password)
```

Example with `admin:admin123`:
```
Authorization: Basic YWRtaW46YWRtaW4xMjM=
```

### API Endpoints

#### 1. **Authentication**

##### Login
- **Endpoint**: `POST /auth/login`
- **Description**: Authenticate user and get credentials
- **Auth Required**: ❌ No
- **Request Body**:
  ```json
  {
    "username": "admin",
    "password": "admin123"
  }
  ```
- **Response (200 OK)**:
  ```json
  {
    "id": 1,
    "username": "admin",
    "message": "Login successful"
  }
  ```
- **Response (401 Unauthorized)**:
  ```json
  {
    "error": "Invalid credentials"
  }
  ```
- **cURL Example**:
  ```bash
  curl -X POST "https://localhost:44399/api/auth/login" \
    -H "Content-Type: application/json" \
    -d '{"username":"admin","password":"admin123"}'
  ```

---

#### 2. **Tasks**

##### Get All Tasks
- **Endpoint**: `GET /tasks`
- **Description**: Retrieve all tasks
- **Auth Required**: ✅ Yes
- **Query Parameters**: None
- **Response (200 OK)**:
  ```json
  [
    {
      "id": 1,
      "title": "Complete project setup",
      "description": "Set up .NET and Angular projects",
      "isCompleted": false,
      "priority": "High",
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": null
    },
    {
      "id": 2,
      "title": "Design database schema",
      "description": "Create tables for tasks and users",
      "isCompleted": true,
      "priority": "High",
      "createdAt": "2024-01-02T00:00:00Z",
      "updatedAt": "2024-01-15T10:30:00Z"
    }
  ]
  ```
- **cURL Example**:
  ```bash
  curl -X GET "https://localhost:44399/api/tasks" \
    -H "Authorization: Basic YWRtaW46YWRtaW4xMjM="
  ```

##### Get Task by ID
- **Endpoint**: `GET /tasks/{id}`
- **Description**: Retrieve a specific task
- **Auth Required**: ✅ Yes
- **Path Parameters**:
  | Parameter | Type    | Description      |
  |-----------|---------|------------------|
  | id        | integer | Task ID          |
- **Response (200 OK)**:
  ```json
  {
    "id": 1,
    "title": "Complete project setup",
    "description": "Set up .NET and Angular projects",
    "isCompleted": false,
    "priority": "High",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null
  }
  ```
- **Response (404 Not Found)**:
  ```json
  { "error": "Task not found" }
  ```
- **cURL Example**:
  ```bash
  curl -X GET "https://localhost:44399/api/tasks/1" \
    -H "Authorization: Basic YWRtaW46YWRtaW4xMjM="
  ```

##### Create Task
- **Endpoint**: `POST /tasks`
- **Description**: Create a new task
- **Auth Required**: ✅ Yes
- **Request Body**:
  ```json
  {
    "title": "New task title",
    "description": "Task description (optional)",
    "priority": "High"
  }
  ```
- **Response (201 Created)**:
  ```json
  {
    "id": 3,
    "title": "New task title",
    "description": "Task description",
    "isCompleted": false,
    "priority": "High",
    "createdAt": "2024-01-16T10:00:00Z",
    "updatedAt": null
  }
  ```
- **Response (400 Bad Request)**:
  ```json
  { "error": "Title is required" }
  ```
- **cURL Example**:
  ```bash
  curl -X POST "https://localhost:44399/api/tasks" \
    -H "Authorization: Basic YWRtaW46YWRtaW4xMjM=" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "New task",
      "description": "Description here",
      "priority": "High"
    }'
  ```

##### Update Task
- **Endpoint**: `PUT /tasks/{id}`
- **Description**: Update an existing task
- **Auth Required**: ✅ Yes
- **Path Parameters**:
  | Parameter | Type    | Description      |
  |-----------|---------|------------------|
  | id        | integer | Task ID          |
- **Request Body**:
  ```json
  {
    "title": "Updated title",
    "description": "Updated description",
    "isCompleted": true,
    "priority": "Medium"
  }
  ```
- **Response (200 OK)**:
  ```json
  {
    "id": 1,
    "title": "Updated title",
    "description": "Updated description",
    "isCompleted": true,
    "priority": "Medium",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-16T10:15:00Z"
  }
  ```
- **Response (404 Not Found)**:
  ```json
  { "error": "Task not found" }
  ```
- **cURL Example**:
  ```bash
  curl -X PUT "https://localhost:44399/api/tasks/1" \
    -H "Authorization: Basic YWRtaW46YWRtaW4xMjM=" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "Updated title",
      "description": "Updated description",
      "isCompleted": true,
      "priority": "Medium"
    }'
  ```

##### Toggle Task Completion
- **Endpoint**: `PATCH /tasks/{id}/toggle`
- **Description**: Toggle task completion status
- **Auth Required**: ✅ Yes
- **Path Parameters**:
  | Parameter | Type    | Description      |
  |-----------|---------|------------------|
  | id        | integer | Task ID          |
- **Request Body**: Empty (or `{}`)
- **Response (200 OK)**:
  ```json
  {
    "id": 1,
    "title": "Complete project setup",
    "description": "Set up .NET and Angular projects",
    "isCompleted": true,
    "priority": "High",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-16T10:20:00Z"
  }
  ```
- **cURL Example**:
  ```bash
  curl -X PATCH "https://localhost:44399/api/tasks/1/toggle" \
    -H "Authorization: Basic YWRtaW46YWRtaW4xMjM="
  ```

##### Delete Task
- **Endpoint**: `DELETE /tasks/{id}`
- **Description**: Delete a task
- **Auth Required**: ✅ Yes
- **Path Parameters**:
  | Parameter | Type    | Description      |
  |-----------|---------|------------------|
  | id        | integer | Task ID          |
- **Response (204 No Content)**:
  ```
  (Empty response body)
  ```
- **Response (404 Not Found)**:
  ```json
  { "error": "Task not found" }
  ```
- **cURL Example**:
  ```bash
  curl -X DELETE "https://localhost:44399/api/tasks/1" \
    -H "Authorization: Basic YWRtaW46YWRtaW4xMjM="
  ```

---

## 🗃️ Database Schema & Queries

### Database Name
```
TaskManagerDB
```

### Table 1: Users

**Schema:**
```sql
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
```

**Columns:**
| Column   | Type         | Required | Notes                              |
|----------|--------------|----------|----------------------------------|
| Id       | int          | ✅ Yes   | Primary Key, Auto-increment       |
| Username | nvarchar(50) | ✅ Yes   | Max 50 characters, Unique         |
| Password | nvarchar(255)| ✅ Yes   | Max 255 characters                |

**Seed Data:**
```sql
INSERT INTO Users (Username, Password) VALUES
('admin', 'admin123');
```

### Table 2: Tasks

**Schema:**
```sql
CREATE TABLE [Tasks] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000),
    [IsCompleted] bit NOT NULL DEFAULT (0),
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2,
    [Priority] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id])
);
```

**Columns:**
| Column      | Type          | Required | Default       | Notes                         |
|-------------|---------------|----------|---------------|-------------------------------|
| Id          | int           | ✅ Yes   | Auto-increment| Primary Key                  |
| Title       | nvarchar(200) | ✅ Yes   | -             | Max 200 characters            |
| Description | nvarchar(1000)| ❌ No    | NULL          | Max 1000 characters           |
| IsCompleted | bit           | ✅ Yes   | false (0)     | Boolean (0 or 1)              |
| CreatedAt   | datetime2     | ✅ Yes   | GETUTCDATE()  | UTC timestamp                 |
| UpdatedAt   | datetime2     | ❌ No    | NULL          | Updated timestamp             |
| Priority    | nvarchar(50)  | ✅ Yes   | 'Medium'      | High, Medium, Low, etc.       |

**Seed Data:**
```sql
INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt, Priority) VALUES
('Complete project setup', 'Set up .NET and Angular projects', 0, '2024-01-01T00:00:00Z', 'High'),
('Design database schema', 'Create tables for tasks and users', 1, '2024-01-02T00:00:00Z', 'High');
```

### Useful SQL Queries

#### 1. View All Tasks
```sql
SELECT * FROM Tasks
ORDER BY CreatedAt DESC;
```

#### 2. View All Users
```sql
SELECT * FROM Users;
```

#### 3. Get Tasks by Priority
```sql
SELECT * FROM Tasks
WHERE Priority = 'High'
ORDER BY CreatedAt DESC;
```

#### 4. Get Completed Tasks
```sql
SELECT * FROM Tasks
WHERE IsCompleted = 1
ORDER BY UpdatedAt DESC;
```

#### 5. Get Incomplete Tasks
```sql
SELECT * FROM Tasks
WHERE IsCompleted = 0
ORDER BY CreatedAt DESC;
```

#### 6. Count Tasks by Status
```sql
SELECT 
    IsCompleted,
    COUNT(*) AS TaskCount
FROM Tasks
GROUP BY IsCompleted;
```

#### 7. Count Tasks by Priority
```sql
SELECT 
    Priority,
    COUNT(*) AS TaskCount,
    SUM(CASE WHEN IsCompleted = 1 THEN 1 ELSE 0 END) AS CompletedCount
FROM Tasks
GROUP BY Priority;
```

#### 8. Search Tasks by Title
```sql
SELECT * FROM Tasks
WHERE Title LIKE '%search_term%'
ORDER BY CreatedAt DESC;
```

#### 9. Get Recently Updated Tasks
```sql
SELECT * FROM Tasks
WHERE UpdatedAt IS NOT NULL
ORDER BY UpdatedAt DESC
LIMIT 10;
```

#### 10. Add a New Task
```sql
INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt, Priority)
VALUES ('Task Title', 'Task Description', 0, GETUTCDATE(), 'Medium');
```

#### 11. Update Task Status
```sql
UPDATE Tasks
SET IsCompleted = 1, UpdatedAt = GETUTCDATE()
WHERE Id = 1;
```

#### 12. Delete a Task
```sql
DELETE FROM Tasks
WHERE Id = 1;
```

#### 13. Get Database Size
```sql
EXEC sp_spaceused 'Tasks';
EXEC sp_spaceused 'Users';
```

#### 14. Check Migrations Applied
```sql
SELECT * FROM [__EFMigrationsHistory]
ORDER BY MigrationId DESC;
```

---

## 🔐 Authentication

### How Authentication Works

1. **User logs in** via `POST /auth/login` with username/password
2. **Browser stores** credentials in memory (session)
3. **Requests** include `Authorization: Basic <base64(username:password)>` header
4. **Backend** verifies credentials using `BasicAuthHandler`
5. **Protected endpoints** require valid authentication

### Protected vs Public Endpoints

| Endpoint             | Auth Required | Notes                      |
|----------------------|--------------|---------------------------|
| POST /auth/login     | ❌ No        | Public endpoint            |
| GET /tasks           | ✅ Yes       | Requires authentication    |
| POST /tasks          | ✅ Yes       | Requires authentication    |
| PUT /tasks/{id}      | ✅ Yes       | Requires authentication    |
| PATCH /tasks/{id}/..| ✅ Yes       | Requires authentication    |
| DELETE /tasks/{id}   | ✅ Yes       | Requires authentication    |

### Creating New Users

To add a new user to the database:

```sql
INSERT INTO Users (Username, Password)
VALUES ('newuser', 'password123');
```

⚠️ **Security Note**: Passwords are currently stored in plain text. For production, implement proper password hashing (bcrypt, argon2, etc.).

---

## 👨‍💻 Development Workflow

### Running Both Frontend & Backend Simultaneously

#### Terminal 1: Start Backend
```bash
cd TaskManagerAPI
dotnet run
# Runs on https://localhost:44399
```

#### Terminal 2: Start Frontend
```bash
cd task-manager
npm start
# Runs on http://localhost:4200
```

#### Terminal 3: Access Application
Open browser: `http://localhost:4200`

### Common Development Tasks

#### Make Changes to Backend

1. Edit file in `TaskManagerAPI/`
2. Backend auto-reloads (Hot reload enabled)
3. Test via Swagger UI or frontend
4. Check logs in terminal

#### Make Changes to Frontend

1. Edit file in `task-manager/src/`
2. Frontend auto-reloads (Watch mode enabled)
3. Browser auto-refreshes
4. Check console for errors

#### Add New API Endpoint

1. Create controller method in `Controllers/`
2. Use `[Authorize]` attribute if protected
3. Restart backend
4. Swagger documentation auto-updates
5. Call from frontend service

#### Add New Database Field

1. Update model in `Models/`
2. Create migration: `dotnet ef migrations add MigrationName`
3. Update database: `dotnet ef database update`
4. Migration runs automatically on next app start
5. Update services/repositories as needed

#### Add New Frontend Component

1. Generate: `ng generate component features/tasks/task-form`
2. Add routing if needed
3. Implement logic
4. Frontend auto-reloads
5. Test manually

---

## 🐛 Troubleshooting

### Common Issues & Solutions

#### Issue 1: "Connection string server instance not found"

**Error Message:**
```
System.Data.SqlClient.SqlException: A network-related or instance-specific error occurred while establishing a connection to SQL Server.
```

**Solution:**
1. Verify SQL Server is running
2. Check instance name in connection string
3. Update `appsettings.json` with correct server name
4. Restart backend

```bash
# Find SQL Server instance
Get-CimInstance -Class Win32_Service | Where-Object {$_.Name -match "SQL"}
```

---

#### Issue 2: "Port 44399 already in use"

**Error Message:**
```
Unhandled exception: System.IO.IOException: Failed to bind to address https://127.0.0.1:44399
```

**Solution:**
```bash
# Find process using port 44399
netstat -ano | findstr :44399

# Kill process (replace PID with actual number)
taskkill /PID <PID> /F

# Or use different port
dotnet run --urls="https://localhost:44400"
```

---

#### Issue 3: "Port 4200 already in use"

**Error Message:**
```
✔ Compiled successfully.
Port 4200 is already in use. Use '--port' to specify a different port.
```

**Solution:**
```bash
# Use different port
ng serve --port 4201

# Or kill process using 4200
netstat -ano | findstr :4200
taskkill /PID <PID> /F
```

---

#### Issue 4: "Cannot GET /api/tasks - 401 Unauthorized"

**Error Message:**
```json
{ "error": "Unauthorized" }
```

**Solution:**
1. Ensure user is logged in
2. Check Authorization header is being sent
3. Verify credentials in `Users` table
4. Login again and retry

---

#### Issue 5: "npm ERR! code ERESOLVE"

**Error Message:**
```
npm ERR! code ERESOLVE
npm ERR! ERESOLVE could not resolve dependencies
```

**Solution:**
```bash
# Clear cache
npm cache clean --force

# Try legacy peer deps flag
npm install --legacy-peer-deps
```

---

#### Issue 6: "Migrations already applied"

**Error Message:**
```
Database is already up to date.
```

**Solution:**
This is normal. Your database is current. No action needed.

---

#### Issue 7: "Database not created"

**Solution:**
1. Check SQL Server connection string
2. Ensure SQL Server service is running
3. Verify trusted connection permissions
4. Run migration manually:
   ```bash
   cd TaskManagerAPI
   dotnet ef database update
   ```
5. Check database in SSMS

---

#### Issue 8: "HTTPS Certificate Error"

**Error Message:**
```
System.Net.Http.HttpRequestException: The SSL connection could not be established
```

**Solution:**
```bash
# Trust .NET HTTPS certificate (Windows)
dotnet dev-certs https --trust

# Or disable HTTPS for development
# Update launchSettings.json - set ASPNETCORE_HTTPS_PORT to empty
```

---

#### Issue 9: "CORS Error in Frontend"

**Error Message:**
```
Access to XMLHttpRequest at 'https://localhost:44399/api/tasks' from origin 'http://localhost:4200' has been blocked by CORS policy
```

**Solution:**
CORS is configured in `Program.cs`. Ensure:
1. Frontend URL is in allowed origins
2. Backend CORS policy includes frontend origin
3. Check `appsettings.json` or `Program.cs` CORS configuration

Update if needed in `Program.cs`:
```csharp
options.AddPolicy("AllowAngular", policy =>
{
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod();
});
```

---

#### Issue 10: "Cannot find module '@angular/core'"

**Error Message:**
```
Cannot find module '@angular/core' or its corresponding type declarations
```

**Solution:**
```bash
# Reinstall dependencies
npm install

# Or clear node_modules and reinstall
rm -r node_modules
npm install
```

---

### Quick Checklist

Before troubleshooting, verify:

- [ ] SQL Server is running
- [ ] Connection string has correct server name
- [ ] .NET 10 SDK is installed: `dotnet --version`
- [ ] Node.js 20+ is installed: `node --version`
- [ ] No ports already in use (44399, 5265, 4200)
- [ ] Firewall allows localhost connections
- [ ] No proxy/VPN blocking local connections
- [ ] Database migrations applied: `dotnet ef migrations list`

---

### Getting Help

If issues persist:

1. **Check logs** in terminal where app is running
2. **Review error message** carefully (often has solution)
3. **Verify prerequisites** are installed
4. **Test API** directly via Swagger UI
5. **Check browser console** for frontend errors (F12)
6. **Restart both** backend and frontend

---

## 📚 Additional Resources

- [Angular Documentation](https://angular.io/docs)
- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQL Server Documentation](https://learn.microsoft.com/sql/)
- [RESTful API Best Practices](https://restfulapi.net/)

---

## 📝 Notes

- Default credentials: `admin` / `admin123`
- Database auto-migrates on application start
- HTTPS is required for API endpoints
- Frontend connects to backend at `https://localhost:44399`
- All timestamps stored in UTC

---

## ✨ Summary

This full-stack application demonstrates modern web development with:
- ✅ Type-safe frontend and backend
- ✅ Secure authentication
- ✅ Responsive UI design
- ✅ RESTful API architecture
- ✅ Database migrations and seeding
- ✅ Error handling and validation

Happy coding! 🚀
