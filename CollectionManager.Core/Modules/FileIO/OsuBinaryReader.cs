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

    public override string ReadString() => ReadByte() == 11 ? base.ReadString() : null;

    public DateTime ReadDateTime()
    {
        long ticks = ReadInt64();
        return ticks < 0L || ticks > DateTime.MaxValue.Ticks || ticks < DateTime.MinValue.Ticks
            ? DateTime.MinValue
            : new DateTime(ticks, DateTimeKind.Utc);
    }

    public object OsuConditionalRead()
    {
        switch (ReadByte())
        {
            case 1:
            {
                return ReadBoolean();
            }
            case 2:
            {
                return ReadByte();
            }
            case 3:
            {
                return ReadUInt16();
            }
            case 4:
            {
                return ReadUInt32();
            }
            case 5:
            {
                return ReadUInt64();
            }
            case 6:
            {
                return ReadSByte();
            }
            case 7:
            {
                return ReadInt16();
            }
            case 8:
            {
                return ReadInt32();
            }
            case 9:
            {
                return ReadInt64();
            }
            case 10:
            {
                return ReadChar();
            }
            case 11:
            {
                return ReadString();
            }
            case 12:
            {
                return ReadSingle();
            }
            case 13:
            {
                return ReadDouble();
            }
            case 14:
            {
                return ReadDecimal();
            }
            case 15:
            {
                return ReadDateTime();
            }
            case 16:
            {
                int num = ReadInt32();
                return num > 0 ? ReadBytes(num) : num < 0 ? null : (object)Array.Empty<byte>();

            }
            case 17:
            {
                int num = ReadInt32();
                return num > 0 ? ReadChars(num) : num < 0 ? null : (object)Array.Empty<char>();
            }
            case 18:
            {
                throw new NotImplementedException("Unused in db.");
            }
            default:
            {
                return null;
            }
        }
    }
}