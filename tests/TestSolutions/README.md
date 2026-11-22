# Test solutions

Two small solutions that `TestSolutionSpecs` in `Fallout.Components.Specs` runs the `Test` target of `ITest` on. They show that both test runners of `dotnet test` keep working.

| Folder | Test runner | Test framework |
|---|---|---|
| `VSTest/` | VSTest | xunit v2 |
| `MTP/` | Microsoft.Testing.Platform | xunit.v3, which writes its own TRX report |

Each folder stands for a repository of its own. It has a `.fallout` directory, a `global.json` that selects the test runner, and a solution with one test project. Each test project has one passing and one skipped test.

`build/` is one build for both. The spec runs it in the folder of a solution, so the `global.json` of that folder selects the test runner, as in a real repository. The spec then checks the exit code and the TRX report. The build goes to `.build/`, so it doesn't overwrite the outputs of the Fallout projects in this repository.

`Directory.Build.props`, `Directory.Build.targets` and `Directory.Packages.props` in this folder keep the settings and test packages of this repository away from the test solutions.

The TRX report and Coverlet extensions of Microsoft.Testing.Platform are covered by Fallout's own test run, which uses them.

To run only these specs:

```powershell
dotnet test --project tests/Fallout.Components.Specs/Fallout.Components.Specs.csproj -- --filter-class Fallout.Components.Specs.TestSolutionSpecs
```
