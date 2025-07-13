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
        if (value is null)
        {
            Write((byte)0);
        }
        else
        {
            Write((byte)11);
            base.Write(value);
        }
    }

    public void Write(DateTimeOffset? value) => Write(value?.DateTime.ToBinary() ?? 0L);
}