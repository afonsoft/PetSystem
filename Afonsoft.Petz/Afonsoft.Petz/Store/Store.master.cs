using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afonsoft.Petz.Store
{
    public partial class Store : MasterPage
    {

        public void ChangeActiveMenu(string id)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ChangeActiveMenu", "ChangeActiveMenu('" + id + "');", true);
        }

        public UserEntity UsuarioLogado
        {
            get
            {
                if (Session["UsuarioLogado"] == null)
                    return null;
                else
                    return (UserEntity)Session["UsuarioLogado"];
            }
            set { Session["UsuarioLogado"] = value; }
        }

        public int CompanyId
        {
            get
            {
                if (Session["CompanyID"] == null)
                    return 0;
                else
                    return (int)Session["CompanyID"];
            }
            set { Session["CompanyID"] = value; }
        }
        public string CompanyName
        {
            get
            {
                if (Session["CompanyName"] == null)
                    return "";
                else
                    return (string)Session["CompanyName"];
            }
            set { Session["CompanyName"] = value; }
        }

        public string CompanyAddress
        {
            get
            {
                if (Session["CompanyAddress"] == null)
                    return "";
                else
                    return (string)Session["CompanyAddress"];
            }
            set { Session["CompanyAddress"] = value; }
        }

        public int AddressId
        {
            get
            {
                if (Session["AddressID"] == null)
                    Session["AddressID"] = 0;
                return (int)Session["AddressID"];
            }
            set { Session["AddressID"] = value; }
        }

        public string SecurityToken
        {
            set { Session["SecurityToken"] = value; }
            get
            {
                if (Session["SecurityToken"] == null)
                    return "";
                else
                    return (string)Session["SecurityToken"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioLogado != null)
            {
                ltUserName.Text = UsuarioLogado.Name;

                if (!IsPostBack)
                {
                    var controller = new CompaniesController();
                    var itens = controller.GetCompaniesUser(UsuarioLogado.Id);
                    if (itens.Length >= 1)
                    {
                        CompanyId = Convert.ToInt32(itens[0].Value);
                        CompanyName = itens[0].Text;

                        var address = controller.GetCompanyAddress(CompanyId).Select(x => new ItemEntity() { Text = x.ToString(), Value = x.Id.ToString() }).ToArray();
                        if (address.Length >= 1)
                        {
                            CompanyAddress = address[0].Text;
                            AddressId = Convert.ToInt32(address[0].Value);
                            RptCompanyAddress.DataSource = address;
                            RptCompanyAddress.DataBind();
                        }
                        ImgCompanyPic.ImageUrl = "/ImageHandler.ashx?ID=" + CompanyId + "&type=COMPANY&token=" + SecurityToken;
                    }
                    RptCompany.DataSource = itens;
                    RptCompany.DataBind();
                }
            }
        }

        protected void lnkUploadPicture_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsuarioLogado != null)
                {
                    int id = Convert.ToInt32(HiddenFieldID.Value);
                    string tipo = HiddenFieldTipo.Value.Trim().ToUpper();
                    string objId = HiddenFieldObjId.Value.Trim().ToUpper();
                    if (FileUploadPicture.HasFile)
                    {
                        var extension = System.IO.Path.GetExtension(FileUploadPicture.FileName);
                        if (extension != null)
                        {
                            string fileExt = extension.ToLower();

                            if (fileExt == ".jpeg" || fileExt == ".jpg")
                            {
                                HttpPostedFile file = FileUploadPicture.PostedFile;
                                if ((file != null) && (file.ContentLength > 0))
                                {
                                    if (IsImage(file) == false)
                                        throw new Exception("Não é uma imagem valida!");

                                    int iFileSize = file.ContentLength;
                                    if (iFileSize > 512000)  // 512KB = 500 * 1024
                                        throw new Exception("Arquivo muito grande! Limite de 512KB!");

                                    switch (tipo)
                                    {
                                        case "C":
                                        case "CLIENT":
                                            new ClientsController().SetClientPicture(id, FileUploadPicture.FileBytes);
                                            break;
                                        case "U":
                                        case "USER":
                                            new UsersController().SetUserPicture(id, FileUploadPicture.FileBytes);
                                            break;
                                        case "L":
                                        case "CONPANY":
                                            new CompaniesController().SetCompanyPicture(id, FileUploadPicture.FileBytes);
                                            ImgCompanyPic.ImageUrl = "/ImageHandler.ashx?ID=" + CompanyId + "&type=COMPANY&token="+ SecurityToken;
                                            break;
                                        case "P":
                                        case "PET":
                                            new PetsController().SetPetPicture(id, FileUploadPicture.FileBytes);
                                            break;
                                        default:
                                            throw new NotImplementedException("Tipo inválido: " + tipo);
                                    }

                                    //Response.Redirect(Request.RawUrl);
                                    ScriptManager.RegisterStartupScript(this, GetType(), "NotifySuccess", "NotifySuccess('Foto atualizada com sucesso!', '" + Request.RawUrl + "');ImageRefresh('" + objId + "','" + id + "', '" + tipo + "');", true);

                                }
                            }
                            else
                            {
                                throw new Exception("Somente arquivos jpg são aceitos!");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Usuário não está mais logado! Efetue o login novamente.");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NotifyError", "NotifyError('" + ex.Message.Replace(Environment.NewLine, "<br/>").Replace("\n", "").Replace("\r", "") + "', null);", true);
            }
        }

        private bool IsImage(HttpPostedFile file)
        {
            return ((file != null) && System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "image/\\S+") && (file.ContentLength > 0));
        }

        protected void RptCompany_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string[] variable = e.CommandArgument.ToString().Split('|');

            if (e.CommandName == "SelectCompany")
            {
                if (CompanyId != Convert.ToInt32(variable[0]))
                {
                    CompanyId = Convert.ToInt32(variable[0]);
                    CompanyName = variable[1];
                    var address = new CompaniesController().GetCompanyAddress(CompanyId).Select(x => new ItemEntity() { Text = x.ToString(), Value = x.Id.ToString() }).ToArray();
                    if (address.Length >= 1)
                    {
                        CompanyAddress = address[0].Text;
                        AddressId = Convert.ToInt32(address[0].Value);
                        RptCompanyAddress.DataSource = address;
                        RptCompanyAddress.DataBind();
                    }
                }
            }
            else if (e.CommandName == "SelectAddress")
            {
                AddressId = Convert.ToInt32(variable[0]);
                CompanyAddress = variable[1];
            }
        }
    }
}