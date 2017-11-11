using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionManagerExtensionsDll.DataTypes
{
    public class ApiScore
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
    }
}
