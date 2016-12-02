using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afonsoft.Petz
{
    public partial class Petz : System.Web.UI.MasterPage
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