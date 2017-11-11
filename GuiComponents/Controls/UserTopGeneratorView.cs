using System;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Controls
{
    public partial class UserTopGeneratorView : UserControl, IUserTopGenerator
    {

        public event EventHandler Start;
        public event EventHandler Abort;
        public event EventHandler GenerateUsernames;
        public event EventHandler CollectionNamingFormatChanged;

        public string Usernames => textBox_usernames.Text;
        public string CollectionNamingFormat => textBox_collectionNameFormat.Text;

        public string CollectionNamingExample
        {
            set
            {
                if (label_collectionNameExample.InvokeRequired)
                {
                    label_collectionNameExample.BeginInvoke((MethodInvoker)(() =>
                    {
                        CollectionNamingExample = value;
                    }));
                    return;
                }
                label_collectionNameExample.Text = value;
            }
        }

        public int AllowedScores { get; private set; } = 0;
        public string ApiKey => textBox_apiKey.Text;
        public double PpMin => Convert.ToDouble(numericUpDown_minimumPP.Value);
        public double PpMax => Convert.ToDouble(numericUpDown_maximumPP.Value);
        public double AccMin => Convert.ToDouble(numericUpDown_accMin.Value);
        public double AccMax => Convert.ToDouble(numericUpDown_accMax.Value);
        public bool GroupByMods => checkBox_GroupByMods.Checked;
        public bool MergeCollectionsWithSameName => checkBox_mergeCollections.Checked;
        public ICollectionListingView CollectionListing => collectionListingView1;

        public string ProcessingStatus
        {
            set
            {
                if (label_processingStatus.InvokeRequired)
                {
                    label_processingStatus.BeginInvoke((MethodInvoker)(() =>
                    {
                        ProcessingStatus = value;
                    }));
                    return;
                }
                label_processingStatus.Text = value;

            }
        }

        public UserTopGeneratorView()
        {
            InitializeComponent();

        }

        private void RadioButton_AllowScores_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = (RadioButton)sender;
            if (radioButton.Checked)
                AllowedScores = Convert.ToInt32(radioButton.Tag);
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        private void button_Abort_Click(object sender, EventArgs e)
        {
            Abort?.Invoke(this, EventArgs.Empty);
        }

        private void button_GenerateUsernames_Click(object sender, EventArgs e)
        {
            GenerateUsernames?.Invoke(this, EventArgs.Empty);
        }

        private void textBox_collectionNameFormat_TextChanged(object sender, EventArgs e)
        {
            CollectionNamingFormatChanged?.Invoke(this, EventArgs.Empty);
        }

        public double ProcessingCompletionPrecentage
        {
            set
            {
                if (progressBar_usernames.InvokeRequired)
                {
                    progressBar_usernames.BeginInvoke((MethodInvoker)(() =>
                    {
                        ProcessingCompletionPrecentage = value;
                    }));
                    return;
                }
                progressBar_usernames.Value = Convert.ToInt32(value);
            }
        }

        public bool IsRunning
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        IsRunning = value;
                    }));
                    return;
                }
                button_Start.Enabled = !value;
                button_Abort.Enabled = value;
                groupBox_configuration.Enabled = !value;
            }
        }
    }
}
