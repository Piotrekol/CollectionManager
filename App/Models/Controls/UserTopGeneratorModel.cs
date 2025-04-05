namespace CollectionManagerApp.Models.Controls;

using CollectionManager.Core.Types;
using CollectionManager.Extensions.DataTypes;
using CollectionManagerApp.Interfaces.Controls;

public class UserTopGeneratorModel : IUserTopGeneratorModel
{
    private readonly Func<string, string> _collectionNameGenerator;

    public event EventHandler Start;
    public event EventHandler Abort;
    public event EventHandler GenerateUsernames;
    private string _generationStatus = "Waiting...";
    private double _generationCompletionPrecentage;
    private OsuCollections _collections;

    public UserTopGeneratorModel(Func<string, string> collectionNameGenerator)
    {
        _collectionNameGenerator = collectionNameGenerator;
    }

    public string GenerationStatus
    {
        get => _generationStatus;
        set
        {
            _generationStatus = value;
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public double GenerationCompletionPrecentage
    {
        get => _generationCompletionPrecentage;
        set
        {
            if (value is < 0 or > 100)
            {
                return;
            }

            _generationCompletionPrecentage = value;
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler StatusChanged;
    public OsuCollections Collections
    {
        get => _collections;
        set
        {
            _collections = value;
            CollectionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler CollectionsChanged;

    public void EmitStart() => Start?.Invoke(this, EventArgs.Empty);

    public void EmitAbort() => Abort?.Invoke(this, EventArgs.Empty);

    public void EmitGenerateUsernames() => GenerateUsernames?.Invoke(this, EventArgs.Empty);

    public event EventHandler SaveCollections;
    public void EmitSaveCollections() => SaveCollections?.Invoke(this, EventArgs.Empty);

    public CollectionGeneratorConfiguration GeneratorConfiguration { get; set; }
    public string GetCollectionNameExample(string pattern) => _collectionNameGenerator(pattern);
}