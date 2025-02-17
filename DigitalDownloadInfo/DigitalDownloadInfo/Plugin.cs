using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DigitalDownloadInfo.Resources;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.ToolBox.Generics;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    [Guid(ClassGuid.ClassID), ComVisible(true)]
    public class Plugin : IDVDProfilerPlugin, IDVDProfilerPluginInfo, IDVDProfilerDataAwarePlugin
    {
        private readonly String SettingsFile;

        private readonly String ErrorFile;

        private readonly String ApplicationPath;

        internal readonly Dictionary<String, UInt16> Companies;

        internal IDVDProfilerAPI Api { get; private set; }

        internal Settings Settings { get; private set; }

        private Boolean DatabaseRestoreRunning = false;

        internal Boolean IsRemoteAccess { get; private set; }

        #region MenuTokens

        private String DvdMenuToken = "";

        private const Int32 DvdMenuId = 1;

        private String CollectionExportMenuToken = "";

        private const Int32 CollectionExportMenuId = 21;

        private String CollectionImportMenuToken = "";

        private const Int32 CollectionImportMenuId = 22;

        private String CollectionExportToCsvMenuToken = "";

        private const Int32 CollectionExportToCsvMenuId = 23;

        private String CollectionFlaggedExportMenuToken = "";

        private const Int32 CollectionFlaggedExportMenuId = 31;

        private String CollectionFlaggedImportMenuToken = "";

        private const Int32 CollectionFlaggedImportMenuId = 32;

        private String CollectionFlaggedExportToCsvMenuToken = "";

        private const Int32 CollectionFlaggedExportToCsvMenuId = 33;

        private String ToolsOptionsMenuToken = "";

        private const Int32 ToolsOptionsMenuId = 41;

        private String ToolsExportOptionsMenuToken = "";

        private const Int32 ToolsExportOptionsMenuId = 42;

        private String ToolsImportOptionsMenuToken = "";

        private const Int32 ToolsImportOptionsMenuId = 43;

        #endregion

        private readonly Dictionary<String, String> FilterTokens;

        static Plugin()
        {
            DVDProfilerHelperAssemblyLoader.Load();
        }

        public Plugin()
        {
            Companies = new Dictionary<String, UInt16>();

            FilterTokens = new Dictionary<String, String>();

            ApplicationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Doena Soft\DigitalDownloadInfo\";

            SettingsFile = ApplicationPath + "DigitalDownloadInfo.xml";

            ErrorFile = Environment.GetEnvironmentVariable("TEMP") + @"\DigitalDownloadInfoCrash.xml";
        }

        #region I.. Members

        #region IDVDProfilerPlugin Members

        public void Load(IDVDProfilerAPI api)
        {
            //System.Diagnostics.Debugger.Launch();

            this.Api = api;

            if (Directory.Exists(ApplicationPath) == false)
            {
                Directory.CreateDirectory(ApplicationPath);
            }

            if (File.Exists(SettingsFile))
            {
                try
                {
                    this.Settings = XmlSerializer<Settings>.Deserialize(SettingsFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeRead, SettingsFile, ex.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.EnsureSettingsAndSetUILanguage();

            this.SetIsRemoteAccess();

            this.RegisterForEvents();

            this.RegisterMenuItems();

            this.RegisterCustomFields();
        }

        public void Unload()
        {
            this.Api.UnregisterMenuItem(DvdMenuToken);

            this.Api.UnregisterMenuItem(CollectionExportMenuToken);
            this.Api.UnregisterMenuItem(CollectionImportMenuToken);
            this.Api.UnregisterMenuItem(CollectionExportToCsvMenuToken);

            this.Api.UnregisterMenuItem(CollectionFlaggedExportMenuToken);
            this.Api.UnregisterMenuItem(CollectionFlaggedImportMenuToken);
            this.Api.UnregisterMenuItem(CollectionFlaggedExportToCsvMenuToken);

            this.Api.UnregisterMenuItem(ToolsOptionsMenuToken);
            this.Api.UnregisterMenuItem(ToolsExportOptionsMenuToken);
            this.Api.UnregisterMenuItem(ToolsImportOptionsMenuToken);

            try
            {
                XmlSerializer<Settings>.Serialize(SettingsFile, this.Settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, SettingsFile, ex.Message)
                    , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Api = null;
        }

        public void HandleEvent(Int32 EventType
            , Object EventData)
        {
            try
            {
                switch (EventType)
                {
                    case (PluginConstants.EVENTID_CustomMenuClick):
                        {
                            this.HandleMenuClick((Int32)EventData);

                            break;
                        }
                    case (PluginConstants.EVENTID_RestoreStarting):
                        {
                            DatabaseRestoreRunning = true;

                            this.RegisterCustomFields(false);

                            break;
                        }
                    case (PluginConstants.EVENTID_DatabaseOpened):
                    case (PluginConstants.EVENTID_RestoreFinished):
                    case (PluginConstants.EVENTID_RestoreCancelled):
                        {
                            DatabaseRestoreRunning = false;

                            this.RegisterCustomFields();

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.CriticalError, ex.Message, ErrorFile), MessageBoxTexts.CriticalErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    if (File.Exists(ErrorFile))
                    {
                        File.Delete(ErrorFile);
                    }

                    this.LogException(ex);
                }
                catch (Exception inEx)
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, ErrorFile, inEx.Message), MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region IDVDProfilerPluginInfo Members

        public String GetName()
            => (Texts.PluginName);

        public String GetDescription()
            => (Texts.PluginDescription);

        public String GetAuthorName()
            => ("Doena Soft.");

        public String GetAuthorWebsite()
            => (Texts.PluginUrl);

        public Int32 GetPluginAPIVersion()
            => (PluginConstants.API_VERSION);

        public Int32 GetVersionMajor()
        {
            Version version = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version;

            Int32 major = version.Major;

            return (major);
        }

        public Int32 GetVersionMinor()
        {
            Version version = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version;

            Int32 minor = version.Minor * 100 + version.Build * 10 + version.Revision;

            return (minor);
        }

        #endregion

        #region IDVDProfilerDataAwarePlugin

        public Boolean ExportsCustomDataXML()
            => ((this.Settings.DefaultValues.ExportToCollectionXml) && (DatabaseRestoreRunning == false));

        public String GetCustomDataXMLForDVD(IDVDInfo SourceDVD)
        {
            if (this.Settings.DefaultValues.ExportToCollectionXml == false)
            {
                return (String.Empty);
            }
            else if (DatabaseRestoreRunning)
            {
                return (String.Empty);
            }

            DataManager dataManager = new DataManager(SourceDVD);

            String company;
            Boolean hasCompany = dataManager.GetCompany(out company);

            String code;
            Boolean hasCode = dataManager.GetCode(out code);

            String xml = String.Empty;

            if (hasCompany || hasCode)
            {
                StringBuilder sb = new StringBuilder("<DigitalDownloadInfo>");

                if (hasCompany)
                {
                    AddTag(sb, Constants.Company, company);
                }
                if (hasCode)
                {
                    AddTag(sb, Constants.Code, code);
                }

                sb.Append("</DigitalDownloadInfo>");

                xml = sb.ToString();
            }

            return (xml);
        }

        public String GetHTMLForDPVarsFunctionSection()
            => (String.Empty);

        public String GetHTMLForDPVarsDataSection(IDVDInfo SourceDVD
            , IDVDInfo CompareDVD)
            => (String.Empty);

        public String GetHTMLForTag(String TagName
            , String FullTag
            , IDVDInfo SourceDVD
            , IDVDInfo CompareDVD
            , out Boolean Handled)
        {
            if (String.IsNullOrEmpty(TagName))
            {
                Handled = false;

                return (null);
            }
            else if (TagName.StartsWith(Constants.HtmlPrefix + ".") == false)
            {
                Handled = false;

                return (null);
            }

            DataManager dataManager = new DataManager(SourceDVD);

            Handled = true;

            String text;
            switch (TagName)
            {
                case (Constants.HtmlPrefix + "." + Constants.Company):
                    {
                        text = HtmlEncode(dataManager.GetCompanyWithFallback());

                        break;
                    }
                case (Constants.HtmlPrefix + "." + Constants.Code):
                    {
                        text = HtmlEncode(dataManager.GetCodeWithFallback());

                        break;
                    }
                case (Constants.HtmlPrefix + "." + Constants.Company + Constants.LabelSuffix):
                    {
                        text = HtmlEncode(Texts.Company);

                        break;
                    }
                case (Constants.HtmlPrefix + "." + Constants.Code + Constants.LabelSuffix):
                    {
                        text = HtmlEncode(Texts.Code);

                        break;
                    }
                default:
                    {
                        Handled = false;

                        text = null;

                        break;
                    }
            }

            return (text);
        }

        public Object GetCustomHTMLTagNames()
            => (new[] { Constants.HtmlPrefix + "." + Constants.Company
                    , Constants.HtmlPrefix + "." + Constants.Code
                    , Constants.HtmlPrefix + "." + Constants.Company + Constants.LabelSuffix
                    , Constants.HtmlPrefix + "." + Constants.Code + Constants.LabelSuffix });

        public Object GetCustomHTMLParamsForTag(String TagName)
            => (null);

        public Boolean FilterFieldMatch(String FieldFilterToken
            , Int32 ComparisonTypeIndex
            , Object ComparisonValue
            , IDVDInfo TestDVD)
        {
            if (ComparisonValue == null)
            {
                return (false);
            }

            String fieldName;
            if (FilterTokens.TryGetValue(FieldFilterToken, out fieldName))
            {
                String text;
                if ((new DataManager(TestDVD)).GetText(fieldName, out text))
                {
                    String compare = ComparisonValue.ToString();

                    Boolean contains = (ComparisonTypeIndex == 0)
                        ? (text.IndexOf(compare, StringComparison.OrdinalIgnoreCase) == 0)
                        : (text.IndexOf(compare, StringComparison.OrdinalIgnoreCase) >= 0);

                    return (contains);
                }
            }

            return (false);
        }

        #endregion

        #endregion

        #region RegisterCustomFields

        internal void RegisterCustomFields(Boolean rebuildFilters = true)
        {
            try
            {
                this.UnregisterCustomFilterField(rebuildFilters);

                #region Schema

                using (Stream stream = typeof(DigitalDownloadInfo).Assembly.GetManifestResourceStream("DoenaSoft.DVDProfiler.DigitalDownloadInfo.DigitalDownloadInfo.xsd"))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        String xsd = sr.ReadToEnd();

                        this.Api.SetGlobalSetting(Constants.FieldDomain, "DigitalDownloadInfoSchema", xsd, Constants.ReadKey, InternalConstants.WriteKey);
                    }
                }

                #endregion

                this.RegisterCustomField(Constants.Company, Texts.Company, rebuildFilters);
                this.RegisterCustomField(Constants.Code, null, rebuildFilters);

                this.PrepareCompanies();
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.CriticalError, ex.Message, ErrorFile)
                        , MessageBoxTexts.CriticalErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    if (File.Exists(ErrorFile))
                    {
                        File.Delete(ErrorFile);
                    }

                    this.LogException(ex);
                }
                catch (Exception inEx)
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, ErrorFile, inEx.Message)
                        , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UnregisterCustomFilterField(Boolean rebuildFilters)
        {
            //System.Diagnostics.Debugger.Launch();

            if (rebuildFilters)
            {
                foreach (String token in FilterTokens.Keys)
                {
                    try
                    {
                        this.Api.RemoveCustomFilterField(token);
                    }
                    catch (COMException)
                    { }
                }

                FilterTokens.Clear();
            }
        }

        private void RegisterCustomField(String fieldName
            , String displayName
            , Boolean rebuildFilters)
        {
            this.Api.CreateCustomDVDField(Constants.FieldDomain, fieldName, PluginConstants.FIELD_TYPE_STRING, Constants.ReadKey, InternalConstants.WriteKey);

            this.Api.SetCustomDVDFieldStorage(Constants.FieldDomain, fieldName, InternalConstants.WriteKey, true, false);

            if (displayName != null)
            {
                //RegisterCustomFilterField(fieldName, displayName, rebuildFilters);
            }
        }

        private void RegisterCustomFilterField(String fieldName
            , String displayName
            , Boolean rebuildFilters)
        {
            if (rebuildFilters)
            {
                //System.Diagnostics.Debugger.Launch();

                String token = this.Api.SetCustomFieldFilterableA(displayName, PluginConstants.FILTER_INPUT_TEXT, new[] { Texts.FilterStartsWith, Texts.FilterContains }, null);

                if (token != null)
                {
                    FilterTokens.Add(token, fieldName);
                }
                else
                {
                    System.Diagnostics.Debug.Fail("No valid token for search!");
                }
            }
        }

        #endregion

        private void SetIsRemoteAccess()
        {
            String name;
            Boolean isRemote;
            String localPath;
            this.Api.GetCurrentDatabaseInformation(out name, out isRemote, out localPath);

            //System.Diagnostics.Debugger.Launch();

            this.IsRemoteAccess = isRemote;
        }

        private void RegisterForEvents()
        {
            this.Api.RegisterForEvent(PluginConstants.EVENTID_DatabaseOpened);

            this.Api.RegisterForEvent(PluginConstants.EVENTID_RestoreStarting);
            this.Api.RegisterForEvent(PluginConstants.EVENTID_RestoreFinished);
            this.Api.RegisterForEvent(PluginConstants.EVENTID_RestoreCancelled);
        }

        private void RegisterMenuItems()
        {
            DvdMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, "DVD", Texts.DDI, DvdMenuId);

            CollectionExportMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Collection\" + Texts.DDI, Texts.ExportToXml, CollectionExportMenuId);

            if (this.IsRemoteAccess == false)
            {
                CollectionImportMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Collection\" + Texts.DDI, Texts.ImportFromXml, CollectionImportMenuId);
            }

            CollectionExportToCsvMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form
                , @"Collection\" + Texts.DDI, Texts.ExportToExcel, CollectionExportToCsvMenuId);

            CollectionFlaggedExportMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Collection\Flagged\" + Texts.DDI, Texts.ExportToXml, CollectionFlaggedExportMenuId);

            if (this.IsRemoteAccess == false)
            {
                CollectionFlaggedImportMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Collection\Flagged\" + Texts.DDI, Texts.ImportFromXml, CollectionFlaggedImportMenuId);
            }

            CollectionFlaggedExportToCsvMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form
                , @"Collection\Flagged\" + Texts.DDI, Texts.ExportToExcel, CollectionFlaggedExportToCsvMenuId);

            ToolsOptionsMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Tools\" + Texts.DDI, Texts.Options, ToolsOptionsMenuId);

            ToolsExportOptionsMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Tools\" + Texts.DDI, Texts.ExportOptions, ToolsExportOptionsMenuId);

            ToolsImportOptionsMenuToken = this.Api.RegisterMenuItem(PluginConstants.FORMID_Main, PluginConstants.MENUID_Form, @"Tools\" + Texts.DDI, Texts.ImportOptions, ToolsImportOptionsMenuId);
        }

        private static void AddTag(StringBuilder sb
            , String tagName
            , String field)
        {
            sb.Append("<");
            sb.Append(tagName);

            String base64;
            field = XmlConvertHelper.GetWindows1252Text(field, out base64);

            if (base64 != null)
            {
                sb.Append(" Base64Title=\"");
                sb.Append(base64);
                sb.Append("\"");
            }

            sb.Append(">");
            sb.Append(field);
            sb.Append("</");
            sb.Append(tagName);
            sb.Append(">");
        }

        private void EnsureSettingsAndSetUILanguage()
        {
            Texts.Culture = DefaultValues.GetUILanguage();

            CultureInfo uiLanguage = this.EnsureSettings();

            Texts.Culture = uiLanguage;

            MessageBoxTexts.Culture = uiLanguage;
        }

        private CultureInfo EnsureSettings()
        {
            if (this.Settings == null)
            {
                this.Settings = new Settings();
            }

            if (this.Settings.DefaultValues == null)
            {
                this.Settings.DefaultValues = new DefaultValues();
            }

            return (this.Settings.DefaultValues.UiLanguage);
        }

        private static String HtmlEncode(String decoded)
        {
            String encoded = String.Join("", decoded.ToCharArray().Select(c =>
                    {
                        Int32 number = c;

                        String newChar = (number > 127) ? ("&#" + number.ToString() + ";") : (HttpUtility.HtmlEncode(c.ToString()));

                        return (newChar);
                    }
                ).ToArray());

            return (encoded);
        }

        private void HandleMenuClick(Int32 MenuEventID)
        {
            try
            {
                switch (MenuEventID)
                {
                    case (DvdMenuId):
                        {
                            this.OpenEditor();

                            break;
                        }
                    case (CollectionExportMenuId):
                        {
                            XmlManager xmlManager = new XmlManager(this);

                            xmlManager.Export(true);

                            break;
                        }
                    case (CollectionImportMenuId):
                        {
                            XmlManager xmlManager = new XmlManager(this);

                            xmlManager.Import(true);

                            break;
                        }
                    case (CollectionExportToCsvMenuId):
                        {
                            CsvManager csvManager = new CsvManager(this);

                            csvManager.Export(true);

                            break;
                        }
                    case (CollectionFlaggedExportMenuId):
                        {
                            XmlManager xmlManager = new XmlManager(this);

                            xmlManager.Export(false);

                            break;
                        }
                    case (CollectionFlaggedImportMenuId):
                        {
                            XmlManager xmlManager = new XmlManager(this);

                            xmlManager.Import(false);

                            break;
                        }
                    case (CollectionFlaggedExportToCsvMenuId):
                        {
                            CsvManager csvManager = new CsvManager(this);

                            csvManager.Export(false);

                            break;
                        }
                    case (ToolsOptionsMenuId):
                        {
                            this.OpenSettings();

                            break;
                        }
                    case (ToolsExportOptionsMenuId):
                        {
                            this.ExportOptions();

                            break;
                        }
                    case (ToolsImportOptionsMenuId):
                        {
                            this.ImportOptions();

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.CriticalError, ex.Message, ErrorFile)
                        , MessageBoxTexts.CriticalErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    if (File.Exists(ErrorFile))
                    {
                        File.Delete(ErrorFile);
                    }

                    this.LogException(ex);
                }
                catch (Exception inEx)
                {
                    MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, ErrorFile, inEx.Message), MessageBoxTexts.ErrorHeader
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal void ImportOptions()
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
                    DefaultValues dv = null;

                    try
                    {
                        dv = XmlSerializer<DefaultValues>.Deserialize(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeRead, ofd.FileName, ex.Message)
                           , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (dv != null)
                    {
                        this.Settings.DefaultValues = dv;

                        Texts.Culture = dv.UiLanguage;

                        MessageBoxTexts.Culture = dv.UiLanguage;

                        MessageBox.Show(MessageBoxTexts.Done, MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        internal void ExportOptions()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.DefaultExt = ".xml";
                sfd.Filter = "XML files|*.xml";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.Title = Texts.SaveXmlFile;
                sfd.FileName = "DigitalDownloadInfoOptions.xml";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DefaultValues dv = this.Settings.DefaultValues;

                    try
                    {
                        XmlSerializer<DefaultValues>.Serialize(sfd.FileName, dv);

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

        private void OpenSettings()
        {
            using (SettingsForm form = new SettingsForm(this))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.RegisterCustomFields();
                }
            }
        }

        private void OpenEditor()
        {
            IDVDInfo profile = this.Api.GetDisplayedDVD();

            String profileId = profile.GetProfileID();

            if (String.IsNullOrEmpty(profileId) == false)
            {
                profile = this.Api.CreateDVD();

                profile.SetProfileID(profileId);

                using (MainForm form = new MainForm(this, profile))
                {
                    form.ShowDialog();
                }
            }
        }

        private void LogException(Exception ex)
        {
            ex = this.WrapCOMException(ex);

            ExceptionXml exceptionXml = new ExceptionXml(ex);

            XmlSerializer<ExceptionXml>.Serialize(ErrorFile, exceptionXml);
        }

        private Exception WrapCOMException(Exception ex)
        {
            Exception returnEx = ex;

            COMException comEx = ex as COMException;

            if (comEx != null)
            {
                String lastApiError = this.Api.GetLastError();

                EnhancedCOMException newEx = new EnhancedCOMException(lastApiError, comEx);

                returnEx = newEx;
            }

            return (returnEx);
        }

        private void PrepareCompanies()
        {
            //Debugger.Launch();

            Companies.Clear();

            Object[] ids = (Object[])(this.Api.GetAllProfileIDs());

            if ((ids != null) && (ids.Length > 0))
            {
                for (Int32 i = 0; i < ids.Length; i++)
                {
                    String id = ids[i].ToString();

                    IDVDInfo profile = this.Api.CreateDVD();

                    profile.SetProfileID(id);

                    DataManager dataManager = new DataManager(profile);

                    String company;
                    if (dataManager.GetCompany(out company))
                    {
                        UInt16 count;
                        if (Companies.TryGetValue(company, out count))
                        {
                            count++;
                            Companies[company] = count;
                        }
                        else
                        {
                            Companies.Add(company, 1);
                        }
                    }
                }
            }
        }

        #region Plugin Registering

        [DllImport("user32.dll")]
        public extern static int SetParent(int child, int parent);

        [ComImport, Guid("0002E005-0000-0000-C000-000000000046")]
        internal class StdComponentCategoriesMgr { }

        [ComRegisterFunction]
        public static void RegisterServer(Type t)
        {
            CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)(new StdComponentCategoriesMgr());

            Guid clsidThis = new Guid(ClassGuid.ClassID);

            Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.RegisterClassImplCategories(ref clsidThis, 1, new[] { catid });
        }

        [ComUnregisterFunction]
        public static void UnregisterServer(Type t)
        {
            CategoryRegistrar.ICatRegister cr = (CategoryRegistrar.ICatRegister)(new StdComponentCategoriesMgr());

            Guid clsidThis = new Guid(ClassGuid.ClassID);

            Guid catid = new Guid("833F4274-5632-41DB-8FC5-BF3041CEA3F1");

            cr.UnRegisterClassImplCategories(ref clsidThis, 1, new[] { catid });
        }

        #endregion
    }
}