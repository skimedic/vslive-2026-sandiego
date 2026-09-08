---
description: 'Guidelines for Copilot'
applyTo: '**'
---
## General Instructions
- Make only high confidence suggestions when reviewing code changes.
- Write code with good maintainability practices, including comments on why certain design decisions were made.
- Handle edge cases and write clear exception handling.
- For libraries or external dependencies, mention their usage and purpose in comments.

## CRITICAL REMINDERS

**YOU MUST ALWAYS:**

* Show your thinking process before implementing.
* Explicitly validate against these guidelines and all other applicable instructions.
* Use the mandatory verification statements.
* Stop and ask for clarification if any guideline is unclear.

---


## Solution Overview

This is the **AutoLot** hands-on lab solution — a car dealership sample app targeting **.NET 10** (see `global.json`). The solution file is `AutoLot.sln`.

### Key Conventions

- **`AutoLot.Models` holds ALL database entity classes** — including infrastructure types like `SeriLogEntry`. Do not recommend moving any entity out of this project.
- **`SeriLogEntry` intentionally does not inherit `BaseEntity`** — the Serilog framework dictates its schema. This is by design, not an oversight.
- **"ViewModel" means any type derived from one or more tables but not directly persisted** — including types populated by inline SQL, UDFs, or views (e.g., `CarViewModel`, `TemporalViewModel<T>`). These live in `AutoLot.Models/ViewModels/`, not in DAL.
- **`IEntityTypeConfiguration<T>` classes live in `AutoLot.Models` alongside their entities** — the `[EntityTypeConfiguration(typeof(T))]` attribute creates a compile-time reference. Moving configs to DAL would break this attribute (DAL → Models; Models cannot reference DAL).



| Project | Role |
|---|---|
| `AutoLot.Models` | Entity classes, ViewModels, base `BaseEntity` |
| `AutoLot.Dal` | `ApplicationDbContext`, repository pattern, EF migrations |
| `AutoLot.Services` | Logging (`IAppLogging`/Serilog), utilities, API client wrappers |
| `AutoLot.Dal.Tests` | xUnit v3 integration tests against SQL Server |
| `AutoLot.Api` | REST API with versioning (v1–v3), Scalar/OpenAPI docs |
| `AutoLot.Mvc` | MVC web front-end |
| `AutoLot.Web` | Razor Pages web front-end |
| `AutoLot.Blazor` + `AutoLot.Blazor.Models` | Blazor web front-end |

---

## Build and Test Commands

```powershell
# Build entire solution
dotnet build AutoLot.sln

# Run all integration tests
dotnet test AutoLot.Dal.Tests

# Run a single test by name (partial match)
dotnet test AutoLot.Dal.Tests --filter "FullyQualifiedName~CarTests.ShouldGetTheCarsByMake"

# Run a single test class
dotnet test AutoLot.Dal.Tests --filter "FullyQualifiedName~CarTests"
```

Tests require a live SQL Server. The default connection string in `AutoLot.Dal.Tests/appsettings.testing.json` points to `(localdb)\MSSQLLocalDB`, database `AutoLot_Hol`. A Docker option (`server=.,5433`) is also available.

---

## Architecture: Data Access Layer

### Entity Hierarchy (`AutoLot.Models`)

- All entities inherit `BaseEntity` (in `Entities/Base/BaseEntity.cs`), which provides:
  - `int Id` — database-generated identity PK (`[Key, DatabaseGenerated(Identity)]`)
  - `long TimeStamp` — optimistic concurrency token (`[Timestamp]`)
- Entity-specific EF configuration lives in `Entities/Configuration/` as `IEntityTypeConfiguration<T>` classes, referenced by `[EntityTypeConfiguration(typeof(XConfiguration))]` on the entity and called explicitly in `OnModelCreating`.

### Repository Hierarchy (`AutoLot.Dal`)

```
IBaseViewRepo<T>  ←  BaseViewRepo<T>   (read-only: GetAll, ExecuteSqlString)
      ↓                    ↓
IBaseRepo<T>      ←  BaseRepo<T>        (full CRUD + bulk ops)
                         ↓
                  TemporalTableBaseRepo<T>  (temporal table support)
```

- Each entity has a concrete repo (`CarRepo`, `MakeRepo`, etc.) and a matching interface (`ICarRepo`, etc.) in `Repos/Interfaces/`.
- The `persist` parameter on mutating methods (default `true`) controls whether `SaveChanges()` is called immediately — set to `false` to batch multiple operations.
- `SaveChanges()` in `BaseRepo` wraps all exceptions as `CustomException`; `ApplicationDbContext.SaveChanges()` maps EF-specific exceptions to typed custom exceptions (`CustomConcurrencyException`, `CustomDbUpdateException`, `CustomRetryLimitExceededException`).

### DbContext (`AutoLot.Dal/EfStructures/ApplicationDbContext.cs`)

- Registered with `AddDbContextPool<ApplicationDbContext>` (connection pool).
- The `ApplicationDbContextFactory` supports design-time tooling and console/test scenarios.
- Inline database functions (`[DbFunction]`) are declared as static methods on the context (scalar) or instance methods returning `IQueryable` (table-valued).

---

## Architecture: API Layer (`AutoLot.Api`)

- All CRUD controllers inherit `BaseCrudController<TEntity>` (in `Controllers/Base/`).
- Dual route convention: `[Route("api/[controller]")]` + `[Route("api/v{version:apiVersion}/[controller]")]` — every controller handles both un-versioned and versioned URLs.
- API versioning uses **all** reader strategies combined (URL segment, query string `api-version`/`v`, headers `x-ms-api-version`/`x-ms-v`, media type). In real-world code, pick one method.
- OpenAPI docs are served at `/openapi/{groupName}.json` and exposed through **Scalar** (`/scalar/...`) and Swagger UI in Development.
- Current API versions: v1, v1.5 (deprecated), v2, v2.5-Beta, v3-Beta.
- JSON serialization: `PropertyNamingPolicy = null` (PascalCase), `ReferenceHandler.IgnoreCycles`, `WriteIndented = true`.
- Model state validation is suppressed (`SuppressModelStateInvalidFilter = true`); validation is handled manually in controllers.

---

## Key Conventions

### Navigation Properties
- Single-instance navigation properties use the `Navigation` suffix: `MakeNavigation`, `CarNavigation`.
- Collection navigation properties use plural names: `Cars`, `Drivers`, `CarDrivers`.
- Always decorate instance nav props with `[ForeignKey(nameof(FkProperty))]`.
- Always decorate all nav props with `[InverseProperty(nameof(OtherSide.Property))]`.
- All collection nav props are initialized to `new List<T>()`.
- Navigation properties on entities that shouldn't serialize via XML use `[XmlIgnore]`.

### Logging
- Use `IAppLogging` (from `AutoLot.Services`) — not `ILogger<T>` directly.
- `IAppLogging` methods use `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]` to automatically capture call site info; do not pass those parameters explicitly.
- Serilog is configured via `builder.ConfigureSerilog()` extension and writes structured log entries to the `SeriLogEntries` table.

### Integration Tests
- All integration test classes inherit `BaseTest` and use `IClassFixture<EnsureAutoLotDatabaseTestFixture>` to guarantee the database is seeded.
- All tests that modify data must use `ExecuteInATransaction(...)` (rolls back) or `ExecuteInASharedTransaction(...)` to leave the database clean.
- Tests use `[Collection("Integration Tests")]` to prevent parallel execution.
- Use `OutputHelper.WriteLine(query.ToQueryString())` to log the generated SQL during development.

### EF Migrations
- **Never create or apply migrations automatically.** Philip has a manual process; all migration commands are logged in `Migrations.txt` at the solution root.
- Use `dotnet ef` commands from `Migrations.txt` as the reference.

### Connection Strings
- Named `"AutoLot"` in all `appsettings*.json` files.
- Development default: `(localdb)\MSSQLLocalDB`, database `AutoLot_Hol`.
- Docker alternative: `server=.,5433`, database `AutoLot_Hol`, `User Id=sa`.

### Project File Conventions
- `<Nullable>disable</Nullable>` — nullable reference types are disabled across all projects.
- Every project has a `GlobalUsings.cs` — do not add `using` directives to new files for namespaces already listed there. Keep entries sorted alphabetically.
- `[Version range]` syntax is used for NuGet package versions (e.g., `[10.*,11.0)`) to stay within a major version.

