namespace CollectionManager.Common.Interfaces.Controls;

using System;

public interface IUsernameGeneratorView
{
    event EventHandler Start;
    event EventHandler Abort;
    string GeneratedUsernames { set; }
    int RankMin { get; }
    int RankMax { get; }
    int CompletionPrecentage { set; }
    string Status { set; }
    bool StartEnabled { set; }
    bool AbortEnabled { set; }
}
