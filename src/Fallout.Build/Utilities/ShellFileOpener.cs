using System.Diagnostics;
using Fallout.Common.IO;

namespace Fallout.Build.Utilities;

/// <summary>
/// Opens files through the operating system shell, so a plan HTML file opens in the default browser.
/// </summary>
internal class ShellFileOpener : IFileOpener
{
    /// <inheritdoc />
    public void Open(AbsolutePath file)
    {
        // UseShellExecute is false by default on .NET Core. Without it, the file is not opened by its default app.
        Process.Start(new ProcessStartInfo
        {
            FileName = file,
            UseShellExecute = true
        });
    }
}
