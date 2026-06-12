// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.PlatformCli.Generation;

namespace ChuA.PlatformCli.Validation;

public sealed class ProjectValidator
{
    private readonly IFileSystem _fileSystem;

    public ProjectValidator(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public ProjectValidationResult Validate(string path)
    {
        var issues = new List<string>();

        if (!_fileSystem.DirectoryExists(path))
        {
            return new ProjectValidationResult(false, [$"Path does not exist: {path}"]);
        }

        RequireFile(path, "README.md", issues);
        RequireFile(path, "LICENSE.txt", issues);
        RequireFile(path, ".gitignore", issues);
        RequireDirectory(path, "src", issues);
        RequireDirectory(path, "tests", issues);
        RequireDirectory(path, "docs", issues);
        RequireFile(path, Path.Combine(".github", "workflows", "ci.yml"), issues);

        return new ProjectValidationResult(issues.Count == 0, issues);
    }

    private void RequireFile(string root, string relativePath, ICollection<string> issues)
    {
        if (!_fileSystem.FileExists(Path.Combine(root, relativePath)))
        {
            issues.Add($"Missing file: {relativePath}");
        }
    }

    private void RequireDirectory(string root, string relativePath, ICollection<string> issues)
    {
        if (!_fileSystem.DirectoryExists(Path.Combine(root, relativePath)))
        {
            issues.Add($"Missing directory: {relativePath}");
        }
    }
}
