using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class OsustatsApiLoginFormView : BaseForm, IOsustatsApiLoginFormView
    {
        public OsustatsApiLoginFormView()
        {
            InitializeComponent();
        }
        public string ApiKey => textBox_apiKey.Text;

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://osustats.ppy.sh/collections/apikey");
        }
    }
}
