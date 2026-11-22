using Fallout.Common.IO;

namespace Fallout.Build.Utilities;

/// <summary>
/// Opens a file with the program that the operating system associates with it.
/// </summary>
internal interface IFileOpener
{
    /// <summary>
    /// Opens <paramref name="file"/> and returns without waiting for the program to close.
    /// </summary>
    void Open(AbsolutePath file);
}
