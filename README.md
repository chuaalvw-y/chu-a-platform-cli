# ChuA Platform CLI

Public-safe scaffolding CLI that demonstrates how I think about repeatable project setup and developer experience.

The CLI creates a generic enterprise-style repository skeleton with documentation, license, source/test folders, a minimal API project, and a CI workflow. It is intentionally simple and dependency-light so reviewers can inspect the generation logic quickly.

This repository does not contain private templates, internal URLs, credentials, customer data, or proprietary implementation details.

## Commands

```powershell
dotnet run --project src\ChuA.PlatformCli\ChuA.PlatformCli.csproj -- version
dotnet run --project src\ChuA.PlatformCli\ChuA.PlatformCli.csproj -- new --name DemoService --output .\artifacts --template api
dotnet run --project src\ChuA.PlatformCli\ChuA.PlatformCli.csproj -- validate --path .\artifacts\DemoService
```

## What It Generates

- `README.md`
- `LICENSE.txt`
- `.gitignore`
- `src/`
- `tests/`
- `docs/`
- `.github/workflows/ci.yml`
- A minimal public-safe API project

## Repository Structure

```text
src/
  ChuA.PlatformCli/
tests/
  ChuA.PlatformCli.Tests/
docs/
  usage.md
  generated-template.md
```

## Test

```powershell
dotnet run --project tests\ChuA.PlatformCli.Tests\ChuA.PlatformCli.Tests.csproj
```

The tests use a small console test harness to avoid third-party test dependencies.

## Roadmap

- Add more templates
- Add dry-run output
- Add richer validation rules
- Add optional GitHub Actions variants

## License

This repository is proprietary and source-available only when shared by the copyright holder. See [LICENSE.txt](LICENSE.txt) for full license information.
