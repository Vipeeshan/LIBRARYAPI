# LibraryAPI

## Description
LibraryAPI is a RESTful Web API built with ASP.NET Core 9, designed to manage a library system including books, members, loans, and categories. It features JWT-based authentication and role-based authorization for librarians.

## Features
- JWT Authentication with email verification
- CRUD operations for Books, Members, Loans, and Categories
- Role-based authorization (e.g., Librarian roles)
- EF Core with SQLite database
- Unit tests with xUnit and InMemory database
- Swagger API documentation
- Docker support for containerized deployment

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQLite (optional, for local database)
- Docker (optional, for containerized deployment)
- Visual Studio 2022 or VS Code

## Getting Started

### Clone the repository
```bash
git clone https://github.com/Vipeeshan/LIBRARYAPI.git
cd LIBRARYAPI
