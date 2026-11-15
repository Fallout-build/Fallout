# Third-party dependencies

The list of third-party packages lives in [`Directory.Packages.props`](https://github.com/Fallout-build/Fallout/blob/develop/Directory.Packages.props). It is grouped by what an update means for people who build with Fallout, and each group's header comment states its rule. This page explains why those rules exist. For how the Fallout packages depend on each other, see the [package graph](architecture.md#package-graph).

## Updating dependencies

Every `10.x` release must be non-breaking ([ADR-0009](adr/0009-gitflow-and-semver-reversion.md)). For a library, that covers more than our own code:

- **A dependency version in a package is a minimum.** When we raise a version, every consumer gets at least that version. A consumer that pins a lower version gets the `NU1605` downgrade error.
- **Transitive pinning makes every package a direct dependency.** `CentralPackageTransitivePinningEnabled` is on, so the `.nuspec` of `Fallout.Common` and `Fallout.Components` lists every third-party package they use (21 today), not just the Fallout packages. Raising a version anywhere in the graph changes what consumers of those two packages receive.
- **Some third-party types are part of our public API.** If a consumer's code uses `GitHubTasks.GitHubClient`, a breaking change in Octokit breaks their build, even though our own code did not change.
- **Some packages run inside the consumer's tools.** `Fallout.Common` bundles the MSBuild tasks and the source generator, with their dependencies (see [What `Fallout.Common` bundles](architecture.md#what-falloutcommon-bundles)). Those DLLs load next to the versions that MSBuild, Visual Studio or the C# compiler already loaded.

So `Directory.Packages.props` has four groups:

| Group | Meaning | Rule |
|---|---|---|
| **Public API** | Types from the package appear in Fallout's public API. | Never raise the major version in `10.x`. Raise the minor or patch version only for a security fix or a fix we need. |
| **Runs in host** | Bundled inside `Fallout.Common` and loaded into the consumer's MSBuild, Visual Studio or C# compiler. | Test with the oldest supported SDK and Visual Studio before raising a version. |
| **Shipped, internal only** | Consumers receive the package as a dependency, but its types are not in our API. | Patch and minor updates are fine when there is a reason. Don't update on a schedule. |
| **Not shipped** | Only used by tests, this repo's build, or the `fallout` / `fallout-migrate` tools. | Safe to update at any time. |

A few packages have extra constraints, noted in comments next to them:

- **NuGet.\*** must match the NuGet version that the .NET SDK loads into MSBuild. A mismatch breaks the build when MSBuild loads our tasks (#677, the NuGet.Frameworks load failure on SDK 10.0.400).
- **Roslyn**: the version of `Microsoft.CodeAnalysis.CSharp` sets the oldest C# compiler that can load `Fallout.Migrate.Analyzers`. `Fallout.SourceGenerators` pins 4.7.0 with `VersionOverride` for the same reason. All Roslyn packages stay on one version, including the ones only `Fallout.Cli` and the tests use.
- **NuGet.Protocol / NuGet.Resolver** are not shipped, but are pinned to the `NuGet.Packaging` version because the Roslyn analyzer test harness pulls in older copies (#677).

## Dependabot

Dependabot opens monthly, grouped PRs for the **Not shipped** group only. Those packages are listed by name in the `allow` list of [`.github/dependabot.yml`](https://github.com/Fallout-build/Fallout/blob/develop/.github/dependabot.yml). Dependabot cannot read `Directory.Packages.props` groups, so the two lists must be kept in sync by hand.

The `allow` list also stops Dependabot from opening *security* PRs for the other groups. That is on purpose: a vulnerability in a shipped package still shows up as a Dependabot alert in the repository's Security tab, and a maintainer raises the version deliberately, following the rules above.

## Adding or removing a dependency

- **Adding:** put the package in the right group of `Directory.Packages.props`. For **Public API** and **Runs in host** packages, add a short comment saying why. For **Not shipped** packages, also add the name to the Dependabot `allow` list.
- **Removing:** remove it from `Directory.Packages.props`, and from the `allow` list if it was there.
- **Raising the major version** of a **Public API** or **Runs in host** package: explain why in the PR body. In `10.x`, a **Public API** major needs a deliberate maintainer decision.

To see which packages a project uses, and which third-party packages a Fallout package really depends on:

```pwsh
dotnet list fallout.slnx package --include-transitive   # whole dependency graph
dotnet pack src/Fallout.Common                           # then read the .nuspec inside the .nupkg
```

## FluentAssertions licensing

As of **v8.0** (Jan 2025) FluentAssertions dropped Apache 2.0 for the proprietary **Xceed Community License**: free for open-source / non-commercial use, paid (per-seat) for commercial use. v7.x remains Apache 2.0. We pin **8.x** (`Directory.Packages.props`) and stay current — this is fine for Fallout because it's (a) an OSS project covered by the free community license, and (b) a **test-only / dev-time** dependency that is never redistributed to consumers of the framework. The standard assertion convention (xUnit + FluentAssertions + Verify) is unchanged — see [AGENTS.md](../AGENTS.md).
