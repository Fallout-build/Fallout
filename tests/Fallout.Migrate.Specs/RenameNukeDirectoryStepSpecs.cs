using System;
using System.IO;
using System.Threading.Tasks;
using Fallout.Common.IO;
using Fallout.Migrate.Common;
using Fallout.Migrate.Steps;
using FluentAssertions;
using Xunit;

namespace Fallout.Migrate.Specs;

public class RenameNukeDirectoryStepSpecs : IDisposable
{
    private const string NukeSchema = """
                                      {
                                        "definitions": {
                                          "Host": { "type": "string" },
                                          "NukeBuild": { "properties": { "Verbosity": { "type": "string" } } }
                                        },
                                        "allOf": [
                                          { "properties": { "Configuration": { "type": "string" } } },
                                          { "$ref": "#/definitions/NukeBuild" }
                                        ]
                                      }
                                      """;

    private readonly AbsolutePath tempDirectory;
    private readonly Summary summary = new();

    public RenameNukeDirectoryStepSpecs()
    {
        tempDirectory = AbsolutePath.Temp("fallout-migrate-test");
    }

    public void Dispose()
    {
        tempDirectory.DeleteDirectory();
    }

    [Fact]
    public async Task Moved_schema_names_its_base_definition_FalloutBuild()
    {
        // Arrange
        (tempDirectory / ".nuke" / "build.schema.json").WriteAllText(NukeSchema, eofLineBreak: false);
        var context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null);

        // Act
        await new RenameNukeDirectoryStep().ExecuteAsync(context, summary);

        // Assert
        summary.DirectoriesRenamed.Should().Be(1);
        summary.EditCount.Should().Be(2);
        var schema = (tempDirectory / ".fallout" / "build.schema.json").ReadAllText();
        schema.Should().Contain("\"FalloutBuild\": {");
        schema.Should().Contain("\"$ref\": \"#/definitions/FalloutBuild\"");
        schema.Should().NotContain("NukeBuild");
    }

    [Fact]
    public async Task Schema_of_an_already_migrated_repo_is_repaired()
    {
        // Arrange
        (tempDirectory / ".fallout" / "build.schema.json").WriteAllText(NukeSchema, eofLineBreak: false);
        var context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null);

        // Act
        await new RenameNukeDirectoryStep().ExecuteAsync(context, summary);

        // Assert
        summary.DirectoriesRenamed.Should().Be(0);
        summary.EditCount.Should().Be(2);
        (tempDirectory / ".fallout" / "build.schema.json").ReadAllText().Should().NotContain("NukeBuild");
    }

    [Fact]
    public async Task Only_the_fallout_schema_is_rewritten_when_both_directories_exist()
    {
        // Arrange
        (tempDirectory / ".nuke" / "build.schema.json").WriteAllText(NukeSchema, eofLineBreak: false);
        (tempDirectory / ".fallout" / "build.schema.json").WriteAllText(NukeSchema, eofLineBreak: false);
        var context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null);

        // Act
        await new RenameNukeDirectoryStep().ExecuteAsync(context, summary);

        // Assert
        summary.DirectoriesRenamed.Should().Be(0);
        summary.EditCount.Should().Be(2);
        summary.Warnings.Should().ContainSingle().Which.Should().Contain("merge their contents manually");
        (tempDirectory / ".fallout" / "build.schema.json").ReadAllText().Should().NotContain("NukeBuild");
        (tempDirectory / ".nuke" / "build.schema.json").ReadAllText().Should().Be(NukeSchema);
    }

    [Fact]
    public async Task Dry_run_leaves_the_schema_unchanged()
    {
        // Arrange
        (tempDirectory / ".nuke" / "build.schema.json").WriteAllText(NukeSchema, eofLineBreak: false);
        var context = new MigrationContext(tempDirectory, dryRun: true, TextWriter.Null);

        // Act
        await new RenameNukeDirectoryStep().ExecuteAsync(context, summary);

        // Assert
        summary.EditCount.Should().Be(2);
        (tempDirectory / ".nuke" / "build.schema.json").ReadAllText().Should().Be(NukeSchema);
        (tempDirectory / ".fallout").DirectoryExists().Should().BeFalse();
    }
}
