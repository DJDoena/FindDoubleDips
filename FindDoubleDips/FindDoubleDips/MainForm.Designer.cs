namespace DoenaSoft.DVDProfiler.FindDoubleDips
{
    partial class MainForm
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
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFlagSetListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportFlagSetListOfSelectedRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDoubleDipsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDoubleDipsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDoupleDipsAsHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PurchasesDataGridView = new System.Windows.Forms.DataGridView();
            this.GroupBox = new System.Windows.Forms.GroupBox();
            this.CheckOriginalTitlesRadioButton = new System.Windows.Forms.RadioButton();
            this.CheckTitlesRadioButton = new System.Windows.Forms.RadioButton();
            this.IgnoreWishListTitlesCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreProductionYearCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IgnoreBoxSetContentsCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreSameDatePurchasesCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreTelevisonTitlesCheckBox = new System.Windows.Forms.CheckBox();
            this.StartSearchingButton = new System.Windows.Forms.Button();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PurchasesDataGridView)).BeginInit();
            this.GroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            resources.ApplyResources(this.MenuStrip, "MenuStrip");
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuStrip.Name = "MenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportFlagSetListToolStripMenuItem,
            this.exportFlagSetListOfSelectedRowsToolStripMenuItem,
            this.openDoubleDipsFileToolStripMenuItem,
            this.saveDoubleDipsFileToolStripMenuItem,
            this.saveDoupleDipsAsHTMLToolStripMenuItem,
            this.saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // exportFlagSetListToolStripMenuItem
            // 
            resources.ApplyResources(this.exportFlagSetListToolStripMenuItem, "exportFlagSetListToolStripMenuItem");
            this.exportFlagSetListToolStripMenuItem.Name = "exportFlagSetListToolStripMenuItem";
            this.exportFlagSetListToolStripMenuItem.Click += new System.EventHandler(this.OnMarkProfilesToolStripMenuItemClick);
            // 
            // exportFlagSetListOfSelectedRowsToolStripMenuItem
            // 
            resources.ApplyResources(this.exportFlagSetListOfSelectedRowsToolStripMenuItem, "exportFlagSetListOfSelectedRowsToolStripMenuItem");
            this.exportFlagSetListOfSelectedRowsToolStripMenuItem.Name = "exportFlagSetListOfSelectedRowsToolStripMenuItem";
            this.exportFlagSetListOfSelectedRowsToolStripMenuItem.Click += new System.EventHandler(this.OnMarkSelectedProfilesToolStripMenuItemClick);
            // 
            // openDoubleDipsFileToolStripMenuItem
            // 
            resources.ApplyResources(this.openDoubleDipsFileToolStripMenuItem, "openDoubleDipsFileToolStripMenuItem");
            this.openDoubleDipsFileToolStripMenuItem.Name = "openDoubleDipsFileToolStripMenuItem";
            this.openDoubleDipsFileToolStripMenuItem.Click += new System.EventHandler(this.OnOpenDoubleDipsFileToolStripMenuItemClick);
            // 
            // saveDoubleDipsFileToolStripMenuItem
            // 
            resources.ApplyResources(this.saveDoubleDipsFileToolStripMenuItem, "saveDoubleDipsFileToolStripMenuItem");
            this.saveDoubleDipsFileToolStripMenuItem.Name = "saveDoubleDipsFileToolStripMenuItem";
            this.saveDoubleDipsFileToolStripMenuItem.Click += new System.EventHandler(this.OnSaveDoubleDipsFileToolStripMenuItemClick);
            // 
            // saveDoupleDipsAsHTMLToolStripMenuItem
            // 
            resources.ApplyResources(this.saveDoupleDipsAsHTMLToolStripMenuItem, "saveDoupleDipsAsHTMLToolStripMenuItem");
            this.saveDoupleDipsAsHTMLToolStripMenuItem.Name = "saveDoupleDipsAsHTMLToolStripMenuItem";
            this.saveDoupleDipsAsHTMLToolStripMenuItem.Click += new System.EventHandler(this.OnSaveDoupleDipsAsHTMLToolStripMenuItemClick);
            // 
            // saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem
            // 
            resources.ApplyResources(this.saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem, "saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem");
            this.saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem.Name = "saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem";
            this.saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem.Click += new System.EventHandler(this.OnSaveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItemClick);
            // 
            // languageToolStripMenuItem
            // 
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.germanToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            // 
            // englishToolStripMenuItem
            // 
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.OnEnglishToolStripMenuItemClick);
            // 
            // germanToolStripMenuItem
            // 
            resources.ApplyResources(this.germanToolStripMenuItem, "germanToolStripMenuItem");
            this.germanToolStripMenuItem.Name = "germanToolStripMenuItem";
            this.germanToolStripMenuItem.Click += new System.EventHandler(this.OnGermamToolStripMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readMeToolStripMenuItem,
            this.checkForUpdateToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // readMeToolStripMenuItem
            // 
            resources.ApplyResources(this.readMeToolStripMenuItem, "readMeToolStripMenuItem");
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Click += new System.EventHandler(this.OnReadMeToolStripMenuItemClick);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            resources.ApplyResources(this.checkForUpdateToolStripMenuItem, "checkForUpdateToolStripMenuItem");
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.OnCheckForUpdateToolStripMenuItemClick);
            // 
            // aboutToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutToolStripMenuItemClick);
            // 
            // PurchasesDataGridView
            // 
            resources.ApplyResources(this.PurchasesDataGridView, "PurchasesDataGridView");
            this.PurchasesDataGridView.AllowUserToAddRows = false;
            this.PurchasesDataGridView.AllowUserToOrderColumns = true;
            this.PurchasesDataGridView.AllowUserToResizeRows = false;
            this.PurchasesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.PurchasesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PurchasesDataGridView.Name = "PurchasesDataGridView";
            this.PurchasesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PurchasesDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.OnCurrentCellDirtyStateChanged);
            // 
            // GroupBox
            // 
            resources.ApplyResources(this.GroupBox, "GroupBox");
            this.GroupBox.Controls.Add(this.CheckOriginalTitlesRadioButton);
            this.GroupBox.Controls.Add(this.CheckTitlesRadioButton);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.TabStop = false;
            // 
            // CheckOriginalTitlesRadioButton
            // 
            resources.ApplyResources(this.CheckOriginalTitlesRadioButton, "CheckOriginalTitlesRadioButton");
            this.CheckOriginalTitlesRadioButton.Checked = true;
            this.CheckOriginalTitlesRadioButton.Name = "CheckOriginalTitlesRadioButton";
            this.CheckOriginalTitlesRadioButton.TabStop = true;
            this.CheckOriginalTitlesRadioButton.UseVisualStyleBackColor = true;
            // 
            // CheckTitlesRadioButton
            // 
            resources.ApplyResources(this.CheckTitlesRadioButton, "CheckTitlesRadioButton");
            this.CheckTitlesRadioButton.Name = "CheckTitlesRadioButton";
            this.CheckTitlesRadioButton.UseVisualStyleBackColor = true;
            // 
            // IgnoreWishListTitlesCheckBox
            // 
            resources.ApplyResources(this.IgnoreWishListTitlesCheckBox, "IgnoreWishListTitlesCheckBox");
            this.IgnoreWishListTitlesCheckBox.Name = "IgnoreWishListTitlesCheckBox";
            this.IgnoreWishListTitlesCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreProductionYearCheckBox
            // 
            resources.ApplyResources(this.IgnoreProductionYearCheckBox, "IgnoreProductionYearCheckBox");
            this.IgnoreProductionYearCheckBox.Name = "IgnoreProductionYearCheckBox";
            this.IgnoreProductionYearCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.IgnoreBoxSetContentsCheckBox);
            this.groupBox1.Controls.Add(this.IgnoreSameDatePurchasesCheckBox);
            this.groupBox1.Controls.Add(this.IgnoreTelevisonTitlesCheckBox);
            this.groupBox1.Controls.Add(this.IgnoreProductionYearCheckBox);
            this.groupBox1.Controls.Add(this.IgnoreWishListTitlesCheckBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // IgnoreBoxSetContentsCheckBox
            // 
            resources.ApplyResources(this.IgnoreBoxSetContentsCheckBox, "IgnoreBoxSetContentsCheckBox");
            this.IgnoreBoxSetContentsCheckBox.Name = "IgnoreBoxSetContentsCheckBox";
            this.IgnoreBoxSetContentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreSameDatePurchasesCheckBox
            // 
            resources.ApplyResources(this.IgnoreSameDatePurchasesCheckBox, "IgnoreSameDatePurchasesCheckBox");
            this.IgnoreSameDatePurchasesCheckBox.Name = "IgnoreSameDatePurchasesCheckBox";
            this.IgnoreSameDatePurchasesCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreTelevisonTitlesCheckBox
            // 
            resources.ApplyResources(this.IgnoreTelevisonTitlesCheckBox, "IgnoreTelevisonTitlesCheckBox");
            this.IgnoreTelevisonTitlesCheckBox.Name = "IgnoreTelevisonTitlesCheckBox";
            this.IgnoreTelevisonTitlesCheckBox.UseVisualStyleBackColor = true;
            // 
            // StartSearchingButton
            // 
            resources.ApplyResources(this.StartSearchingButton, "StartSearchingButton");
            this.StartSearchingButton.Name = "StartSearchingButton";
            this.StartSearchingButton.UseVisualStyleBackColor = true;
            this.StartSearchingButton.Click += new System.EventHandler(this.OnStartSearchingButtonClick);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StartSearchingButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GroupBox);
            this.Controls.Add(this.PurchasesDataGridView);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMainFormClosing);
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PurchasesDataGridView)).EndInit();
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.DataGridView PurchasesDataGridView;
        private System.Windows.Forms.GroupBox GroupBox;
        private System.Windows.Forms.RadioButton CheckOriginalTitlesRadioButton;
        private System.Windows.Forms.RadioButton CheckTitlesRadioButton;
        private System.Windows.Forms.CheckBox IgnoreProductionYearCheckBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportFlagSetListToolStripMenuItem;
        private System.Windows.Forms.CheckBox IgnoreWishListTitlesCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox IgnoreSameDatePurchasesCheckBox;
        private System.Windows.Forms.CheckBox IgnoreTelevisonTitlesCheckBox;
        private System.Windows.Forms.ToolStripMenuItem exportFlagSetListOfSelectedRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDoubleDipsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDoubleDipsFileToolStripMenuItem;
        private System.Windows.Forms.Button StartSearchingButton;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem germanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.CheckBox IgnoreBoxSetContentsCheckBox;
        private System.Windows.Forms.ToolStripMenuItem saveDoupleDipsAsHTMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDoubleDipsOfSelectedRowsAsHTMLToolStripMenuItem;
    }
}