using System;
using System.Web.UI;

namespace Afonsoft.Petz
{
    public partial class Petz : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ScriptManagerMain_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            ScriptManagerMain.AsyncPostBackErrorMessage = e.Exception.Message;
        }
    }
}