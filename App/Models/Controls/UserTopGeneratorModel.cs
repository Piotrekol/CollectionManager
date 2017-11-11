using System;
using App.Interfaces;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.DataTypes;

namespace App.Models
{
    public class UserTopGeneratorModel : IUserTopGeneratorModel
    {
        private readonly Func<string, string> _collectionNameGenerator;

        public event EventHandler Start;
        public event EventHandler Abort;
        public event EventHandler GenerateUsernames;
        private string _generationStatus = "Waiting...";
        private double _generationCompletionPrecentage = 0.0d;
        private Collections _collections;

        public UserTopGeneratorModel(Func<string,string> collectionNameGenerator)
        {
            _collectionNameGenerator = collectionNameGenerator;
        }

        public string GenerationStatus
        {
            get { return _generationStatus; }
            set
            {
                _generationStatus = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public double GenerationCompletionPrecentage
        {
            get { return _generationCompletionPrecentage; }
            set
            {
                if (value < 0 || value > 100)
                    return;
                _generationCompletionPrecentage = value;
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler StatusChanged;
        public Collections Collections
        {
            get { return _collections; }
            set
            {
                _collections = value;
                CollectionsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler CollectionsChanged;

        public void EmitStart()
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        public void EmitAbort()
        {
            Abort?.Invoke(this, EventArgs.Empty);
        }

        public void EmitGenerateUsernames()
        {
            GenerateUsernames?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler SaveCollections;
        public void EmitSaveCollections()
        {
            SaveCollections?.Invoke(this,EventArgs.Empty);
        }

        public CollectionGeneratorConfiguration GeneratorConfiguration { get; set; }
        public string GetCollectionNameExample(string pattern)
        {
            return _collectionNameGenerator(pattern);
        }
    }
}