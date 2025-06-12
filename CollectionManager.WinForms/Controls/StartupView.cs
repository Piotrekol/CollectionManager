namespace GuiComponents.Controls;

using CollectionManager.Common;
using CollectionManager.Common.Interfaces.Controls;
using System;
using System.Windows.Forms;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2011:Avoid infinite recursion", Justification = "WinForms Invoke calls.")]
public partial class StartupView : UserControl, IStartupView
{
    public event GuiHelpers.StartupCollectionEventArgs StartupCollectionOperation;
    public event GuiHelpers.StartupDatabaseEventArgs StartupDatabaseOperation;
    public event EventHandler UpdateTextClicked;

    public string LoadDatabaseStatusText
    {
        get => label_LoadDatabaseStatus.Text;
        set
        {
            if (label_LoadDatabaseStatus.InvokeRequired)
            {
                _ = label_LoadDatabaseStatus.Invoke(() => label_LoadDatabaseStatus.Text = value);
                return;
            }

            label_LoadDatabaseStatus.Text = value;
        }
    }

    public bool UseSelectedOptionsOnStartup
    {
        get => checkBox_DoNotShowOnStartup.Checked;
        set
        {
            if (checkBox_DoNotShowOnStartup.InvokeRequired)
            {
                _ = checkBox_DoNotShowOnStartup.Invoke(() => checkBox_DoNotShowOnStartup.Checked = value);
                return;
            }

            checkBox_DoNotShowOnStartup.Checked = value;
        }
    }

    public bool LoadOsuCollectionButtonEnabled
    {
        get => button_LoadOsuCollection.Enabled;
        set
        {
            if (button_LoadOsuCollection.InvokeRequired)
            {
                _ = button_LoadOsuCollection.Invoke(() => button_LoadOsuCollection.Enabled = value);
                return;
            }

            button_LoadOsuCollection.Enabled = value;
        }
    }

    public string CollectionStatusText
    {
        get => label_OpenedCollection.Text;
        set
        {
            if (label_OpenedCollection.InvokeRequired)
            {
                _ = label_OpenedCollection.Invoke(() => CollectionStatusText = value);
                return;
            }

            label_OpenedCollection.Visible = !string.IsNullOrEmpty(value);
            label_OpenedCollection.Text = value;
        }
    }

    public bool CollectionButtonsEnabled
    {
        get => button_LoadCollectionFromFile.Enabled;
        set
        {
            if (button_LoadCollectionFromFile.InvokeRequired)
            {
                _ = button_LoadCollectionFromFile.Invoke(() => CollectionButtonsEnabled = value);
            }

            button_DoNothing.Enabled
                = button_LoadCollectionFromFile.Enabled
                = value;

            button_LoadOsuCollection.Enabled = LoadOsuCollectionButtonEnabled && value;
        }
    }

    public bool DatabaseButtonsEnabled
    {
        get => button_LoadDatabase.Enabled;
        set
        {
            if (button_LoadDatabase.InvokeRequired)
            {
                _ = button_LoadDatabase.Invoke(() => DatabaseButtonsEnabled = value);
                return;
            }

            button_LoadDatabase.Enabled
                = button_LoadDatabaseSkip.Enabled
                = flowLayoutPanel_loadOsuInstanceDbButtons.Enabled
                = value;
        }
    }

    public bool UseSelectedOptionsOnStartupEnabled
    {
        get => checkBox_DoNotShowOnStartup.Enabled;
        set
        {
            if (checkBox_DoNotShowOnStartup.InvokeRequired)
            {
                _ = checkBox_DoNotShowOnStartup.Invoke(() => checkBox_DoNotShowOnStartup.Enabled = value);
                return;
            }

            checkBox_DoNotShowOnStartup.Enabled = value;
        }
    }

    public IInfoTextView InfoTextView => infoTextView1;

    public bool LoadLazerDatabaseButtonEnabled
    {
        get => button_LoadLazer.Enabled;
        set
        {
            if (button_LoadLazer.InvokeRequired)
            {
                _ = button_LoadLazer.Invoke(() => LoadLazerDatabaseButtonEnabled = value);
                return;
            }

            button_LoadLazer.Enabled = value;
        }
    }

    public bool LoadStableDatabaseButtonEnabled
    {
        get => button_LoadStable.Enabled;
        set
        {
            if (button_LoadStable.InvokeRequired)
            {
                _ = button_LoadStable.Invoke(() => LoadStableDatabaseButtonEnabled = value);
                return;
            }

            button_LoadStable.Enabled = value;
        }
    }

    public StartupView()
    {
        InitializeComponent();
        flowLayoutPanel1.SetFlowBreak(button_DoNothing, true);
        button_DoNothing.Tag = StartupCollectionAction.None;
        button_LoadOsuCollection.Tag = StartupCollectionAction.LoadOsuCollection;
        button_LoadCollectionFromFile.Tag = StartupCollectionAction.LoadCollectionFromFile;

        button_LoadDatabase.Tag = StartupDatabaseAction.LoadFromDifferentLocation;
        button_LoadDatabaseSkip.Tag = StartupDatabaseAction.Unload;
        button_LoadLazer.Tag = StartupDatabaseAction.LoadLazer;
        button_LoadStable.Tag = StartupDatabaseAction.LoadStable;
    }

    private void button_StartupAction_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        StartupCollectionOperation?.Invoke(this, (StartupCollectionAction)button.Tag);
    }

    private void button_DatabaseAction_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        StartupDatabaseOperation?.Invoke(this, (StartupDatabaseAction)button.Tag);
    }
}
