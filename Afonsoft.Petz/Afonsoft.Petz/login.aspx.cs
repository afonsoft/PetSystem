using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Library;
using System;

namespace Afonsoft.Petz
{
    public partial class login : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["m"] != null)
                Alert(Request.QueryString["m"]);


            if (!IsPostBack)
            {
                SessionString = "";
                SecurityToken = "";
                ClientOrUserId = 0;
                Session["UsuarioLogado"] = null;
#if DEBUG
                //for test
                loginname.Text = "admin";
                password.Text = "Senha#12";
                //btnEntrar_Click(sender, e);

                Response.Redirect("iCalHandler.ashx?EventId=1&CompanyId=1");
#endif
            }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityController controller = new SecurityController();
                SessionString = controller.RandomString(8);
                string ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].Replace(".", "").Replace(":", "");
                if (String.IsNullOrEmpty(ipAddress))
                    if (Context.Request.UserHostAddress != null)
                        ipAddress = Context.Request.UserHostAddress.Replace(".", "").Replace(":", "");

                string radioButtonStoreOrClinet = rdClinet.Checked ? "C" : RdStore.Checked ? "L" : "";

                if (radioButtonStoreOrClinet == "C")
                {
                    ClientOrUserId = controller.AuthenticateClient(loginname.Text, password.Text);
                    if (ClientOrUserId > 0)
                    {
                        SecurityToken = Cryptographic.Encryptor(SessionString + "|" + ClientOrUserId + "|" + DateTime.Now.AddMinutes(20).ToString("yyyyMMddHHmmss") + "|" + ipAddress + "|C");
                        Response.Redirect("~/Client/index.aspx?Token=" + SecurityToken);
                    }
                    else
                    {
                        throw new Exception("Usuário ou Senha inválido!");
                    }
                }
                else if (radioButtonStoreOrClinet == "L")
                {
                    ClientOrUserId = controller.AuthenticateUser(loginname.Text, password.Text);
                    if (ClientOrUserId > 0)
                    {
                        SecurityToken = Cryptographic.Encryptor(SessionString + "|" + ClientOrUserId + "|" + DateTime.Now.AddMinutes(20).ToString("yyyyMMddHHmmss") + "|" + ipAddress + "|L");
                        Response.Redirect("~/Store/index.aspx?Token=" + SecurityToken);
                    }
                    else
                    {
                        throw new Exception("Usuário ou Senha inválido!");
                    }
                }
                else
                {
                    Alert("Selecione uma opção!");
                }
            }
            catch (Exception ex)
            {
                Alert(ex);
            }
        }
    }
}