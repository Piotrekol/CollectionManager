using System;
using System.Collections.Generic;
using System.IO;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.Modules.FileIO.FileCollections
{
    public class OsdbCollectionHandler
    {
        //TODO: Make lastEditor per-collection variable, not file-wide.
        //TODO: Better way to do .osdb versioning while allowing all versions to be loaded/saved

        private BinaryReader _binReader;
        private BinaryWriter _binWriter;
        private FileStream _fileStream;
        private ILogger _logger;
        private MemoryStream _memStream;
        private Dictionary<string, int> _versions = new Dictionary<string, int>()
        {
            {"o!dm",1 },
            {"o!dm2",2 },
            {"o!dm3",3 },
            {"o!dm4",4 }
        };

        private string CurrentVersion => "o!dm4";
        public OsdbCollectionHandler(ILogger logger)
        {
            _logger = logger;
        }

        protected virtual void Error(string message)
        {
            //Helpers.Error(message);
        }
        protected virtual void Info(string message)
        {
            //Helpers.Info(message);
        }
        public void WriteOsdb(Collections collections, string fullFileDir, string editor, bool skipMissing = false)
        {
            OpenFile(fullFileDir, true);
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
                Beatmaps beatmapsPossibleToSave = new Beatmaps();
                HashSet<string> beatmapWithHashOnly = new HashSet<string>();

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
                    _binWriter.Write(beatmap.UserComment);
                }
                _binWriter.Write(beatmapWithHashOnly.Count);
                foreach (var beatmapHash in beatmapWithHashOnly)
                {
                    _binWriter.Write(beatmapHash);
                }

            }

            _binWriter.Write("By Piotrekol");
            CloseFile(true);
        }

        public Collections ReadOsdb(string fullFileDir, MapCacher mapCacher)
        {
            int fileVersion = -1;
            DateTime fileDate = DateTime.Now;
            var collections = new Collections();
            OpenFile(fullFileDir, false);
            _binReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string versionString = _binReader.ReadString();
            //check header
            if(_versions.ContainsKey(versionString))
                    fileVersion = _versions[versionString];
            if (fileVersion == -1)
            {
                Error("Unrecognized osdb file version");
            }
            else
            {
                _logger?.Log("Starting file load");
                fileDate = DateTime.FromOADate(_binReader.ReadDouble());
                _logger?.Log(">Date: " + fileDate);
                string lastEditor = _binReader.ReadString();
                _logger?.Log(">LastEditor: " + lastEditor);
                int numberOfCollections = _binReader.ReadInt32();
                _logger?.Log(">Collections: " + numberOfCollections);
                for (int i = 0; i < numberOfCollections; i++)
                {
                    var name = _binReader.ReadString();
                    var numberOfBeatmaps = _binReader.ReadInt32();
                    _logger?.Log(">Number of maps in collection {0}: {1} named:{2}", i.ToString(), numberOfBeatmaps.ToString(), name);
                    var collection = new Collection(mapCacher) { Name = name, LastEditorUsername = lastEditor };
                    for (int j = 0; j < numberOfBeatmaps; j++)
                    {
                        var map = new BeatmapExtension();
                        map.MapId = _binReader.ReadInt32();
                        if (fileVersion >= 2)
                            map.MapSetId = _binReader.ReadInt32();
                        map.ArtistRoman = _binReader.ReadString();
                        map.TitleRoman = _binReader.ReadString();
                        map.DiffName = _binReader.ReadString();
                        map.Md5 = _binReader.ReadString();
                        if (fileVersion >= 4)
                            map.UserComment = _binReader.ReadString();
                        collection.AddBeatmap(map);
                    }

                    if (fileVersion >= 3)
                    {
                        var numberOfMapHashes = _binReader.ReadInt32();
                        for (int j = 0; j < numberOfMapHashes; j++)
                        {
                            string hash = _binReader.ReadString();
                            collection.AddBeatmapByHash(hash);
                        }
                    }


                    collections.Add(collection);
                }
            }
            if (_binReader.ReadString() != "By Piotrekol")
            {
                Error("File footer is invalid, with could mean that this file is corrupted. CONTINUE AT YOUR OWN RISK");
            }


            CloseFile(false);


            collections = IssuseVersionRelevantProcedures(fileVersion, fileDate, collections);

            return collections;
        }

        private Collections IssuseVersionRelevantProcedures(int fileVersion, DateTime fileDate, Collections collections)
        {
            if (fileVersion != -1)
            {
                if (fileVersion < 3)
                {
                    Info("This collection was generated using an older version of Collection Manager." + Environment.NewLine +
                        "All download links in this collection will not work." + Environment.NewLine +
                        "File version: " + fileVersion + Environment.NewLine +
                        "Date: " + fileDate);
                }
            }
            return collections;
        }


        private void OpenFile(string fileDir, bool forWriting = false)
        {
            if (forWriting)
            {
                _fileStream = new FileStream(fileDir, FileMode.Create, FileAccess.ReadWrite);
                _binWriter = new BinaryWriter(_fileStream);

            }
            else
            {
                _fileStream = new FileStream(fileDir, FileMode.Open, FileAccess.Read);
                _memStream = new MemoryStream();
                _fileStream.CopyTo(_memStream);
                _fileStream.Close();
                _binReader = new BinaryReader(_memStream);

            }
        }

        private void CloseFile(bool forWriting = false)
        {
            try
            {
                if (forWriting)
                {
                    _binWriter.Close();
                }
                else
                {
                    _binReader.Close();
                }
            }
            catch { }
        }
    }
}
