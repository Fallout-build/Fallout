using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using Fallout.Common.Utilities.Collections;

namespace Fallout.Common.IO;

public static class CompressionExtensions
{
    public static void CompressTo(this AbsolutePath directory, AbsolutePath archiveFile, Func<AbsolutePath, bool> filter = null)
    {
        if (archiveFile.HasExtension(".zip"))
            directory.ZipTo(archiveFile, filter);
        else if (archiveFile.HasExtension(".tar.gz", ".tgz"))
            directory.TarGZipTo(archiveFile, filter);
        else if (archiveFile.HasExtension(".tar.bz2", ".tbz2", ".tbz"))
            directory.TarBZip2To(archiveFile, filter);
        else
            Assert.Fail($"Unknown archive extension for archive '{Path.GetFileName(archiveFile)}'");
    }

    public static void UncompressTo(this AbsolutePath archiveFile, AbsolutePath directory)
    {
        if (archiveFile.HasExtension(".zip"))
            archiveFile.UnZipTo(directory);
        else if (archiveFile.HasExtension(".tar.gz", ".tgz"))
            archiveFile.UnTarGZipTo(directory);
        else if (archiveFile.HasExtension(".tar.bz2", ".tbz2", ".tbz"))
            archiveFile.UnTarBZip2To(directory);
        else
            Assert.Fail($"Unknown archive extension for archive '{Path.GetFileName(archiveFile)}'");
    }

    public static void ZipTo(
        this AbsolutePath directory,
        AbsolutePath archiveFile,
        Func<AbsolutePath, bool> filter = null,
        CompressionLevel compressionLevel = CompressionLevel.Optimal,
        FileMode fileMode = FileMode.CreateNew)
    {
        archiveFile.Parent.CreateDirectory();

        filter ??= _ => true;
        var files = directory.GetFiles(depth: int.MaxValue).Where(filter).ToList();

        using var fileStream = File.Open(archiveFile, fileMode, FileAccess.ReadWrite);
        using var writer = WriterFactory.OpenWriter(
            fileStream,
            ArchiveType.Zip,
            new ZipWriterOptions(CompressionType.Deflate, compressionLevel.ToSharpCompressCompressionLevel()));

        void AddFile(AbsolutePath file)
        {
            var entryName = directory.GetUnixRelativePathTo(file);
            writer.Write(entryName, file);
        }

        files.ForEach(AddFile);
    }

    public static void UnZipTo(this AbsolutePath archiveFile, AbsolutePath directory)
    {
        UncompressArchive(archiveFile, directory);
    }

    public static void TarGZipTo(
        this AbsolutePath baseDirectory,
        AbsolutePath archiveFile,
        IEnumerable<AbsolutePath> files,
        FileMode fileMode = FileMode.CreateNew)
    {
        CompressTar(baseDirectory, archiveFile, [.. files], fileMode, CompressionType.GZip);
    }

    public static void TarGZipTo(
        this AbsolutePath directory,
        AbsolutePath archiveFile,
        Func<AbsolutePath, bool> filter = null,
        FileMode fileMode = FileMode.CreateNew)
    {
        filter ??= _ => true;
        var files = directory.GetFiles(depth: int.MaxValue).Where(filter);
        directory.TarGZipTo(archiveFile, files, fileMode);
    }

    public static void TarBZip2To(
        this AbsolutePath directory,
        AbsolutePath archiveFile,
        IEnumerable<AbsolutePath> files,
        FileMode fileMode = FileMode.CreateNew)
    {
        CompressTar(directory, archiveFile, [.. files], fileMode, CompressionType.BZip2);
    }

    public static void TarBZip2To(
        this AbsolutePath directory,
        AbsolutePath archiveFile,
        Func<AbsolutePath, bool> filter = null,
        FileMode fileMode = FileMode.CreateNew)
    {
        filter ??= _ => true;
        var files = directory.GetFiles(depth: int.MaxValue).Where(filter);
        directory.TarBZip2To(archiveFile, files, fileMode);
    }

    public static void UnTarGZipTo(this AbsolutePath archiveFile, AbsolutePath directory)
    {
        UncompressArchive(archiveFile, directory);
    }

    public static void UnTarBZip2To(this AbsolutePath archiveFile, AbsolutePath directory)
    {
        UncompressArchive(archiveFile, directory);
    }

    private static void CompressTar(
        AbsolutePath baseDirectory,
        AbsolutePath archiveFile,
        IReadOnlyCollection<AbsolutePath> files,
        FileMode fileMode,
        CompressionType compressionType)
    {
        archiveFile.Parent.CreateDirectory();
        using var fileStream = File.Open(archiveFile, fileMode, FileAccess.ReadWrite);
        using var writer = WriterFactory.OpenWriter(fileStream, ArchiveType.Tar, new WriterOptions(compressionType));

        void AddFile(AbsolutePath file)
        {
            var entryName = baseDirectory.GetUnixRelativePathTo(file);
            // ReSharper disable once AccessToDisposedClosure
            writer.Write(entryName, file);
        }

        files.ForEach(AddFile);
    }

    private static SharpCompress.Compressors.Deflate.CompressionLevel ToSharpCompressCompressionLevel(
        this CompressionLevel compressionLevel)
    {
        return compressionLevel switch
        {
            CompressionLevel.NoCompression => SharpCompress.Compressors.Deflate.CompressionLevel.None,
            CompressionLevel.Fastest => SharpCompress.Compressors.Deflate.CompressionLevel.BestSpeed,
            CompressionLevel.Optimal => SharpCompress.Compressors.Deflate.CompressionLevel.Default,
            _ => SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression
        };
    }

    private static void UncompressArchive(AbsolutePath archiveFile, AbsolutePath directory)
    {
        using var fileStream = File.OpenRead(archiveFile);
        using var reader = ReaderFactory.OpenReader(fileStream);

        directory.CreateDirectory();

        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory)
                continue;

            reader.WriteEntryToDirectory(directory, new ExtractionOptions
            {
                ExtractFullPath = true,
                Overwrite = true
            });
        }
    }
}
