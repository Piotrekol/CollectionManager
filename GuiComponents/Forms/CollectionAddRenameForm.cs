using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
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
            get
            {
                return _isRenameForm;
            }
            set
            {
                _isRenameForm = value;
                collectionRenameView1.IsRenameView = value;
                if (value)
                    Text = "Collection Manager - Rename collection";
                else
                    Text = "Collection Manager - Add collection";
            }
        }
    }
}
