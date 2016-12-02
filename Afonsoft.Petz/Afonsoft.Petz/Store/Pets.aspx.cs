using System;

namespace Afonsoft.Petz.Store
{
    public partial class Pets : PageBaseSecurity
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var store = (Store)(Page.Master);
            store?.ChangeActiveMenu("pets");
        }
    }
}