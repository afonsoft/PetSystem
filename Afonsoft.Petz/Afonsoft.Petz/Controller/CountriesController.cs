using Afonsoft.Petz.API.DataBase;
using Afonsoft.Petz.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afonsoft.Petz.API.Controller
{
    public class CountriesController
    {
        public CountriesEntity[] GetCountries(Nullable<int> ID = null)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Countries
                        .Where(x => x.date_delete == null && x.country_id == (ID == null ? x.country_id : ID.Value))
                        .Select(x => new CountriesEntity()
                        {
                            ID = x.country_id,
                            Name = x.country_name,
                            statesEntity = (db.petz_States
                                            .Where(s => s.date_delete == null
                                                    && s.country_id == x.country_id)
                                                    .Select(y => new StatesEntity()
                                                    {
                                                        Abbreviation = y.state_abbreviation,
                                                        ID = y.state_id,
                                                        Name = y.state_name
                                                    }).ToArray())
                        }).ToArray();
            }
        }
    }
}