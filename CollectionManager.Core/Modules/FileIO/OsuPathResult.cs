namespace CollectionManager.Core.Modules.FileIo;

using CollectionManager.Core.Types;

public sealed record OsuPathResult(string Path, OsuType Type, string? StablePath = default, string? LazerPath = default);
