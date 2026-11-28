using System;

namespace Fallout.Common.IO;

partial class AbsolutePathExtensions
{
    /// <summary>
    /// Returns the line break for a file that Fallout generates into the repository: the one that ends
    /// the first line of the existing file, or LF when the file doesn't exist yet or has no line break.
    /// </summary>
    /// <remarks>
    /// Regenerating unchanged content then leaves the file byte-identical in an LF checkout
    /// (<c>eol=lf</c>) and in a CRLF checkout (<c>core.autocrlf=true</c>), so a changed file hash means
    /// changed content. <see cref="Environment.NewLine"/> would rewrite every file of an LF checkout
    /// with CRLF on Windows, and a fixed LF would rewrite every file of a CRLF checkout.
    /// </remarks>
    internal static string GetGeneratedFileLineBreak(this AbsolutePath path)
    {
        var text = path.FileExists() ? path.ReadAllText() : string.Empty;
        var index = text.IndexOf('\n');
        return index > 0 && text[index - 1] == '\r' ? "\r\n" : "\n";
    }

    /// <summary>
    /// Writes a file that Fallout generates into the repository, with the line break that
    /// <see cref="GetGeneratedFileLineBreak"/> returns.
    /// </summary>
    internal static AbsolutePath WriteGeneratedText(this AbsolutePath path, string content)
    {
        var lineBreak = path.GetGeneratedFileLineBreak();
        return path.WriteAllText(content.Replace("\r\n", "\n").Replace("\n", lineBreak));
    }
}
