namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Exceptions;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class OsdbCollectionHandler
{
    private static readonly Dictionary<string, int> _versions = new()
    {
        {"o!dm", 1},
        {"o!dm2", 2},
        {"o!dm3", 3},
        {"o!dm4", 4},
        {"o!dm5", 5},
        {"o!dm6", 6},
        {"o!dm7", 7},
        {"o!dm8", 8},
        {"o!dm7min", 1007},
        {"o!dm8min", 1008},
    };

    public static string CurrentVersion(bool minimalWrite = false) => "o!dm8" + (minimalWrite ? "min" : "");

    private static bool IsFullCollection(string versionString)
        => !isMinimalCollection(versionString);
    private static bool isMinimalCollection(string versionString)
        => versionString.EndsWith("min");

    public static void WriteOsdb(OsuCollections collections, string fullFileDir, string editor, bool minimalWrite = false)
    {
        using FileStream fileStream = new(fullFileDir, FileMode.Create, FileAccess.ReadWrite);
        WriteOsdb(collections, fileStream, editor, minimalWrite);
    }

    public static void WriteOsdb(OsuCollections collections, Stream outputStream, string editor, bool minimalWrite = false)
    {
        using MemoryStream osdbMemoryStream = new();
        using BinaryWriter osdbBinaryWriter = new(osdbMemoryStream);
        WriteOsdb(collections, osdbBinaryWriter, editor, minimalWrite);

        using BinaryWriter outputBinaryWriter = new(outputStream, Encoding.UTF8, true);
        outputBinaryWriter.Write(CurrentVersion(minimalWrite));
        CompressStream(osdbMemoryStream, outputStream);
    }

    public static void CompressStream(Stream inputStream, Stream outputStream)
    {
        using GZipArchive archive = GZipArchive.Create();
        long inputLength = inputStream.Position;
        inputStream.Position = 0;
        _ = archive.AddEntry("uncompressed_collection_NOTosdb.NOTosdb", inputStream, inputLength, DateTime.UtcNow);
        archive.SaveTo(outputStream, new WriterOptions(CompressionType.GZip));
    }
    private static void WriteOsdb(OsuCollections collections, BinaryWriter _binWriter, string editor,
        bool minimalWrite = false)
    {
        //header
        _binWriter.Write(CurrentVersion(minimalWrite));
        //save date
        _binWriter.Write(DateTime.Now.ToOADate());
        //who saved given osdb
        _binWriter.Write(editor);
        //number of collections stored in osdb
        _binWriter.Write(collections.Count);
        //bool ignoreMissingMaps = false;
        foreach (IOsuCollection collection in collections)
        {
            Beatmaps beatmapsPossibleToSave = [];
            HashSet<string> beatmapWithHashOnly = [];

            foreach (Beatmap beatmap in collection.KnownBeatmaps)
            {
                beatmapsPossibleToSave.Add(beatmap);
            }

            foreach (Beatmap beatmap in collection.DownloadableBeatmaps)
            {
                beatmapsPossibleToSave.Add(beatmap);
            }

            foreach (Beatmap partialBeatmap in collection.UnknownBeatmaps)
            {
                if (partialBeatmap.TitleRoman != "" || partialBeatmap.MapSetId > 0)
                {
                    beatmapsPossibleToSave.Add(partialBeatmap);
                }
                else
                {
                    _ = beatmapWithHashOnly.Add(partialBeatmap.Md5);
                }
            }

            _binWriter.Write(collection.Name);
            _binWriter.Write(collection.OnlineId);
            _binWriter.Write(beatmapsPossibleToSave.Count);
            //Save beatmaps
            foreach (Beatmap beatmap in beatmapsPossibleToSave)
            {
                _binWriter.Write(beatmap.MapId);
                _binWriter.Write(beatmap.MapSetId);
                if (!minimalWrite)
                {
                    _binWriter.Write(beatmap.ArtistRoman);
                    _binWriter.Write(beatmap.TitleRoman);
                    _binWriter.Write(beatmap.DiffName);
                }

                _binWriter.Write(beatmap.Md5);
                _binWriter.Write(((BeatmapExtension)beatmap).UserComment);
                _binWriter.Write((byte)beatmap.PlayMode);
                _binWriter.Write(beatmap.StarsNomod);
            }

            _binWriter.Write(beatmapWithHashOnly.Count);
            foreach (string beatmapHash in beatmapWithHashOnly)
            {
                _binWriter.Write(beatmapHash);
            }
        }

        _binWriter.Write("By Piotrekol");
    }

    public static IEnumerable<OsuCollection> ReadOsdb(string fullFileDir, MapCacher mapCacher)
    {
        BinaryReader reader;
        using (FileStream fileStream = new(fullFileDir, FileMode.Open, FileAccess.Read))
        {
            MemoryStream memoryStream = new();
            fileStream.CopyTo(memoryStream);
            reader = new BinaryReader(memoryStream);
        }

        _ = reader.BaseStream.Seek(0, SeekOrigin.Begin);
        int fileVersion = -1;

        string versionString = reader.ReadString();
        //check header
        if (_versions.TryGetValue(versionString, out int value))
        {
            fileVersion = value;
        }

        try
        {
            if (fileVersion == -1)
            {
                throw new CorruptedFileException($"Unrecognized osdb file version (got: {versionString})");
            }
            else
            {
                if (fileVersion >= 7)
                {
                    BinaryReader previousReader = reader;
                    reader = StartReadingFirstFileInArchive(reader.BaseStream);
                    previousReader.Dispose();
                    _ = reader.ReadString(); //version string
                }

                DateTime fileDate = DateTime.FromOADate(reader.ReadDouble());
                string lastEditor = reader.ReadString();
                int numberOfCollections = reader.ReadInt32();

                for (int i = 0; i < numberOfCollections; i++)
                {
                    string name = reader.ReadString();
                    int onlineId = -1;
                    if (fileVersion >= 7)
                    {
                        onlineId = reader.ReadInt32();
                    }

                    int numberOfBeatmaps = reader.ReadInt32();
                    OsuCollection collection = new(mapCacher)
                    { Name = name, LastEditorUsername = lastEditor, OnlineId = onlineId };
                    for (int j = 0; j < numberOfBeatmaps; j++)
                    {
                        BeatmapExtension map = new()
                        {
                            MapId = reader.ReadInt32()
                        };
                        if (fileVersion >= 2)
                        {
                            map.MapSetId = reader.ReadInt32();
                        }

                        if (!isMinimalCollection(versionString))
                        {
                            map.ArtistRoman = reader.ReadString();
                            map.TitleRoman = reader.ReadString();
                            map.DiffName = reader.ReadString();
                        }

                        map.Md5 = reader.ReadString();
                        if (fileVersion >= 4)
                        {
                            map.UserComment = reader.ReadString();
                        }

                        if (fileVersion >= 8 || (fileVersion >= 5 && IsFullCollection(versionString)))
                        {
                            map.PlayMode = (PlayMode)reader.ReadByte();
                        }

                        if (fileVersion >= 8 || (fileVersion >= 6 && IsFullCollection(versionString)))
                        {
                            double starRating = reader.ReadDouble();
                            if (starRating >= float.Epsilon)
                            {
                                map.ModPpStars.Add(map.PlayMode, new StarRating { { 0, (float)starRating } });
                            }
                        }

                        collection.AddBeatmap(map);
                    }

                    if (fileVersion >= 3)
                    {
                        int numberOfMapHashes = reader.ReadInt32();
                        for (int j = 0; j < numberOfMapHashes; j++)
                        {
                            string hash = reader.ReadString();
                            collection.AddBeatmapByHash(hash);
                        }
                    }

                    yield return collection;
                }
            }

            if (reader.ReadString() != "By Piotrekol")
            {
                throw new CorruptedFileException("File footer is invalid, this collection might be corrupted.");
            }
        }
        finally
        {
            if (fileVersion >= 7)
            {
                reader.Dispose();
            }
        }
    }

    private static BinaryReader StartReadingFirstFileInArchive(Stream baseStream)
    {
        using GZipArchive archiveReader = GZipArchive.Open(baseStream);
        MemoryStream memStream = new();
        archiveReader.Entries.First().WriteTo(memStream);
        memStream.Position = 0;
        return new BinaryReader(memStream);
    }
}