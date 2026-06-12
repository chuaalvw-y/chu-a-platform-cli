// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.PlatformCli.Generation;

public sealed class ProjectGenerator
{
    private readonly IFileSystem _fileSystem;

    public ProjectGenerator(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public GenerationResult Generate(GenerateProjectRequest request)
    {
        if (!string.Equals(request.Template, "api", StringComparison.OrdinalIgnoreCase))
        {
            return GenerationResult.Failure($"Unsupported template: {request.Template}");
        }

        if (!ProjectName.IsValid(request.Name))
        {
            return GenerationResult.Failure("Project name must contain only letters, numbers, dots, underscores, or hyphens.");
        }

        var root = Path.GetFullPath(Path.Combine(request.OutputPath, request.Name));
        if (_fileSystem.DirectoryExists(root) && _fileSystem.DirectoryHasEntries(root) && !request.Force)
        {
            return GenerationResult.Failure($"Output folder already exists and is not empty: {root}");
        }

        var apiProjectName = ProjectName.ToPascalCase(request.Name);

        CreateFolders(root);
        WriteRootFiles(root, request.Name, apiProjectName);
        WriteApiProject(root, apiProjectName);

        return GenerationResult.Success($"Generated '{request.Name}' at {root}");
    }

    private void CreateFolders(string root)
    {
        foreach (var folder in new[]
        {
            root,
            Path.Combine(root, ".github", "workflows"),
            Path.Combine(root, "docs"),
            Path.Combine(root, "src"),
            Path.Combine(root, "tests")
        })
        {
            _fileSystem.CreateDirectory(folder);
        }
    }

    private void WriteRootFiles(string root, string repositoryName, string apiProjectName)
    {
        _fileSystem.WriteAllText(Path.Combine(root, "README.md"), Templates.Readme(repositoryName, apiProjectName));
        _fileSystem.WriteAllText(Path.Combine(root, "LICENSE.txt"), Templates.License());
        _fileSystem.WriteAllText(Path.Combine(root, ".gitignore"), Templates.GitIgnore());
        _fileSystem.WriteAllText(Path.Combine(root, "docs", "architecture.md"), Templates.Architecture(repositoryName));
        _fileSystem.WriteAllText(Path.Combine(root, ".github", "workflows", "ci.yml"), Templates.Ci(apiProjectName));
        _fileSystem.WriteAllText(Path.Combine(root, "tests", ".gitkeep"), string.Empty);
    }

    private void WriteApiProject(string root, string apiProjectName)
    {
        var projectFolder = Path.Combine(root, "src", apiProjectName);
        _fileSystem.CreateDirectory(projectFolder);
        _fileSystem.WriteAllText(Path.Combine(projectFolder, $"{apiProjectName}.csproj"), Templates.ApiProject());
        _fileSystem.WriteAllText(Path.Combine(projectFolder, "Program.cs"), Templates.Program(apiProjectName));
        _fileSystem.WriteAllText(Path.Combine(projectFolder, "appsettings.json"), Templates.AppSettings());
    }
}
