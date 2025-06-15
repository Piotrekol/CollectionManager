namespace CollectionManager.WinForms.Controls;

using BrightIdeasSoftware;

partial class ScoresListingView
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ListViewScores = new FastDataListView();
        TotalScore = new OLVColumn();
        PlayerName = new OLVColumn();
        C300 = new OLVColumn();
        C100 = new OLVColumn();
        C50 = new OLVColumn();
        Geki = new OLVColumn();
        Katu = new OLVColumn();
        Miss = new OLVColumn();
        MaxCombo = new OLVColumn();
        Perfect = new OLVColumn();
        Mods = new OLVColumn();
        Date = new OLVColumn();
        Acc = new OLVColumn();
        ((System.ComponentModel.ISupportInitialize)ListViewScores).BeginInit();
        SuspendLayout();
        // 
        // ListViewScores
        // 
        ListViewScores.AllColumns.Add(TotalScore);
        ListViewScores.AllColumns.Add(PlayerName);
        ListViewScores.AllColumns.Add(C300);
        ListViewScores.AllColumns.Add(C100);
        ListViewScores.AllColumns.Add(C50);
        ListViewScores.AllColumns.Add(Geki);
        ListViewScores.AllColumns.Add(Katu);
        ListViewScores.AllColumns.Add(Miss);
        ListViewScores.AllColumns.Add(MaxCombo);
        ListViewScores.AllColumns.Add(Perfect);
        ListViewScores.AllColumns.Add(Mods);
        ListViewScores.AllColumns.Add(Date);
        ListViewScores.AllColumns.Add(Acc);
        ListViewScores.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { TotalScore, PlayerName, MaxCombo, Mods, Acc, C300, C100, C50, Geki, Katu, Miss, Date, Perfect });
        ListViewScores.DataSource = null;
        ListViewScores.Dock = System.Windows.Forms.DockStyle.Fill;
        ListViewScores.EmptyListMsg = "";
        ListViewScores.Location = new System.Drawing.Point(0, 0);
        ListViewScores.Name = "ListViewScores";
        ListViewScores.ShowGroups = false;
        ListViewScores.Size = new System.Drawing.Size(783, 411);
        ListViewScores.TabIndex = 0;
        ListViewScores.View = System.Windows.Forms.View.Details;
        ListViewScores.VirtualMode = true;
        // 
        // TotalScore
        // 
        TotalScore.AspectName = "TotalScore";
        TotalScore.IsEditable = false;
        TotalScore.Text = "Score";
        // 
        // PlayerName
        // 
        PlayerName.AspectName = "PlayerName";
        PlayerName.IsEditable = false;
        PlayerName.Text = "Name";
        // 
        // C300
        // 
        C300.AspectName = "C300";
        C300.IsEditable = false;
        C300.IsVisible = false;
        C300.Text = "300";
        C300.Width = 40;
        // 
        // C100
        // 
        C100.AspectName = "C100";
        C100.IsEditable = false;
        C100.IsVisible = false;
        C100.Text = "100";
        C100.Width = 40;
        // 
        // C50
        // 
        C50.AspectName = "C50";
        C50.IsEditable = false;
        C50.IsVisible = false;
        C50.Text = "50";
        C50.Width = 40;
        // 
        // Geki
        // 
        Geki.AspectName = "Geki";
        Geki.IsEditable = false;
        Geki.IsVisible = false;
        Geki.Text = "Geki";
        Geki.Width = 40;
        // 
        // Katu
        // 
        Katu.AspectName = "Katu";
        Katu.IsEditable = false;
        Katu.IsVisible = false;
        Katu.Text = "Katu";
        Katu.Width = 40;
        // 
        // Miss
        // 
        Miss.AspectName = "Miss";
        Miss.IsEditable = false;
        Miss.Text = "Miss";
        Miss.Width = 40;
        // 
        // MaxCombo
        // 
        MaxCombo.AspectName = "MaxCombo";
        MaxCombo.IsEditable = false;
        MaxCombo.Text = "Combo";
        // 
        // Perfect
        // 
        Perfect.AspectName = "Perfect";
        Perfect.CheckBoxes = true;
        Perfect.IsEditable = false;
        Perfect.IsVisible = false;
        Perfect.Text = "Perfect";
        // 
        // Mods
        // 
        Mods.AspectName = "Mods";
        Mods.IsEditable = false;
        Mods.Text = "Mods";
        // 
        // Date
        // 
        Date.AspectName = "Date";
        Date.IsEditable = false;
        Date.Text = "Date";
        Date.Width = 120;
        // 
        // Acc
        // 
        Acc.AspectName = "Accuracy";
        Acc.Text = "Acc";
        // 
        // ScoresListingView
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(ListViewScores);
        Name = "ScoresListingView";
        Size = new System.Drawing.Size(783, 411);
        ((System.ComponentModel.ISupportInitialize)ListViewScores).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private BrightIdeasSoftware.FastDataListView ListViewScores;
    private BrightIdeasSoftware.OLVColumn PlayerName;
    private BrightIdeasSoftware.OLVColumn C300;
    private BrightIdeasSoftware.OLVColumn C100;
    private BrightIdeasSoftware.OLVColumn C50;
    private BrightIdeasSoftware.OLVColumn Geki;
    private BrightIdeasSoftware.OLVColumn Katu;
    private BrightIdeasSoftware.OLVColumn Miss;
    private BrightIdeasSoftware.OLVColumn TotalScore;
    private BrightIdeasSoftware.OLVColumn MaxCombo;
    private BrightIdeasSoftware.OLVColumn Perfect;
    private BrightIdeasSoftware.OLVColumn Mods;
    private BrightIdeasSoftware.OLVColumn Date;
    private BrightIdeasSoftware.OLVColumn Acc;
}
