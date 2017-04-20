using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Script.Services;
using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using System.Threading;

namespace Afonsoft.Petz.Store
{
    public partial class index : PageBaseSecurity
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCalender(int id, int addressId)
        {
            if (id == Convert.ToInt32(HttpContext.Current.Session["CompanyID"]))
            {
                SchedulingController controller = new SchedulingController();
                EventObject[] arrayOfScheduling = controller.GetCalenderEvent(id, addressId);
                return JsonConvert.SerializeObject(arrayOfScheduling);
            }
            return "";
        }
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string DeleteEvent(int companyId, int addressId, int eventId)
        {
            try
            {
                if (companyId >= 0 && companyId == Convert.ToInt32(HttpContext.Current.Session["CompanyID"]))
                {
                    Thread.Sleep(1000);
                    SchedulingController controller = new SchedulingController();
                    EventObject[] arrayOfScheduling = controller.GetCalenderEvent(companyId, addressId);

                    bool isEventCompany = arrayOfScheduling.Count(x => Convert.ToInt32(x.id) == eventId) > 0;
                    if (isEventCompany)
                    {
                        if (controller.DeleteSheduling(eventId))
                        {
                            return JsonConvert.SerializeObject(new { isOk = true, mensagem = "Evento ("+ arrayOfScheduling.FirstOrDefault(x => Convert.ToInt32(x.id) == eventId) + ") excluido com sucesso!" });
                        }
                        else
                            return JsonConvert.SerializeObject(new { isOk = false, mensagem = "Não é possível excluir esse evento!" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { isOk = false, mensagem = "Não é possível excluir esse evento!" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { isOk = false, mensagem = "Não é possível excluir esse evento!" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { isOk = false, mensagem = ex.Message });
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            var store = (Store)(Page.Master);
            store.ChangeActiveMenu("index");

            if (Request.QueryString["m"] != null)
                Alert(Request.QueryString["m"]);

            if (UsuarioLogado == null)
                return;
        }
    }
}