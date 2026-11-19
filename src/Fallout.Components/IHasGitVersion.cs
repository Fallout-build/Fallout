using Fallout.Common;
using Fallout.Common.Tools.GitVersion;

namespace Fallout.Components;

public interface IHasGitVersion : IFalloutBuild
{
    // GitVersion 7 installs under tools/net10.0/<rid> and tools/net11.0/<rid>. net10.0 runs without the .NET 11 runtime.
    [GitVersion(NoFetch = true, Framework = "net10.0")]
    [Required]
    GitVersion Versioning => TryGetValue(() => Versioning);
}
