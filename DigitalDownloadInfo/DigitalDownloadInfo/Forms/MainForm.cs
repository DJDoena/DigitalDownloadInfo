using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DigitalDownloadInfo.Resources;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    internal sealed partial class MainForm : Form
    {
        private readonly Plugin Plugin;

        private readonly IDVDInfo Profile;

        private readonly DataManager DataManager;

        private Boolean DataChanged;

        internal MainForm(Plugin plugin
            , IDVDInfo profile)
        {
            Plugin = plugin;

            Profile = profile;

            InitializeComponent();

            DataManager = new DataManager(Profile);

            SetTextBoxes();

            List<String> companies = new List<String>(Plugin.Companies.Keys);
            companies.Sort();
            CompanyComboBox.Items.AddRange(companies.ToArray());

            SetLabels();

            SetReadOnlies();

            DataChanged = false;
        }

        private void SetReadOnlies()
        {
            if (Plugin.IsRemoteAccess)
            {
                ImportFromXMLToolStripMenuItem.Enabled = false;
                PasteAllToolStripMenuItem.Enabled = false;
                SaveButton.Enabled = false;

                SetControlsReadonly(Controls);
            }
        }

        private void SetControlsReadonly(Control.ControlCollection controls)
        {
            if (controls != null)
            {
                foreach (Control control in controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = true;
                    }
                    else if (control is ComboBox)
                    {
                        ((ComboBox)control).Enabled = true;
                    }
                    else
                    {
                        SetControlsReadonly(control.Controls);
                    }
                }
            }
        }

        private void SetTextBoxes()
        {
            CompanyComboBox.Text = DataManager.GetCompanyWithFallback();
            CodeTextBox.Text = DataManager.GetCodeWithFallback();
        }

        private void SetLabels()
        {
            CompanyLabel.Text = Texts.Company;
            CodeLabel.Text = Texts.Code;

            #region Misc

            #region Menu

            EditToolStripMenuItem.Text = Texts.Edit;
            CopyAllToolStripMenuItem.Text = Texts.CopyAllToClipboard;
            PasteAllToolStripMenuItem.Text = Texts.PasteAllFromClipboard;

            ToolsToolStripMenuItem.Text = Texts.Tools;
            OptionsToolStripMenuItem.Text = Texts.Options;
            ExportToXMLToolStripMenuItem.Text = Texts.ExportToXml;
            ImportFromXMLToolStripMenuItem.Text = Texts.ImportFromXml;
            ExportOptionsToolStripMenuItem.Text = Texts.ExportOptions;
            ImportOptionsToolStripMenuItem.Text = Texts.ImportOptions;

            HelpToolStripMenuItem.Text = Texts.Help;
            CheckForUpdatesToolStripMenuItem.Text = Texts.CheckForUpdates;
            AboutToolStripMenuItem.Text = Texts.About;

            #endregion

            #region Buttons

            SaveButton.Text = Texts.Save;
            DiscardButton.Text = Texts.Cancel;

            #endregion

            #endregion
        }

        private void OnSaveButtonClick(Object sender
            , EventArgs e)
        {
            String company;
            if (DataManager.GetCompany(out company))
            {
                UInt16 count;
                if (Plugin.Companies.TryGetValue(company, out count))
                {
                    count--;
                    if (count == 0)
                    {
                        Plugin.Companies.Remove(company);
                    }
                    else
                    {
                        Plugin.Companies[company] = count;
                    }
                }
            }

            DataManager.SetCompany(CompanyComboBox.Text);

            if (DataManager.GetCompany(out company))
            {
                UInt16 count;

                if (Plugin.Companies.TryGetValue(company, out count))
                {
                    count++;
                    Plugin.Companies[company] = count;
                }
                else
                {
                    Plugin.Companies.Add(company, 1);
                }
            }

            DataManager.SetCode(CodeTextBox.Text);
                        
            DataChanged = false;

            Close();
        }

        private void OnDiscardButtonClick(Object sender
            , EventArgs e)
        {
            Close();
        }

        private void OnOptionsToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            using (SettingsForm form = new SettingsForm(Plugin))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Plugin.RegisterCustomFields();
                }
            }
        }

        private void OnExportToXMLToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.DefaultExt = ".xml";
                sfd.Filter = "XML files|*.xml";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.Title = Texts.SaveXmlFile;
                sfd.FileName = "DigitalDownloadInfo." + Profile.GetProfileID() + ".xml";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DigitalDownloadInfo ddi = GetDigitalDownloadInfoForXmlStructure();

                    try
                    {
                        DVDProfilerSerializer<DigitalDownloadInfo>.Serialize(sfd.FileName, ddi);

                        MessageBox.Show(MessageBoxTexts.Done, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, sfd.FileName, ex.Message)
                            , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private DigitalDownloadInfo GetDigitalDownloadInfoForXmlStructure()
        {
            DigitalDownloadInfo ddi = new DigitalDownloadInfo();

            ddi.Company = GetText(CompanyComboBox);
            ddi.Code = GetText(CodeTextBox);

            return (ddi);
        }

        private Text GetText(TextBox textBox)
        {
            Text text = null;

            String title = textBox.Text;

            if (String.IsNullOrEmpty(title) == false)
            {
                text = new Text();
                text.Value = title;
            }

            return (text);
        }

        private Text GetText(ComboBox comboBox)
        {
            Text text = null;

            String title = comboBox.Text;

            if (String.IsNullOrEmpty(title) == false)
            {
                text = new Text();
                text.Value = title;
            }

            return (text);
        }

        private void OnImportFromXMLToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.Filter = "XML files|*.xml";
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Title = Texts.LoadXmlFile;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    DigitalDownloadInfo ddi = null;

                    try
                    {
                        ddi = DVDProfilerSerializer<DigitalDownloadInfo>.Deserialize(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeRead, ofd.FileName, ex.Message)
                           , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (ddi != null)
                    {
                        SetDigitalDownloadInfoFromXmlStructure(ddi);
                    }
                }
            }
        }

        private void SetDigitalDownloadInfoFromXmlStructure(DigitalDownloadInfo ddi)
        {
            SetText(ddi.Company, CompanyComboBox);
            SetText(ddi.Code, CodeTextBox);
        }

        private void SetText(Text text
            , TextBox textBox)
        {
            textBox.Text = (text?.Value != null) ? (text.Value) : (String.Empty);
        }

        private void SetText(Text text
            , ComboBox comboBox)
        {
            comboBox.Text = (text?.Value != null) ? (text.Value) : (String.Empty);
        }

        private void OnCheckForUpdatesToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            OnlineAccess.Init("Doena Soft.", "DigitalDownloadInfo");

            OnlineAccess.CheckForNewVersion("http://doena-soft.de/dvdprofiler/3.9.0/versions.xml", this, "DigitalDownloadInfo", GetType().Assembly);
        }

        private void OnAboutToolStripMenuItemClick(Object sender, EventArgs e)
        {
            using (AboutBox aboutBox = new AboutBox(GetType().Assembly))
            {
                aboutBox.ShowDialog();
            }
        }

        private void OnDataChanged(Object sender
            , EventArgs e)
        {
            DataChanged = true;
        }

        private void OnFormClosing(Object sender, FormClosingEventArgs e)
        {
            if (DataChanged)
            {
                if (MessageBox.Show(MessageBoxTexts.AbandonChangesText, MessageBoxTexts.AbandonChangesHeader, MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void OnImportOptionsToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            Plugin.ImportOptions();

            SetLabels();
        }

        private void OnExportOptionsToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            Plugin.ExportOptions();
        }

        private void OnCopyAllToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            DigitalDownloadInfo ddi = GetDigitalDownloadInfoForXmlStructure();

            String xml = DVDProfilerSerializer<DigitalDownloadInfo>.ToString(ddi);

            try
            {
                Clipboard.SetDataObject(xml, true, 4, 250);
            }
            catch
            { }
        }

        private void OnPasteAllToolStripMenuItemClick(Object sender
            , EventArgs e)
        {
            DigitalDownloadInfo ddi = null;

            try
            {
                String xml = Clipboard.GetText();

                ddi = DVDProfilerSerializer<DigitalDownloadInfo>.FromString(xml);
            }
            catch
            {
                MessageBox.Show(MessageBoxTexts.PasteFromClipboardFailed, MessageBoxTexts.ErrorHeader
                    , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (ddi != null)
            {
                SetDigitalDownloadInfoFromXmlStructure(ddi);
            }
        }
    }
}