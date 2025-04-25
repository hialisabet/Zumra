# Zumra - Todo Management API

A simple web application using ASP.NET Core with Clean Architecture principles and CQRS pattern.

## Project Structure

The solution follows Clean Architecture principles with the following projects:

- **Zumra.Domain** - Contains enterprise logic and entities
- **Zumra.Application** - Contains business logic
- **Zumra.Infrastructure** - Contains all external concerns
- **Zumra.API** - The presentation layer (Web API)
- **Tests Projects** - Unit tests for all layers

## Prerequisites

- .NET 9.0 SDK
- SQL Server (or LocalDB)

## Setup and Running

1. **Clone the repository**

2. **Update the Connection String**
   
   Check the connection string in `Zumra.API/appsettings.json` and update it if necessary.

3. **Apply Database Migrations**

   Run the following commands from the root of the solution:

   ```powershell
   cd src/Zumra.Infrastructure
   dotnet ef migrations add InitialCreate --startup-project ../Zumra.API/Zumra.API.csproj
   cd ../Zumra.API
   dotnet ef database update
   ```

   Alternatively, you can run the PowerShell script `CreateMigration.ps1`.

4. **Run the application**

   ```bash
   cd src/Zumra.API
   dotnet run
   ```

5. **Access the API**

   Once the application is running, you can access the API at:

   - API: https://localhost:5001
   - Swagger UI: https://localhost:5001/swagger

## API Endpoints

The API provides the following endpoints:

- `GET /api/TodoItems` - Get all todo items
- `GET /api/TodoItems/{id}` - Get a specific todo item by ID
- `POST /api/TodoItems` - Create a new todo item
- `PUT /api/TodoItems/{id}` - Update an existing todo item
- `DELETE /api/TodoItems/{id}` - Delete a todo item

## Testing

To run the tests:

```bash
dotnet test
```

## Architecture

### Clean Architecture

This solution follows Clean Architecture principles:

- **Domain Layer** - Contains all entities, enums, exceptions, interfaces, and types
- **Application Layer** - Contains all business logic and interfaces
- **Infrastructure Layer** - Contains all external concerns
- **Presentation Layer** - Contains all presentation logic

### CQRS Pattern

The Command Query Responsibility Segregation pattern is implemented using MediatR:

- **Commands** - Used to change state
- **Queries** - Used to retrieve data
