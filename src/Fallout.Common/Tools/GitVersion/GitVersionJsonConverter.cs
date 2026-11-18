using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fallout.Common.Tools.GitVersion;

/// <summary>
/// Reads GitVersion 5, 6, and 7 JSON into <see cref="GitVersion"/>.
/// GitVersion 7 renamed the pre-release fields and removed <c>CommitsSinceVersionSource</c>.
/// </summary>
internal sealed class GitVersionJsonConverter : JsonConverter<GitVersion>
{
    public override GitVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        var isVersion7 = root.TryGetProperty("PreReleaseLabelName", out _)
                         || root.TryGetProperty("SemVerSourceSha", out _)
                         || root.TryGetProperty("CommitCountSourceDistance", out _);

        string preReleaseTag;
        string preReleaseTagWithDash;
        string preReleaseLabel;
        string preReleaseLabelWithDash;
        if (isVersion7)
        {
            preReleaseTag = ReadString(root, "PreReleaseLabel");
            preReleaseTagWithDash = ReadString(root, "PreReleaseLabelWithDash");
            preReleaseLabel = ReadString(root, "PreReleaseLabelName");
            preReleaseLabelWithDash = ReadString(root, "PreReleaseLabelNameWithDash");
        }
        else
        {
            preReleaseTag = ReadString(root, "PreReleaseTag");
            preReleaseTagWithDash = ReadString(root, "PreReleaseTagWithDash");
            preReleaseLabel = ReadString(root, "PreReleaseLabel");
            preReleaseLabelWithDash = ReadString(root, "PreReleaseLabelWithDash");
        }

        var semVer = ReadString(root, "SemVer");
        var commitsSinceVersionSource = ReadFirst(root, "CommitsSinceVersionSource", "VersionSourceDistance");

        return new GitVersion(
            ReadInt(root, "Major"),
            ReadInt(root, "Minor"),
            ReadInt(root, "Patch"),
            preReleaseTag,
            preReleaseTagWithDash,
            preReleaseLabel,
            preReleaseLabelWithDash,
            ReadString(root, "PreReleaseNumber"),
            ReadString(root, "WeightedPreReleaseNumber"),
            ReadString(root, "BuildMetaData"),
            ReadString(root, "BuildMetaDataPadded"),
            ReadString(root, "FullBuildMetaData"),
            ReadString(root, "MajorMinorPatch"),
            semVer,
            ReadString(root, "LegacySemVer"),
            ReadString(root, "LegacySemVerPadded"),
            ReadString(root, "AssemblySemVer"),
            ReadString(root, "AssemblySemFileVer"),
            ReadString(root, "FullSemVer"),
            ReadString(root, "InformationalVersion"),
            ReadString(root, "BranchName"),
            ReadString(root, "EscapedBranchName"),
            ReadString(root, "Sha"),
            ReadString(root, "ShortSha"),
            ReadString(root, "NuGetVersionV2") ?? semVer,
            ReadString(root, "NuGetVersion"),
            ReadString(root, "NuGetPreReleaseTagV2"),
            ReadString(root, "NuGetPreReleaseTag"),
            ReadString(root, "VersionSourceSha"),
            commitsSinceVersionSource,
            ReadString(root, "CommitsSinceVersionSourcePadded"),
            ReadNullableInt(root, "UncommittedChanges"),
            ReadString(root, "CommitDate"),
            ReadString(root, "VersionSourceSemVer"),
            ReadString(root, "VersionSourceIncrement"),
            ReadFirst(root, "VersionSourceDistance", "CommitsSinceVersionSource"),
            preReleaseLabel,
            preReleaseLabelWithDash,
            ReadString(root, "SemVerSourceSha"),
            ReadString(root, "SemVerSourceSemVer"),
            ReadString(root, "SemVerSourceIncrement"),
            ReadString(root, "CommitCountSourceSha"),
            ReadString(root, "CommitCountSourceDistance"),
            ReadString(root, "CustomVersion"));
    }

    public override void Write(Utf8JsonWriter writer, GitVersion value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Major", value.Major);
        writer.WriteNumber("Minor", value.Minor);
        writer.WriteNumber("Patch", value.Patch);
        WriteString(writer, "PreReleaseLabel", value.PreReleaseTag);
        WriteString(writer, "PreReleaseLabelWithDash", value.PreReleaseTagWithDash);
        WriteString(writer, "PreReleaseLabelName", value.PreReleaseLabel);
        WriteString(writer, "PreReleaseLabelNameWithDash", value.PreReleaseLabelWithDash);
        WriteString(writer, "PreReleaseNumber", value.PreReleaseNumber);
        WriteString(writer, "WeightedPreReleaseNumber", value.WeightedPreReleaseNumber);
        WriteString(writer, "BuildMetaData", value.BuildMetaData);
        WriteString(writer, "FullBuildMetaData", value.FullBuildMetaData);
        WriteString(writer, "MajorMinorPatch", value.MajorMinorPatch);
        WriteString(writer, "SemVer", value.SemVer);
        WriteString(writer, "AssemblySemVer", value.AssemblySemVer);
        WriteString(writer, "AssemblySemFileVer", value.AssemblySemFileVer);
        WriteString(writer, "FullSemVer", value.FullSemVer);
        WriteString(writer, "InformationalVersion", value.InformationalVersion);
        WriteString(writer, "BranchName", value.BranchName);
        WriteString(writer, "EscapedBranchName", value.EscapedBranchName);
        WriteString(writer, "Sha", value.Sha);
        WriteString(writer, "ShortSha", value.ShortSha);
        WriteNullableNumber(writer, "UncommittedChanges", value.UncommittedChanges);
        WriteString(writer, "CommitDate", value.CommitDate);
        WriteString(writer, "VersionSourceSha", value.VersionSourceSha);
        WriteString(writer, "VersionSourceSemVer", value.VersionSourceSemVer);
        WriteString(writer, "VersionSourceIncrement", value.VersionSourceIncrement);
        WriteString(writer, "VersionSourceDistance", value.VersionSourceDistance);
        WriteString(writer, "SemVerSourceSha", value.SemVerSourceSha);
        WriteString(writer, "SemVerSourceSemVer", value.SemVerSourceSemVer);
        WriteString(writer, "SemVerSourceIncrement", value.SemVerSourceIncrement);
        WriteString(writer, "CommitCountSourceSha", value.CommitCountSourceSha);
        WriteString(writer, "CommitCountSourceDistance", value.CommitCountSourceDistance);
        WriteString(writer, "CustomVersion", value.CustomVersion);
        writer.WriteEndObject();
    }

    private static string ReadFirst(JsonElement root, params string[] names)
    {
        foreach (var name in names)
        {
            var value = ReadString(root, name);
            if (value != null)
            {
                return value;
            }
        }

        return null;
    }

    private static string ReadString(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var value))
        {
            return null;
        }

        return NumberToStringJsonConverter.FromElement(value);
    }

    private static int ReadInt(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Number)
        {
            return 0;
        }

        return value.GetInt32();
    }

    private static int? ReadNullableInt(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Number)
        {
            return null;
        }

        return value.GetInt32();
    }

    private static void WriteString(Utf8JsonWriter writer, string name, string value)
    {
        if (value == null)
        {
            writer.WriteNull(name);
        }
        else
        {
            writer.WriteString(name, value);
        }
    }

    private static void WriteNullableNumber(Utf8JsonWriter writer, string name, int? value)
    {
        if (value == null)
        {
            writer.WriteNull(name);
        }
        else
        {
            writer.WriteNumber(name, value.Value);
        }
    }
}
