using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afonsoft.Petz.Store
{
    public partial class RatingDetail : PageBaseSecurity
    {
        public string TypeOfRating
        {
            get
            {
                if (ViewState["TypeOfRating"] == null)
                    ViewState["TypeOfRating"] = "";
                return Convert.ToString(ViewState["TypeOfRating"]);
            }
            set { ViewState["TypeOfRating"] = value; }
        }
        public int IdOfType
        {
            get
            {
                if (ViewState["IdOfType"] == null)
                    ViewState["IdOfType"] = 0;
                return Convert.ToInt32(ViewState["IdOfType"]);
            }
            set { ViewState["IdOfType"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioLogado == null)
                return;

            if (!IsPostBack)
            {
                if (Request.QueryString["IdOfType"] != null && Request.QueryString["TypeOfRating"] != null)
                {
                    TypeOfRating = Request.QueryString["TypeOfRating"];
                    IdOfType = Convert.ToInt32(Request.QueryString["IdOfType"]);
                    List<RatingHistoric> arrayOfRatingHistoric = new List<RatingHistoric>();
                    RatingController controller = new RatingController();
                    switch (TypeOfRating)
                    {
                        case "C":
                        case "CLIENT":
                            arrayOfRatingHistoric.AddRange(controller.GetClientRatingHistoric(IdOfType));
                            break;
                        case "U":
                        case "USER":
                            arrayOfRatingHistoric.AddRange(controller.GetUserRatingHistoric(IdOfType));
                            break;
                        case "L":
                        case "CONPANY":
                            arrayOfRatingHistoric.AddRange(controller.GetCompanyRatingHistoric(IdOfType));
                            break;
                        case "P":
                        case "PET":
                            arrayOfRatingHistoric.AddRange(controller.GetPetRatingHistoric(IdOfType));
                            break;
                        default:
                            throw new NotImplementedException("TypeOfRating inválido: " + TypeOfRating);
                    }
                    arrayOfRatingHistoric.Sort();
                    RepeaterRating.DataSource = arrayOfRatingHistoric.Select(x => new { value = x.ToString() });
                    RepeaterRating.DataBind();
                }
            }
        }
    }
}