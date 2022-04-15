# AmberEggApi - .NET 6 Open API Template
This is a starting point (TEMPLATE) for Clean Architecture with .NET 6. [Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html) is just the latest in a series of names for the same loosely-coupled, dependency-inverted architecture. You will also find it named [hexagonal](http://alistair.cockburn.us/Hexagonal+architecture), [ports-and-adapters](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html), or [onion architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/).

## Summary
This project AmberEggApi cover concepts about:  
 - [CQRS](http://www.codeproject.com/Articles/555855/Introduction-to-CQRS)
 - [Dependency Injection](http://en.wikipedia.org/wiki/Dependency_injection)
 - [Loose Coupling](http://en.wikipedia.org/wiki/Loose_coupling)
 - [SOLID Principles](http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29)
 
## .Net Version
.NET 6.0

## 3rd Party Nuget Packages 
- Autofac
- AutoMapper
- FluentAssertiong
- NUnit
- Swagger (Swashbuckle) 

## Repository
- Entity Framework + InMmemory
- Entity Framework Data Migration
 
## Development Tools
 - Visual Studio Code
 - GIT Bash
 - GitHub(Repos, Actions)
 - MSSQL Server Management Studio 18 
 - Swagger Editor  

## How to clone this project

```
cd\
mkdir repo
cd repo
git clone https://github.com/diegosmorf/AmberEggApi.git
```

### Cloning the strucuture using your namespace
```
powershell .\clone-to-your-namespace.ps1 "Your.Namespace"
cd ..\Your.Namespace
```

### Restore, Build and Test
```
dotnet restore
dotnet build
dotnet test
dotnet run --p AmberEggApi.WebApi

```

you can access this API via browser: http://localhost:5200/swagger

## 0 - Core
Api.Common.Cqrs.Core is a basic set of interfaces for building a command and event driven CQRS application. 

- Commands are created and dispatched by the application, 
- They are received by command handlers which apply behaviors on the domain model
- Which generates events 
- Collected by the command handler
- Then published
- Received by event handlers which update the read/query model 
- Consumed by the front end of the application via query services.

## 1 - Domain
Domain commands and handlers specifically affect the domain model's aggregate roots. 

Some of the basic premises of CQRS are modeled by these interfaces either explicitly or in their documentation.

- Commands and events should be immutable
- The query model should be immutable, except from the event handlers responsible for updating them when triggered by events published from domain model changes
- Domain commands and handlers should only affect a single aggregate root instance in the domain model - more complex operations should be handled by sagas

## 2 - Application Service
This project will expose domain features to external world (e.g.: apis, apps, windows services, desktop apps) and it is responsible for business rules as well.

## 3 - Infrastructure

This project contains implementations of the interfaces defined in the inner layers of the solution. They may be dependent on external libraries or resources. Note that the implementations themselves are internal and should only be used for injection via their implemented interfaces. 

## 4 - Entry Points 

API project.

## 5 - Tests

DomainTest: NUnit will test ApplicationService classes with no external dependencies. All Infrastructure dependencies must be mocked. 

IntegrationTests:Longer running, more involved tests that test the integration of multiple components and external dependencies as Database/Email.

AcceptanceTests: SpecFlow acceptance tests project (may modify data, so are meant to run in non-production environments)

## You shouldn't find:

  - Binaries committed to source control.
  - Unnecessary project or library references or third party frameworks.
  - Many "try" blocks - code defensively and throw exceptions if something is wrong.
  - Third party APIs exposed via public interfaces.
