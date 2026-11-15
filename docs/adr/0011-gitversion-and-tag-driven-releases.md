# ADR-0011: Use GitVersion, and let release tags publish to nuget.org

## Status

Accepted (2026-10-22).

This ADR replaces the versioning rules in [ADR-0009](0009-gitflow-and-semver-reversion.md). Before, `version.json` and Nerdbank.GitVersioning set the version. Now GitVersion does.

This ADR also replaces [ADR-0002](0002-v11-off-nuget-by-default.md)'s rule that nuget.org is always opt-in. A GA or `-rc` tag now publishes there on its own.

The branch model from ADR-0009 does not change.

## Context

`v10.5.0-rc.2` was tagged, but the build produced `10.5.0-preview.31`. The tag does not set the version. The version came from `version.json`, and nobody had changed it on the release branch. The GitHub Packages push succeeded with the wrong version. The GitHub Release step then failed.

Two more problems showed up while checking this:

- A release needed a hand-made "set the version" commit before each tag. Nothing checked that the commit existed.
- nuget.org needed a manual `workflow_dispatch` with a flag, and then an approval. It was easy to forget, and a tag push alone published nothing there.

Other projects of the maintainer (FluentAssertions, Mockly) use GitVersion with the GitFlow preset. A tag is enough to release in those projects.

## Decision

### 1. GitVersion computes the version

- `GitVersion.yml` uses the `GitFlow/v1` workflow. `Build.cs` reads it through the `[GitVersion]` attribute and `IHasGitVersion`.
- `Nerdbank.GitVersioning` and `version.json` are removed from the build. The `NerdbankGitVersioning` tool wrapper stays in `Fallout.Common` for consumers.
- `develop` builds are `-alpha.N`. `release/*` builds are `-beta.N`.
- Assembly versions are `Major.Minor.0.0` (`assembly-versioning-scheme: MajorMinor`), as with Nerdbank.GitVersioning.

### 2. A tag is the version

- Tags have no `v` prefix: `10.5.0` and `10.5.0-rc.1`. Old `v10.x` tags stay as they are.
- The release workflow passes the tag to the build as `ReleaseTag`. `Build.cs` uses it as the package version.
- GitVersion cannot do this itself on a tag build. The checkout is detached, and GitVersion ignores a tag whose label (`rc`) differs from the branch label.

### 3. Routing

| Trigger | Version | Goes to |
|---|---|---|
| Push to `develop` | `10.5.0-alpha.N` (next minor after the last GA tag) | GitHub Packages |
| Push to `release/vX.Y` or `release/X.Y` | `10.5.0-beta.N` | GitHub Packages |
| Tag `10.5.0-rc.N` | as tagged | nuget.org, GitHub Packages, GitHub Release |
| Tag `10.5.0` | as tagged | nuget.org, GitHub Packages, GitHub Release |
| Any other prerelease tag, such as `10.5.0-beta.1` | as tagged | GitHub Packages and GitHub Release only |

The `Nuke.*` shim packages still never go to nuget.org ([#47](https://github.com/Fallout-build/Fallout/issues/47) — those IDs belong to the original NUKE maintainer).

### 4. No approval step

The `nuget-org` environment keeps its secret but has no required reviewers. Tag protection (repo admins only) is the gate, so tag ruleset 17017817 must also cover `refs/tags/[0-9]*`. A push to nuget.org cannot be undone, so only tag admins should create `X.Y.Z` and `-rc` tags.

## Consequences

- A release is a tag push. No version commit is needed.
- The tag cannot disagree with the package version, because the tag is the version.
- A wrong or accidental tag publishes to nuget.org at once. nuget.org can only unlist a version, not delete it.
- Old `-alpha.N` and `-preview.N` packages on GitHub Packages can be pruned by `prune-preview-packages.yml`. GA and `-rc` versions are never pruned.
- A local `dotnet build` outside the build project gets no GitVersion version. Use `./build.ps1` to get versioned output.
