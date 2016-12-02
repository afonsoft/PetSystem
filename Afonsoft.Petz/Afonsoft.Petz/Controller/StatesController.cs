using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class StatesController
    {
        public StatesEntity[] GetStates(int id )
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return (from s in db.petz_States
                        join c in db.petz_Countries on s.country_id equals c.country_id
                        where s.date_delete == null
                        && s.state_id == (id <= 0 ? s.state_id : id)
                        select new StatesEntity()
                        {
                            Id = s.state_id,
                            Name = s.state_name,
                            Abbreviation = s.state_abbreviation,
                            Country = new CountriesEntity()
                            {
                                Id = c.country_id,
                                Name = c.country_name
                            }
                        }).ToArray();
            }
        }
    }
}