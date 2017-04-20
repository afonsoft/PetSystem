using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class SchedulingController
    {
        private readonly ClientsController _clientController = new ClientsController();
        private readonly PetsController _petController = new PetsController();
        private readonly ServiceController _serviceController = new ServiceController();
        private readonly CompaniesController _companyController = new CompaniesController();
        private readonly AddressController _addressController = new AddressController();
        private readonly StatusController _statusController = new StatusController();
        private DateTime _filterStart = DateTime.Now.AddMonths(-6);
        private DateTime _filterEnd = DateTime.Now.AddMonths(6);
        private int _filterMonth = 6;

        /// <summary>
        /// Filtro das query, default 6 Month.
        /// </summary>
        public int FilterMonth
        {
            get
            {
                return _filterMonth;
            }
            set
            {
                _filterMonth = value;
                _filterStart = DateTime.Now.AddMonths(_filterMonth * -1);
                _filterEnd = DateTime.Now.AddMonths(_filterMonth);
            }
        }
      

        public CompanyCalenderEntity[] GetCompanyCalender(int id)
        {
            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return (from s in db.petz_Pet_Scheduling
                        join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                        where a.company_id == id
                            && s.scheduling_date_start >= _filterStart
                            && s.scheduling_date_start <= _filterEnd
                        select new CompanyCalenderEntity
                        {
                            Id = s.scheduling_id,
                            CompanyId = a.company_id,
                            AddressId = a.address_id,
                            DateStart = s.scheduling_date_start,
                            DateEnd = s.scheduling_date_end,
                            Day = s.scheduling_date_start.Day,
                            Month = s.scheduling_date_start.Month,
                            Year = s.scheduling_date_start.Year,
                            Hour = s.scheduling_date_start.Hour,
                            Minute = s.scheduling_date_start.Minute,
                            Status = (StatusEnum)Enum.ToObject(typeof(StatusEnum), s.status_id)
            }).ToArray();
            }
        }

        public bool DeleteSheduling(int id)
        {
            if (id <= 0)
                return false;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                petz_Pet_Scheduling p = db.petz_Pet_Scheduling.FirstOrDefault(x => x.scheduling_id == id);
                if (p != null)
                {
                    db.petz_Pet_Scheduling.Remove(p);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void SetSheduling(SchedulingEntityInsertOrUpdate insertOrUpdate)
        {

            if (insertOrUpdate == null)
                throw new ArgumentNullException("insertOrUpdate", "InsertOrUpdate is null");

            if (insertOrUpdate.ClientId <= 0)
                throw new ArgumentNullException("insertOrUpdate.ClientId", "InsertOrUpdate.ClientId is null");

            if (insertOrUpdate.PetId <= 0)
                throw new ArgumentNullException("insertOrUpdate.PetId", "InsertOrUpdate.PetId is null");

            if (insertOrUpdate.CompanyId <= 0)
                throw new ArgumentNullException("insertOrUpdate.CompanyId", "InsertOrUpdate.CompanyId is null");

            if (insertOrUpdate.AddressId <= 0)
                throw new ArgumentNullException("insertOrUpdate.AddressId", "InsertOrUpdate.AddressId is null");


            if (insertOrUpdate.DateStart > insertOrUpdate.DateEnd)
                throw new ArgumentOutOfRangeException("insertOrUpdate.DateStart", "DateStart greater than DateEnd");

            if (insertOrUpdate.DateStart < DateTime.Now.AddMinutes(10))
                throw new ArgumentOutOfRangeException("insertOrUpdate.DateStart", "DateStart greater than today's date");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                petz_Pet_Scheduling p;
                if (insertOrUpdate.Id > 0)
                {
                    p = db.petz_Pet_Scheduling.FirstOrDefault(x => x.scheduling_id == insertOrUpdate.Id);
                    if (p == null)
                    {
                        p = new petz_Pet_Scheduling();
                        insertOrUpdate.Id = 0;
                        p.date_insert = DateTime.Now;
                        p.status_id = (int)StatusEnum.EventCreateByClient; //Evento Criado pelo Cliente 
                        if (insertOrUpdate.UserId.HasValue)
                            p.insert_user_id = insertOrUpdate.UserId.Value;
                        else
                            p.insert_client_id = insertOrUpdate.ClientId;
                    }
                    else
                    {
                        p.date_update = DateTime.Now;
                        p.status_id = (int)StatusEnum.EventChangedByClient; //Evento Alterado pelo Cliente
                        if (insertOrUpdate.UserId.HasValue)
                            p.update_user_id = insertOrUpdate.UserId.Value;
                        else
                            p.update_client_id = insertOrUpdate.ClientId;
                    }
                }
                else
                {
                    p = new petz_Pet_Scheduling
                    {
                        status_id = (int) StatusEnum.EventCreateByClient,
                        date_insert = DateTime.Now
                    };
                    //Evento Criado pelo Cliente 
                    if (insertOrUpdate.UserId.HasValue)
                        p.insert_user_id = insertOrUpdate.UserId.Value;
                    else
                        p.insert_client_id = insertOrUpdate.ClientId;
                }

                int companyAddressId = db.petz_Company_Address.Where(x => x.company_id == insertOrUpdate.CompanyId && x.address_id == insertOrUpdate.AddressId).Select(x => x.company_address_id).FirstOrDefault();

                p.client_id = insertOrUpdate.ClientId;
                p.company_address_id = companyAddressId;
                p.pet_id = insertOrUpdate.PetId;
                p.scheduling_comments = insertOrUpdate.Comments;
                p.scheduling_date_start = insertOrUpdate.DateStart;
                p.scheduling_date_end = insertOrUpdate.DateEnd;

                p.service_id = insertOrUpdate.ServiceId;
                p.employees_id = insertOrUpdate.EmployeeId;

                if (insertOrUpdate.Id <= 0)
                    db.petz_Pet_Scheduling.Add(p);

                db.SaveChanges();
            }
        }

        public void SetConfirmShedulingByClient(int schedulingId, int clientId)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (schedulingId <= 0)
                throw new ArgumentNullException("schedulingId", "SchedulingID is null");

            var schedulingClientUpd = GetSchedulingClient(clientId).FirstOrDefault(x => x.Id == schedulingId);

            if (schedulingClientUpd == null)
                throw new ArgumentOutOfRangeException("schedulingId", schedulingId, "This schedule does not belong to you.");

            if (schedulingClientUpd.Status.Id != (int)StatusEnum.EventChangedByCompany)
                throw new OperationCanceledException("This schedule is not waiting for confirmation.");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var upd = db.petz_Pet_Scheduling.FirstOrDefault(x => x.scheduling_id == schedulingId);
                if (upd != null) upd.status_id = (int)StatusEnum.EventConfirmedByClient;
                db.SaveChanges();
            }
        }

        public void SetConfirmShedulingByCompany(int schedulingId, int company)
        {
            if (company <= 0)
                throw new ArgumentNullException("company", "Company is null");

            if (schedulingId <= 0)
                throw new ArgumentNullException("schedulingId", "SchedulingID is null");

            var schedulingClientUpd = GetSchedulingCompany(company).FirstOrDefault(x => x.Id == schedulingId);

            if (schedulingClientUpd == null)
                throw new ArgumentOutOfRangeException("schedulingId", schedulingId, "This schedule does not belong to you.");

            if (schedulingClientUpd.Status.Id != (int)StatusEnum.EventChangedByClient)
                throw new OperationCanceledException("This schedule is not waiting for confirmation.");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var upd = db.petz_Pet_Scheduling.FirstOrDefault(x => x.scheduling_id == schedulingId);
                if (upd != null) upd.status_id = (int)StatusEnum.EventConfirmedByCompany;
                db.SaveChanges();
            }
        }

        public EventObject[] GetCalenderEvent(int companyId, int addressId = 0)
        {
            List<EventObject> arrayOfEventObject = new List<EventObject>();
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var scheduling = (from s in db.petz_Pet_Scheduling
                                  join c in db.petz_Clients on s.client_id equals c.client_id
                                  join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                                  where a.company_id == companyId
                                  && a.address_id == (addressId == 0 ? s.scheduling_id : addressId)
                                  && s.scheduling_date_start >= _filterStart
                                  && s.scheduling_date_start <= _filterEnd
                                  select new
                                  {
                                      id = s.scheduling_id.ToString(),
                                      title = c.client_name,
                                      start = s.scheduling_date_start,
                                      end = s.scheduling_date_end,
                                      s.status_id,
                                      a.company_id,
                                  }).ToArray();


                foreach (var s in scheduling)
                {
                    EventObject ev = new EventObject();
                    var st = _statusController.GetStatusCompany(s.company_id, s.status_id);
                    ev.backgroundColor = st.BackgroundColor;
                    ev.borderColor = st.BorderColor;
                    ev.textColor = st.TextColor;
                    ev.editable = "false";
                    ev.id = s.id;
                    ev.title = s.title;
                    ev.start = s.start.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
                    ev.end = s.end.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
                    arrayOfEventObject.Add(ev);
                }
            }
            return arrayOfEventObject.ToArray();
        }

        public SchedulingEntity[] GetSchedulingCompany(int companyId, int schedulingId = 0)
        {
            List<SchedulingEntity> arrayOfSchedulingEntity = new List<SchedulingEntity>();
           
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var petScheduling = (from s in db.petz_Pet_Scheduling
                                     join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                                     where a.company_id == companyId
                                         && s.scheduling_date_start >= _filterStart
                                         && s.scheduling_date_start <= _filterEnd
                                         && s.scheduling_id == (schedulingId == 0 ? s.scheduling_id : schedulingId)
                                         select new {
                                             s.scheduling_id,
                                             s.scheduling_comments,
                                             s.scheduling_date_start,
                                             s.scheduling_date_end,
                                             s.pet_id,
                                             s.client_id,
                                             a.company_id,
                                             a.address_id,
                                             s.service_id,
                                             s.employees_id,
                                             s.status_id
                                         }
                                      ).ToArray();

                foreach (var ps in petScheduling)
                {
                    SchedulingEntity entity = new SchedulingEntity
                    {
                        Id = ps.scheduling_id,
                        CompanyId = ps.company_id,
                        Comments = ps.scheduling_comments,
                        DateStart = ps.scheduling_date_start,
                        DateEnd = ps.scheduling_date_end,
                        AddressId = ps.address_id,
                        Address = _addressController.GetAddress(ps.address_id),
                        Pet = _petController.GetPet(ps.pet_id),
                        Client = _clientController.GetClient(ps.client_id),
                        Status = _statusController.GetStatusCompany(ps.company_id, ps.status_id),
                        Company = _companyController.GetCompany(ps.company_id)
                    };


                    if (ps.service_id.HasValue)
                        entity.Service = _serviceController.GetService(ps.service_id.Value);
                    if(ps.employees_id.HasValue)
                        entity.Employee = _companyController.GetEmployees(ps.employees_id.Value);

                    arrayOfSchedulingEntity.Add(entity);
                }
            }

            return arrayOfSchedulingEntity.ToArray();
        }
        public SchedulingEntity[] GetSchedulingPet(int id)
        {
            List<SchedulingEntity> arrayOfSchedulingEntity = new List<SchedulingEntity>();
           
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var petScheduling = (from s in db.petz_Pet_Scheduling
                                     join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                                     where s.pet_id == id
                                         && s.scheduling_date_start >= _filterStart
                                         && s.scheduling_date_start <= _filterEnd
                                     select new
                                     {
                                         s.scheduling_id,
                                         s.scheduling_comments,
                                         s.scheduling_date_start,
                                         s.scheduling_date_end,
                                         s.pet_id,
                                         s.client_id,
                                         a.company_id,
                                         a.address_id,
                                         s.service_id,
                                         s.employees_id,
                                         s.status_id
                                     }).ToArray();

                foreach (var ps in petScheduling)
                {
                    SchedulingEntity entity = new SchedulingEntity
                    {
                        Id = ps.scheduling_id,
                        CompanyId = ps.company_id,
                        Comments = ps.scheduling_comments,
                        DateStart = ps.scheduling_date_start,
                        DateEnd = ps.scheduling_date_end,
                        AddressId = ps.address_id,
                        Address = _addressController.GetAddress(ps.address_id),
                        Pet = _petController.GetPet(ps.pet_id),
                        Client = _clientController.GetClient(ps.client_id),
                        Status = _statusController.GetStatusCompany(ps.company_id, ps.status_id),
                        Company = _companyController.GetCompany(ps.company_id)
                    };


                    if (ps.service_id.HasValue)
                        entity.Service = _serviceController.GetService(ps.service_id.Value);
                    if (ps.employees_id.HasValue)
                        entity.Employee = _companyController.GetEmployees(ps.employees_id.Value);

                    arrayOfSchedulingEntity.Add(entity);
                }
            }
            return arrayOfSchedulingEntity.ToArray();
        }

        public SchedulingEntity[] GetSchedulingClient(int id)
        {
            List<SchedulingEntity> arrayOfSchedulingEntity = new List<SchedulingEntity>();
            
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var petScheduling = (from s in db.petz_Pet_Scheduling
                                     join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                                     where s.client_id == id
                                         && s.scheduling_date_start >= _filterStart
                                         && s.scheduling_date_start <= _filterEnd
                                     select new
                                     {
                                         s.scheduling_id,
                                         s.scheduling_comments,
                                         s.scheduling_date_start,
                                         s.scheduling_date_end,
                                         s.pet_id,
                                         s.client_id,
                                         a.company_id,
                                         a.address_id,
                                         s.service_id,
                                         s.employees_id,
                                         s.status_id
                                     }).ToArray();
                
                foreach (var ps in petScheduling)
                {
                    SchedulingEntity entity = new SchedulingEntity
                    {
                        Id = ps.scheduling_id,
                        Comments = ps.scheduling_comments,
                        DateStart = ps.scheduling_date_start,
                        DateEnd = ps.scheduling_date_end,
                        AddressId = ps.address_id,
                        Address = _addressController.GetAddress(ps.address_id),
                        Pet = _petController.GetPet(ps.pet_id),
                        Client = _clientController.GetClient(ps.client_id),
                        Status = _statusController.GetStatusCompany(ps.company_id, ps.status_id),
                        Company = _companyController.GetCompany(ps.company_id)
                    };
                    if (ps.service_id.HasValue)
                        entity.Service = _serviceController.GetService(ps.service_id.Value);
                    if (ps.employees_id.HasValue)
                        entity.Employee = _companyController.GetEmployees(ps.employees_id.Value);

                    arrayOfSchedulingEntity.Add(entity);
                }
            }
            return arrayOfSchedulingEntity.ToArray();
        }
    }
}