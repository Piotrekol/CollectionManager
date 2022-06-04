using Common;
using Gui.Misc;
using GuiComponents.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool LoadDefaultCollectionButtonEnabled
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

        public bool ButtonsEnabled
        {
            get => button_DoNothing.Enabled;
            set
            {
                button_DoNothing.Enabled 
                    = button_LoadOsuCollection.Enabled
                    = button_LoadCollectionFromFile.Enabled 
                    = button_LoadDatabase.Enabled 
                    = button_LoadDatabaseSkip.Enabled 
                    = value;
            }
        }

        public StartupView()
        {
            InitializeComponent();
            button_DoNothing.Tag = StartupCollectionAction.None;
            button_LoadOsuCollection.Tag = StartupCollectionAction.LoadDefaultCollection;
            button_LoadCollectionFromFile.Tag = StartupCollectionAction.LoadCollection;

            button_LoadDatabase.Tag = StartupDatabaseAction.LoadFromDifferentLocation;
            button_LoadDatabaseSkip.Tag = StartupDatabaseAction.None;
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
