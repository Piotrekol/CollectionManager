using Common;
using Gui.Misc;
using GuiComponents.Interfaces;
using System;
using System.Windows.Forms;

namespace GuiComponents.Controls
{
    public partial class StartupView : UserControl, IStartupView
    {
        public event GuiHelpers.StartupCollectionEventArgs StartupCollectionOperation;
        public event GuiHelpers.StartupDatabaseEventArgs StartupDatabaseOperation;

        public string LoadDatabaseStatusText
        {
            get => label_LoadDatabaseStatus.Text;
            set
            {
                if (label_LoadDatabaseStatus.InvokeRequired)
                {
                    label_LoadDatabaseStatus.Invoke((MethodInvoker)(() =>
                    {
                        label_LoadDatabaseStatus.Text = value;
                    }));
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
                    checkBox_DoNotShowOnStartup.Invoke((MethodInvoker)(() =>
                    {
                        checkBox_DoNotShowOnStartup.Checked = value;
                    }));
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
                    button_LoadOsuCollection.Invoke((MethodInvoker)(() =>
                    {
                        button_LoadOsuCollection.Enabled = value;
                    }));
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
                    label_OpenedCollection.Invoke((MethodInvoker)(() =>
                    {
                        label_OpenedCollection.Visible = !string.IsNullOrEmpty(value);
                        label_OpenedCollection.Text = value;
                    }));
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
                    button_LoadCollectionFromFile.Invoke((MethodInvoker)(() =>
                    {
                        button_DoNothing.Enabled
                            = button_LoadCollectionFromFile.Enabled
                            = value;

                        button_LoadOsuCollection.Enabled = LoadOsuCollectionButtonEnabled && value;
                    }));
                    return;
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
                    button_LoadDatabase.Invoke((MethodInvoker)(() =>
                    {
                        button_LoadDatabase.Enabled
                            = button_LoadDatabaseSkip.Enabled
                            = value;
                    }));
                    return;
                }

                button_LoadDatabase.Enabled
                    = button_LoadDatabaseSkip.Enabled
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
                    checkBox_DoNotShowOnStartup.Invoke((MethodInvoker)(() =>
                    {
                        checkBox_DoNotShowOnStartup.Enabled = value;
                    }));
                    return;
                }

                checkBox_DoNotShowOnStartup.Enabled = value;
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
        }

        private void button_StartupAction_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            StartupCollectionOperation?.Invoke(this, (StartupCollectionAction)button.Tag);
        }

        private void button_DatabaseAction_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            StartupDatabaseOperation?.Invoke(this, (StartupDatabaseAction)button.Tag);
        }
    }
}
