# MOT AI Backend

A .NET 8 backend integrating a government MOT API with OAuth, enriched by AI-driven analysis and personalised user context.

- Vehicle MOT data via DVSA API (OAuth 2.0)
- AI-powered vehicle risk analysis
- JWT authentication (register/login)
- User profile personalisation
- Search history persistence

---

## Project Structure

```id="4imw9x"
Controllers/
  AuthController.cs       Handles register/login
  UserController.cs       Profile endpoints
  VehicleController.cs    Vehicle lookup + AI analysis

Services/
  AuthService.cs          Password hashing + JWT generation
  DvsaService.cs          DVSA OAuth + MOT API integration
  AiService.cs            OpenAI integration + analysis
  VehicleService.cs       Core orchestration logic

Models/
  User.cs
  SearchHistory.cs
  VehicleResponse.cs
  AiResult.cs

Dtos/
  RegisterRequest.cs
  LoginRequest.cs
  UpdateUserProfileRequest.cs

Data/
  AppDbContext.cs         EF Core DB context
```

## Getting Started

### 1. Install dependencies

`dotnet restore`

### 2. Configure environment

Create and update

`appsettings.Development.json`

```id="4imw9x"
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MotInsightDb;Trusted_Connection=True;"
  },
  "OpenAiKey": "YOUR_OPENAI_KEY",
  "Dvsa": {
    "ClientId": "...",
    "ClientSecret": "...",
    "TokenUrl": "...",
    "Scope": "...",
    "ApiKey": "..."
  }
}
```

### 3. Configure backend API

`Add-Migration InitialCreate
Update-Database`

### 4. Run the API

`dotnet run`

Swagger available at:

`https://localhost:7001/swagger` (for example)

## Authentication

JWT-based authentication.

### Endpoints

#### Register

`POST /api/auth/register`

#### Login

`POST /api/auth/login`

Returns:

```id="4imw9x"
{
  "token": "JWT_TOKEN"
}
```

## User Profile

### Get profile

`GET /api/user/profile
Authorization: Bearer <token>`

### Update profile

`PUT /api/user/profile
Authorization: Bearer <token>`

```id="4imw9x"
{
  "yearlyMileage": 12000,
  "drivingType": "high",
  "mechanicalKnowledge": "medium"
}
```

## Vehicle Endpoint

### Get vehicle analysis

`GET /api/vehicle/{reg}`

`Authorization: Bearer <token>`

### Example Response

```id="4imw9x"
{
  "vehicle": {
    "registration": "DU10ZNT",
    "make": "VOLKSWAGEN",
    "model": "GOLF",
    "lastTestResult": "PASSED",
    "lastTestDate": "2026-01-19"
  },
  "defects": {
    "major": [],
    "minor": [],
    "advisory": []
  },
  "ai": {
    "risk": "Low",
    "summary": "Vehicle is in good condition...",
    "recommendations": [
      "Continue regular servicing",
      "Monitor wear over time"
    ]
  }
}
```

## Architecture Review

`Client → API → DVSA (OAuth) → Data parsing → AI analysis → Response`

### Flow

-   1. Fetch MOT data from DVSA API
-   2. Parse and structure defects
-   3. Enrich with user profile (if logged in)
-   4. Generate AI analysis
-   5. Return structured response
-   6. Persist search history

## Environment Variables (Production)

Use environment variables instead of appsettings:

```id="4imw9x"
ConnectionStrings__DefaultConnection
OpenAiKey
Dvsa__ClientId
Dvsa__ClientSecret
Dvsa__TokenUrl
Dvsa__Scope
Dvsa__ApiKey
```
