namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Properties;
using System.IO;
using System.Text;

public class OsuBinaryReader : BinaryReader
{
    public OsuBinaryReader([NotNull] Stream input) : base(input)
    {
    }

    public OsuBinaryReader([NotNull] Stream input, [NotNull] Encoding encoding) : base(input, encoding)
    {
    }

    public override string ReadString() => ReadByte() == 11 ? base.ReadString() : "";
}