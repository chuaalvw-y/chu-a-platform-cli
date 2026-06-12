// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using System.Text;

namespace ChuA.PlatformCli.Generation;

public static class ProjectName
{
    public static bool IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && value.All(character => char.IsLetterOrDigit(character) || character is '.' or '_' or '-');
    }

    public static string ToPascalCase(string value)
    {
        var builder = new StringBuilder();
        var capitalizeNext = true;

        foreach (var character in value)
        {
            if (character is '.' or '_' or '-')
            {
                capitalizeNext = true;
                continue;
            }

            builder.Append(capitalizeNext ? char.ToUpperInvariant(character) : character);
            capitalizeNext = false;
        }

        return builder.Length == 0 ? "GeneratedApi" : builder.ToString();
    }
}
