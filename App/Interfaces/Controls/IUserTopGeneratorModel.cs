using System;
using CollectionManager.DataTypes;
using CollectionManagerExtensionsDll.DataTypes;

namespace App.Interfaces
{
    public interface IUserTopGeneratorModel
    {
        string GenerationStatus { get; set; }
        double GenerationCompletionPrecentage { get; set; }
        Collections Collections { get; set; }
        CollectionGeneratorConfiguration GeneratorConfiguration { get; set; }
        event EventHandler CollectionsChanged;
        event EventHandler StatusChanged;
        event EventHandler Start;
        event EventHandler Abort;
        event EventHandler GenerateUsernames;
        event EventHandler SaveCollections;

        void EmitStart();
        void EmitAbort();
        void EmitGenerateUsernames();
        void EmitSaveCollections();
        string GetCollectionNameExample(string pattern);


    }
}