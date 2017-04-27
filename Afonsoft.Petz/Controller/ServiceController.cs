using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class ServiceController
    {
        public ServiceEntity GetService(int id )
        {

            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Service
                        .Where(x => x.service_id == id && x.date_delete == null )
                        .Select(x => new ServiceEntity()
                        {
                            Id = x.service_id,
                            Name = x.service_name,
                            EstimatedTime = x.service_estimated_time
                        })
                        .FirstOrDefault();
            }
        }

        public ServiceEntity[] GetServices(int? id = 0)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Service
                        .Where(x => (x.service_id == id || id == 0) && x.date_delete == null)
                        .Select(x => new ServiceEntity()
                        {
                            Id = x.service_id,
                            Name = x.service_name,
                            EstimatedTime = x.service_estimated_time
                        })
                        .ToArray();
            }
        }
    }
}