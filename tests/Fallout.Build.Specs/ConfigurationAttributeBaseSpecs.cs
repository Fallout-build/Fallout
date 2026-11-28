using System;
using System.Collections.Generic;
using System.IO;
using Fallout.Common.CI;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using Fallout.Common.Utilities;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public sealed class ConfigurationAttributeBaseSpecs : IDisposable
{
    private readonly AbsolutePath root = AbsolutePath.Temp("configuration-line-breaks").CreateDirectory();

    private AbsolutePath ConfigurationFile => root / "build.yml";

    public void Dispose()
    {
        root.DeleteDirectory();
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Regenerating_keeps_the_line_breaks_of_the_existing_file(string lineBreak)
    {
        // Arrange
        File.WriteAllText(ConfigurationFile, $"# old{lineBreak}name: old{lineBreak}");

        // Act
        Generate();

        // Assert
        var text = File.ReadAllText(ConfigurationFile);
        text.Should().Contain("name: build").And.Be(text.ReplaceLineEndings(lineBreak));
    }

    [Fact]
    public void A_new_configuration_file_gets_LF_line_breaks()
    {
        // Act
        Generate();

        // Assert
        File.ReadAllText(ConfigurationFile).Should().NotContain("\r");
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Regenerating_unchanged_configuration_keeps_the_file_hash(string lineBreak)
    {
        // Arrange
        Generate();
        File.WriteAllText(ConfigurationFile, File.ReadAllText(ConfigurationFile).ReplaceLineEndings(lineBreak));
        var hash = ConfigurationFile.GetFileHash();

        // Act
        Generate();

        // Assert
        ConfigurationFile.GetFileHash().Should().Be(hash);
    }

    private void Generate()
    {
        new FileConfigurationAttribute(ConfigurationFile).Generate(Array.Empty<ExecutableTarget>());
    }

    private sealed class FileConfigurationAttribute(AbsolutePath file) : ConfigurationAttributeBase
    {
        public override Type HostType => typeof(FileConfigurationAttribute);

        public override AbsolutePath ConfigurationFile => file;

        public override IEnumerable<AbsolutePath> GeneratedFiles => [file];

        public override IEnumerable<string> RelevantTargetNames => [];

        public override IEnumerable<string> IrrelevantTargetNames => [];

        public override CustomFileWriter CreateWriter(StreamWriter streamWriter)
        {
            return new CustomFileWriter(streamWriter, indentationFactor: 2, commentPrefix: "#");
        }

        public override ConfigurationEntity GetConfiguration(IReadOnlyCollection<ExecutableTarget> relevantTargets)
        {
            return new Configuration();
        }
    }

    private sealed class Configuration : ConfigurationEntity
    {
        public override void Write(CustomFileWriter writer)
        {
            writer.WriteLine("name: build");
            writer.WriteLine("on: push");
        }
    }
}
