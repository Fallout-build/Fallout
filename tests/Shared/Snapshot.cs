using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace Fallout.Testing;

/// <summary>
/// Minimal snapshot testing: compares text against a <c>*.verified.*</c> file and shows a DiffPlex diff on mismatch.
/// On mismatch the actual text is written to a <c>*.received.*</c> file; copy it over the verified file to accept it.
/// </summary>
internal static class Snapshot
{
    /// <summary>The snapshot base name: <c>{SpecsClass}.{Method}</c>, where the class is taken from the file name.</summary>
    public static string BaseName(string sourcePath, string member)
        => $"{Path.GetFileNameWithoutExtension(sourcePath)}.{member}";

    /// <param name="actual">The text to compare.</param>
    /// <param name="extension">File extension of the snapshot, without the dot.</param>
    /// <param name="parameters">Optional suffix for parameterized tests, e.g. <c>number=1</c>.</param>
    /// <param name="fileName">Overrides the default <c>{Class}.{Method}</c> file name.</param>
    /// <param name="directory">Overrides the snapshot directory (default: next to the calling source file).</param>
    public static void Verify(
        string actual,
        string extension = "txt",
        string parameters = null,
        string fileName = null,
        string directory = null,
        [CallerFilePath] string sourcePath = "",
        [CallerMemberName] string member = "")
    {
        var name = fileName ?? BaseName(sourcePath, member) + (parameters is null ? "" : "_" + parameters);
        var folder = Path.Combine(Path.GetDirectoryName(sourcePath)!, directory ?? string.Empty);
        var verifiedPath = Path.Combine(folder, $"{name}.verified.{extension}");
        var receivedPath = Path.Combine(folder, $"{name}.received.{extension}");

        var expected = File.Exists(verifiedPath) ? Normalize(File.ReadAllText(verifiedPath)) : string.Empty;
        actual = Normalize(actual);

        var diff = InlineDiffBuilder.Diff(expected, actual);
        if (!diff.HasDifferences)
        {
            File.Delete(receivedPath);
            return;
        }

        File.WriteAllText(receivedPath, actual + "\n", new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        throw new InvalidOperationException($"Snapshot {Path.GetFileName(verifiedPath)} differs. Accept the change by replacing it with {Path.GetFileName(receivedPath)}.{Environment.NewLine}{Stringify(diff)}");
    }

    private static string Normalize(string text)
        => text.TrimStart('﻿').Replace("\r\n", "\n").TrimEnd('\n');

    private static string Stringify(DiffPaneModel diff)
    {
        var builder = new StringBuilder();
        foreach (var line in diff.Lines)
        {
            switch (line.Type)
            {
                case ChangeType.Inserted:
                    builder.Append("+ ");
                    break;
                case ChangeType.Deleted:
                    builder.Append("- ");
                    break;
                default:
                    continue;
            }

            builder.AppendLine(line.Text);
        }

        return builder.ToString();
    }
}
