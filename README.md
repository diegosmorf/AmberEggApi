# AmberEggApi Project - .NET9 Clean Architecture API Template

[![OpenSSF Best Practices](https://www.bestpractices.dev/projects/9250/badge)](https://www.bestpractices.dev/projects/9250)
[![AmberEggApi Build Test](https://github.com/diegosmorf/AmberEggApi/actions/workflows/pipeline-build-main.yml/badge.svg)](https://github.com/diegosmorf/AmberEggApi/actions/workflows/pipeline-build-main.yml)
[![SonarCloud - Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=diegosmorf_AmberEggApi&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=diegosmorf_AmberEggApi)

AmberEggApi Project is an open-source project (community asset) written in .NET, which target accelerate API development using strong Enterprise patterns.

## Summary

This project covers concepts about:  

- [Clean Architecture](https://8thlight.com/blog/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Hexagonal Architecture](http://alistair.cockburn.us/Hexagonal+architecture)
- [Onion architecture](http://jeffreypalermo.com/blog/the-onion-architecture-part-1/)
- [CQRS](http://www.codeproject.com/Articles/555855/Introduction-to-CQRS)
- [Dependency Injection](http://en.wikipedia.org/wiki/Dependency_injection)
- [Loose Coupling](http://en.wikipedia.org/wiki/Loose_coupling)
- [SOLID Principles](http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29)
- [Ports-and-adapters](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html)

## .Net Version

- [.NET 9.0](https://dotnet.microsoft.com/en-us/download)

## NuGet Packages Dependencies

- Autofac
- AutoMapper
- FluentAssertions
- XUnit
- Swagger

## Repository

- Entity Framework + InMemory + Data Migration

## Development Tools

- Visual Studio Code
- GIT Bash
- GitHub(Repos, Actions)
- MSSQL Server Management Studio
- Swagger Editor  

## How to clone this project

```Powershell
cd\
mkdir repo
cd repo
git clone https://github.com/diegosmorf/AmberEggApi.git
```

### Rename the structure using your namespace

```Powershell
powershell .\clone-to-your-namespace.ps1 -newNS "Your.Namespace"
cd ..\Your.Namespace
```

### Restore, Build and Test

```Powershell
dotnet restore
dotnet build
dotnet test
dotnet run --p AmberEggApi.WebApi

```

you can access this API via browser: <http://localhost:5200/swagger>

## 0 - Core

AmberEggApi.Contracts is a basic set of interfaces for building a command and event driven CQRS application.

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

This project will expose domain features to external world (e.g.: API, Apps, Windows Services, Desktop apps) and it is responsible for business rules as well.

## 3 - Infrastructure

This project contains implementations of the interfaces defined in the inner layers of the solution. They may be dependent on external libraries or resources. Note that the implementations themselves are internal and should only be used for injection via their implemented interfaces.


In the project AmberEggApi.Database, we have migrations for the Entity Framework Core database context (generated automatically by ef cli). 

Installing the EF Core CLI tools:
```bash
dotnet tool install --global dotnet-ef
```

To create a new migration, use the following command in the terminal:
```bash
dotnet ef migrations add <MigrationName>
```

To deploy on local database:
```bash
dotnet ef migrations update
```

## 4 - Entry Points

API project exposing business controllers.

## 5 - Tests

DomainTest: NUnit will test ApplicationService classes with no external dependencies. All Infrastructure dependencies must be mocked.

IntegrationTests:Longer running, more involved tests that test the integration of multiple components and external dependencies as Database/Email.

## You shouldn't find

- Binaries committed to source control.
- Unnecessary project or library references or third party frameworks.
- Many "try" blocks - code defensively and throw exceptions if something is wrong.
- Third party APIs exposed via public interfaces.
