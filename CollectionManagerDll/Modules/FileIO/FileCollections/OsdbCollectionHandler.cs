using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuDb;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace CollectionManager.Modules.FileIO.FileCollections
{
    public class OsdbCollectionHandler
    {
        //TODO: Make lastEditor per-collection variable, not file-wide.
        //TODO: Better way to do .osdb versioning while allowing all versions to be loaded/saved

        private BinaryReader _binReader;
        private readonly ILogger _logger;
        private MemoryStream _memStream;

        private readonly Dictionary<string, int> _versions = new Dictionary<string, int>
        {
            {"o!dm", 1},
            {"o!dm2", 2},
            {"o!dm3", 3},
            {"o!dm4", 4},
            {"o!dm5", 5},
            {"o!dm6", 6},
            {"o!dm7", 7},
            {"o!dm7min", 1007}
        };

        public OsdbCollectionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public string CurrentVersion(bool minimalWrite = false)
        {
            return "o!dm7" + (minimalWrite ? "min" : "");
        }

        private bool isMinimalCollection(string versionString)
        {
            return versionString.EndsWith("min");
        }

        protected virtual void Error(string message)
        {
            //Helpers.Error(message);
        }

        protected virtual void Info(string message)
        {
            //Helpers.Info(message);
        }

        public void WriteOsdb(Collections collections, string fullFileDir, string editor, bool minimalWrite = false)
        {
            using (var fileStream = new FileStream(fullFileDir, FileMode.Create, FileAccess.ReadWrite))
            {
                WriteOsdb(collections, fileStream, editor, minimalWrite);
            }
        }

        public void WriteOsdb(Collections collections, Stream outputStream, string editor, bool minimalWrite = false)
        {
            using (var osdbMemoryStream = new MemoryStream())
            using (var osdbBinaryWriter = new BinaryWriter(osdbMemoryStream))
            {
                WriteOsdb(collections, osdbBinaryWriter, editor, minimalWrite);

                using (var outputBinaryWriter = new BinaryWriter(outputStream))
                {
                    outputBinaryWriter.Write(CurrentVersion(minimalWrite));
                    CompressStream(osdbMemoryStream, outputStream);
                }
            }
        }

        public void CompressStream(Stream inputStream, Stream outputStream)
        {
            using (var archive = GZipArchive.Create())
            {
                var inputLength = inputStream.Position;
                inputStream.Position = 0;
                archive.AddEntry("collection.osdb", inputStream, inputLength, DateTime.UtcNow);
                archive.SaveTo(outputStream, new WriterOptions(CompressionType.GZip));
            }
        }
        private void WriteOsdb(Collections collections, BinaryWriter _binWriter, string editor,
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
            foreach (var collection in collections)
            {
                var beatmapsPossibleToSave = new Beatmaps();
                var beatmapWithHashOnly = new HashSet<string>();

                foreach (var beatmap in collection.KnownBeatmaps)
                {
                    beatmapsPossibleToSave.Add(beatmap);
                }

                foreach (var beatmap in collection.DownloadableBeatmaps)
                {
                    beatmapsPossibleToSave.Add(beatmap);
                }

                foreach (var partialBeatmap in collection.UnknownBeatmaps)
                {
                    if (partialBeatmap.TitleRoman != "" || partialBeatmap.MapSetId > 0)
                    {
                        beatmapsPossibleToSave.Add(partialBeatmap);
                    }
                    else
                    {
                        beatmapWithHashOnly.Add(partialBeatmap.Md5);
                    }
                }

                _binWriter.Write(collection.Name);
                _binWriter.Write(collection.OnlineId);
                _binWriter.Write(beatmapsPossibleToSave.Count);
                //Save beatmaps
                foreach (var beatmap in beatmapsPossibleToSave)
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
                    if (!minimalWrite)
                    {
                        _binWriter.Write((byte)beatmap.PlayMode);
                        _binWriter.Write(beatmap.StarsNomod);
                    }
                }

                _binWriter.Write(beatmapWithHashOnly.Count);
                foreach (var beatmapHash in beatmapWithHashOnly)
                {
                    _binWriter.Write(beatmapHash);
                }
            }

            _binWriter.Write("By Piotrekol");
        }

        public IEnumerable<Collection> ReadOsdb(string fullFileDir, MapCacher mapCacher)
        {
            var fileVersion = -1;
            var fileDate = DateTime.Now;
            var collections = new Collections();

            using (var fileStream = new FileStream(fullFileDir, FileMode.Open, FileAccess.Read))
            {
                _memStream = new MemoryStream();
                fileStream.CopyTo(_memStream);
                _binReader = new BinaryReader(_memStream);
            }


            _binReader.BaseStream.Seek(0, SeekOrigin.Begin);
            var versionString = _binReader.ReadString();
            //check header
            if (_versions.ContainsKey(versionString))
            {
                fileVersion = _versions[versionString];
            }

            if (fileVersion == -1)
            {
                Error("Unrecognized osdb file version");
            }
            else
            {
                if (fileVersion >= 7)
                {
                    using (var archiveReader = GZipArchive.Open(_memStream))
                    {
                        var memStream = new MemoryStream();
                        archiveReader.Entries.First().WriteTo(memStream);
                        memStream.Position = 0;
                        _binReader = new BinaryReader(memStream);
                        _binReader.ReadString(); //version string
                    }
                }

                _logger?.Log("Starting file load");
                fileDate = DateTime.FromOADate(_binReader.ReadDouble());
                _logger?.Log(">Date: " + fileDate);
                var lastEditor = _binReader.ReadString();
                _logger?.Log(">LastEditor: " + lastEditor);
                var numberOfCollections = _binReader.ReadInt32();
                _logger?.Log(">Collections: " + numberOfCollections);
                for (var i = 0; i < numberOfCollections; i++)
                {
                    var name = _binReader.ReadString();
                    var onlineId = -1;
                    if (fileVersion >= 7)
                    {
                        onlineId = _binReader.ReadInt32();
                    }

                    var numberOfBeatmaps = _binReader.ReadInt32();
                    _logger?.Log(">Number of maps in collection {0}: {1} named:{2}", i.ToString(),
                        numberOfBeatmaps.ToString(), name);
                    var collection = new Collection(mapCacher)
                    { Name = name, LastEditorUsername = lastEditor, OnlineId = onlineId };
                    for (var j = 0; j < numberOfBeatmaps; j++)
                    {
                        var map = new BeatmapExtension();
                        map.MapId = _binReader.ReadInt32();
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

                        if (!isMinimalCollection(versionString))
                        {
                            if (fileVersion >= 5)
                            {
                                map.PlayMode = (PlayMode)_binReader.ReadByte();
                            }

                            if (fileVersion >= 6)
                            {
                                map.ModPpStars.Add(map.PlayMode, new Dictionary<int, double>
                                {
                                    {0, _binReader.ReadDouble()}
                                });
                            }
                        }

                        collection.AddBeatmap(map);
                    }

                    if (fileVersion >= 3)
                    {
                        var numberOfMapHashes = _binReader.ReadInt32();
                        for (var j = 0; j < numberOfMapHashes; j++)
                        {
                            var hash = _binReader.ReadString();
                            collection.AddBeatmapByHash(hash);
                        }
                    }

                    yield return collection;
                }
            }

            if (_binReader.ReadString() != "By Piotrekol")
            {
                Error("File footer is invalid, with could mean that this file is corrupted. CONTINUE AT YOUR OWN RISK");
            }

            _binReader.Close();

            collections = IssuseVersionRelevantProcedures(fileVersion, fileDate, collections);
        }

        private Collections IssuseVersionRelevantProcedures(int fileVersion, DateTime fileDate, Collections collections)
        {
            if (fileVersion != -1)
            {
                if (fileVersion < 3)
                {
                    Info("This collection was generated using an older version of Collection Manager." +
                         Environment.NewLine +
                         "All download links in this collection will not work." + Environment.NewLine +
                         "File version: " + fileVersion + Environment.NewLine +
                         "Date: " + fileDate);
                }
            }

            return collections;
        }
    }
}