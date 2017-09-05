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
        short C300 { get; set; }
        short C100 { get; set; }
        short C50 { get; set; }
        short Geki { get; set; }
        short Katu { get; set; }
        short Miss { get; set; }
        int TotalScore { get; set; }
        short MaxCombo { get; set; }
        bool Perfect { get; set; }
        int Mods { get; set; }
        string ReplayData { get; set; }
        DateTime Date { get; set; }
        long DateTicks { get; set; }

        int CompressedReplayLength { get; set; }
        byte[] CompressedReplay { get; set; }
        long OnlineScoreId { get; set; }
    }
}