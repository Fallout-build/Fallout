using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fallout.Common.Utilities;

namespace Fallout.Common.Tools.GitVersion;

/// <summary>
/// Reads GitVersion 5, 6, and 7 JSON into <see cref="GitVersion"/>.
/// GitVersion 7 renamed the pre-release fields and removed <c>CommitsSinceVersionSource</c>.
/// Use <see cref="Options"/> when parsing tool output. The record itself keeps the default serializer.
/// Numeric fields such as <c>PreReleaseNumber</c> still use <see cref="NumberToStringJsonConverter"/>, so <c>JsonSerializer.Deserialize</c> keeps working.
/// </summary>
internal sealed class GitVersionJsonConverter : JsonConverter<GitVersion>
{
    internal static readonly JsonSerializerOptions Options = CreateOptions();

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
            ReadFirst(root, "CommitsSinceVersionSource", "VersionSourceDistance"),
            ReadString(root, "CommitsSinceVersionSourcePadded"),
            ReadNullableInt(root, "UncommittedChanges"),
            ReadString(root, "CommitDate"))
        {
            VersionSourceSemVer = ReadString(root, "VersionSourceSemVer"),
            VersionSourceIncrement = ReadString(root, "VersionSourceIncrement"),
            SemVerSourceSha = ReadString(root, "SemVerSourceSha"),
            SemVerSourceSemVer = ReadString(root, "SemVerSourceSemVer"),
            SemVerSourceIncrement = ReadString(root, "SemVerSourceIncrement"),
            CommitCountSourceSha = ReadString(root, "CommitCountSourceSha"),
            CommitCountSourceDistance = ReadString(root, "CommitCountSourceDistance"),
            CustomVersion = ReadString(root, "CustomVersion")
        };
    }

    public override void Write(Utf8JsonWriter writer, GitVersion value, JsonSerializerOptions options)
        => throw new NotSupportedException("GitVersion JSON is only read from the tool output.");

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions(JsonExtensions.DefaultSerializerOptions);
        options.Converters.Add(new GitVersionJsonConverter());
        return options;
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

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.TryGetInt64(out var integer)
                ? integer.ToString(CultureInfo.InvariantCulture)
                : value.GetDouble().ToString(CultureInfo.InvariantCulture),
            JsonValueKind.Null => null,
            _ => null
        };
    }

    private static int ReadInt(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var value) ||
            value.ValueKind != JsonValueKind.Number ||
            !value.TryGetInt32(out var number))
        {
            throw new JsonException($"GitVersion JSON property '{name}' must be a number.");
        }

        return number;
    }

    private static int? ReadNullableInt(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Number)
        {
            return null;
        }

        return value.GetInt32();
    }
}
