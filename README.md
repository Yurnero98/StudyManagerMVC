# StudyManagerMVC

Web version of the StudyManager application with full CRUD functionality for students, groups, and courses. Built using ASP.NET Core MVC with a clean architectural approach.

## Technologies

- ASP.NET Core MVC  
- Entity Framework Core  
- MS SQL Server  
- Clean Architecture (Domain, Application, Infrastructure, Web)  
- Razor Views  
- AutoMapper  
- Dependency Injection  
- MSTest (unit testing with in-memory SQLite)  

## Features

- Create, update, and delete students, groups, and courses  
- Assign students to groups and groups to courses  
- Structured solution with clear separation of concerns  
- DTOs and ViewModels for safer data transfer  
- Clean and responsive Razor UI  
- Model validation with user-friendly error messages  
- Logging and exception handling  
- Unit tests for application services using MSTest and in-memory SQLite database  

## Folder Structure

- `StudyManagerMVC.Domain/` – Core business models and interfaces  
- `StudyManagerMVC.Application/` – DTOs, interfaces, and services  
- `StudyManagerMVC.Infrastructure/` – Data access and EF Core repositories  
- `StudyManagerMVC.Presentation/` – MVC controllers and Razor views  
- `StudyManagerMVC.Application.Tests/` – Unit tests for core application logic  

## Prerequisites

- .NET 8 SDK

- MS SQL Server (local or remote)

### Clone the repository

- git clone https://github.com/Yurnero98/StudyManagerMVC.git
- cd StudyManagerMVC

### Configure the Database

In StudyManagerMVC.Presentation/appsettings.json, update the connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=StudyManagerDb;Integrated Security=true;TrustServerCertificate=true;"
}

### Apply EF Core Migrations

- cd StudyManagerMVC.Presentation
- dotnet ef database update

### Run the Application

- dotnet run --project StudyManagerMVC.Presentation

### 🧪 Running Tests

Tests are located in the StudyManagerMVC.Application.Tests/ project and cover the Application layer.
Run tests using:
- cd StudyManagerMVC.Application.Tests
- dotnet test

## License

MIT
