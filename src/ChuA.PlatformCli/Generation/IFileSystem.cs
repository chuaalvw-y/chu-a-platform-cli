// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.PlatformCli.Generation;

public interface IFileSystem
{
    bool DirectoryExists(string path);

    bool FileExists(string path);

    bool DirectoryHasEntries(string path);

    void CreateDirectory(string path);

    void WriteAllText(string path, string contents);
}
