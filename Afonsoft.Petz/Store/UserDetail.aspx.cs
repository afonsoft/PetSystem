using Afonsoft.Petz.Controller;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Afonsoft.Petz.Store
{
    public partial class UserDetail : PageBaseSecurity
    {
        public int UserId
        {
            get
            {
                if (Session["UserID"] == null)
                    Session["UserID"] = 0;
                return Convert.ToInt32(Session["UserID"]);
            }
            set { Session["UserID"] = value; }
        }

        public int UserRating
        {
            get
            {
                if (ViewState["UserRating"] == null)
                    ViewState["UserRating"] = 0;
                return Convert.ToInt32(ViewState["UserRating"]);
            }
            set { ViewState["UserRating"] = value; }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetPassword(string oldPwd, string newPwd)
        {
            try
            {
                if (HttpContext.Current.Session["UserID"] != null)
                {
                    int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                    UsersController controller = new UsersController();
                    if (controller.SerUserPassword(userId, oldPwd, newPwd))
                    {
                        return JsonConvert.SerializeObject(new { isOk = true, mensagem = "Senha atualizada com sucesso!" });
                    }
                    else
                        return JsonConvert.SerializeObject(new { isOk = false, mensagem = "Não foi possivel atualizar a senha!" });

                }
                return JsonConvert.SerializeObject(new { isOk = false, mensagem = "Usuário não está mais logado." });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { isOk = false, mensagem = ex.Message });
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioLogado == null)
                return;

            if (!IsPostBack)
            {
                UserId = UsuarioLogado.Id;
                ltUserName.Text = UsuarioLogado.Name;
                ltUserEmail.Text = UsuarioLogado.Email;
                ltUserLogin.Text = UsuarioLogado.UserName;
                UserRating =  Convert.ToInt16(new RatingController().GetUserRatingValue(UsuarioLogado.Id));
            }
        }
    }
}