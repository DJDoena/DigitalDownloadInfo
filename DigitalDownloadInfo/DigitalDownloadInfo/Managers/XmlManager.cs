using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DigitalDownloadInfo.Resources;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using DoenaSoft.ToolBox.Generics;
using Invelos.DVDProfilerPlugin;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    internal sealed class XmlManager
    {
        private readonly Plugin Plugin;

        public XmlManager(Plugin plugin)
        {
            Plugin = plugin;
        }

        #region Export

        internal void Export(Boolean exportAll)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.DefaultExt = ".xml";
                sfd.Filter = "XML files|*.xml";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.Title = Texts.SaveXmlFile;
                sfd.FileName = "DigitalDownloadInfo.xml";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    using (ProgressWindow progressWindow = new ProgressWindow())
                    {
                        #region Progress

                        progressWindow.ProgressBar.Minimum = 0;
                        progressWindow.ProgressBar.Step = 1;
                        progressWindow.CanClose = false;

                        #endregion

                        Object[] ids = this.GetProfileIds(exportAll);

                        DigitalDownloadInfoList ddis = new DigitalDownloadInfoList();

                        ddis.Profiles = new Profile[ids.Length];

                        #region Progress

                        progressWindow.ProgressBar.Maximum = ids.Length;

                        progressWindow.Show();

                        if (TaskbarManager.IsPlatformSupported)
                        {
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                            TaskbarManager.Instance.SetProgressValue(0, progressWindow.ProgressBar.Maximum);
                        }

                        Int32 onePercent = progressWindow.ProgressBar.Maximum / 100;

                        if ((progressWindow.ProgressBar.Maximum % 100) != 0)
                        {
                            onePercent++;
                        }

                        #endregion

                        for (Int32 i = 0; i < ids.Length; i++)
                        {
                            String id = ids[i].ToString();

                            IDVDInfo profile;
                            Plugin.Api.DVDByProfileID(out profile, id, PluginConstants.DATASEC_AllSections, 0);

                            ddis.Profiles[i] = this.GetXmlProfile(profile);

                            #region Progress

                            progressWindow.ProgressBar.PerformStep();

                            if (TaskbarManager.IsPlatformSupported)
                            {
                                TaskbarManager.Instance.SetProgressValue(progressWindow.ProgressBar.Value, progressWindow.ProgressBar.Maximum);
                            }

                            if ((progressWindow.ProgressBar.Value % onePercent) == 0)
                            {
                                Application.DoEvents();
                            }

                            #endregion
                        }

                        try
                        {
                            XmlSerializer<DigitalDownloadInfoList>.Serialize(sfd.FileName, ddis);

                            #region Progress

                            Application.DoEvents();

                            if (TaskbarManager.IsPlatformSupported)
                            {
                                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                            }

                            progressWindow.CanClose = true;
                            progressWindow.Close();

                            #endregion

                            MessageBox.Show(String.Format(MessageBoxTexts.DoneWithNumber, ids.Length, MessageBoxTexts.Exported)
                                , MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeWritten, sfd.FileName, ex.Message)
                                , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            #region Progress

                            if (progressWindow.Visible)
                            {
                                if (TaskbarManager.IsPlatformSupported)
                                {
                                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                                }

                                progressWindow.CanClose = true;
                                progressWindow.Close();
                            }

                            #endregion
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private Profile GetXmlProfile(IDVDInfo profile)
        {
            DataManager dataManager = new DataManager(profile);

            Profile xmlProfile = new Profile();

            xmlProfile.Id = profile.GetProfileID();
            xmlProfile.Title = profile.GetTitle();

            DigitalDownloadInfo ddi = new DigitalDownloadInfo();

            xmlProfile.DigitalDownloadInfo = ddi;

            ddi.Company = this.GetText(dataManager.GetCompany);
            ddi.Code = this.GetText(dataManager.GetCode);

            return (xmlProfile);
        }

        private Text GetText(DataManager.GetTextDelegate getText)
        {
            Text text = null;

            String title;
            if (getText(out title))
            {
                text = new Text();

                text.Value = title;
            }

            return (text);
        }

        #endregion

        #region Import

        internal void Import(Boolean importAll)
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
                    Cursor.Current = Cursors.WaitCursor;

                    DigitalDownloadInfoList ddis = null;

                    try
                    {
                        ddis = XmlSerializer<DigitalDownloadInfoList>.Deserialize(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format(MessageBoxTexts.FileCantBeRead, ofd.FileName, ex.Message)
                           , MessageBoxTexts.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (ddis != null)
                    {
                        Int32 count = 0;

                        if (ddis.Profiles?.Length > 0)
                        {
                            using (ProgressWindow progressWindow = new ProgressWindow())
                            {
                                #region Progress

                                progressWindow.ProgressBar.Minimum = 0;
                                progressWindow.ProgressBar.Step = 1;
                                progressWindow.CanClose = false;

                                #endregion

                                Object[] ids = this.GetProfileIds(importAll);

                                Dictionary<String, Boolean> profileIds = new Dictionary<String, Boolean>(ids.Length);

                                #region Progress

                                progressWindow.ProgressBar.Maximum = ids.Length;
                                progressWindow.Show();

                                if (TaskbarManager.IsPlatformSupported)
                                {
                                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                                    TaskbarManager.Instance.SetProgressValue(0, progressWindow.ProgressBar.Maximum);
                                }

                                Int32 onePercent = progressWindow.ProgressBar.Maximum / 100;

                                if ((progressWindow.ProgressBar.Maximum % 100) != 0)
                                {
                                    onePercent++;
                                }

                                #endregion

                                for (Int32 i = 0; i < ids.Length; i++)
                                {
                                    profileIds.Add(ids[i].ToString(), true);
                                }

                                foreach (Profile xmlProfile in ddis.Profiles)
                                {
                                    if ((xmlProfile?.DigitalDownloadInfo != null) && (profileIds.ContainsKey(xmlProfile.Id)))
                                    {
                                        IDVDInfo profile = Plugin.Api.CreateDVD();

                                        profile.SetProfileID(xmlProfile.Id);

                                        DataManager dataManager = new DataManager(profile);

                                        DigitalDownloadInfo ddi = xmlProfile.DigitalDownloadInfo;

                                        this.SetTitle(ddi.Company, dataManager.SetCompany);
                                        this.SetTitle(ddi.Code, dataManager.SetCode);

                                        count++;
                                    }

                                    #region Progress

                                    progressWindow.ProgressBar.PerformStep();

                                    if (TaskbarManager.IsPlatformSupported)
                                    {
                                        TaskbarManager.Instance.SetProgressValue(progressWindow.ProgressBar.Value, progressWindow.ProgressBar.Maximum);
                                    }

                                    if ((progressWindow.ProgressBar.Value % onePercent) == 0)
                                    {
                                        Application.DoEvents();
                                    }

                                    #endregion
                                }

                                #region Progress

                                Application.DoEvents();

                                if (TaskbarManager.IsPlatformSupported)
                                {
                                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                                }

                                progressWindow.CanClose = true;
                                progressWindow.Close();

                                #endregion
                            }
                        }

                        MessageBox.Show(String.Format(MessageBoxTexts.DoneWithNumber, count, MessageBoxTexts.Imported)
                                , MessageBoxTexts.InformationHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void SetTitle(Text text
            , Action<String> setText)
        {
            if (text?.Value != null)
            {
                setText(text.Value);
            }
            else
            {
                setText(String.Empty);
            }
        }

        #endregion

        private Object[] GetProfileIds(Boolean allIds)
            => ((allIds) ? ((Object[])(Plugin.Api.GetAllProfileIDs())) : ((Object[])(Plugin.Api.GetFlaggedProfileIDs())));
    }
}