# Overview
This API provides authorized CRUD endpoints for tasks. It also supports role-based authorization with an admin-only endpoint.

User authentication is handled using JSON Web Tokens (JWT). Refresh tokens are implemented and rotated whenever the client requests a new access token.

### Data Validation
DTOs are used for data transfer and input validation. Basic validation is done using data annotations, while business-level validation (such as unique email constraints) is handled using Fluent API rules.

## Design Pattern
The API is separated into layers.

Authentication > Controllers > Services > Database (DbContext)

Controllers handle HTTP requests, services contain business logic, and the database layer manages data persistence.

### Summary of Features
- JWT Authentication
- Refresh Token Rotation
- Role-based Authorization
- DTOs for data transfer
- Input Validation
- Custom `ServiceResult<T>` wrapper for consistent API responses
- ILogger logging with custom extension methods
- Clear Separation of Concerns
- Clean Code Structure
## Main Endpoints

Authentication
- POST /UserAuth/register
- POST /UserAuth/login
- PUT /UserAuth/refresh-token
- PUT /UserAuth/log-out

Tasks
- GET /Task/get-tasks
- GET /Task/get-a-task/{id}
- POST /Task/create-a-task
- PUT /Task/update-a-task
- DELETE /Task/delete-a-task/{id}

Admin
- GET /Task/get-all-tasks

## Key Packages

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Tools