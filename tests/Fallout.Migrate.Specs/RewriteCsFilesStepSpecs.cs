using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Common.IO;
using Fallout.Migrate.Common;
using Fallout.Migrate.Steps;
using FluentAssertions;
using Xunit;

namespace Fallout.Migrate.Specs;

public class RewriteCsFilesStepSpecs : IDisposable
{
    private readonly AbsolutePath tempDirectory;
    private readonly MigrationContext context;
    private readonly Summary summary = new();

    public RewriteCsFilesStepSpecs()
    {
        tempDirectory = AbsolutePath.Temp("fallout-migrate-test");
        context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null);
    }

    public void Dispose()
    {
        tempDirectory.DeleteDirectory();
    }

    [Fact]
    public async Task Nuke_using_directives_are_rewritten_to_fallout()
    {
        (tempDirectory / "Build.cs").WriteAllText("""
                                                  using Nuke.Common;
                                                  using Nuke.Common.IO;
                                                  using Fallout.Common;
                                                  """);

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(2);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText();
        buildCs.Should().Contain("using Fallout.Common;");
        buildCs.Should().Contain("using Fallout.Common.IO;");
    }

    [Fact]
    public async Task Qualified_nuke_type_references_are_rewritten_to_fallout()
    {
        (tempDirectory / "Build.cs").WriteAllText("var x = new Nuke.Common.Tools.DotNet.DotNetTasks();");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("var x = new Fallout.Common.Tools.DotNet.DotNetTasks();");
    }

    [Fact]
    public async Task Nuke_inside_another_dotted_name_is_left_alone()
    {
        // Arrange
        const string input = """
                             using Acme.Nuke.Tools;
                             using Nuke.Common;
                             """;

        (tempDirectory / "Build.cs").WriteAllText(input, eofLineBreak: false);

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        // Assert
        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText();
        buildCs.Should().Contain("using Acme.Nuke.Tools;");
        buildCs.Should().Contain("using Fallout.Common;");
    }

    [Fact]
    public async Task Project_model_namespace_inside_another_dotted_name_is_left_alone()
    {
        // Arrange
        const string input = """
                             using Acme.Nuke.Common.ProjectModel;
                             using Nuke.Common.ProjectModel;
                             """;

        (tempDirectory / "Build.cs").WriteAllText(input, eofLineBreak: false);

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        // Assert
        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText();
        buildCs.Should().Contain("using Acme.Nuke.Common.ProjectModel;");
        buildCs.Should().Contain("using Fallout.Solutions;");
    }

    [Theory]
    [InlineData("bin")]
    [InlineData("obj")]
    [InlineData(".git")]
    public async Task Files_in_an_ignored_folder_below_the_root_are_skipped(string folder)
    {
        // Arrange
        const string input = "using Nuke.Common;";
        var ignoredFile = tempDirectory / "src" / folder / "Gen.cs";
        ignoredFile.WriteAllText(input, eofLineBreak: false);

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        // Assert
        summary.EditCount.Should().Be(0);
        ignoredFile.ReadAllText().Should().Be(input);
    }

    [Fact]
    public async Task Files_are_rewritten_when_the_repository_sits_under_a_bin_folder()
    {
        // Arrange
        var repository = tempDirectory / "bin" / "repo";
        (repository / "Build.cs").WriteAllText("using Nuke.Common;");
        var repositoryContext = new MigrationContext(repository, dryRun: false, TextWriter.Null);

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(repositoryContext, summary);

        // Assert
        summary.EditCount.Should().Be(1);
        (repository / "Build.cs").ReadAllText().Should().Contain("using Fallout.Common;");
    }

    [Fact]
    public async Task NukeBuild_base_type_is_renamed_to_FalloutBuild()
    {
        (tempDirectory / "Build.cs").WriteAllText("class Build : NukeBuild { }");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("class Build : FalloutBuild { }");
    }

    [Fact]
    public async Task INukeBuild_interface_is_renamed_to_IFalloutBuild()
    {
        (tempDirectory / "Build.cs").WriteAllText("public static int IsApplicable(INukeBuild build) => 0;");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("public static int IsApplicable(IFalloutBuild build) => 0;");
    }

    [Fact]
    public async Task Nuke_as_part_of_another_identifier_is_not_matched()
    {
        // A type like `NukeAdjacentThing` must not match `\bNukeBuild\b`.
        const string input = "var x = new NukeBuilderXYZ();";
        (tempDirectory / "Build.cs").WriteAllText(input);

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(0);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be(input);
    }

    [Fact]
    public async Task Lowercase_nuke_prefix_in_a_path_is_not_matched()
    {
        // ".nuke/foo" filenames stay as-is — handled by ScriptRewriter / DirectoryRenamer.
        const string input = """var path = "/repo/.nuke/parameters.json";""";
        (tempDirectory / "Build.cs").WriteAllText(input);

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(0);
    }

    [Fact]
    public async Task Nuke_project_model_using_is_rewritten_to_solutions()
    {
        // The solution types moved to Fallout.Solutions in v11 — a NUKE-era
        // `using` must land there, not on the dead Fallout.Common.ProjectModel.
        (tempDirectory / "Build.cs").WriteAllText("using Nuke.Common.ProjectModel;");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("using Fallout.Solutions;");
    }

    [Fact]
    public async Task Qualified_nuke_project_model_type_is_rewritten_to_solutions()
    {
        (tempDirectory / "Build.cs").WriteAllText("Nuke.Common.ProjectModel.Solution x;");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("Fallout.Solutions.Solution x;");
    }

    [Fact]
    public async Task Already_partially_migrated_project_model_namespace_is_salvaged()
    {
        // Code previously run through a prefix-only migrator lands on the dead
        // Fallout.Common.ProjectModel; the ProjectModel rule salvages it.
        (tempDirectory / "Build.cs").WriteAllText("using Fallout.Common.ProjectModel;");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("using Fallout.Solutions;");
    }

    [Fact]
    public async Task ProjectModel_as_part_of_another_identifier_is_not_truncated()
    {
        // `ProjectModelFoo` must not be truncated by the ProjectModel rule.
        (tempDirectory / "Build.cs").WriteAllText("using Nuke.Common.ProjectModelFoo;");

        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("using Fallout.Common.ProjectModelFoo;");
    }

    [Fact]
    public async Task IHaz_component_interfaces_are_renamed_to_IHas()
    {
        // Arrange
        (tempDirectory / "Build.cs").WriteAllText("""
                                                  interface IBuildPaths : IHazSolution, IHazArtifacts;
                                                  var solution = From<IHazSolution>().Solution;
                                                  """);

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        // Assert
        summary.EditCount.Should().Be(3);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText();
        buildCs.Should().Contain("IBuildPaths : IHasSolution, IHasArtifacts;");
        buildCs.Should().Contain("From<IHasSolution>()");
        buildCs.Should().NotContain("IHaz");
    }

    [Fact]
    public async Task User_defined_IHaz_interfaces_keep_their_name()
    {
        // Arrange
        (tempDirectory / "Build.cs").WriteAllText("interface IHazSourcePaths : IHazSolution;");

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        // Assert
        summary.EditCount.Should().Be(1);
        var buildCs = (tempDirectory / "Build.cs").ReadAllText().Trim();
        buildCs.Should().Be("interface IHazSourcePaths : IHasSolution;");
    }

    [Fact]
    public async Task Every_IHaz_alias_in_the_Nuke_Components_shim_is_renamed()
    {
        // The rename list is hard-coded. This spec fails when the shim gains an IHaz* alias
        // that the list doesn't have.
        // Arrange
        var shimDirectory = AbsolutePath.Create(AppContext.BaseDirectory)
            .FindParentOrSelf(x => (x / "fallout.slnx").FileExists()) / "src" / "Shims" / "Nuke.Components";
        var aliases = shimDirectory.GetFiles("*.cs")
            .SelectMany(x => Regex.Matches(x.ReadAllText(), @"\binterface\s+(?<name>IHaz\w+)"))
            .Select(x => x.Groups["name"].Value)
            .ToList();
        (tempDirectory / "Build.cs").WriteAllText(string.Join("\n", aliases), eofLineBreak: false);

        // Act
        await new RewriteCsFilesStep().ExecuteAsync(context, summary);

        // Assert
        aliases.Should().NotBeEmpty();
        (tempDirectory / "Build.cs").ReadAllText()
            .Should().Be(string.Join("\n", aliases.Select(x => "IHas" + x["IHaz".Length..])));
    }
}
