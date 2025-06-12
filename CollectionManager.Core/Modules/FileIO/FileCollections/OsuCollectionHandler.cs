namespace CollectionManager.Core.Modules.FileIo.FileCollections;

using CollectionManager.Core.Interfaces;
using CollectionManager.Core.Modules.FileIo.OsuDb;
using CollectionManager.Core.Types;
using System.IO;

public class OsuCollectionHandler
{
    private BinaryReader BinReader;
    private BinaryWriter BinWriter;
    private FileStream FileStream;

    private readonly ILogger logger;
    public int LastfileDate;

    public OsuCollectionHandler(ILogger logger)
    {
        this.logger = logger;
    }
    public int readInt32() => BinReader.ReadInt32();
    public string readString()
    {
        try
        {
            return BinReader.ReadByte() == 11 ? BinReader.ReadString() : "";
        }
        catch
        {
            throw new IOException("This isn't valid osu! collection!");
        }
    }

    private void openFile(string fileDir, bool forWriting = false)
    {
        if (forWriting)
        {
            FileStream = new FileStream(fileDir, FileMode.Create, FileAccess.ReadWrite);
            BinWriter = new BinaryWriter(FileStream);

        }
        else
        {
            FileStream = new FileStream(fileDir, FileMode.Open, FileAccess.Read);
            BinReader = new BinaryReader(FileStream);

        }
    }

    private void closeFile(bool forWriting = false)
    {
        try
        {
            if (forWriting)
            {
                BinWriter.Close();
            }
            else
            {
                BinReader.Close();
            }
        }
        catch { }
    }
    public OsuCollections LoadCollections(string fullFileDir, MapCacher mapCacher)
    {
        openFile(fullFileDir);

        LastfileDate = readInt32();
        int numOfCollections = readInt32();
        logger?.Log("Reading " + numOfCollections + " Collections");
        OsuCollections loadedCollections = [];

        try
        {
            for (int i = 1; i <= numOfCollections; i++)
            {
                string collectionName = readString();
                int numberOfDiffs = readInt32();

                OsuCollection collection = new(mapCacher) { Name = collectionName };
                int j = 0;
                for (j = 0; j < numberOfDiffs; j++)//j=1 <=
                {
                    string md5 = readString();
                    collection.AddBeatmapByHash(md5);
                }

                loadedCollections.Add(collection);
                logger?.Log(">Number of maps in collection {0}: {1} named:{2}", i.ToString(), numberOfDiffs.ToString(), collection.Name);
            }
        }
        catch (IOException)
        {
            throw new IOException("This isn't valid osu! collection!");
        }

        closeFile();
        return loadedCollections;
    }

    public void SaveCollections(OsuCollections collections, string fullFileDir)
    {
        openFile(fullFileDir, true);

        //last edit time
        //BinWriter.Write((int)DateTime.Now.Ticks);
        BinWriter.Write(LastfileDate);
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
        foreach (IOsuCollection collection in collections)
        {
            BinWriter.Write((byte)0x0b);
            BinWriter.Write(collection.Name);
            BinWriter.Write(collection.NumberOfBeatmaps);
            foreach (BeatmapExtension beatmap in collection.AllBeatmaps())
            {
                BinWriter.Write((byte)0x0b);
                BinWriter.Write(beatmap.Md5);
            }
        }

        closeFile(true);
    }
}