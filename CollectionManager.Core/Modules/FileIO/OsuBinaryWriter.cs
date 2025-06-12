namespace CollectionManager.Core.Modules.FileIo;

using System.IO;
using System.Text;

public class OsuBinaryWriter : BinaryWriter
{
    public OsuBinaryWriter(Stream output) : base(output)
    {
    }

    public OsuBinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
    {
    }

    public override void Write(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Write((byte)0);
        }
        else
        {
            Write((byte)11);
            base.Write(value);
        }
    }
}