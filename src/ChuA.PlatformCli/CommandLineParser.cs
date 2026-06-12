// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.PlatformCli;

public static class CommandLineParser
{
    public static ParsedCommand Parse(IReadOnlyList<string> args)
    {
        if (args.Count == 0)
        {
            return new ParsedCommand("help", new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase));
        }

        var command = args[0].Trim().ToLowerInvariant();
        var options = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        for (var index = 1; index < args.Count; index++)
        {
            var token = args[index];
            if (!token.StartsWith("--", StringComparison.Ordinal))
            {
                continue;
            }

            var name = token[2..];
            var nextIndex = index + 1;
            if (nextIndex < args.Count && !args[nextIndex].StartsWith("--", StringComparison.Ordinal))
            {
                options[name] = args[nextIndex];
                index = nextIndex;
            }
            else
            {
                options[name] = null;
            }
        }

        return new ParsedCommand(command, options);
    }
}
