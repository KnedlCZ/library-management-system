# library-management-system

ASP.NET Core MVC sample project for a small library management system using EF Core, PostgreSQL and dependency injection.

## Requirements

- .NET SDK 10
- PostgreSQL running locally
- EF Core CLI tools

Install the EF Core CLI tools if you do not already have them:

```powershell
dotnet tool install --global dotnet-ef
```

## PostgreSQL setup

Create a PostgreSQL database named `library_management`.

The connection string is configured in `LibraryManagementSystem/appsettings.json`:

```json
"DefaultConnection": "Host=localhost;Port=5432;Database=library_management;Username=postgres;Password=postgres"
```

Update the username or password there if your local PostgreSQL credentials are different.

## Run migrations

From the project folder:

```powershell
cd LibraryManagementSystem
dotnet ef database update
```

The application also runs pending migrations automatically on startup.

To create a new migration after changing the EF Core models:

```powershell
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Start the project

From the project folder:

```powershell
cd LibraryManagementSystem
dotnet restore
dotnet run
```

Open the URL shown in the terminal, usually `https://localhost:5001` or `http://localhost:5000`.
