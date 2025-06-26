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

- `Domain/` – Core business models and interfaces  
- `Application/` – DTOs, interfaces, and services  
- `Infrastructure/` – Data access and EF Core repositories  
- `Web/` – MVC controllers and Razor views  
- `Tests/` – Unit tests for core application logic  

## Screenshots

*(You can include UI screenshots here)*

## License

MIT
