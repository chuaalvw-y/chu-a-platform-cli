// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.PlatformCli;

public sealed record ParsedCommand(string Command, IReadOnlyDictionary<string, string?> Options)
{
    public string? GetOption(string name)
    {
        return Options.TryGetValue(name, out var value) ? value : null;
    }

    public bool HasFlag(string name)
    {
        return Options.ContainsKey(name);
    }
}
