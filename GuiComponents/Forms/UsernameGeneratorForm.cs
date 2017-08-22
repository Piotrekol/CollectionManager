using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Interfaces.Controls;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class UsernameGeneratorForm : BaseForm, IUsernameGeneratorForm
    {
        public UsernameGeneratorForm()
        {
            InitializeComponent();
        }

        public IUsernameGeneratorView view => usernameGeneratorView1;
    }
}
