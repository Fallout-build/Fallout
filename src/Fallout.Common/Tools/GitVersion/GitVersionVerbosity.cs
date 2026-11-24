using System;

namespace Fallout.Common.Tools.GitVersion;

public partial class GitVersionVerbosity
{
    [Obsolete("Use Quiet. Removed in v11.",
        DiagnosticId = "FALLOUTOBS003",
        UrlFormat = "https://github.com/Fallout-build/Fallout/blob/main/docs/obsolete_apis.md")]
    public static GitVersionVerbosity debug = (GitVersionVerbosity)"debug";

    [Obsolete("Use Minimal or Normal. Removed in v11.",
        DiagnosticId = "FALLOUTOBS003",
        UrlFormat = "https://github.com/Fallout-build/Fallout/blob/main/docs/obsolete_apis.md")]
    public static GitVersionVerbosity info = (GitVersionVerbosity)"info";

    [Obsolete("Use Normal. Removed in v11.",
        DiagnosticId = "FALLOUTOBS003",
        UrlFormat = "https://github.com/Fallout-build/Fallout/blob/main/docs/obsolete_apis.md")]
    public static GitVersionVerbosity warn = (GitVersionVerbosity)"warn";

    [Obsolete("Use Minimal. Removed in v11.",
        DiagnosticId = "FALLOUTOBS003",
        UrlFormat = "https://github.com/Fallout-build/Fallout/blob/main/docs/obsolete_apis.md")]
    public static GitVersionVerbosity error = (GitVersionVerbosity)"error";

    [Obsolete("Use Quiet. Removed in v11.",
        DiagnosticId = "FALLOUTOBS003",
        UrlFormat = "https://github.com/Fallout-build/Fallout/blob/main/docs/obsolete_apis.md")]
    public static GitVersionVerbosity none = (GitVersionVerbosity)"none";
}
