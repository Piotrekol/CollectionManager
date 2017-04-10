using System.IO;
using CollectionManager.DataTypes;
using CollectionManager.Interfaces;
using CollectionManager.Modules.FileIO.OsuDb;

namespace CollectionManager.Modules.FileIO.FileCollections
{
    public class OsuCollectionHandler
    {
        private BinaryReader BinReader;
        private BinaryWriter BinWriter;
        private FileStream FileStream;

        private ILogger logger;
        public int LastfileDate;

        public OsuCollectionHandler(ILogger logger)
        {
            this.logger = logger;
        }
        public int readInt32()
        {
            return this.BinReader.ReadInt32();
        }
        public string readString()
        {
            try
            {
                if (this.BinReader.ReadByte() == 11)
                {
                    return BinReader.ReadString();
                }
                return "";
            }
            catch { throw new System.IO.IOException("This isn't valid osu! collection!"); }
        }

        private void openFile(string fileDir, bool forWriting = false)
        {
            if (forWriting)
            {
                this.FileStream = new FileStream(fileDir, FileMode.Create, FileAccess.ReadWrite);
                this.BinWriter = new BinaryWriter(this.FileStream);

            }
            else
            {
                this.FileStream = new FileStream(fileDir, FileMode.Open, FileAccess.Read);
                this.BinReader = new BinaryReader(this.FileStream);

            }
        }

        private void closeFile(bool forWriting = false)
        {
            try
            {
                if (forWriting)
                    this.BinWriter.Close();
                else
                    this.BinReader.Close();
            }
            catch { }
        }
        public Collections LoadCollections(string fullFileDir, MapCacher mapCacher)
        {
            openFile(fullFileDir);


            LastfileDate = readInt32();
            int numOfCollections = readInt32();
            logger?.Log("Reading " + numOfCollections + " Collections");
            Collections loadedCollections = new Collections();

            try
            {
                for (int i = 1; i <= numOfCollections; i++)
                {
                    var collectionName = readString();
                    var numberOfDiffs = readInt32();

                    var collection = new Collection(mapCacher) { Name = collectionName };
                    var j = 0;
                    for (j = 0; j < numberOfDiffs; j++)//j=1 <=
                    {
                        var md5 = readString();
                        collection.AddBeatmapByHash(md5);
                    }
                    loadedCollections.Add(collection);
                    logger?.Log(">Number of maps in collection {0}: {1} named:{2}", i.ToString(), numberOfDiffs.ToString(), collection.Name);
                }
            }
            catch (System.IO.IOException) { throw new System.IO.IOException("This isn't valid osu! collection!"); }
            closeFile();
            return loadedCollections;
        }

        public void SaveCollections(Collections collections, string fullFileDir)
        {
            openFile(fullFileDir, true);

            //last edit time
            //BinWriter.Write((int)DateTime.Now.Ticks);
            BinWriter.Write((int)this.LastfileDate);
            //collection count
            BinWriter.Write(collections.Count);

            /*collections
             * repeated:
                * 0x0b
                * (string) collection name
                * (int) number of beatmaps in collection
                * repeated: 
                     * 0x0b
                     * (string) beatmap hash
             */
            foreach (var collection in collections)
            {
                BinWriter.Write((byte)0x0b);
                BinWriter.Write(collection.Name);
                BinWriter.Write(collection.NumberOfBeatmaps);
                foreach (var beatmap in collection.AllBeatmaps())
                {
                    BinWriter.Write((byte)0x0b);
                    BinWriter.Write(beatmap.Md5);
                }
            }
            closeFile(true);
        }
    }
}