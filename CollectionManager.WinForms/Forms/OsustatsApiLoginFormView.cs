namespace GuiComponents.Forms;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Forms;
using CollectionManager.WinForms.Forms;
using System.Windows.Forms;

public partial class OsustatsApiLoginFormView : BaseForm, IOsustatsApiLoginFormView
{
    public OsustatsApiLoginFormView()
    {
        InitializeComponent();
    }
    public string ApiKey => textBox_apiKey.Text;

    private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => ProcessExtensions.OpenUrl("https://osustats.ppy.sh/collections/apikey");
}
