using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Fallout.Common;
using Fallout.Common.IO;

partial class Build
{
    readonly AbsolutePath CompressionDir = AbsolutePath.Temp("compression");
    readonly AbsolutePath CompressionSourceDir = TemporaryDirectory / "CompressMe";

    Target SetupCompression => _ => _
        .Triggers(CleanupCompression)
        .Executes(() =>
        {
            CompressionDir.CreateOrCleanDirectory();
            CompressionSourceDir.CreateOrCleanDirectory();

            GenerateFiles(
                rootDirectory: CompressionSourceDir,
                fileCount: 100,
                maximumDirectoryDepth: 10);
        });

    Target RunCompressionTests => _ => _
        .DependsOn(UncompressZip, UncompressTarGz, UncompressTarBZip2);

    Target CleanupCompression => _ => _
        .After(UncompressZip, UncompressTarGz, UncompressTarBZip2)
        .Executes(() =>
        {
            CompressionDir.DeleteDirectory();
            CompressionSourceDir.DeleteDirectory();
        });

    Target CompressZip => _ => _
        .DependsOn(SetupCompression)
        .Executes(() =>
        {
            AbsolutePath archive = CompressionDir / $"{nameof(CompressZip)}.zip";
            AbsolutePath directory = CompressionSourceDir;
            directory.ZipTo(archive, compressionLevel: CompressionLevel.SmallestSize);
        });

    Target CompressTarGz => _ => _
        .DependsOn(SetupCompression)
        .Executes(() =>
        {
            AbsolutePath archive = CompressionDir / $"{nameof(CompressTarGz)}.tar.gz";
            AbsolutePath directory = CompressionSourceDir;
            directory.TarGZipTo(archive);
        });

    Target CompressBZip2 => _ => _
        .DependsOn(SetupCompression)
        .Executes(() =>
        {
            AbsolutePath archive = CompressionDir / $"{nameof(CompressBZip2)}.tar.bz2";
            AbsolutePath directory = CompressionSourceDir;
            directory.TarBZip2To(archive);
        });

    Target UncompressZip => _ => _
        .DependsOn(CompressZip)
        .Executes(() =>
        {
            AbsolutePath archive = CompressionDir / $"{nameof(CompressZip)}.zip";
            AbsolutePath directory = CompressionDir / nameof(UncompressZip);
            archive.UnZipTo(directory);
        });

    Target UncompressTarGz => _ => _
        .DependsOn(CompressTarGz)
        .Executes(() =>
        {
            AbsolutePath archive = CompressionDir / $"{nameof(CompressTarGz)}.tar.gz";
            AbsolutePath directory = CompressionDir / nameof(UncompressTarGz);
            archive.UncompressTo(directory);
        });

    Target UncompressTarBZip2 => _ => _
        .DependsOn(CompressBZip2)
        .Executes(() =>
        {
            AbsolutePath archive = CompressionDir / $"{nameof(CompressBZip2)}.tar.bz2";
            AbsolutePath directory = CompressionDir / nameof(UncompressTarBZip2);
            archive.UncompressTo(directory);
        });

    private static void GenerateFiles(AbsolutePath rootDirectory, int fileCount, int maximumDirectoryDepth)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(fileCount);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumDirectoryDepth);

        rootDirectory.CreateOrCleanDirectory();

        for (int i = 1; i <= fileCount; i++)
        {
            var targetDirectory = CreateRandomDirectoryPath(rootDirectory, maximumDirectoryDepth);

            AbsolutePath filePath = targetDirectory / $"file_{i:D3}_{Guid.NewGuid():N}.txt";

            filePath.WriteAllText(GenerateRandomContent(minimumLength: 100, maximumLength: 1_000), Encoding.UTF8);
        }
    }

    private static AbsolutePath CreateRandomDirectoryPath(AbsolutePath rootDirectory, int maximumDepth)
    {
        // A depth of zero puts the file directly in the root directory.
        int depth = Random.Shared.Next(0, maximumDepth + 1);
        var currentDirectory = rootDirectory;

        for (int level = 0; level < depth; level++)
        {
            string folderName =
                $"folder_{level + 1}_{Random.Shared.Next(1, 21):D2}";

            currentDirectory /= folderName;
        }

        return currentDirectory.CreateOrCleanDirectory();
    }

    private static string GenerateRandomContent(int minimumLength, int maximumLength)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        int length = Random.Shared.Next(minimumLength, maximumLength + 1);

        var content = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            content.Append(characters[Random.Shared.Next(characters.Length)]);
        }

        return content.ToString();
    }
}
