namespace CollectionManager.Core.Types;
using System.IO;

public class LazerFile(string Hash, string FileName)
{
    public string Hash { get; } = Hash;
    public string FileName { get; } = FileName;

    public string RelativeRealmFilePath => Path.Combine(Hash.Remove(1), Hash.Remove(2), Hash);
}
