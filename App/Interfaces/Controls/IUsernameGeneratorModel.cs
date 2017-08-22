using System;
using System.Collections.Generic;

namespace App.Interfaces
{
    public interface IUsernameGeneratorModel
    {

        event EventHandler Start;
        event EventHandler Abort;
        event EventHandler StatusChanged;
        event EventHandler Complete;
        List<string> GeneratedUsernames { get; set; }
        string GeneratedUsernamesStr { get; }
        string Status { set; get; }
        int CompletionPrecentage { get; set; }
        int StartRank { get; set; }
        int EndRank { get; set; }
        void EmitStart();
        void EmitAbort();
        void EmitComplete();

    }
}