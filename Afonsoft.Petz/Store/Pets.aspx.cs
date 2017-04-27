using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Store
{
    public partial class Pets : PageBaseSecurity
    {

        public PetsEntity[] ListPetsEntities
        {
            get { return Session["Pets"] as PetsEntity[]; }
            set { Session["Pets"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var store = (Store)(Page.Master);
            store.ChangeActiveMenu("pets");
            if (UsuarioLogado == null)
                return;

            if (!IsPostBack)
            {
                ListPetsEntities = null;
                PopularPets();

                if (Request.QueryString["ID"] != null)
                {
                    int petId = Convert.ToInt32(Request.QueryString["ID"]);
                    var infoPet = ListPetsEntities.FirstOrDefault(x => x.Id == petId);
                    if (infoPet != null)
                    {

                    }
                }
            }
        }

        private void PopularPets()
        {
            if (CompanyId > 0)
            {
                CompaniesController controller = new CompaniesController();
                ListPetsEntities = controller.GetCompanyPets(CompanyId);
                rptPetEntities.DataSource = ListPetsEntities;
                rptPetEntities.DataBind();
            }
        }
    }
}