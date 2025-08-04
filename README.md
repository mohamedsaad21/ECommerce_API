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
- Caching using in-memory strategies  
- Stripe Payment Integration  
- Eager Loading for related entities  
- No Tracking queries to improve read performance  

## Tech Stack

- .NET 8 Web API  
- Entity Framework Core  
- SQL Server  
- AutoMapper  
- JWT Bearer Authentication  
- FluentValidation  
- Stripe.Net  

## Project Structure

- `Domain` – Entities and core logic  
- `Application` – DTOs, services, contracts  
- `Infrastructure` – Data access layer  
- `Presentation` – Controllers, filters, authentication  
- `Shared` – Common helpers, response formats  

## Setup

1. Clone the repo

   ```bash
   git clone https://github.com/your-username/ECommerce_API.git
   cd ECommerce_API
