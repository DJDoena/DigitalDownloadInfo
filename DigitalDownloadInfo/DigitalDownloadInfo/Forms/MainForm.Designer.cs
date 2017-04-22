namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
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
            if (disposing && (components != null))
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportFromXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CodeLabel = new System.Windows.Forms.Label();
            this.CodeTextBox = new System.Windows.Forms.TextBox();
            this.CompanyLabel = new System.Windows.Forms.Label();
            this.CompanyComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            resources.ApplyResources(this.SaveButton, "SaveButton");
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.DiscardButton, "DiscardButton");
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.OnDiscardButtonClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.HelpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyAllToolStripMenuItem,
            this.PasteAllToolStripMenuItem});
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            resources.ApplyResources(this.EditToolStripMenuItem, "EditToolStripMenuItem");
            // 
            // CopyAllToolStripMenuItem
            // 
            this.CopyAllToolStripMenuItem.Name = "CopyAllToolStripMenuItem";
            resources.ApplyResources(this.CopyAllToolStripMenuItem, "CopyAllToolStripMenuItem");
            this.CopyAllToolStripMenuItem.Click += new System.EventHandler(this.OnCopyAllToolStripMenuItemClick);
            // 
            // PasteAllToolStripMenuItem
            // 
            this.PasteAllToolStripMenuItem.Name = "PasteAllToolStripMenuItem";
            resources.ApplyResources(this.PasteAllToolStripMenuItem, "PasteAllToolStripMenuItem");
            this.PasteAllToolStripMenuItem.Click += new System.EventHandler(this.OnPasteAllToolStripMenuItemClick);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OptionsToolStripMenuItem,
            this.ExportToXMLToolStripMenuItem,
            this.ImportFromXMLToolStripMenuItem,
            this.ExportOptionsToolStripMenuItem,
            this.ImportOptionsToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            resources.ApplyResources(this.ToolsToolStripMenuItem, "ToolsToolStripMenuItem");
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            resources.ApplyResources(this.OptionsToolStripMenuItem, "OptionsToolStripMenuItem");
            this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OnOptionsToolStripMenuItemClick);
            // 
            // ExportToXMLToolStripMenuItem
            // 
            this.ExportToXMLToolStripMenuItem.Name = "ExportToXMLToolStripMenuItem";
            resources.ApplyResources(this.ExportToXMLToolStripMenuItem, "ExportToXMLToolStripMenuItem");
            this.ExportToXMLToolStripMenuItem.Click += new System.EventHandler(this.OnExportToXMLToolStripMenuItemClick);
            // 
            // ImportFromXMLToolStripMenuItem
            // 
            this.ImportFromXMLToolStripMenuItem.Name = "ImportFromXMLToolStripMenuItem";
            resources.ApplyResources(this.ImportFromXMLToolStripMenuItem, "ImportFromXMLToolStripMenuItem");
            this.ImportFromXMLToolStripMenuItem.Click += new System.EventHandler(this.OnImportFromXMLToolStripMenuItemClick);
            // 
            // ExportOptionsToolStripMenuItem
            // 
            this.ExportOptionsToolStripMenuItem.Name = "ExportOptionsToolStripMenuItem";
            resources.ApplyResources(this.ExportOptionsToolStripMenuItem, "ExportOptionsToolStripMenuItem");
            this.ExportOptionsToolStripMenuItem.Click += new System.EventHandler(this.OnExportOptionsToolStripMenuItemClick);
            // 
            // ImportOptionsToolStripMenuItem
            // 
            this.ImportOptionsToolStripMenuItem.Name = "ImportOptionsToolStripMenuItem";
            resources.ApplyResources(this.ImportOptionsToolStripMenuItem, "ImportOptionsToolStripMenuItem");
            this.ImportOptionsToolStripMenuItem.Click += new System.EventHandler(this.OnImportOptionsToolStripMenuItemClick);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckForUpdatesToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            resources.ApplyResources(this.HelpToolStripMenuItem, "HelpToolStripMenuItem");
            // 
            // CheckForUpdatesToolStripMenuItem
            // 
            this.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem";
            resources.ApplyResources(this.CheckForUpdatesToolStripMenuItem, "CheckForUpdatesToolStripMenuItem");
            this.CheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.OnCheckForUpdatesToolStripMenuItemClick);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            resources.ApplyResources(this.AboutToolStripMenuItem, "AboutToolStripMenuItem");
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutToolStripMenuItemClick);
            // 
            // CodeLabel
            // 
            resources.ApplyResources(this.CodeLabel, "CodeLabel");
            this.CodeLabel.Name = "CodeLabel";
            // 
            // CodeTextBox
            // 
            resources.ApplyResources(this.CodeTextBox, "CodeTextBox");
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.TextChanged += new System.EventHandler(this.OnDataChanged);
            // 
            // CompanyLabel
            // 
            resources.ApplyResources(this.CompanyLabel, "CompanyLabel");
            this.CompanyLabel.Name = "CompanyLabel";
            // 
            // CompanyComboBox
            // 
            this.CompanyComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.CompanyComboBox, "CompanyComboBox");
            this.CompanyComboBox.Name = "CompanyComboBox";
            this.CompanyComboBox.TextChanged += new System.EventHandler(this.OnDataChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.SaveButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.Controls.Add(this.CompanyComboBox);
            this.Controls.Add(this.CodeLabel);
            this.Controls.Add(this.CodeTextBox);
            this.Controls.Add(this.CompanyLabel);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CheckForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportFromXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PasteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.Label CodeLabel;
        private System.Windows.Forms.TextBox CodeTextBox;
        private System.Windows.Forms.Label CompanyLabel;
        private System.Windows.Forms.ComboBox CompanyComboBox;
    }
}