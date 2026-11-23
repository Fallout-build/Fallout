using System.Collections.Generic;
using Fallout.Common;
using Fallout.Components;
using Fallout.Solutions;

/// <summary>
/// Runs the <see cref="ITest.Test" /> target on one test solution.
/// </summary>
/// <remarks>
/// <c>TestSolutionSpecs</c> in <c>Fallout.Components.Specs</c> runs this build, so the <c>Test</c> target of the repository
/// runs it for both solutions. It runs it in the folder of each solution. The <c>.fallout</c> directory there makes that
/// folder the root directory, and the <c>global.json</c> there selects the test runner.
/// </remarks>
class Build : FalloutBuild, ITest
{
    public static int Main() => Execute<Build>(x => ((ITest)x).Test);

    IEnumerable<Project> ITest.TestProjects => ((IHasSolution)this).Solution.GetAllProjects("*.Specs");
}
