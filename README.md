# E-Commerce API (.NET 8)

A clean, modular REST API for managing an e-commerce platform.

## Features

- Clean Architecture
- Repository Pattern + Unit of Work
- DTOs for separation of concerns
- Pagination for large data sets
- Fluent API configurations (EF Core)
- JWT Authentication and Authorization
- Role-based access control
- AutoMapper for mapping entities and DTOs
- Validation using Filters

## Tech Stack

- .NET 8 Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Bearer Authentication
- FluentValidation

## Project Structure

- `Domain` – Entities and core logic
- `Application` – DTOs, services, contracts
- `Infrastructure` – Data access layer
- `Presentation` – Controllers, filters, authentication
- `Shared` – Common helpers, response formats

## Setup

1. Clone the repo  
2. Update `appsettings.json` with your DB and JWT settings  
3. Apply migrations

```bash
dotnet ef migrations add Init
dotnet ef database update
