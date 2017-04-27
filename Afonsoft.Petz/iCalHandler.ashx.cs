using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Model;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization.iCalendar.Serializers;

namespace Afonsoft.Petz
{

    /// <summary>
    /// Summary description for iCalHandler
    /// </summary>
    [WebService(Namespace = "http://pet.afonsoft.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // ReSharper disable once InconsistentNaming
    public class iCalHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.ContentType = "text/calendar";
            int eventId = Int32.Parse(context.Request.QueryString["EventId"]);
            int companyId = Int32.Parse(context.Request.QueryString["CompanyId"]);

            SchedulingController controller = new SchedulingController();
            SchedulingEntity scheduling = controller.GetSchedulingCompany(companyId, eventId).FirstOrDefault();
            if (scheduling != null)
            {
                string description = "Informações: " + scheduling.Comments + Environment.NewLine + Environment.NewLine;
                description += description += "Pet: " + scheduling.Pet + Environment.NewLine;
                description += "Sexo: " + (scheduling.Pet.Sex == EnumSex.Male ? "M" : "F") + Environment.NewLine;
                description += "Tamanho: " + scheduling.Pet.Size + Environment.NewLine;
                description += "Raça: " + scheduling.Pet.Breed != null
                    ? scheduling.Pet.Breed.SubSpecies + " | " + scheduling.Pet.Breed.Name
                    : scheduling.Pet.SubSpecies.ToString();
                if (scheduling.Pet.Breed != null || scheduling.Pet.Breed.UrlReference != null)
                    description += Environment.NewLine + "Informações da Raça: " + scheduling.Pet.Breed.UrlReference;
                description += Environment.NewLine + Environment.NewLine;

                description += "Endereço: " + scheduling.Address + Environment.NewLine;
                if (scheduling.Service != null)
                    description += "Serviço: " + scheduling.Service.Name + Environment.NewLine;
                else
                    description += "Serviço: Nenhum selecionado" + Environment.NewLine;
                if (scheduling.Employee != null)
                    description += "Funcionário: " + scheduling.Employee.User.Name + Environment.NewLine;
                else
                    description += "Funcionário: Nenhum Funcionário de preferencia" + Environment.NewLine;



                //http://stackoverflow.com/questions/30661839/generate-and-send-ical-event-to-outlook
                Calendar iCal = new Calendar
                {
                    Method = "PUBLISH",
                    Version = "2.0"
                };
                Event calendarEvent = iCal.Create<Event>();
                calendarEvent.Summary = scheduling.Company.Name + " (" + scheduling.Pet.Name + ")";
                calendarEvent.Start = new CalDateTime(scheduling.DateStart);
                calendarEvent.End = new CalDateTime(scheduling.DateEnd);
                calendarEvent.Status = EventStatus.Confirmed;
                calendarEvent.Description = description;
                calendarEvent.Location = scheduling.Address.ToString();
                calendarEvent.IsAllDay = false;
                calendarEvent.Uid = Guid.NewGuid().ToString();
                calendarEvent.Organizer = scheduling.Company.Email != null
                    ? new Organizer(scheduling.Company.Email)
                    : new Organizer("petsystem@afonsoft.com.br");
                calendarEvent.Url = new Uri("http://pet.afonsoft.com.br");
                calendarEvent.Categories.Add("PetShop");

                foreach (var companyPhone in scheduling.Company.Phones)
                {
                    calendarEvent.Contacts.Add(companyPhone.ToString());
                }
                calendarEvent.Comments.Add("Calendário gerado automátio pelo sistema.");
                calendarEvent.Comments.Add("Favor não alteraro ou responde-lo.");
                
                CalendarSerializer serializer = new CalendarSerializer(iCal);
                string eventCode = serializer.SerializeToString(iCal);
                context.Response.Write(eventCode);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}