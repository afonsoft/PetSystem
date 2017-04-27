using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Library;
using Afonsoft.Petz.Model;
using System;
using System.Globalization;
using System.Web.UI;

namespace Afonsoft.Petz.Store
{
    public class PageBaseSecurity : PageBase
    {
        public UserEntity UsuarioLogado
        {
            get
            {
                return (UserEntity) Session["UsuarioLogado"];
            }
            set { Session["UsuarioLogado"] = value; }
        }

        public int CompanyId
        {
            get
            {
                if (Session["CompanyID"] == null)
                {
                    Session["CompanyID"] = 0;
                    if (UsuarioLogado != null)
                    {
                        var itens = new CompaniesController().GetCompaniesUser(UsuarioLogado.Id);
                        if (itens.Length >= 1)
                        {
                            Session["CompanyID"] = Convert.ToInt32(itens[0].Value);
                        }
                    }
                }

                return (int)Session["CompanyID"];
            }
            set { Session["CompanyID"] = value; }
        }

        public int AddressId
        {
            get
            {
                if (Session["AddressID"] == null)
                {
                    Session["AddressID"] = 0;
                    if (UsuarioLogado != null)
                    {
                        CompaniesController controller = new CompaniesController();
                        var itens = controller.GetCompaniesUser(UsuarioLogado.Id);
                        if (itens.Length >= 1)
                        {
                            var address = controller.GetCompanyAddress(Convert.ToInt32(itens[0].Value));
                            if (address.Length >= 1)
                                Session["AddressID"] = address[0].Id;
                        }
                    }
                }

                return (int)Session["AddressID"];
            }
            set { Session["AddressID"] = value; }
        }

        public bool IsSystemAdmin
        {
            get
            {
                if (UsuarioLogado == null)
                    return false;

                return UsuarioLogado.IsSystemAdmin;
            }
        }

        public bool IsCompanyAdmin
        {
            get
            {
                if (UsuarioLogado == null)
                    return false;

                if (CompanyId <= 0)
                    return false;

                if (UsuarioLogado.IsSystemAdmin)
                    return true;
                else
                {
                    SecurityController controller = new SecurityController();
                    return controller.IsAdmin(CompanyId, UsuarioLogado.Id);
                }

            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                ValidSecurityToken();
            }
            catch (Exception ex)
            {
                Response.Redirect("~/login.aspx?m=" + FixString(ex.Message));
            }
        }
        private void ValidSecurityToken()
        {
            bool isNew = false;
            if (string.IsNullOrEmpty(SecurityToken) && Request.QueryString["Token"] != null)
            {
                SecurityToken = Request.QueryString["Token"];
                isNew = true;
            }


            if (string.IsNullOrEmpty(SecurityToken))
                throw new Exception("SecurityToken is invalid");

            //SessionId|ID|yyyyMMddHHmmss|IpAddress
            string[] variableToken;
            try
            {
                variableToken = Cryptographic.Decryptor(SecurityToken).Split('|');
            }
            catch (Exception ex)
            {
                throw new Exception("SecurityToken is invalid - " + ex.Message, ex);
            }

            if (variableToken.Length <= 3)
                throw new Exception("SecurityToken is invalid");

            string clientIdOrUserId = variableToken[1].Trim();
            string sessionId = variableToken[0].Trim();
            string date = variableToken[2].Trim();
            string ipSecurity = variableToken[3].Trim().Replace(".", "").Replace(":", "").Replace("\0", "");
            //string tipoUserOrClient = variableToken[4].Trim().Replace("\0", "");
            string ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].Replace(".", "").Replace(":", "");
            if (String.IsNullOrEmpty(ipAddress))
                if (Context.Request.UserHostAddress != null)
                    ipAddress = Context.Request.UserHostAddress.Replace(".", "").Replace(":", "");

            if (ipSecurity != ipAddress && ipAddress != "1" && ipAddress != "127001")
                throw new Exception("Ip is invalid. Your IP: " + ipAddress + " and Ip Security: " + ipSecurity);

            int id;
            if (!int.TryParse(clientIdOrUserId, out id))
                throw new Exception("Client or User is invalid");

            if (SessionString != sessionId && isNew == false)
                throw new Exception("SessionId is invalid");

            if (DateTime.Now.AddMinutes(-1) > DateTime.ParseExact(date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                throw new Exception("Session expired");

            if (id != ClientOrUserId && isNew == false && ClientOrUserId != 0)
                throw new Exception("Client or User is invalid");

            if (UsuarioLogado == null || isNew)
            {
                SecurityController controller = new SecurityController();
                UsuarioLogado = controller.GetUser(ClientOrUserId);
            }
        }

        /// <summary>
        /// Abrir uma modal com o Ajax
        /// </summary>
        /// <param name="title">Titulo do Modal</param>
        /// <param name="url">Url do Modal</param>
        public void ModalAjax(string title, string url)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "ModalAjax","ModalAjax('" + title + "','" + FixSecurityUrl(url) + "');", true);
        }

        public string FixSecurityUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return "";
            if (url.IndexOf("?Token=", StringComparison.Ordinal) > 0 || url.IndexOf("&Token=", StringComparison.Ordinal) > 0)
                return url;
            if (url.IndexOf("?", StringComparison.Ordinal) < 0)
                return url + "?Token=" + SecurityToken;
            if (url.IndexOf("&", StringComparison.Ordinal) > 0)
                return url + "&Token=" + SecurityToken;
            if (url.IndexOf("?", StringComparison.Ordinal) > 0)
                return url + "&Token=" + SecurityToken;
            return url;
        }
    }
}