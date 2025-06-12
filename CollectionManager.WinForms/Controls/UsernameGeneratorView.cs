namespace GuiComponents.Controls;

using CollectionManager.Common.Interfaces.Controls;
using System;
using System.Windows.Forms;

public partial class UsernameGeneratorView : UserControl, IUsernameGeneratorView
{
    public UsernameGeneratorView()
    {
        InitializeComponent();
    }

    public event EventHandler Start;
    public event EventHandler Abort;
    public int RankMin => Convert.ToInt32(numericUpDown_StartRank.Value);
    public int RankMax => Convert.ToInt32(numericUpDown_EndRank.Value);

    public int CompletionPrecentage
    {
        set
        {
            if (value is < 0 or > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (progressBar_usernames.InvokeRequired)
            {
                _ = progressBar_usernames.BeginInvoke((MethodInvoker)(() => CompletionPrecentage = value));
                return;
            }

            progressBar_usernames.Value = value;
        }
    }

    public string Status { get; set; }

    private void button_Start_Click(object sender, EventArgs e) => Start?.Invoke(this, EventArgs.Empty);

    private void button_Abort_Click(object sender, EventArgs e) => Abort?.Invoke(this, EventArgs.Empty);

    public string GeneratedUsernames
    {
        set
        {
            if (textBox1.InvokeRequired)
            {
                _ = textBox1.BeginInvoke((MethodInvoker)(() => GeneratedUsernames = value));
                return;
            }

            textBox1.Text = value;
        }
    }

    public bool StartEnabled
    {
        set
        {
            if (button_Start.InvokeRequired)
            {
                _ = button_Start.BeginInvoke((MethodInvoker)(() => StartEnabled = value));
                return;
            }

            button_Start.Enabled = value;
        }
    }

    public bool AbortEnabled
    {
        set
        {
            if (button_Abort.InvokeRequired)
            {
                _ = button_Abort.BeginInvoke((MethodInvoker)(() => AbortEnabled = value));
                return;
            }

            button_Abort.Enabled = value;
        }
    }
}
