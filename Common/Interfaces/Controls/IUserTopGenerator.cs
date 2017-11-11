using System;
using CollectionManager.DataTypes;

namespace GuiComponents.Interfaces
{
    public interface IUserTopGenerator
    {
        event EventHandler Start;
        event EventHandler Abort;
        event EventHandler GenerateUsernames;
        event EventHandler CollectionNamingFormatChanged;
        bool IsRunning { set; }
        string ApiKey { get; }
        string Usernames { get; }
        string CollectionNamingFormat { get; }
        string CollectionNamingExample { set; }
        int AllowedScores { get; }
        double PpMin { get; }
        double PpMax { get; }
        double AccMin { get; }
        double AccMax { get; }
        bool GroupByMods { get; }
        bool MergeCollectionsWithSameName { get; }

        ICollectionListingView CollectionListing { get; }

        string ProcessingStatus { set; }
        double ProcessingCompletionPrecentage { set; }
    }
}