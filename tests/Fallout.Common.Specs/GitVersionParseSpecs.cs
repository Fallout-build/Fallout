// Copyright 2026 Maintainers of Fallout.
// Originally based on NUKE by Matthias Koch and contributors.
// Distributed under the MIT License.
// https://github.com/Fallout-build/Fallout/blob/main/LICENSE

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
    public void Can_parse_a_v6_response()
    {
        var result = GitVersion6Json.GetJson<GitVersion>();

        result.Should().BeEquivalentTo(new
        {
            BuildMetaData = "7",
            CommitsSinceVersionSource = "7",
            PreReleaseNumber = "1",
            WeightedPreReleaseNumber = "1",
            Major = 8,
            Minor = 10,
            Patch = 1,
            BranchName = "switch-to-fallout",
            FullSemVer = "8.10.1-switch-to-fallout.1+7",
            UncommittedChanges = 13,
            PreReleaseTag = "switch-to-fallout.1",
            PreReleaseLabel = "switch-to-fallout",
            PreReleaseLabelName = "switch-to-fallout",
            VersionSourceDistance = "7",
            NuGetVersionV2 = "8.10.1-switch-to-fallout.1"
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
    public void Can_parse_a_v7_response()
    {
        var result = GitVersion7Json.GetJson<GitVersion>();

        result.Should().BeEquivalentTo(new
        {
            Major = 11,
            Minor = 1,
            Patch = 0,
            BuildMetaData = "355",
            PreReleaseNumber = "1",
            WeightedPreReleaseNumber = "1",
            PreReleaseTag = "fix-tools-gitversion-v7.1",
            PreReleaseLabel = "fix-tools-gitversion-v7",
            PreReleaseLabelName = "fix-tools-gitversion-v7",
            PreReleaseTagWithDash = "-fix-tools-gitversion-v7.1",
            PreReleaseLabelWithDash = "-fix-tools-gitversion-v7",
            CommitsSinceVersionSource = "355",
            VersionSourceDistance = "355",
            CommitCountSourceDistance = "355",
            SemVerSourceIncrement = "Minor",
            NuGetVersionV2 = "11.1.0-fix-tools-gitversion-v7.1",
            FullSemVer = "11.1.0-fix-tools-gitversion-v7.1+355",
            UncommittedChanges = 0,
            CustomVersion = ""
        });
    }
}
