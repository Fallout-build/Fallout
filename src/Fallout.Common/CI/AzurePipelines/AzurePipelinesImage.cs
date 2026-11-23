using Fallout.Common.Tooling;

namespace Fallout.Common.CI.AzurePipelines;

/// <summary>
/// See <a href="https://learn.microsoft.com/en-us/azure/devops/pipelines/agents/hosted">Microsoft-hosted agents</a>
/// </summary>
public enum AzurePipelinesImage
{
    [EnumValue("windows-2022")]
    Windows2022,

    [EnumValue("windows-2019")]
    Windows2019,

    [EnumValue("ubuntu-22.04")]
    Ubuntu2204,

    [EnumValue("ubuntu-20.04")]
    Ubuntu2004,

    [EnumValue("macOS-13")]
    MacOs13,

    [EnumValue("macOS-12")]
    MacOs12,

    [EnumValue("macOS-11")]
    MacOs11,

    [EnumValue("windows-latest")]
    WindowsLatest,

    [EnumValue("ubuntu-latest")]
    UbuntuLatest,

    [EnumValue("macOS-latest")]
    MacOsLatest,

    /// <summary>Windows Server 2025 with Visual Studio 2026.</summary>
    [EnumValue("windows-2025-vs2026")]
    Windows2025Vs2026,

    /// <summary>Windows Server 2025 with Visual Studio 2022.</summary>
    [EnumValue("windows-2025")]
    Windows2025,

    [EnumValue("ubuntu-26.04")]
    Ubuntu2604,

    [EnumValue("ubuntu-24.04")]
    Ubuntu2404,

    [EnumValue("macOS-26")]
    MacOs26,

    [EnumValue("macOS-15")]
    MacOs15,
}
