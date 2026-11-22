using System.Collections.Generic;
using Fallout.Common;
using Fallout.Components;
using Fallout.Solutions;

/// <summary>
/// Runs the <see cref="ITest.Test" /> target on one test solution.
/// </summary>
/// <remarks>
/// Run it in the folder of that solution. The <c>.fallout</c> directory there makes it the root directory, and the
/// <c>global.json</c> there selects the test runner.
/// </remarks>
class Build : FalloutBuild, ITest
{
    public static int Main() => Execute<Build>(x => ((ITest)x).Test);

    IEnumerable<Project> ITest.TestProjects => ((IHasSolution)this).Solution.GetAllProjects("*.Specs");
}
