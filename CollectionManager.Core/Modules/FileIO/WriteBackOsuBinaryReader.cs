namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Properties;
using System.IO;
using System.Text;

/// <summary>
/// osu! binary reader that preserves null-marker strings as <c>null</c> (instead of empty string),
/// so an osu!.db can be byte-perfect round tripped via <see cref="OsuBinaryWriter"/>.
/// </summary>
public sealed class WriteBackOsuBinaryReader : OsuBinaryReader
{
    public WriteBackOsuBinaryReader([NotNull] Stream input) : base(input)
    {
    }

    public WriteBackOsuBinaryReader([NotNull] Stream input, [NotNull] Encoding encoding) : base(input, encoding)
    {
    }

    public override string ReadString() => ReadRawOsuString();
}
