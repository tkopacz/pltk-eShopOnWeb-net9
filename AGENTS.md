# Repository Guidelines

## Project Structure & Module Organization
The solution is rooted in `eShopOnWeb.sln`. Domain logic and entities live in `src/ApplicationCore`, while infrastructure concerns (EF Core contexts, repositories, integrations) sit in `src/Infrastructure`. The default MVC front end is `src/Web`; API endpoints run from `src/PublicApi`; Blazor admin tooling resides in `src/BlazorAdmin` with shared contracts in `src/BlazorShared`. Aspire hosting assets are under `src/eShopWeb.AppHost` and `src/eShopWeb.AspireServiceDefaults`. Supporting docs and deployment scripts are in `docs/` and `infra/`. Automated tests are organized by scope inside `tests/FunctionalTests`, `tests/IntegrationTests`, `tests/PublicApiIntegrationTests`, and `tests/UnitTests`.

## Build, Test, and Development Commands
Restore dependencies once per environment: `dotnet restore`. Build everything with `dotnet build eShopOnWeb.sln`. Run the primary web app locally from `src/Web`: `dotnet run --launch-profile https`. Start the public API when exercising Blazor admin: `dotnet run --project src/PublicApi/PublicApi.csproj`. Execute all tests with `dotnet test eShopOnWeb.sln --settings CodeCoverage.runsettings`. For focused runs, target a single project (e.g., `dotnet test tests/UnitTests/UnitTests.csproj`).

## Coding Style & Naming Conventions
The repo enforces .NET defaults through `.editorconfig`: four-space indentation, UTF-8 files, trailing newline. Use PascalCase for classes, interfaces, records, and public members; camelCase for locals and private fields (prefix `_` for private fields when guided by the project’s patterns). Favor explicit async suffixes (`Async`) and be mindful of nullability annotations. Before pushing, run `dotnet format --verify-no-changes` to ensure code style compliance.

## Testing Guidelines
Tests use xUnit. Mirror production namespaces and append `Tests` to folders and classes (e.g., `BasketServiceTests`). Keep Arrange-Act-Assert sections clear and minimize shared mutable test state. When adding integration coverage, prefer existing fixtures in the respective `tests/*IntegrationTests` project. Use `dotnet test --filter FullyQualifiedName~Scenario` to iterate quickly, and include new coverage in CI-relevant projects.

## Commit & Pull Request Guidelines
Write imperative, concise commit messages (`Add basket caching guard`). Reference issues when applicable (`Fix login redirect #123`). Pull requests should summarize the change, list impacted areas, call out new migrations or configuration steps, and attach UI screenshots for visible changes. Confirm `dotnet build` and suite-wide `dotnet test` succeed before requesting review, and note any skipped checks or follow-up items explicitly.

## Environment & Configuration Notes
Local development defaults to SQL Server; toggle in-memory storage by setting `"UseOnlyInMemoryDatabase": true` in `src/Web/appsettings.json`. Secrets belong in user secrets or the Azure Key Vault workflow described in `README.md`. Keep connection strings and API keys out of source-controlled JSON files.
