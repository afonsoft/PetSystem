using Afonsoft.Petz.Library;
using System;
using System.Globalization;
using System.IO;
using System.Web.UI;


namespace Afonsoft.Petz
{

    public class PageBase : Page
    {
        #region Compressor ViewState
        private readonly ObjectStateFormatter _objectStateFormatter = new ObjectStateFormatter();
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            try
            {
                byte[] viewStateArray;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    _objectStateFormatter.Serialize(memoryStream, viewState);
                    viewStateArray = memoryStream.ToArray();
                }
                base.SavePageStateToPersistenceMedium(Convert.ToBase64String(Compressor.Compress(viewStateArray)));
            }
            catch
            {
                base.SavePageStateToPersistenceMedium(viewState);
            }
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            try
            {
                // ReSharper disable once RedundantAssignment
                String viewState = "";
                // ReSharper disable once PossibleNullReferenceException
                viewState = base.LoadPageStateFromPersistenceMedium().ToString() != "System.Web.UI.Pair" ? base.LoadPageStateFromPersistenceMedium().ToString() : ((System.Web.UI.Pair)base.LoadPageStateFromPersistenceMedium()).Second.ToString();

                byte[] bytes = Convert.FromBase64String(viewState);
                bytes = Compressor.Decompress(bytes);
                return _objectStateFormatter.Deserialize(Convert.ToBase64String(bytes));
            }
            catch
            {
                return base.LoadPageStateFromPersistenceMedium();
            }
        }
        #endregion

       private  readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");

        /// <summary>
        /// Recupere o objeto CultureInfo do Brasil, para usar no Convert.ToDateTime
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public CultureInfo cultureInfo => _cultureInfo;

        public int ClientOrUserId
        {
            get
            {
                if (Session["ClientOrUserID"] == null)
                    return 0;
                else
                    return (int)Session["ClientOrUserID"];
            }
            set { Session["ClientOrUserID"] = value; }
        }
        public string SessionString
        {
            get
            {
                if (Session["SessionString"] == null)
                    return "";
                else
                    return (string)Session["SessionString"];
            }
            set { Session["SessionString"] = value; }
        }

        public string SecurityToken
        {
            set { Session["SecurityToken"] = value; }
            get
            {
                if (Session["SecurityToken"] == null)
                    return "";
                else
                    return (string)Session["SecurityToken"];
            }
        }

        public string ByteArrayToBase64Image(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return "/Images/noPic.jpg";

            if (byteArrayIn.Length <= 0)
                return "/Images/noPic.jpg";

            string base64String = Convert.ToBase64String(byteArrayIn, 0, byteArrayIn.Length);
            return "data:image/jpg;base64," + base64String;
        }
        public void Alert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ModalAlert", "ModalAlert('" + FixString(msg) + "');", true);
        }

        public void Alert(Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ModalAlert", "ModalAlert('" + FixString(ex.Message) + "');", true);
        }
        public void Alert(string msg, Exception ex)
        {
            string s = msg + " - " + ex.Message;
            ScriptManager.RegisterStartupScript(this, GetType(), "ModalAlert", "ModalAlert('" + FixString(s) + "');", true);
        }

        public void NotifySuccess(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifySucess", "NotifySuccess('" + FixString(msg) + "', null);", true);
        }
        public void NotifySuccess(string msg, string url)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifySucess", "NotifySuccess('" + FixString(msg) + "', '" + url + "');", true);
        }
        public void NotifyError(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifyError", "NotifyError('" + FixString(msg) + "', null);", true);
        }
        public void NotifyError(string msg, string url)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifyError", "NotifyError('" + FixString(msg) + "', '" + url + "');", true);
        }

        public void NotifyWarning(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifyWarning", "NotifyWarning('" + FixString(msg) + "', null);", true);
        }
        public void NotifyWarning(string msg, string url)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifyWarning", "NotifyWarning('" + FixString(msg) + "', '" + url + "');", true);
        }

        public void NotifyInfo(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifyInfo", "NotifyInfo('" + FixString(msg) + "', null);", true);
        }
        public void NotifyInfo(string msg, string url)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "NotifyInfo", "NotifyInfo('" + FixString(msg) + "', '" + url + "');", true);
        }

        /// <summary>
        /// Remove breakline
        /// </summary>
        public string FixString(string txt)
        {
            return
                txt.Replace(Environment.NewLine, "<br/>")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("'", "`")
                    .RemoveSpecialCharacters();
        }
    }
}