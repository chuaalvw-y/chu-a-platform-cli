// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.PlatformCli;
using ChuA.PlatformCli.Generation;
using ChuA.PlatformCli.Validation;

var tests = new TestRunner();

tests.Run("Parser captures command and options", () =>
{
    var parsed = CommandLineParser.Parse(["new", "--name", "Demo", "--template", "api", "--force"]);

    Assert.Equal("new", parsed.Command, "Command should parse.");
    Assert.Equal("Demo", parsed.GetOption("name"), "Name option should parse.");
    Assert.Equal("api", parsed.GetOption("template"), "Template option should parse.");
    Assert.True(parsed.HasFlag("force"), "Force flag should parse.");
});

tests.Run("Generator creates expected baseline files", () =>
{
    using var workspace = TempWorkspace.Create();
    var generator = new ProjectGenerator(new RealFileSystem());

    var result = generator.Generate(new GenerateProjectRequest("demo-service", workspace.Path, "api", false));
    var root = System.IO.Path.Combine(workspace.Path, "demo-service");

    Assert.True(result.Succeeded, "Generation should succeed.");
    Assert.FileExists(System.IO.Path.Combine(root, "README.md"));
    Assert.FileExists(System.IO.Path.Combine(root, "LICENSE.txt"));
    Assert.FileExists(System.IO.Path.Combine(root, ".github", "workflows", "ci.yml"));
    Assert.DirectoryExists(System.IO.Path.Combine(root, "src", "DemoService"));
});

tests.Run("Validator reports missing files", () =>
{
    using var workspace = TempWorkspace.Create();
    Directory.CreateDirectory(System.IO.Path.Combine(workspace.Path, "empty"));

    var validator = new ProjectValidator(new RealFileSystem());
    var result = validator.Validate(System.IO.Path.Combine(workspace.Path, "empty"));

    Assert.False(result.Succeeded, "Empty project should fail validation.");
    Assert.True(result.Issues.Any(issue => issue.Contains("README.md", StringComparison.Ordinal)), "Missing README should be reported.");
});

tests.Run("CLI version command writes version", async () =>
{
    using var output = new StringWriter();
    using var error = new StringWriter();

    var exitCode = await CliApp.RunAsync(["version"], output, error, new RealFileSystem());

    Assert.Equal(0, exitCode, "Version command should succeed.");
    Assert.True(output.ToString().Contains(CliApp.Version, StringComparison.Ordinal), "Version output should include version.");
});

tests.Complete();

internal sealed class TempWorkspace : IDisposable
{
    private TempWorkspace(string path)
    {
        Path = path;
        Directory.CreateDirectory(Path);
    }

    public string Path { get; }

    public static TempWorkspace Create()
    {
        return new TempWorkspace(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "chu-a-platform-cli-tests", Guid.NewGuid().ToString("N")));
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, recursive: true);
        }
    }
}

internal sealed class TestRunner
{
    private int _count;

    public void Run(string name, Action test)
    {
        test();
        _count++;
        Console.WriteLine($"PASS: {name}");
    }

    public void Run(string name, Func<Task> test)
    {
        test().GetAwaiter().GetResult();
        _count++;
        Console.WriteLine($"PASS: {name}");
    }

    public void Complete()
    {
        Console.WriteLine($"{_count} tests passed.");
    }
}

internal static class Assert
{
    public static void True(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void False(bool condition, string message)
    {
        True(!condition, message);
    }

    public static void Equal<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException($"{message} Expected '{expected}', got '{actual}'.");
        }
    }

    public static void FileExists(string path)
    {
        True(File.Exists(path), $"Expected file to exist: {path}");
    }

    public static void DirectoryExists(string path)
    {
        True(Directory.Exists(path), $"Expected directory to exist: {path}");
    }
}
