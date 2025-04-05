namespace CollectionManager.Extensions.DataTypes;

using System.Collections.Generic;

public class CollectionGeneratorConfiguration
{
    /// <summary>
    /// string.format pattern target for saving collection name
    /// {0} - username
    /// {1} - Mods used in specific play
    /// </summary>
    public string CollectionNameSavePattern { get; set; }

    /// <summary>
    /// List of usernames to process.
    /// </summary>
    public List<string> Usernames { get; set; } = [];

    /// <summary>
    /// api key that should be used to get data
    /// </summary>
    public string ApiKey { get; set; } = "";

    /// <summary>
    /// Gamemode to get top scores from
    /// </summary>
    public int Gamemode { get; set; }

    public ScoreSaveConditions ScoreSaveConditions;
}