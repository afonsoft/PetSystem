using Afonsoft.Petz.Controller;
using System;
using System.Web;

namespace Afonsoft.Petz.Store
{
    public partial class Settings : PageBaseSecurity
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var store = (Store)(Page.Master);
            store?.ChangeActiveMenu("settings");
            if (UsuarioLogado == null)
                return;

            if (IsCompanyAdmin == false)
                Response.Redirect("~/Store/index.aspx?m=" + HttpUtility.UrlEncode("Você não tem permissão para acessar as configurações."));

            GetCompany(CompanyId);
        }
        private void GetCompany(int id)
        {
            CompaniesController controller = new CompaniesController();
            var company = controller.GetCompany(id);
            if (company != null)
            {
                ImgPetShop.ImageUrl = ByteArrayToBase64Image(controller.GetCompanyPicture(id));
                ltName.Text = company.ToString();
                ltAddress.Text = ((company.Address != null && company.Address.Length > 0) ? company.Address[0].ToString() : "");
                ltPhone.Text = ((company.Phones != null && company.Phones.Length > 0) ? company.Phones[0].ToString() : "");
            }
        }
    }
}