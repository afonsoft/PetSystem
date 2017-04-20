using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Store
{
    public partial class CalenderDetail : PageBaseSecurity
    {
        public string PetId { get; set; }
        public string ClientId { get; set; }
        public int ClientRating { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["EventID"] != null)
                {
                    if (CompanyId != Convert.ToInt32(Request.QueryString["ID"]))
                        return;

                    int eventId = Convert.ToInt32(Request.QueryString["EventID"]);
                    SchedulingController controller = new SchedulingController();
                    SchedulingEntity scheduling = controller.GetSchedulingCompany(CompanyId, eventId).FirstOrDefault();
                    if (scheduling != null)
                    {
                        lNomeClient.Text = scheduling.Client.Name;
                        ClientId = scheduling.Client.Id.ToString();
                        ltObs.Text = scheduling.Comments;
                        ltDataPet.Text = scheduling.DateStart.ToString("dd/MM/yyyy");
                        lHoraInicioPet.Text = scheduling.DateStart.ToString("HH:mm");
                        lHoraFimPet.Text = scheduling.DateEnd.ToString("HH:mm");

                        if (scheduling.Client.Phones != null && scheduling.Client.Phones.Length > 0)
                        {
                            string[] phones = scheduling.Client.Phones.Select(x => x.Phone).ToArray();
                            lTelefoneClient.Text = string.Join(" | ", phones);
                        }
                        else
                        {
                            lTelefoneClient.Text = "Nenhum telefone";
                        }

                        lNomePet.Text = scheduling.Pet.ToString();
                        PetId = scheduling.Pet.Id.ToString();
                        lSexoPet.Text = scheduling.Pet.Sex == EnumSex.Male ? "M" : "F";
                        lTamanhoPet.Text = scheduling.Pet.Size.ToString();
                        lCorPet.Text = scheduling.Pet.Color;

                        ltRacaPet.Text = scheduling.Pet.Breed != null ? scheduling.Pet.Breed.ToString() : scheduling.Pet.SubSpecies.ToString();

                        ClientRating = Convert.ToInt16(scheduling.Client.Rating);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert(ex);
            }
        }
    }
}