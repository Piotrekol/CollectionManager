using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using CollectionManager.DataTypes;
using CollectionManager.Enums;
using CollectionManager.Exceptions;
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
            {"o!dm8", 8},
            {"o!dm9", 9},
            {"o!dm7min", 1007},
            {"o!dm8min", 1008},
        };

        private const string CurrentVersion = "o!dm9";

        public OsdbCollectionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteOsdb(Collections collections, string fullFileDir, string editor)
        {
            using (var fileStream = new FileStream(fullFileDir, FileMode.Create, FileAccess.ReadWrite))
            {
                WriteOsdb(collections, fileStream, editor);
            }
        }

        public void WriteOsdb(Collections collections, Stream outputStream, string editor)
        {
            using (var osdbMemoryStream = new MemoryStream())
            using (var osdbBinaryWriter = new BinaryWriter(osdbMemoryStream))
            {
                WriteOsdb(collections, osdbBinaryWriter, editor);

                using (var outputBinaryWriter = new BinaryWriter(outputStream, Encoding.UTF8, true))
                {
                    outputBinaryWriter.Write(CurrentVersion);
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
        private void WriteOsdb(Collections collections, BinaryWriter _binWriter, string editor)
        {
            //header
            _binWriter.Write(CurrentVersion);
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

                // Custom Fields
                var customFieldDefLookup = new Dictionary<string, CustomFieldDefinition>();
                if (collection.CustomFieldDefinitions == null)
                {
                    _binWriter.Write(0);
                }
                else
                {
                    _binWriter.Write(collection.CustomFieldDefinitions.Count);
                    foreach (var def in collection.CustomFieldDefinitions)
                    {
                        customFieldDefLookup.Add(def.Key, def);
                        _binWriter.Write(def.Key);
                        _binWriter.Write((byte)def.Type);
                        _binWriter.Write(def.DisplayText);
                    }
                }

                _binWriter.Write(beatmapsPossibleToSave.Count);
                //Save beatmaps
                foreach (var beatmap in beatmapsPossibleToSave)
                {
                    _binWriter.Write(beatmap.MapId);
                    _binWriter.Write(beatmap.MapSetId);
                    _binWriter.Write(beatmap.ArtistRoman);
                    _binWriter.Write(beatmap.TitleRoman);
                    _binWriter.Write(beatmap.DiffName);

                    _binWriter.Write(beatmap.Md5);
                    _binWriter.Write(((BeatmapExtension)beatmap).UserComment);
                    _binWriter.Write((byte)beatmap.PlayMode);
                    _binWriter.Write(beatmap.StarsNomod);

                    // completely skip the custom fields section if there are no definitions in the header of this collection
                    if(customFieldDefLookup.Count > 0)
                    {
                        var validCustomFieldWriteActions = new List<Action>();
                        foreach (var kvp in ((BeatmapExtension)beatmap).GetCustomFields())
                        {
                            if (!customFieldDefLookup.TryGetValue(kvp.Key, out var def) ||
                                !CustomFieldWriters.TryGetValue(def.Type, out var writerAction))
                            {
                                // skip value if definition or its stream writer doesn't exist
                                continue;
                            }

                            validCustomFieldWriteActions.Add(() =>
                            {
                                _binWriter.Write(kvp.Key);
                                writerAction(_binWriter, kvp.Value);
                            });
                        }

                        _binWriter.Write(validCustomFieldWriteActions.Count);
                        foreach(var writeAction in validCustomFieldWriteActions)
                        {
                            writeAction();
                        }
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
                throw new CorruptedFileException($"Unrecognized osdb file version (got: {versionString})");
            }
            else
            {
                bool minimalCollection = false;
                if(fileVersion > 1000)
                {
                    minimalCollection = true;
                    fileVersion -= 1000;
                }

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

                    var collection = new Collection(mapCacher)
                    {
                        Name = name,
                        LastEditorUsername = lastEditor,
                        OnlineId = onlineId
                    };

                    var customFieldDefinitions = new Dictionary<string, CustomFieldDefinition>();
                    if (fileVersion >= 9)
                    {
                        var numberOfCustomFieldDefinitions = _binReader.ReadInt32();
                        for (var j = 0; j < numberOfCustomFieldDefinitions; j++)
                        {
                            var key = _binReader.ReadString();
                            var def = new CustomFieldDefinition
                            {
                                Key = key,
                                Type = (CustomFieldType)_binReader.ReadByte(),
                                DisplayText = _binReader.ReadString()
                            };
                            try
                            {
                                customFieldDefinitions.Add(key, def);
                            }
                            catch (ArgumentException)
                            {
                                // custom field key is already used
                                throw new CorruptedFileException($"Collection declared multiple custom fields with the same key");
                            }
                        }
                    }

                    var numberOfBeatmaps = _binReader.ReadInt32();
                    _logger?.Log(">Number of maps in collection {0}: {1} named:{2}", i.ToString(),
                        numberOfBeatmaps.ToString(), name);
                    var actuallyUsedCustomFieldDefinitions = new HashSet<string>();
                    for (var j = 0; j < numberOfBeatmaps; j++)
                    {
                        var map = new BeatmapExtension();
                        map.MapId = _binReader.ReadInt32();
                        if (fileVersion >= 2)
                        {
                            map.MapSetId = _binReader.ReadInt32();
                        }

                        if (!minimalCollection)
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

                        // versions 5-7 field existed only in full collections, since version 8 field is always here
                        if (fileVersion >= 8 || (fileVersion >= 5 && !minimalCollection))
                        {
                            map.PlayMode = (PlayMode)_binReader.ReadByte();
                        }

                        // versions 6-7 field existed only in full collections, since version 8 field is always here
                        if (fileVersion >= 8 || (fileVersion >= 6 && !minimalCollection))
                        {
                            map.ModPpStars.Add(map.PlayMode, new StarRating { { 0, _binReader.ReadDouble() } });
                        }

                        // completely skip the custom fields section regardless of version if there are no definitions in the header of this collection
                        if (fileVersion >= 9 && customFieldDefinitions.Count > 0)
                        {
                            var numberOfCustomFieldValues = _binReader.ReadInt32();
                            for (var k = 0; k < numberOfCustomFieldValues; k++)
                            {
                                var key = _binReader.ReadString();
                                if (!customFieldDefinitions.TryGetValue(key, out var customFieldDef) ||
                                    !CustomFieldReaders.TryGetValue(customFieldDef.Type, out var readerFunc))
                                {
                                    // skip value, it has no definition or we dont know how to handle this type
                                    continue;
                                }

                                var customFieldValue = readerFunc(_binReader);
                                map.SetCustomFieldValue(key, customFieldValue);
                                actuallyUsedCustomFieldDefinitions.Add(key);
                            }
                        }

                        collection.AddBeatmap(map);
                    }

                    collection.CustomFieldDefinitions = customFieldDefinitions
                        .Where(x => actuallyUsedCustomFieldDefinitions.Contains(x.Key))
                        .Select(x => x.Value)
                        .ToList()
                        .AsReadOnly();

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
                _binReader.Close();
                throw new CorruptedFileException("File footer is invalid, this collection might be corrupted.");
            }

            _binReader.Close();
        }

        private static readonly Dictionary<CustomFieldType, Func<BinaryReader, object>> CustomFieldReaders = new()
        {
            { CustomFieldType.String, reader => reader.ReadString() },
            { CustomFieldType.Boolean, reader => reader.ReadBoolean() },
            { CustomFieldType.GameMode, reader => (PlayMode)reader.ReadByte() },
            { CustomFieldType.Grade, reader => (OsuGrade)reader.ReadByte() },
            { CustomFieldType.UInt8, reader => reader.ReadByte() },
            { CustomFieldType.UInt16, reader => reader.ReadUInt16() },
            { CustomFieldType.UInt32, reader => reader.ReadUInt32() },
            { CustomFieldType.UInt64, reader => reader.ReadUInt64() },
            { CustomFieldType.Int8, reader => reader.ReadSByte() },
            { CustomFieldType.Int16, reader => reader.ReadInt16() },
            { CustomFieldType.Int32, reader => reader.ReadInt32() },
            { CustomFieldType.Int64, reader => reader.ReadInt64() },
            { CustomFieldType.DateTime, reader => new DateTime(reader.ReadInt64()) },
            { CustomFieldType.Single, reader => reader.ReadSingle() },
            { CustomFieldType.Double, reader => reader.ReadDouble() },
        };

        private static readonly Dictionary<CustomFieldType, Action<BinaryWriter, object>> CustomFieldWriters = new()
        {
            { CustomFieldType.String, (writer, value) => writer.Write((string)value) },
            { CustomFieldType.Boolean, (writer, value) => writer.Write((bool)value) },
            { CustomFieldType.GameMode, (writer, value) => writer.Write((byte)value) },
            { CustomFieldType.Grade, (writer, value) => writer.Write((byte)value) },
            { CustomFieldType.UInt8, (writer, value) => writer.Write((byte)value) },
            { CustomFieldType.UInt16, (writer, value) => writer.Write((ushort)value) },
            { CustomFieldType.UInt32, (writer, value) => writer.Write((uint)value) },
            { CustomFieldType.UInt64, (writer, value) => writer.Write((ulong)value) },
            { CustomFieldType.Int8, (writer, value) => writer.Write((sbyte)value) },
            { CustomFieldType.Int16, (writer, value) => writer.Write((short)value) },
            { CustomFieldType.Int32, (writer, value) => writer.Write((int)value) },
            { CustomFieldType.Int64, (writer, value) => writer.Write((long)value) },
            { CustomFieldType.DateTime, (writer, value) => writer.Write(((DateTime)value).Ticks) },
            { CustomFieldType.Single, (writer, value) => writer.Write((float)value) },
            { CustomFieldType.Double, (writer, value) => writer.Write((double)value) },
        };
    }
}