using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DigitalDownloadInfo.Resources;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    public partial class SettingsForm : Form
    {
        private readonly Plugin Plugin;

        public SettingsForm(Plugin plugin)
        {
            Plugin = plugin;

            InitializeComponent();

            SetSettings();

            SetLabels();

            SetComboBoxes();
        }

        private void SetSettings()
        {
            ExportToCollectionXmlCheckBox.Checked = Plugin.Settings.DefaultValues.ExportToCollectionXml;
        }

        private void SetComboBoxes()
        {
            Dictionary<Int32, CultureInfo> uiLanguages = new Dictionary<Int32, CultureInfo>(2);

            CultureInfo en = CultureInfo.GetCultureInfo("en");

            uiLanguages.Add(en.LCID, en);

            CultureInfo de = CultureInfo.GetCultureInfo("de");

            uiLanguages.Add(de.LCID, de);

            UiLanguageComboBox.DataSource = new BindingSource(uiLanguages, null);
            UiLanguageComboBox.DisplayMember = "Value";
            UiLanguageComboBox.ValueMember = "Key";
            UiLanguageComboBox.Text = Plugin.Settings.DefaultValues.UiLanguage.DisplayName;
        }

        private void SetLabels()
        {
            Text = Texts.Options;

            MiscTabPage.Text = Texts.Misc;

            SaveButton.Text = Texts.Save;
            DiscardButton.Text = Texts.Cancel;

            ExportToCollectionXmlCheckBox.Text = Texts.ExportToCollectionXml;

            UiLanguageLabel.Text = Texts.UiLanguage;
        }

        private void OnDiscardButtonClick(Object sender
            , EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void OnSaveButtonClick(Object sender
            , EventArgs e)
        {
            Plugin.Settings.DefaultValues.ExportToCollectionXml = ExportToCollectionXmlCheckBox.Checked;

            CultureInfo uiLanguage = GetUiLanguage();

            Plugin.Settings.DefaultValues.UiLanguage = uiLanguage;

            Texts.Culture = uiLanguage;

            MessageBoxTexts.Culture = uiLanguage;

            DialogResult = DialogResult.OK;

            Close();
        }

        private CultureInfo GetUiLanguage()
        {
            KeyValuePair<Int32, CultureInfo> kvp = (KeyValuePair<Int32, CultureInfo>)(UiLanguageComboBox.SelectedItem);

            CultureInfo ci = kvp.Value;

            return (ci);
        }
    }
}