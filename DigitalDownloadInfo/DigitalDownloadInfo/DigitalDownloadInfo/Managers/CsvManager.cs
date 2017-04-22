using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DoenaSoft.DVDProfiler.DigitalDownloadInfo.Resources;
using DoenaSoft.DVDProfiler.DVDProfilerHelper;
using Invelos.DVDProfilerPlugin;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    internal sealed class CsvManager
    {
        private readonly Plugin Plugin;

        public CsvManager(Plugin plugin)
        {
            Plugin = plugin;
        }

        #region Export

        internal void Export(Boolean exportAll)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.DefaultExt = ".csv";
                sfd.Filter = "CSV (comma-separated values) files|*.csv";
                sfd.OverwritePrompt = true;
                sfd.RestoreDirectory = true;
                sfd.Title = Texts.SaveXmlFile;
                sfd.FileName = "DigitalDownloadInfos.csv";

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

                        CultureInfo currentCulture = Application.CurrentCulture;

                        String listSeparator = currentCulture.TextInfo.ListSeparator;

                        String dateFormat = currentCulture.DateTimeFormat.ShortDatePattern;

                        Object[] ids = (exportAll) ? ((Object[])(Plugin.Api.GetAllProfileIDs())) : ((Object[])(Plugin.Api.GetFlaggedProfileIDs()));

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

                        try
                        {
                            using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    #region Header

                                    WriteText(sw, Texts.Id, listSeparator);
                                    WriteText(sw, Texts.Title, listSeparator);
                                    WriteText(sw, Texts.Edition, listSeparator);
                                    WriteText(sw, Texts.SortTitle, listSeparator);
                                    WriteText(sw, Texts.Company, listSeparator);
                                    WriteText(sw, Texts.Code, listSeparator);

                                    sw.WriteLine();

                                    #endregion

                                    for (Int32 i = 0; i < ids.Length; i++)
                                    {
                                        #region Row

                                        String id = ids[i].ToString();

                                        IDVDInfo profile;
                                        Plugin.Api.DVDByProfileID(out profile, id, PluginConstants.DATASEC_AllSections, 0);

                                        DataManager dataManager = new DataManager(profile);

                                        WriteText(sw, profile.GetFormattedProfileID(), listSeparator);
                                        WriteText(sw, profile.GetTitle(), listSeparator);
                                        WriteText(sw, profile.GetEdition(), listSeparator);
                                        WriteText(sw, profile.GetSortTitle(), listSeparator);
                                        WriteText(sw, dataManager.GetCompanyWithFallback(), listSeparator);
                                        WriteText(sw, dataManager.GetCodeWithFallback(), listSeparator);

                                        sw.WriteLine();

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

                                        #endregion
                                    }
                                }
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

        private static void WriteText(StreamWriter sw
            , String text
            , String listSeparator)
        {
            sw.Write("\"");

            if (String.IsNullOrEmpty(text) == false)
            {
                text = text.Replace("\"", "\"\"");
            }

            sw.Write(text);
            sw.Write("\"");
            sw.Write(listSeparator);
        }

        #endregion
    }
}