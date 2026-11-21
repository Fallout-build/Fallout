using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Fallout.Testing;
using Microsoft.CodeAnalysis;

namespace Fallout.SourceGenerators.Specs;

internal static class GeneratorSnapshot
{
    /// <summary>
    /// Snapshots every generated source as <c>{Class}.{Method}#{hintName}.verified.cs</c>
    /// and, when there are any, the diagnostics as <c>{Class}.{Method}.verified.txt</c>.
    /// </summary>
    public static void Verify(
        GeneratorDriver driver,
        [CallerFilePath] string sourcePath = "",
        [CallerMemberName] string member = "")
    {
        var result = driver.GetRunResult();
        var baseName = Snapshot.BaseName(sourcePath, member);

        foreach (var source in result.Results.SelectMany(r => r.GeneratedSources))
        {
            Snapshot.Verify(
                source.SourceText.ToString(),
                "cs",
                fileName: $"{baseName}#{Path.GetFileNameWithoutExtension(source.HintName)}",
                sourcePath: sourcePath,
                member: member);
        }

        if (result.Diagnostics.Length > 0)
        {
            var builder = new StringBuilder();
            foreach (var diagnostic in result.Diagnostics)
            {
                builder.AppendLine($"{diagnostic.Severity} {diagnostic.Id}: {diagnostic.GetMessage()}");
            }

            Snapshot.Verify(builder.ToString(), sourcePath: sourcePath, member: member);
        }
    }
}
