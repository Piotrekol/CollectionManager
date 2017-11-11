using System;
using System.Collections.Generic;
using App.Interfaces;

namespace App.Models
{
    public class UsernameGeneratorModel : IUsernameGeneratorModel
    {
        public event EventHandler Start;
        public event EventHandler Abort;
        public event EventHandler StatusChanged;
        public event EventHandler Complete;
        public List<string> GeneratedUsernames { get; set; } = new List<string>();

        public string GeneratedUsernamesStr => string.Join(",", GeneratedUsernames);

        public int StartRank { get; set; }
        public int EndRank { get; set; }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private int _completionPrecentage;
        public int CompletionPrecentage
        {
            get { return _completionPrecentage; }
            set
            {
                _completionPrecentage = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void EmitStart()
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        public void EmitAbort()
        {
            Abort?.Invoke(this, EventArgs.Empty);
        }

        public void EmitComplete()
        {
            Complete?.Invoke(this, EventArgs.Empty);
        }

    }
}