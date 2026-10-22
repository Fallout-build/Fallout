using System;
using System.IO;
using Fallout.Common.IO;
using Fallout.Migrate.Common;
using FluentAssertions;
using Xunit;

namespace Fallout.Migrate.Specs;

public class MigrationFileOperationsSpecs : IDisposable
{
    private readonly AbsolutePath tempDirectory = AbsolutePath.Temp("fallout-migrate-test");

    public MigrationFileOperationsSpecs()
    {
        tempDirectory.CreateDirectory();
    }

    public void Dispose()
    {
        tempDirectory.DeleteDirectory();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Unreadable_files_are_reported_once_and_not_rewritten(bool pathIsDirectory)
    {
        // Arrange
        AbsolutePath path = tempDirectory / "unreadable.props";
        if (pathIsDirectory)
        {
            path.CreateDirectory();
        }

        var context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null);
        var summary = new Summary();
        bool rewriterCalled = false;

        // Act
        MigrationFileOperations.ApplyRewrite(context, path, content =>
        {
            rewriterCalled = true;
            return new RewriteResult(content, 0);
        }, summary);

        // Assert
        rewriterCalled.Should().BeFalse();
        summary.Warnings.Should().ContainSingle().Which.Should().StartWith("could not read unreadable.props:");
        summary.FilesChanged.Should().Be(0);
        summary.EditCount.Should().Be(0);
    }

    [Fact]
    public void Rewrite_failures_propagate_instead_of_becoming_read_warnings()
    {
        // Arrange
        AbsolutePath path = tempDirectory / "Version.props";
        path.WriteAllText("<Project />", eofLineBreak: false);
        var context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null);
        var summary = new Summary();
        var failure = new IOException("rewrite failed");

        // Act
        Action rewrite = () => MigrationFileOperations.ApplyRewrite(context, path, _ => throw failure, summary);

        // Assert
        rewrite.Should().Throw<IOException>().Which.Should().BeSameAs(failure);
        summary.Warnings.Should().BeEmpty();
        path.ReadAllText().Should().Be("<Project />");
    }
}
