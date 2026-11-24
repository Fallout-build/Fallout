// Copyright 2026 Maintainers of Fallout.
// Originally based on NUKE by Matthias Koch and contributors.
// Distributed under the MIT License.
// https://github.com/Fallout-build/Fallout/blob/main/LICENSE

using System.Collections.Generic;
using System.Text.Json;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.GitVersion;
using Fallout.Common.Utilities;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

/// <summary>
/// GitVersion JSON deserialisation.
/// GitVersion 6 emits some fields as JSON numbers. GitVersion 7 renames the pre-release fields.
/// </summary>
public class GitVersionParseSpecs
{
    // Exact payload from issue #218 — BuildMetaData, CommitsSinceVersionSource,
    // PreReleaseNumber and WeightedPreReleaseNumber are numbers, not strings.
    private const string GitVersion6Json = """
                                           {
                                             "AssemblySemFileVer": "8.10.1.0",
                                             "AssemblySemVer": "8.10.1.0",
                                             "BranchName": "switch-to-fallout",
                                             "BuildMetaData": 7,
                                             "CommitDate": "2026-05-26",
                                             "CommitsSinceVersionSource": 7,
                                             "EscapedBranchName": "switch-to-fallout",
                                             "FullBuildMetaData": "7.Branch.switch-to-fallout.Sha.6419fa509c7934f0b34a0ea5e5306c44c0a9a259",
                                             "FullSemVer": "8.10.1-switch-to-fallout.1+7",
                                             "InformationalVersion": "8.10.1-switch-to-fallout.1+7.Branch.switch-to-fallout.Sha.6419fa509c7934f0b34a0ea5e5306c44c0a9a259",
                                             "Major": 8,
                                             "MajorMinorPatch": "8.10.1",
                                             "Minor": 10,
                                             "Patch": 1,
                                             "PreReleaseLabel": "switch-to-fallout",
                                             "PreReleaseLabelWithDash": "-switch-to-fallout",
                                             "PreReleaseNumber": 1,
                                             "PreReleaseTag": "switch-to-fallout.1",
                                             "PreReleaseTagWithDash": "-switch-to-fallout.1",
                                             "SemVer": "8.10.1-switch-to-fallout.1",
                                             "Sha": "6419fa509c7934f0b34a0ea5e5306c44c0a9a259",
                                             "ShortSha": "6419fa5",
                                             "UncommittedChanges": 13,
                                             "VersionSourceDistance": 7,
                                             "VersionSourceIncrement": "None",
                                             "VersionSourceSemVer": "8.10.0",
                                             "VersionSourceSha": "0954811776a71005283221b91aabafc5fec332b7",
                                             "WeightedPreReleaseNumber": 1
                                           }
                                           """;

    [Fact]
    public void GitVersion_6_output_keeps_the_pre_release_meaning()
    {
        var result = GitVersion6Json.GetJson<GitVersion>(GitVersionJsonConverter.Options);

        result.Should().BeEquivalentTo(new GitVersion(
            Major: 8,
            Minor: 10,
            Patch: 1,
            PreReleaseTag: "switch-to-fallout.1",
            PreReleaseTagWithDash: "-switch-to-fallout.1",
            PreReleaseLabel: "switch-to-fallout",
            PreReleaseLabelWithDash: "-switch-to-fallout",
            PreReleaseNumber: "1",
            WeightedPreReleaseNumber: "1",
            BuildMetaData: "7",
            BuildMetaDataPadded: null,
            FullBuildMetaData: "7.Branch.switch-to-fallout.Sha.6419fa509c7934f0b34a0ea5e5306c44c0a9a259",
            MajorMinorPatch: "8.10.1",
            SemVer: "8.10.1-switch-to-fallout.1",
            LegacySemVer: null,
            LegacySemVerPadded: null,
            AssemblySemVer: "8.10.1.0",
            AssemblySemFileVer: "8.10.1.0",
            FullSemVer: "8.10.1-switch-to-fallout.1+7",
            InformationalVersion: "8.10.1-switch-to-fallout.1+7.Branch.switch-to-fallout.Sha.6419fa509c7934f0b34a0ea5e5306c44c0a9a259",
            BranchName: "switch-to-fallout",
            EscapedBranchName: "switch-to-fallout",
            Sha: "6419fa509c7934f0b34a0ea5e5306c44c0a9a259",
            ShortSha: "6419fa5",
            NuGetVersionV2: "8.10.1-switch-to-fallout.1",
            NuGetVersion: null,
            NuGetPreReleaseTagV2: null,
            NuGetPreReleaseTag: null,
            VersionSourceSha: "0954811776a71005283221b91aabafc5fec332b7",
            CommitsSinceVersionSource: "7",
            CommitsSinceVersionSourcePadded: null,
            UncommittedChanges: 13,
            CommitDate: "2026-05-26")
        {
            VersionSourceSemVer = "8.10.0",
            VersionSourceIncrement = "None"
        });
    }

    // Captured from GitVersion 7.0.0. PreReleaseLabel is the old PreReleaseTag value.
    private const string GitVersion7Json = """
        {
          "AssemblySemFileVer": "11.1.0.0",
          "AssemblySemVer": "11.1.0.0",
          "BranchName": "fix/tools/gitversion-v7",
          "BuildMetaData": 355,
          "CommitCountSourceDistance": 355,
          "CommitCountSourceSha": "21c084996fcbfd387952f8e94fd4d23d2f9d5c34",
          "CommitDate": "2026-11-18",
          "CustomVersion": "",
          "EscapedBranchName": "fix-tools-gitversion-v7",
          "FullBuildMetaData": "355.Branch.fix-tools-gitversion-v7.Sha.defc50a5e3d0985c39754309e036aa11d07a3e86",
          "FullSemVer": "11.1.0-fix-tools-gitversion-v7.1+355",
          "InformationalVersion": "11.1.0-fix-tools-gitversion-v7.1+355.Branch.fix-tools-gitversion-v7.Sha.defc50a5e3d0985c39754309e036aa11d07a3e86",
          "Major": 11,
          "MajorMinorPatch": "11.1.0",
          "Minor": 1,
          "Patch": 0,
          "PreReleaseLabelName": "fix-tools-gitversion-v7",
          "PreReleaseLabelNameWithDash": "-fix-tools-gitversion-v7",
          "PreReleaseNumber": 1,
          "PreReleaseLabel": "fix-tools-gitversion-v7.1",
          "PreReleaseLabelWithDash": "-fix-tools-gitversion-v7.1",
          "SemVer": "11.1.0-fix-tools-gitversion-v7.1",
          "SemVerSourceIncrement": "Minor",
          "SemVerSourceSemVer": "11.0.18",
          "SemVerSourceSha": "21c084996fcbfd387952f8e94fd4d23d2f9d5c34",
          "Sha": "defc50a5e3d0985c39754309e036aa11d07a3e86",
          "ShortSha": "defc50a",
          "UncommittedChanges": 0,
          "VersionSourceDistance": 355,
          "VersionSourceIncrement": "None",
          "VersionSourceSemVer": "11.0.18",
          "VersionSourceSha": "21c084996fcbfd387952f8e94fd4d23d2f9d5c34",
          "WeightedPreReleaseNumber": 1
        }
        """;

    [Fact]
    public void GitVersion_7_output_keeps_the_version_6_meaning()
    {
        var result = GitVersion7Json.GetJson<GitVersion>(GitVersionJsonConverter.Options);

        result.Should().BeEquivalentTo(new GitVersion(
            Major: 11,
            Minor: 1,
            Patch: 0,
            PreReleaseTag: "fix-tools-gitversion-v7.1",
            PreReleaseTagWithDash: "-fix-tools-gitversion-v7.1",
            PreReleaseLabel: "fix-tools-gitversion-v7",
            PreReleaseLabelWithDash: "-fix-tools-gitversion-v7",
            PreReleaseNumber: "1",
            WeightedPreReleaseNumber: "1",
            BuildMetaData: "355",
            BuildMetaDataPadded: null,
            FullBuildMetaData: "355.Branch.fix-tools-gitversion-v7.Sha.defc50a5e3d0985c39754309e036aa11d07a3e86",
            MajorMinorPatch: "11.1.0",
            SemVer: "11.1.0-fix-tools-gitversion-v7.1",
            LegacySemVer: null,
            LegacySemVerPadded: null,
            AssemblySemVer: "11.1.0.0",
            AssemblySemFileVer: "11.1.0.0",
            FullSemVer: "11.1.0-fix-tools-gitversion-v7.1+355",
            InformationalVersion: "11.1.0-fix-tools-gitversion-v7.1+355.Branch.fix-tools-gitversion-v7.Sha.defc50a5e3d0985c39754309e036aa11d07a3e86",
            BranchName: "fix/tools/gitversion-v7",
            EscapedBranchName: "fix-tools-gitversion-v7",
            Sha: "defc50a5e3d0985c39754309e036aa11d07a3e86",
            ShortSha: "defc50a",
            NuGetVersionV2: "11.1.0-fix-tools-gitversion-v7.1",
            NuGetVersion: null,
            NuGetPreReleaseTagV2: null,
            NuGetPreReleaseTag: null,
            VersionSourceSha: "21c084996fcbfd387952f8e94fd4d23d2f9d5c34",
            CommitsSinceVersionSource: "355",
            CommitsSinceVersionSourcePadded: null,
            UncommittedChanges: 0,
            CommitDate: "2026-11-18")
        {
            VersionSourceSemVer = "11.0.18",
            VersionSourceIncrement = "None",
            SemVerSourceSha = "21c084996fcbfd387952f8e94fd4d23d2f9d5c34",
            SemVerSourceSemVer = "11.0.18",
            SemVerSourceIncrement = "Minor",
            CommitCountSourceSha = "21c084996fcbfd387952f8e94fd4d23d2f9d5c34",
            CommitCountSourceDistance = "355",
            CustomVersion = ""
        });
    }

    [Fact]
    public void Stdout_json_is_parsed_when_stderr_has_a_warning()
    {
        // Arrange
        IReadOnlyCollection<Output> output =
        [
            new Output { Type = OutputType.Err, Text = "warning: shallow repository" },
            new Output { Type = OutputType.Std, Text = GitVersion6Json }
        ];

        // Act
        var result = GitVersionTasks.ReadToolOutput(output);

        // Assert
        result.Major.Should().Be(8);
        result.PreReleaseTag.Should().Be("switch-to-fallout.1");
    }

    [Fact]
    public void Missing_major_throws()
    {
        // Arrange
        const string json = """{ "Minor": 1, "Patch": 0 }""";

        // Act
        var act = () => json.GetJson<GitVersion>(GitVersionJsonConverter.Options);

        // Assert
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Default_serialization_keeps_the_csharp_property_names()
    {
        // Arrange
        var version = new GitVersion(
            1, 0, 0, "beta.1", "-beta.1", "beta", "-beta",
            null, null, null, null, null, "1.0.0", "1.0.0-beta.1",
            null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null);

        // Act
        var json = version.ToJson();

        // Assert
        json.Should().Contain("PreReleaseTag");
        json.Should().NotContain("PreReleaseLabelName");
    }
}
