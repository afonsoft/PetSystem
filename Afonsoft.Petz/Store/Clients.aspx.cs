using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Store
{
    public partial class ClientsPage : PageBaseSecurity
    {

        public ClientEntity[] ListClientEntities
        {
            get { return Session["Clients"] as ClientEntity[]; }
            set { Session["Clients"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var store = (Store) (Page.Master);
            store.ChangeActiveMenu("clients");

            if (UsuarioLogado == null)
                return;

            if (!IsPostBack)
            {
                ListClientEntities = null;
                PopularClientes();

                if (Request.QueryString["ID"] != null)
                {
                    int clientId = Convert.ToInt32(Request.QueryString["ID"]);
                    var infoClient = ListClientEntities.FirstOrDefault(x => x.Id == clientId);
                    if (infoClient != null)
                    {
                        ModalAjax("Cliente - " + infoClient.Name, "/Store/ClientDetail.aspx?ID=" + clientId);
                    }
                }
            }
        }

        private void PopularClientes()
        {
            if (CompanyId > 0)
            {
                CompaniesController controller = new CompaniesController();
                ListClientEntities = controller.GetCompanyClient(CompanyId);
                rptClientEntities.DataSource = ListClientEntities;
                rptClientEntities.DataBind();
            }
        }
    }
}