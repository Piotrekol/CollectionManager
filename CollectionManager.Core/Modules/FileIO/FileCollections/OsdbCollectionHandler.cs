namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Enums;
using CollectionManager.Core.Exceptions;
using CollectionManager.Core.Interfaces;
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
    //TODO: Make lastEditor per-collection variable, not file-wide.
    //TODO: Better way to do .osdb versioning while allowing all versions to be loaded/saved

    private BinaryReader _binReader;
    private readonly ILogger _logger;
    private MemoryStream _memStream;

    private readonly Dictionary<string, int> _versions = new()
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

    public OsdbCollectionHandler(ILogger logger)
    {
        _logger = logger;
    }

    public string CurrentVersion(bool minimalWrite = false) => "o!dm8" + (minimalWrite ? "min" : "");

    private bool IsFullCollection(string versionString)
        => !isMinimalCollection(versionString);
    private bool isMinimalCollection(string versionString)
        => versionString.EndsWith("min");

    public void WriteOsdb(OsuCollections collections, string fullFileDir, string editor, bool minimalWrite = false)
    {
        using FileStream fileStream = new(fullFileDir, FileMode.Create, FileAccess.ReadWrite);
        WriteOsdb(collections, fileStream, editor, minimalWrite);
    }

    public void WriteOsdb(OsuCollections collections, Stream outputStream, string editor, bool minimalWrite = false)
    {
        using MemoryStream osdbMemoryStream = new();
        using BinaryWriter osdbBinaryWriter = new(osdbMemoryStream);
        WriteOsdb(collections, osdbBinaryWriter, editor, minimalWrite);

        using BinaryWriter outputBinaryWriter = new(outputStream, Encoding.UTF8, true);
        outputBinaryWriter.Write(CurrentVersion(minimalWrite));
        CompressStream(osdbMemoryStream, outputStream);
    }

    public void CompressStream(Stream inputStream, Stream outputStream)
    {
        using GZipArchive archive = GZipArchive.Create();
        long inputLength = inputStream.Position;
        inputStream.Position = 0;
        _ = archive.AddEntry("uncompressed_collection_NOTosdb.NOTosdb", inputStream, inputLength, DateTime.UtcNow);
        archive.SaveTo(outputStream, new WriterOptions(CompressionType.GZip));
    }
    private void WriteOsdb(OsuCollections collections, BinaryWriter _binWriter, string editor,
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

    public IEnumerable<OsuCollection> ReadOsdb(string fullFileDir, MapCacher mapCacher)
    {
        int fileVersion = -1;
        _ = DateTime.Now;

        _ = new OsuCollections();

        using (FileStream fileStream = new(fullFileDir, FileMode.Open, FileAccess.Read))
        {
            _memStream = new MemoryStream();
            fileStream.CopyTo(_memStream);
            _binReader = new BinaryReader(_memStream);
        }

        _ = _binReader.BaseStream.Seek(0, SeekOrigin.Begin);
        string versionString = _binReader.ReadString();
        //check header
        if (_versions.ContainsKey(versionString))
        {
            fileVersion = _versions[versionString];
        }

        if (fileVersion == -1)
        {
            throw new CorruptedFileException($"Unrecognized osdb file version (got: {versionString})");
        }
        else
        {
            if (fileVersion >= 7)
            {
                using GZipArchive archiveReader = GZipArchive.Open(_memStream);
                MemoryStream memStream = new();
                archiveReader.Entries.First().WriteTo(memStream);
                memStream.Position = 0;
                _binReader = new BinaryReader(memStream);
                _ = _binReader.ReadString(); //version string
            }

            _logger?.Log("Starting file load");
            DateTime fileDate = DateTime.FromOADate(_binReader.ReadDouble());
            _logger?.Log(">Date: " + fileDate);
            string lastEditor = _binReader.ReadString();
            _logger?.Log(">LastEditor: " + lastEditor);
            int numberOfCollections = _binReader.ReadInt32();
            _logger?.Log(">Collections: " + numberOfCollections);
            for (int i = 0; i < numberOfCollections; i++)
            {
                string name = _binReader.ReadString();
                int onlineId = -1;
                if (fileVersion >= 7)
                {
                    onlineId = _binReader.ReadInt32();
                }

                int numberOfBeatmaps = _binReader.ReadInt32();
                _logger?.Log(">Number of maps in collection {0}: {1} named:{2}", i.ToString(),
                    numberOfBeatmaps.ToString(), name);
                OsuCollection collection = new(mapCacher)
                { Name = name, LastEditorUsername = lastEditor, OnlineId = onlineId };
                for (int j = 0; j < numberOfBeatmaps; j++)
                {
                    BeatmapExtension map = new()
                    {
                        MapId = _binReader.ReadInt32()
                    };
                    if (fileVersion >= 2)
                    {
                        map.MapSetId = _binReader.ReadInt32();
                    }

                    if (!isMinimalCollection(versionString))
                    {
                        map.ArtistRoman = _binReader.ReadString();
                        map.TitleRoman = _binReader.ReadString();
                        map.DiffName = _binReader.ReadString();
                    }

                    map.Md5 = _binReader.ReadString();
                    if (fileVersion >= 4)
                    {
                        map.UserComment = _binReader.ReadString();
                    }

                    if (fileVersion >= 8 || (fileVersion >= 5 && IsFullCollection(versionString)))
                    {
                        map.PlayMode = (PlayMode)_binReader.ReadByte();
                    }

                    if (fileVersion >= 8 || (fileVersion >= 6 && IsFullCollection(versionString)))
                    {
                        map.ModPpStars.Add(map.PlayMode, new StarRating { { 0, _binReader.ReadDouble() } });
                    }

                    collection.AddBeatmap(map);
                }

                if (fileVersion >= 3)
                {
                    int numberOfMapHashes = _binReader.ReadInt32();
                    for (int j = 0; j < numberOfMapHashes; j++)
                    {
                        string hash = _binReader.ReadString();
                        collection.AddBeatmapByHash(hash);
                    }
                }

                yield return collection;
            }
        }

        if (_binReader.ReadString() != "By Piotrekol")
        {
            _binReader.Close();
            throw new CorruptedFileException("File footer is invalid, this collection might be corrupted.");
        }

        _binReader.Close();
    }
}