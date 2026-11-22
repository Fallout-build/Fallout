using System.IO;
using System.Runtime.CompilerServices;
using Fallout.Common.CI;
using Fallout.Common.Utilities;
using Fallout.Testing;

namespace Fallout.Common.Specs.CI;

public static class ConfigurationEntityVerifier
{
    public static void Verify(
        ConfigurationEntity entity,
        [CallerFilePath] string sourcePath = "",
        [CallerMemberName] string member = "")
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, leaveOpen: true);
        entity.Write(new CustomFileWriter(writer, indentationFactor: 2, commentPrefix: "#"));
        writer.Flush();

        stream.Seek(offset: 0, SeekOrigin.Begin);
        Snapshot.Verify(new StreamReader(stream).ReadToEnd(), sourcePath: sourcePath, member: member);
    }
}
