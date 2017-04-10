using System;
using App.Interfaces;

namespace App.Models
{
    public class CollectionAddRenameModel : ICollectionAddRenameModel
    {
        public event EventHandler Submited;
        public Func<string, bool> IsCollectionNameValid { get; }
        public string OrginalCollectionName { get; }
        public string NewCollectionName { get; set; } = "";
        private bool _newCollectionNameIsValid = true;
        public bool UserCanceled { get; set; } = false;

        public bool NewCollectionNameIsValid
        {
            get { return !UserCanceled && _newCollectionNameIsValid; }
            set { _newCollectionNameIsValid = value; }
        }

        public void EmitSubmited()
        {
            Submited?.Invoke(this, EventArgs.Empty);
        }

        public CollectionAddRenameModel(Func<string, bool> isCollectionNameValid, string orginalCollectionName="")
        {
            IsCollectionNameValid = isCollectionNameValid;
            OrginalCollectionName = orginalCollectionName;
        }
    }
}