# E-Commerce API

# A RESTful E-Commerce API built with .NET 8, using Clean Architecture.

# 

# Features

# Clean Architecture structure (Presentation, Application, Domain, Infrastructure)

# 

# Repository and Unit of Work patterns

# 

# DTOs for input/output separation

# 

# JWT Authentication and role-based Authorization

# 

# AutoMapper integration

# 

# Validation using Action Filters

# 

# Pagination for API responses

# 

# Fluent API for entity configuration (EF Core)

# 

# Tech Stack

# ASP.NET Core Web API

# 

# Entity Framework Core

# 

# SQL Server

# 

# AutoMapper

# 

# FluentValidation

# 

# JWT Bearer Authentication

# 

# Project Structure

# Domain – Business entities and enums

# 

# Application – DTOs, Interfaces, Services, Use Cases

# 

# Infrastructure – EF Core, DB Context, Repositories

# 

# Presentation – API Controllers, Filters, Middlewares

# 

# Authentication

# Login/Register using JWT tokens

# 

# Tokens include roles and claims

# 

# Protected endpoints using \[Authorize] and role-based access

# 

# Pagination

# Applied on GET endpoints

# 

# Accepts query parameters:

# 

# pageNumber

# 

# pageSize

# 

# Example

# bash

# Copy

# Edit

# GET /api/products?pageNumber=2\&pageSize=10

# Validation

# Centralized validation using Action Filters

# 

# Custom response format for validation errors

# 

# Clean controller logic

# 

# Configuration

# Update appsettings.json with your DB and JWT settings.

# 

# Run migrations:

# 

# bash

# Copy

# Edit

# dotnet ef migrations add Init

# dotnet ef database update

# Run the app:

# 

# bash

# Copy

# Edit

# dotnet run

# Usage

# Register

# http

# Copy

# Edit

# POST /api/auth/register

# Login

# http

# Copy

# Edit

# POST /api/auth/login

# Get Products (Paginated)

# http

# Copy

# Edit

# GET /api/products?pageNumber=1\&pageSize=10

# Protected Route

# http

# Copy

# Edit

# GET /api/orders

# Authorization: Bearer {your\_token}

# Tools

# Swagger UI for API testing

# 

# Global error handling middleware

# 

# Role seeding for admin/user

# 

# Contributing

# Pull requests are welcome. For major changes, open an issue first.

