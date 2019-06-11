using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiComponents.Forms;

namespace GuiComponents
{
    public partial class YesNoForm : BaseForm
    {
        public bool DoNotAskAgain { get; private set; }
        public YesNoForm()
        {
            InitializeComponent();
        }

        internal static (DialogResult dialogResult, bool doNotAskAgain) ShowDialog(string text, string caption,
            string doNotAskAgainText)
        {
            using (var f = new YesNoForm())
            {
                f.label_text.Text = text;
                f.Text = $"Collection Manager - {caption}";
                f.checkBox_doNotAskAgain.Text = doNotAskAgainText;

                return (f.ShowDialog(), f.DoNotAskAgain);
            }
        }

        private void CheckBox_doNotAskAgain_CheckedChanged(object sender, EventArgs e)
        {
            this.DoNotAskAgain = checkBox_doNotAskAgain.Checked;
        }
    }
}
