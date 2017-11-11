using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GuiComponents.Interfaces;

namespace GuiComponents.Forms
{
    public partial class UserTopGeneratorForm : BaseForm, IUserTopGeneratorForm
    {
        public UserTopGeneratorForm()
        {
            InitializeComponent();
        }

        public IUserTopGenerator UserTopGeneratorView => userTopGenerator1;
    }
}
