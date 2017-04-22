using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    [ComVisible(false)]
    public sealed class DefaultValues
    {
        #region Misc

        public Int32 UiLcid
        {
            get
            {
                return (UiLanguage.LCID);
            }
            set
            {
                UiLanguage = CultureInfo.GetCultureInfo(value);
            }
        }

        [XmlIgnore]
        internal CultureInfo UiLanguage;

        public Boolean ExportToCollectionXml = false;

        #endregion

        public DefaultValues()
        {
            UiLanguage = GetUILanguage();
        }

        internal static CultureInfo GetUILanguage()
            => ((Thread.CurrentThread.CurrentUICulture.Name.StartsWith("de")) ? (CultureInfo.GetCultureInfo("de")) : (CultureInfo.GetCultureInfo("en")));
    }
}