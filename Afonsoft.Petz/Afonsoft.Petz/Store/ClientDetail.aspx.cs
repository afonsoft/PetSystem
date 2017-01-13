using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;

namespace Afonsoft.Petz.Store
{
    public partial class ClientDetail : PageBaseSecurity
    {
        public string ClientId { get; set; }
        public int ClientRating { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                ClientId = Request.QueryString["ID"];
                ClientsController controller = new ClientsController();
                var client = controller.GetClient(Convert.ToInt32(ClientId));
                if (client != null)
                {
                    ClientRating = Convert.ToInt16(client.Rating);
                    lNomeClient.Text = client.Name;
                    lDocumentoClient.Text = client.Document;
                    lSexoClient.Text = client.Sex == EnumSex.Male ? "Masculino" : "Feminino";
                    if (client.Phones != null && client.Phones.Length > 0)
                    {
                        string[] phones = client.Phones.Select(x => x.Phone).ToArray();
                        lTelefoneClient.Text = string.Join(" | ", phones);
                    }
                    else
                    {
                        lTelefoneClient.Text = "Nenhum telefone";
                    }
                    string json = "[" + string.Join(",", client.Address.Select(x => x.Json()).ToArray()) + "]";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SearchFilter", "carregarPontos(" + json + ");", true);
                }
            }
        }
    }
}