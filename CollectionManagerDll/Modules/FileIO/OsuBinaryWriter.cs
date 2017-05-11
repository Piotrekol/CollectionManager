using System.IO;
using System.Text;

namespace CollectionManager.Modules.FileIO
{
    public class OsuBinaryWriter:BinaryWriter
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
                Write((byte)0);
            else
            {
                Write((byte)11);
                base.Write(value);
            }
        }
    }
}