using System.IO;
using System.Text;
using CollectionManager.Annotations;

namespace CollectionManager.Modules.FileIO
{
    public class OsuBinaryReader : BinaryReader
    {
        public OsuBinaryReader([NotNull] Stream input) : base(input)
        {
        }

        public OsuBinaryReader([NotNull] Stream input, [NotNull] Encoding encoding) : base(input, encoding)
        {
        }

        public override string ReadString()
        {
            if (ReadByte() == 11)
                return base.ReadString();
            else
                return "";
        }
    }
}