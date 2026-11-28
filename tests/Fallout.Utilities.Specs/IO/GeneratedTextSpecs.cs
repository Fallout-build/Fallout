using System;
using System.IO;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public sealed class GeneratedTextSpecs : IDisposable
{
    private const string MixedContent = "a\r\nb\nc\n";

    private readonly AbsolutePath root = AbsolutePath.Temp("generated-text").CreateDirectory();

    private AbsolutePath TextFile => root / "file.txt";

    public void Dispose()
    {
        root.DeleteDirectory();
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Generated_text_takes_the_line_break_of_the_existing_file(string lineBreak)
    {
        // Arrange
        File.WriteAllText(TextFile, $"old{lineBreak}");

        // Act
        TextFile.WriteGeneratedText(MixedContent);

        // Assert
        File.ReadAllText(TextFile).Should().Be($"a{lineBreak}b{lineBreak}c{lineBreak}");
    }

    [Fact]
    public void Generated_text_in_a_new_file_has_LF_line_breaks()
    {
        // Act
        TextFile.WriteGeneratedText(MixedContent);

        // Assert
        File.ReadAllText(TextFile).Should().Be("a\nb\nc\n");
    }

    [Fact]
    public void A_file_without_a_line_break_counts_as_LF()
    {
        // Arrange
        File.WriteAllText(TextFile, "old");

        // Act
        TextFile.WriteGeneratedText(MixedContent);

        // Assert
        File.ReadAllText(TextFile).Should().Be("a\nb\nc\n");
    }
}
