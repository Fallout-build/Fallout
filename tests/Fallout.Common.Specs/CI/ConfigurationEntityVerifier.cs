using System.IO;
using System.Threading.Tasks;
using Fallout.Common.CI;
using Fallout.Common.Utilities;
using VerifyXunit;

namespace Fallout.Common.Specs.CI;

public static class ConfigurationEntityVerifier
{
    public static Task Verify(ConfigurationEntity entity)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, leaveOpen: true);
        entity.Write(new CustomFileWriter(writer, indentationFactor: 2, commentPrefix: "#"));
        writer.Flush();

        stream.Seek(offset: 0, SeekOrigin.Begin);
        return Verifier.Verify(new StreamReader(stream).ReadToEnd());
    }
}
