using System;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

[Collection(ToolPathResolverStateCollection.Name)]
public sealed class EnvironmentExecutableSpecs : IDisposable
{
    private readonly AbsolutePath root = AbsolutePath.Temp("environment-executables").CreateDirectory();
    private readonly string variable = "FALLOUT_SPEC_TOOL_" + Guid.NewGuid().ToString("N");

    [Fact]
    public void Existing_paths_with_spaces_keep_their_exact_value()
    {
        var path = root / "directory with spaces" / "tool";
        path.Parent.CreateDirectory();
        path.WriteAllText(string.Empty);
        ((AbsolutePath)(path + ".exe")).WriteAllText(string.Empty);
        Environment.SetEnvironmentVariable(variable, path);

        ToolPathResolver.TryGetEnvironmentExecutable(variable).Should().Be(path);
    }

    [Fact]
    public void Extensionless_paths_resolve_exe_files_only_on_windows()
    {
        var path = root / "directory with spaces" / "tool";
        path.Parent.CreateDirectory();
        ((AbsolutePath)(path + ".exe")).WriteAllText(string.Empty);
        Environment.SetEnvironmentVariable(variable, path);

        if (EnvironmentInfo.IsWin)
        {
            ToolPathResolver.TryGetEnvironmentExecutable(variable).Should().Be(path + ".exe");
        }
        else
        {
            var resolve = () => ToolPathResolver.TryGetEnvironmentExecutable(variable);
            resolve.Should().Throw<Exception>().WithMessage("*does not exist*");
        }
    }

    [Fact]
    public void Missing_explicit_paths_still_fail()
    {
        (root / "missing.exe.exe").WriteAllText(string.Empty);
        Environment.SetEnvironmentVariable(variable, root / "missing.exe");

        var resolve = () => ToolPathResolver.TryGetEnvironmentExecutable(variable);

        resolve.Should().Throw<Exception>().WithMessage("*does not exist*");
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(variable, null);
        root.DeleteDirectory();
    }
}
