// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using System.IO;
using ChuA.PlatformCli.Generation;
using ChuA.PlatformCli.Validation;

namespace ChuA.PlatformCli;

public static class CliApp
{
    public const string Version = "1.0.0";

    public static async Task<int> RunAsync(string[] args, TextWriter output, TextWriter error, IFileSystem fileSystem)
    {
        var parsed = CommandLineParser.Parse(args);

        return parsed.Command switch
        {
            "new" => await RunNewAsync(parsed, output, error, fileSystem),
            "validate" => RunValidate(parsed, output, error, fileSystem),
            "version" => WriteVersion(output),
            "help" or "" => WriteHelp(output),
            _ => WriteUnknownCommand(parsed.Command, error)
        };
    }

    private static async Task<int> RunNewAsync(ParsedCommand parsed, TextWriter output, TextWriter error, IFileSystem fileSystem)
    {
        var name = parsed.GetOption("name");
        if (string.IsNullOrWhiteSpace(name))
        {
            await error.WriteLineAsync("Missing required option: --name");
            return 2;
        }

        var outputPath = parsed.GetOption("output") ?? Directory.GetCurrentDirectory();
        var template = parsed.GetOption("template") ?? "api";
        var force = parsed.HasFlag("force");

        var generator = new ProjectGenerator(fileSystem);
        var result = generator.Generate(new GenerateProjectRequest(name, outputPath, template, force));

        if (!result.Succeeded)
        {
            await error.WriteLineAsync(result.Message);
            return 1;
        }

        await output.WriteLineAsync(result.Message);
        return 0;
    }

    private static int RunValidate(ParsedCommand parsed, TextWriter output, TextWriter error, IFileSystem fileSystem)
    {
        var path = parsed.GetOption("path") ?? Directory.GetCurrentDirectory();
        var validator = new ProjectValidator(fileSystem);
        var result = validator.Validate(path);

        if (result.Succeeded)
        {
            output.WriteLine("Validation passed.");
            return 0;
        }

        error.WriteLine("Validation failed:");
        foreach (var issue in result.Issues)
        {
            error.WriteLine($"- {issue}");
        }

        return 1;
    }

    private static int WriteVersion(TextWriter output)
    {
        output.WriteLine($"ChuA Platform CLI {Version}");
        return 0;
    }

    private static int WriteHelp(TextWriter output)
    {
        output.WriteLine("ChuA Platform CLI");
        output.WriteLine();
        output.WriteLine("Commands:");
        output.WriteLine("  new --name <name> [--output <path>] [--template api] [--force]");
        output.WriteLine("  validate [--path <path>]");
        output.WriteLine("  version");
        return 0;
    }

    private static int WriteUnknownCommand(string command, TextWriter error)
    {
        error.WriteLine($"Unknown command: {command}");
        return 2;
    }
}
