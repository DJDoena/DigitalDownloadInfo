using System;
using System.Text;
using Invelos.DVDProfilerPlugin;

namespace DoenaSoft.DVDProfiler.DigitalDownloadInfo
{
    internal sealed class DataManager
    {
        private const String TextNotSet = null;

        private readonly IDVDInfo Profile;

        internal DataManager(IDVDInfo profile)
        {
            Profile = profile;
        }

        #region Company

        internal String GetCompanyWithFallback()
            => (GetTextWithFallback(GetCompany));

        internal Boolean GetCompany(out String company)
            => (GetText(Constants.Company, out company));

        internal void SetCompany(String company)
        {
            SetText(Constants.Company, company);
        }

        #endregion

        #region Code
        internal String GetCodeWithFallback()
            => (GetTextWithFallback(GetCode));

        internal Boolean GetCode(out String code)
            => (GetText(Constants.Code, out code));

        internal void SetCode(String code)
        {
            SetText(Constants.Code, code);
        }

        #endregion

        #region Text

        internal delegate Boolean GetTextDelegate(out String text);

        internal Boolean GetText(String fieldName, out String text)
        {
            String decoded = TextNotSet;

            String encoded = Profile.GetCustomString(Constants.FieldDomain, fieldName, Constants.ReadKey, TextNotSet);

            if (encoded != TextNotSet)
            {
                decoded = Encoding.Unicode.GetString(Convert.FromBase64String(encoded));
            }

            text = decoded;

            return (text != TextNotSet);
        }

        private String GetTextWithFallback(GetTextDelegate getText)
        {
            String text;
            if (getText(out text) == false)
            {
                text = String.Empty;
            }

            return (text);
        }

        private void SetText(String fieldName
            , String decoded)
        {
            String encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(decoded));

            Profile.SetCustomString(Constants.FieldDomain, fieldName, InternalConstants.WriteKey, encoded);
        }

        #endregion 
    }
}