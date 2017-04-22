namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.MiscTabPage = new System.Windows.Forms.TabPage();
            this.ExportToCollectionXmlCheckBox = new System.Windows.Forms.CheckBox();
            this.UiLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.UiLanguageLabel = new System.Windows.Forms.Label();
            this.MainTabControl.SuspendLayout();
            this.MiscTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(367, 426);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 2;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.OnDiscardButtonClick);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(286, 426);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.MiscTabPage);
            this.MainTabControl.Location = new System.Drawing.Point(12, 12);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(434, 408);
            this.MainTabControl.TabIndex = 0;
            // 
            // MiscTabPage
            // 
            this.MiscTabPage.Controls.Add(this.ExportToCollectionXmlCheckBox);
            this.MiscTabPage.Controls.Add(this.UiLanguageComboBox);
            this.MiscTabPage.Controls.Add(this.UiLanguageLabel);
            this.MiscTabPage.Location = new System.Drawing.Point(4, 22);
            this.MiscTabPage.Name = "MiscTabPage";
            this.MiscTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.MiscTabPage.Size = new System.Drawing.Size(426, 382);
            this.MiscTabPage.TabIndex = 1;
            this.MiscTabPage.Text = "Misc.";
            this.MiscTabPage.UseVisualStyleBackColor = true;
            // 
            // ExportToCollectionXmlCheckBox
            // 
            this.ExportToCollectionXmlCheckBox.AutoSize = true;
            this.ExportToCollectionXmlCheckBox.Location = new System.Drawing.Point(9, 33);
            this.ExportToCollectionXmlCheckBox.Name = "ExportToCollectionXmlCheckBox";
            this.ExportToCollectionXmlCheckBox.Size = new System.Drawing.Size(135, 17);
            this.ExportToCollectionXmlCheckBox.TabIndex = 9;
            this.ExportToCollectionXmlCheckBox.Text = "Export to Collection.xml";
            this.ExportToCollectionXmlCheckBox.UseVisualStyleBackColor = true;
            // 
            // UiLanguageComboBox
            // 
            this.UiLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UiLanguageComboBox.FormattingEnabled = true;
            this.UiLanguageComboBox.Location = new System.Drawing.Point(270, 6);
            this.UiLanguageComboBox.Name = "UiLanguageComboBox";
            this.UiLanguageComboBox.Size = new System.Drawing.Size(150, 21);
            this.UiLanguageComboBox.TabIndex = 8;
            // 
            // UiLanguageLabel
            // 
            this.UiLanguageLabel.AutoSize = true;
            this.UiLanguageLabel.Location = new System.Drawing.Point(6, 9);
            this.UiLanguageLabel.Name = "UiLanguageLabel";
            this.UiLanguageLabel.Size = new System.Drawing.Size(69, 13);
            this.UiLanguageLabel.TabIndex = 7;
            this.UiLanguageLabel.Text = "UI Language";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.SaveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.ClientSize = new System.Drawing.Size(459, 466);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.SaveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(475, 505);
            this.MinimumSize = new System.Drawing.Size(475, 505);
            this.Name = "SettingsForm";
            this.Text = "Options";
            this.MainTabControl.ResumeLayout(false);
            this.MiscTabPage.ResumeLayout(false);
            this.MiscTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox UiLanguageComboBox;
        private System.Windows.Forms.Label UiLanguageLabel;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage MiscTabPage;
        private System.Windows.Forms.CheckBox ExportToCollectionXmlCheckBox;
    }
}