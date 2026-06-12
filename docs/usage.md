# Usage

## Create a Project

```powershell
dotnet run --project src\ChuA.PlatformCli\ChuA.PlatformCli.csproj -- new --name DemoService --output .\artifacts --template api
```

Options:

| Option | Required | Description |
| --- | --- | --- |
| `--name` | Yes | Project/repository name to generate. |
| `--output` | No | Parent output folder. Defaults to the current directory. |
| `--template` | No | Template name. Currently supports `api`. |
| `--force` | No | Allows generation into an existing folder. |

## Validate a Project

```powershell
dotnet run --project src\ChuA.PlatformCli\ChuA.PlatformCli.csproj -- validate --path .\artifacts\DemoService
```

Validation checks for the baseline files and folders expected in a public-safe repository.

## Version

```powershell
dotnet run --project src\ChuA.PlatformCli\ChuA.PlatformCli.csproj -- version
```
