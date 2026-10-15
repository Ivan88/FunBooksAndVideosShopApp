# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

# Project

This is a .NET 10 app using minimal API

# Commands

- Build: `dotnet build FunBooksAndVideosShopApp.sln`
- Run: `dotnet run --project ShopWebApp`
- Test all: `dotnet test ShopTests`
- Test single: `dotnet test ShopTests --filter "FullyQualifiedName~<TestName>"`
- Lint: none configured (no .editorconfig or analyzers). `dotnet format` is available if needed.

# Code Style
- Use 4-space indentation (existing .cs files use tabs; follow the file you're editing unless asked to reformat)
- `Nullable` is disabled in every project

# Architecture

Layered solution. Dependencies point toward `ShopModels`; `ShopWebApp` is the composition root.

- `ShopModels`: records and domain exceptions. `Product` is an abstract record with `[JsonPolymorphic]` (discriminator `type`: `book`/`video`/`membership`), so new product types need a `[JsonDerivedType]` entry.
- `ShopReposInterfaces` / `ShopRepos`: in-memory repos simulating DB tables. They are registered as singletons so state persists across requests.
- `ShopDomainInterfaces` / `ShopDomain`: services (`OrderService`, `ProductService`) and `IBusinessRule` implementations in `Rules/`.
- `ShopWebApp/Program.cs`: DI registration, all endpoints, and the global exception handler.
- `ShopTests`: xUnit + Moq.

Folder names differ from namespaces: `ShopDomain` uses `ShopServices`, and `ShopDomainInterfaces` uses `DomainInterfaces`.

Key flows:
- `OrderService.ProcessOrder` marks the order processed, then runs every registered `IBusinessRule`. To add a rule, implement `IBusinessRule` and register it in `Program.cs`.
- Domain exceptions map to HTTP status codes in the `UseExceptionHandler` switch in `Program.cs` (404 `EntityNotFoundException`, 409 for the order exceptions, 400 for bad requests and System.Text.Json errors). New exceptions need a case there.
