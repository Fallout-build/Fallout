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
        // Workaround for https://github.com/dotnet/corefx/issues/10361
        Process.Start(new ProcessStartInfo
        {
            FileName = file,
            UseShellExecute = true
        });
    }
}
