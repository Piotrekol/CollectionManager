using Common;

namespace App.Models
{
    public class StartupSettings
    {
        public StartupDatabaseAction StartupDatabaseAction { get; set; }
        public StartupCollectionAction StartupCollectionAction { get; set; }
        public bool UseSelectedOptionsOnStartup { get; set; }
        public string OsuLocation { get; set; }
    }
}