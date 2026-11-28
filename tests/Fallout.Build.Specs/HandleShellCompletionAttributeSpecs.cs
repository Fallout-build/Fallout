using System;
using System.IO;
using Fallout.Build.Execution;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

[Collection(ProcessGlobalStateCollection.Name)]
public sealed class HandleShellCompletionAttributeSpecs : IDisposable
{
    private readonly AbsolutePath root = AbsolutePath.Temp("shell-completion").CreateDirectory();
    private readonly BuildContext context = BuildContext.Activate();
    private readonly SchemaBuild build;

    public HandleShellCompletionAttributeSpecs()
    {
        build = new SchemaBuild(root);
    }

    private AbsolutePath SchemaFile => root / ".fallout" / "build.schema.json";

    private AbsolutePath ParametersFile => root / ".fallout" / "parameters.json";

    public void Dispose()
    {
        context.Dispose();
        root.DeleteDirectory();
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Writing_an_unchanged_schema_leaves_the_existing_file_byte_identical(string lineBreak)
    {
        // Arrange
        SchemaFile.Parent.CreateDirectory();
        File.WriteAllText(SchemaFile, SchemaUtility.GetJsonString(build).ReplaceLineEndings(lineBreak));
        var bytes = File.ReadAllBytes(SchemaFile);

        // Act
        OnBuildCreated();

        // Assert
        File.ReadAllBytes(SchemaFile).Should().Equal(bytes);
    }

    [Fact]
    public void A_new_schema_gets_LF_line_breaks()
    {
        // Act
        OnBuildCreated();

        // Assert
        File.ReadAllText(SchemaFile).Should().Contain("\n").And.NotContain("\r");
    }

    [Fact]
    public void A_new_default_parameters_file_gets_LF_line_breaks()
    {
        // Act
        OnBuildCreated();

        // Assert
        File.ReadAllText(ParametersFile).Should().Contain("\"$schema\"").And.NotContain("\r");
    }

    private void OnBuildCreated()
    {
        new HandleShellCompletionAttribute { Build = build }.OnBuildCreated([]);
    }

    private sealed class SchemaBuild(AbsolutePath root) : FalloutBuild, IFalloutBuild
    {
        AbsolutePath IFalloutBuild.RootDirectory => root;
        AbsolutePath IFalloutBuild.BuildProjectFile => root / "build" / "_build.csproj";
    }
}
