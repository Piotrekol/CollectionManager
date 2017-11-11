using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Interfaces.Controls
{
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
}
