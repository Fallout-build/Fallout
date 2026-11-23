using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using FluentAssertions;
using Xunit;

namespace Fallout.Components.Specs;

public class TestingPlatformSpecs
{
    private static readonly TestingPlatformRun LocalRun =
        new(CollectCoverage: false, OnGitHubActions: false, OnTeamCity: false, OnAzurePipelines: false);

    [Fact]
    public void Extensions_are_found_from_the_builder_hooks_of_their_packages()
    {
        // Act
        TestingPlatformExtensions extensions = TestingPlatform.GetExtensions(
            [
                "Microsoft.Testing.Extensions.TrxReport.TestingPlatformBuilderHook",
                "Coverlet.MTP.TestingPlatformBuilderHook",
                "GitHubActionsTestLogger.MtpIntegration",
                "Microsoft.Testing.Extensions.HangDump.TestingPlatformBuilderHook"
            ],
            isXunitProject: true);

        // Assert
        extensions.Should().Be(
            TestingPlatformExtensions.TrxReport |
            TestingPlatformExtensions.Coverlet |
            TestingPlatformExtensions.GitHubActionsTestLogger |
            TestingPlatformExtensions.Xunit);
    }

    [Fact]
    public void The_trx_report_extension_writes_a_trx_file_named_after_the_project()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(TestingPlatformExtensions.TrxReport | TestingPlatformExtensions.Xunit, LocalRun);

        // Assert
        settings.GetArguments().Should().Equal("test", "--", "--report-trx", "--report-trx-filename", "A.Specs.trx");
    }

    [Fact]
    public void An_xunit_project_without_the_trx_report_extension_writes_its_own_trx_file()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(TestingPlatformExtensions.Xunit, LocalRun);

        // Assert
        settings.GetArguments().Should().Equal("test", "--", "--report-xunit-trx", "--report-xunit-trx-filename", "A.Specs.trx");
    }

    [Fact]
    public void A_project_without_a_trx_report_gets_no_trx_option()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(TestingPlatformExtensions.None, LocalRun);

        // Assert
        settings.GetArguments().Should().Equal("test");
    }

    [Fact]
    public void Coverlet_writes_cobertura_files_named_after_the_project()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(
            TestingPlatformExtensions.Coverlet | TestingPlatformExtensions.CodeCoverage,
            LocalRun with { CollectCoverage = true });

        // Assert
        settings.GetArguments().Should().Equal(
            "test", "--", "--coverlet", "--coverlet-output-format", "cobertura",
            "--coverlet-exclude-by-file", "**/*.Generated.cs", "--coverlet-file-prefix", "A.Specs");
    }

    [Fact]
    public void Coverlet_also_writes_the_teamcity_format_on_teamcity()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(
            TestingPlatformExtensions.Coverlet,
            LocalRun with { CollectCoverage = true, OnTeamCity = true });

        // Assert
        settings.CoverletOutputFormats.Should().Equal(
            DotNetTestCoverletOutputFormat.cobertura,
            DotNetTestCoverletOutputFormat.teamcity);
    }

    [Fact]
    public void Microsoft_code_coverage_writes_a_cobertura_file_named_after_the_project()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(TestingPlatformExtensions.CodeCoverage, LocalRun with { CollectCoverage = true });

        // Assert
        settings.GetArguments().Should().Equal(
            "test", "--", "--coverage", "--coverage-output-format", "cobertura", "--coverage-output", "A.Specs.cobertura.xml");
    }

    [Fact]
    public void Coverage_is_collected_only_when_asked()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(TestingPlatformExtensions.Coverlet, LocalRun);

        // Assert
        settings.Coverlet.Should().BeNull();
    }

    [Fact]
    public void The_github_reporter_is_added_only_on_github_actions()
    {
        // Act
        DotNetTestMtpSettings local = Apply(TestingPlatformExtensions.GitHubActionsTestLogger, LocalRun);
        DotNetTestMtpSettings onGitHub = Apply(
            TestingPlatformExtensions.GitHubActionsTestLogger,
            LocalRun with { OnGitHubActions = true });

        // Assert
        local.TestApplicationArguments.Should().BeNull();
        onGitHub.TestApplicationArguments.Should().Equal("--report-github");
    }

    [Fact]
    public void The_microsoft_github_report_is_preferred_over_the_test_logger()
    {
        // Act
        DotNetTestMtpSettings settings = Apply(
            TestingPlatformExtensions.GitHubActionsReport | TestingPlatformExtensions.GitHubActionsTestLogger,
            LocalRun with { OnGitHubActions = true });

        // Assert
        settings.GetArguments().Should().Equal("test", "--", "--report-gh");
    }

    [Fact]
    public void The_azure_devops_report_is_added_only_on_azure_pipelines()
    {
        // Act
        DotNetTestMtpSettings local = Apply(TestingPlatformExtensions.AzureDevOpsReport, LocalRun);
        DotNetTestMtpSettings onAzure = Apply(
            TestingPlatformExtensions.AzureDevOpsReport,
            LocalRun with { OnAzurePipelines = true });

        // Assert
        local.ReportAzureDevOps.Should().BeNull();
        onAzure.ReportAzureDevOps.Should().BeTrue();
    }

    private static DotNetTestMtpSettings Apply(TestingPlatformExtensions extensions, TestingPlatformRun run)
        => TestingPlatform.ApplyProject(new DotNetTestMtpSettings(), "A.Specs", extensions, run);
}
