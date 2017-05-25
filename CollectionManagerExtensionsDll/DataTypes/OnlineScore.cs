using System;

namespace CollectionManagerExtensionsDll.DataTypes
{
    public class OnlineScore
    {

        public int BeatmapId { get; set; }
        public int Score { get; set; }
        public string Username { get; set; }
        public int Count300 { get; set; }
        public int Count100 { get; set; }
        public int Count50 { get; set; }
        public int Countmiss { get; set; }
        public int Maxcombo { get; set; }
        public int Countkatu { get; set; }
        public int Countgeki { get; set; }
        public int Perfect { get; set; }
        public int EnabledMods { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Rank { get; set; }
        public double Pp { get; set; }
        public double PpRounded => Math.Round(Pp, 0);

        public double Acc
        {
            get
            {
                var totalHits = Countmiss + Count50 + Count100 + Count300;
                var pointsOfHits = Count50 * 50 + Count100 * 100 + Count300 * 300;
                double acc = ((double)pointsOfHits / (double)(totalHits * 300)) * 100;
                return Math.Round(acc, 2);
            }
        }
    }
}