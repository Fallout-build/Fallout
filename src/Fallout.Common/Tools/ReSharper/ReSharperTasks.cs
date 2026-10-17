using Fallout.Common.CI.TeamCity;
using Fallout.Common.Tooling;

namespace Fallout.Common.Tools.ReSharper;

partial class ReSharperTasks
{
    /// <summary>The executable of the <c>JetBrains.ReSharper.GlobalTools</c> global .NET tool.</summary>
    public const string PathExecutable = "jb";

    /// <summary>
    /// Uses the <c>jb</c> global .NET tool from the PATH when it is installed, and the
    /// <c>JetBrains.ReSharper.GlobalTools</c> package from the NuGet packages folder otherwise.
    /// The <c>RESHARPER_EXE</c> environment variable overrides both.
    /// </summary>
    protected override string GetToolPath(ToolOptions options = null)
    {
        return ToolPathResolver.TryGetPathExecutable(PathExecutable)
               ?? NuGetToolPathResolver.GetPackageExecutable(PackageId, PackageExecutable);
    }

    protected override void PostProcess(ToolOptions options)
    {
        // TeamCity's inspection importer reads the XML report format only.
        if (options is ReSharperInspectCodeSettings { Output: { } output } settings && settings.Format == ReSharperFormat.Xml)
        {
            TeamCity.Instance?.ImportData(TeamCityImportType.ReSharperInspectCode, output);
        }
    }
}
