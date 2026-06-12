// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.PlatformCli.Generation;

public sealed record GenerationResult(bool Succeeded, string Message)
{
    public static GenerationResult Success(string message)
    {
        return new GenerationResult(true, message);
    }

    public static GenerationResult Failure(string message)
    {
        return new GenerationResult(false, message);
    }
}
