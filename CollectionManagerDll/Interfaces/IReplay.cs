using System;
using CollectionManager.Enums;

namespace CollectionManager.Interfaces
{
    public interface IReplay
    {
        PlayMode PlayMode { get; set; }
        int Version { get; set; }
        string MapHash { get; set; }
        string PlayerName { get; set; }
        string ReplayHash { get; set; }
        int C300 { get; set; }
        int C100 { get; set; }
        int C50 { get; set; }
        int Geki { get; set; }
        int Katu { get; set; }
        int Miss { get; set; }
        long TotalScore { get; set; }
        int MaxCombo { get; set; }
        bool Perfect { get; set; }
        int Mods { get; set; }
        double AdditionalMods { get; set; }
        string ReplayData { get; set; }
        DateTimeOffset Date { get; set; }
        long DateTicks { get; set; }

        int CompressedReplayLength { get; set; }
        byte[] CompressedReplay { get; set; }
        long OnlineScoreId { get; set; }
    }
}