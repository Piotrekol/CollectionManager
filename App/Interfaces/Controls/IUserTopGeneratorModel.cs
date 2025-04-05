namespace CollectionManagerApp.Interfaces.Controls;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;

public interface IUserTopGeneratorModel
{
    string GenerationStatus { get; set; }
    double GenerationCompletionPrecentage { get; set; }
    OsuCollections Collections { get; set; }
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