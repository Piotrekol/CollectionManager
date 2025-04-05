namespace GuiComponents.Forms;

using CollectionManager.Common.Interfaces.Controls;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;

public partial class CollectionAddRenameForm : BaseForm, ICollectionAddRenameForm
{
    public CollectionAddRenameForm()
    {
        InitializeComponent();
        IsRenameForm = collectionRenameView1.IsRenameView;

        AcceptButton = collectionRenameView1.button_rename;
        CancelButton = collectionRenameView1.button_cancel;
    }

    public ICollectionRenameView CollectionRenameView => collectionRenameView1;
    private bool _isRenameForm;

    public bool IsRenameForm
    {
        get => _isRenameForm;
        set
        {
            _isRenameForm = value;
            collectionRenameView1.IsRenameView = value;
            Text = value ? "Collection Manager - Rename collection" : "Collection Manager - Add collection";
        }
    }
}
