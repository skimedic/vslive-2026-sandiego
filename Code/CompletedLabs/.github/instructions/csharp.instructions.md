---
description: 'Guidelines for building C# applications'
applyTo: '**/*.cs'
---

# C# Development

## C# Instructions
- Always use the latest version C#, currently C# 14 features.
- Utilize modern language features (e.g., records, pattern matching) to write concise and robust code.

## Naming Conventions
- Follow PascalCase for component names, method names, and public members.
- Use camelCase for local variables.
- Use _camelCase for private fields.
- Prefix interface names with "I" (e.g., IUserService).

## Project and File Structure
- The projects do not use nullable reference types. 
- One file per class, and the file name should match the class name.
- Every project name must start with the product name, and then the project type (e.g., DocumentManagement.API).
- Every project must have a GlobalUsings.cs file that includes common usings
- Don't include using statements in new files if they are already in the GlobalUsings.cs file.
- When adding a new class, always add it to the appropriate namespace and include the namespace in the GlobalUsings.cs file.
- When adding new entries into the GlobalUsings.cs file, always sort them alphabetically, and don't remove any existing entries unless they are no longer used in the project.

## Formatting
- Apply code-formatting style defined in `.editorconfig`.
- Always use file scoped namespaces. 
- Always use single-line using directives.
- Always combine attributes on a single line when possible. 
- Use expression bodied members when possible. 
- Single line if statements must still use braces. 
- Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Use ternary operators when appropriate. 
- Use internal over private. 
- All classes and methods are public unless told otherwise. 
- Don't add a constructor unless instructed to do so and use primary constructors when possible.
- Don't declare a class level variable if the parameter from the primary constructor can be used.
- Don't initialize properties unless instructed to do so.

## Configuration & Settings

- Use strongly-typed configuration classes with data annotations
- Implement validation attributes (Required, NotEmptyOrWhitespace)
- Use IConfiguration binding for settings
- Support appsettings.json configuration files

## Testing

- Always include test cases for critical paths of the application.
- Guide users through creating unit tests.
- Do not emit "Act", "Arrange" or "Assert" comments.

## Async Operations
- Asynchronous Programming: Use async and await for I/O-bound operations to ensure scalability.
- **ConfigureAwait(false)**: Always use `ConfigureAwait(false)` in library code to avoid deadlocks:
  ```csharp
  var result = await SomeAsyncMethod().ConfigureAwait(false);
  ```

Your goal is to help me follow best practices for asynchronous programming in C#.

### Naming Conventions

- Use the 'Async' suffix for all async methods
- Match method names with their synchronous counterparts when applicable (e.g., `GetDataAsync()` for `GetData()`)

### Return Types

- Return `Task<T>` when the method returns a value
- Return `Task` when the method doesn't return a value
- Consider `ValueTask<T>` for high-performance scenarios to reduce allocations
- Avoid returning `void` for async methods except for event handlers

### Exception Handling

- Use try/catch blocks around await expressions
- Avoid swallowing exceptions in async methods
- Propagate exceptions with `Task.FromException()` instead of throwing in async Task returning methods

### Performance

- Use `Task.WhenAll()` for parallel execution of multiple tasks
- Use `Task.WhenAny()` for implementing timeouts or taking the first completed task
- Avoid unnecessary async/await when simply passing through task results
- Consider cancellation tokens for long-running operations

### Common Pitfalls

- Never use `.Wait()`, `.Result`, or `.GetAwaiter().GetResult()` in async code
- Avoid mixing blocking and async code
- Don't create async void methods (except for event handlers)
- Always await Task-returning methods

When reviewing my C# code, identify these issues and suggest improvements that follow these best practices.

## String Operations
- **StringBuilder for concatenation**: Use `StringBuilder` for multiple string concatenations
- **StringComparison**: Always specify `StringComparison` for string operations:
  ```csharp
  string.Equals(other, StringComparison.OrdinalIgnoreCase)
  ```
## Monetary Values
- Use decimal type for all monetary calculations.
- Implement currency-aware value objects.
- Handle rounding according to financial standards.
- Maintain precision throughout calculation chains.

## SOLID Principles
- Single Responsibility Principle (SRP): A class should have only one reason to change.
- Open/Closed Principle (OCP): Software entities should be open for extension but closed for modification.
- Liskov Substitution Principle (LSP): Subtypes must be substitutable for their base types.
- Interface Segregation Principle (ISP): No client should be forced to depend on methods it does not use.
- Dependency Inversion Principle (DIP): Depend on abstractions, not on concretions.

## Exception Handling
- Specific exceptions: Catch specific exception types
- If exception falls through to generic Exception, ensure it is logged and re-thrown as a custom exception appropriately
- Don't swallow exceptions: Always log or re-throw exceptions appropriately
- Use using for disposable resources: Ensures proper cleanup even when exceptions occur

## Performance Considerations
- **Avoid boxing**: Be aware of boxing/unboxing with value types and generics
- **String interning**: Use `string.Intern()` judiciously for frequently used strings
- **Lazy initialization**: Use `Lazy<T>` for expensive object creation
- **Avoid reflection in hot paths**: Cache `MethodInfo`, `PropertyInfo` objects when possible

## Entity Framework Guidance:
- Use MaxLength instead of StringLength for maximum length attributes. 
- Use Attributes when possible instead of Fluent API for configuration.
- Always configure relationships using the Fluent API in the DbContext class.
- When creating collection navigation properties, always initialize them to new instances of the collection as an List<T>. 
- Use ICollection<T> for collection navigation properties.
- Name navigation properties using plural names for collections and singular names with "Navigation" as the suffix for single instances.
- Always use the nameof() function instead of string literals when specifying foreign key names and InverseProperties in attributes.
- Always add the ForeignKey atribute for intance navigation properties unless otherwise specified.
- Always add the InverseProperty attribute for all navigation properties.
- All navigation properties should have get and set, even when initialized.

### Data Context Design

- Keep DbContext classes focused and cohesive
- Use attributes where possible for simple configurations
- Also use Fluent API where attributes don't suuffice. Never depend on EF Core conventions for relationships and constraints
- Use constructor injection for configuration options
- Override OnModelCreating for fluent API configuration
- Separate entity configurations using IEntityTypeConfiguration
- Consider using DbContextFactory pattern for console apps or tests

### Entity Design

- Use meaningful primary keys (consider natural vs surrogate keys)
- Implement proper relationships (one-to-one, one-to-many, many-to-many)
- Use data annotations or fluent API for constraints and validations
- Implement appropriate navigational properties
- Consider using owned entity types for value objects

### Performance

- Use AsNoTracking() for read-only queries
- Implement pagination for large result sets with Skip() and Take()
- Use Include() to eager load related entities when needed
- Consider projection (Select) to retrieve only required fields
- Use compiled queries for frequently executed queries
- Avoid N+1 query problems by properly including related data

### Migrations

- Create small, focused migrations
- Name migrations descriptively
- Verify migration SQL scripts before applying to production
- Consider using migration bundles for deployment
- Add data seeding through migrations when appropriate

### Querying

- Use IQueryable judiciously and understand when queries execute
- Prefer strongly-typed LINQ queries over raw SQL
- Use appropriate query operators (Where, OrderBy, GroupBy)
- Consider database functions for complex operations
- Implement specifications pattern for reusable queries

### Change Tracking & Saving

- Use appropriate change tracking strategies
- Batch your SaveChanges() calls
- Implement concurrency control for multi-user scenarios
- Consider using transactions for multiple operations
- Use appropriate DbContext lifetimes (scoped for web apps)

### Security

- Avoid SQL injection by using parameterized queries
- Implement appropriate data access permissions
- Be careful with raw SQL queries
- Consider data encryption for sensitive information
- Use migrations to manage database user permissions

### Testing

- Use in-memory database provider for unit tests
- Create separate testing contexts with SQLite for integration tests
- Mock DbContext and DbSet for pure unit tests
- Test migrations in isolated environments
- Consider snapshot testing for model changes

When reviewing my EF Core code, identify issues and suggest improvements that follow these best practices.
