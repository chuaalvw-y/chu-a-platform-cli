// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.PlatformCli.Generation;

public static class Templates
{
    public static string Readme(string repositoryName, string apiProjectName)
    {
        return $$"""
        # {{repositoryName}}

        Public-safe generated API starter.

        ## Run

        ```powershell
        dotnet run --project src\{{apiProjectName}}\{{apiProjectName}}.csproj
        ```

        ## Endpoints

        - `GET /health`
        - `GET /api/sample`

        ## License

        This repository is proprietary and source-available only when shared by the copyright holder. See `LICENSE.txt`.
        """;
    }

    public static string License()
    {
        return """
        Copyright (c) 2026 Alvin Wilsen Chan Chua
        GitHub: chuaalvw-y

        All rights reserved.

        Permission is granted to use this software for personal, educational, or internal evaluation purposes only.

        You may not modify, adapt, reverse engineer, create derivative works from, sell, sublicense, redistribute, publish, or include this software in any commercial product or service without prior written permission from the copyright holder.

        You may not remove or alter any copyright, license, or attribution notices.

        This software is provided "as is", without warranty of any kind, express or implied. The copyright holder is not liable for any claims, damages, or other liability arising from the use of this software.

        Any rights not expressly granted in this license are reserved by the copyright holder.
        """;
    }

    public static string GitIgnore()
    {
        return """
        bin/
        obj/
        .vs/
        .vscode/
        TestResults/
        artifacts/
        *.user
        *.suo
        """;
    }

    public static string Architecture(string repositoryName)
    {
        return $$"""
        # Architecture

        `{{repositoryName}}` is a generated public-safe API starter.

        The generated structure is intentionally small:

        - `src/` contains runtime source.
        - `tests/` is reserved for automated checks.
        - `docs/` contains architecture notes.
        - `.github/workflows/` contains CI.

        Expand the project with application, domain, infrastructure, and tests as the system grows.
        """;
    }

    public static string Ci(string apiProjectName)
    {
        return $$"""
        name: CI

        on:
          push:
            branches: [ main ]
          pull_request:
            branches: [ main ]

        jobs:
          build:
            runs-on: ubuntu-latest
            steps:
              - uses: actions/checkout@v4
              - uses: actions/setup-dotnet@v4
                with:
                  dotnet-version: '10.0.x'
              - run: dotnet build src/{{apiProjectName}}/{{apiProjectName}}.csproj --configuration Release
        """;
    }

    public static string ApiProject()
    {
        return """
        <Project Sdk="Microsoft.NET.Sdk.Web">
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <ImplicitUsings>enable</ImplicitUsings>
            <Nullable>enable</Nullable>
          </PropertyGroup>
        </Project>
        """;
    }

    public static string Program(string apiProjectName)
    {
        return $$"""
        // Copyright (c) 2026 Alvin Wilsen Chan Chua
        // GitHub: chuaalvw-y
        // Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
        // See LICENSE.txt in the project root for full license information.

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();

        var app = builder.Build();

        app.MapHealthChecks("/health");
        app.MapGet("/api/sample", () => Results.Ok(new
        {
            service = "{{apiProjectName}}",
            status = "ready"
        }));

        app.Run();
        """;
    }

    public static string AppSettings()
    {
        return """
        {
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.AspNetCore": "Warning"
            }
          },
          "AllowedHosts": "*"
        }
        """;
    }
}
