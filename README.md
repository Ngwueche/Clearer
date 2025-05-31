
# AuthService

**AuthService** is a .NET 9.0 Core-based authentication microservice that provides:

- Secure login
- User registration
- JWT-based authentication
- Role-based authorization
- InMemory cache (Expandable to a more persistent cache)

---

## Project Structure

```

AuthService/
│
├── AuthService.Api/                         # Entry point for the API
│   ├── Controllers/                         # API endpoints (AuthController)
│   ├── Extensions/                          # Extension methods for service registration, etc.
│   ├── Globals/                             # Global constants or static configurations
│   ├── appsettings.json                     # Application configuration (JWT, DB, etc.)
│   ├── AuthService.http                     # HTTP client test file (REST Client)
│   └── Program.cs                           # Main entry point and service pipeline setup
│
├── AuthService.Application/                 # Application layer with core logic contracts
│   ├── DTOs/                                # Request and response data transfer objects
│   ├── RepositoryInterfaces/                # Abstractions for data access
│   └── ServiceInterfaces/                   # Abstractions for business logic
│
├── AuthService.Data/                        # EF Core DB context and migrations
│   ├── EFCore/                              # DbContext and configurations
│   └── Migrations/                          # EF Core migrations
│
├── AuthService.Domain/                      # Domain models and enums
│   ├── Entities/                            # Entity classes (ApplicationUser, Role)
│   └── Enums/                               # Role names
│
├── AuthService.Infrastructure/              # Infrastructure layer implementing interfaces
│   ├── RepositoryImplementations/           # EF Core repositories implementing data interfaces
│   ├── ServiceImplementations/              # Business logic implementation (AuthenticationService)
│   └── Utilities/                           # Helpers (PasswordHelper, JWT generator, etc.)
└── AuthService.sln                          # Solution file

````

---

## Steps to Run the App

### 1. Clone the Repository

```bash
git clone https://github.com/Ngwueche/Clearer.git
cd AuthService
````

### 2. Open in Visual Studio

### 3. Create an MSSQL Database

Update the database name in `appsettings.json` and run the application. The application will seed the database with the required roles.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=HQ-TECH-L1314\\SQLEXPRESS;Initial Catalog=<YourOwnDB>;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True"
}
```

---

# API Endpoints

---

## POST `/api/auth/login`

### Request Schema

```json
{
  "username": "Admin",
  "password": "Admin123"
}
```

### Response Schema

```json
{
  "responseData": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "fullName": "Wisdom Ngwueche",
    "refreshToken": "01972551-583e-7d64-a0c8-15a5168aa99f",
    "role": "User"
  },
  "responseMsg": "Success",
  "isSuccess": true,
  "responseCode": "00"
}
```

---

## POST `/api/auth/refresh-token`

### Request Schema

```json
{
  "username": "admin",
  "refreshToken": "01972551-583e-7d64-a0c8-15a5168aa99f"
}
```

### Response Schema

```json
{
  "responseData": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "fullName": "Wisdom Ngwueche",
    "refreshToken": "01972551-583e-7d64-a0c8-15a5168aa99f",
    "role": "User"
  },
  "responseMsg": "Success",
  "isSuccess": true,
  "responseCode": "00"
}
```

---

## POST `/api/auth/register`

> **Note**: `roleId` must be obtained from the `/api/auth/get-roles` endpoint.

### Request Schema

```json
{
  "firstName": "Wisdom",
  "lastName": "Ngwueche",
  "otherName": "Chukwuma",
  "userName": "user234",
  "password": "Qwerty",
  "roleId": "01972151-b379-74bc-9309-cadf2cbc24be"
}
```

### Response Schema

```json
{
  "responseData": null,
  "responseMsg": "User registered successfully",
  "isSuccess": true,
  "responseCode": "00"
}
```

---

## POST `/api/auth/deactivate-user`

### Request Schema

```json
{
  "username": "user234"
}
```

### Response Schema

```json
{
  "responseData": null,
  "responseMsg": "User deactivated successfully",
  "isSuccess": true,
  "responseCode": "00"
}
```

---

## POST `/api/auth/delete-user`

### Request Schema

```json
{
  "username": "user234"
}
```

### Response Schema

```json
{
  "responseData": null,
  "responseMsg": "User Deleted successfully",
  "isSuccess": true,
  "responseCode": "00"
}
```

---

## GET `/api/auth/get-roles`

### Response Schema

```json
{
  "responseData": [
    {
      "id": "01972151-b379-759b-bde7-6f318df8029d",
      "roleName": "Admin"
    },
    {
      "id": "01972151-b379-74bc-9309-cadf2cbc24be",
      "roleName": "User"
    }
  ],
  "responseMsg": "Success",
  "isSuccess": true,
  "responseCode": "00"
}
```
