using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Library;
using Afonsoft.Petz.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class CompaniesController
    {
        public CompaniesEntity GetCompany(int id)
        {
            return GetCompanies(id).FirstOrDefault();
        }

        public CompaniesEntity[] GetCompanies(int? id = 0, String name = "")
        {
            List<CompaniesEntity> arrayOfCompany = new List<CompaniesEntity>();
            Petz_dbEntities db = new Petz_dbEntities();
            var companiesSimple = db.petz_Companies.Where(x => x.company_id == id || id == 0).ToArray();
            var companies = companiesSimple
                .Where(x => x.date_delete == null)
                .Select(x => new
                {
                    Id = x.company_id,
                    Name = x.company_name,
                    Comments = x.company_comments,
                    NickName = x.company_nickname,
                    Email = x.company_email
                }).ToArray();
            RatingController rating = new RatingController();
            foreach (var c in companies)
            {
                CompaniesEntity x = new CompaniesEntity
                {
                    Id = c.Id,
                    Name = c.Name,
                    Comments = c.Comments,
                    NickName = c.NickName,
                    Email = c.Email
                };
                x.Address = GetCompanyAddress(x.Id);
                x.Service = GetCompanyService(x.Id);
                x.Employees = GetCompanyEmployees(x.Id);
                x.Phones = GetCompanyPhone(x.Id);
                x.WebCam = GetCompanyWebCam(x.Id);
                x.Rating = rating.GetCompanyRatingValue(x.Id);
                x.WorkDay = GetCompanyWork(x.Id);
                arrayOfCompany.Add(x);
            }
            if (!string.IsNullOrEmpty(name))
                return arrayOfCompany.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToArray();
            return arrayOfCompany.ToArray();
        }

        public void SetCompanyPicture(int id, byte[] byteArrayImage)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            if (byteArrayImage == null)
                throw new ArgumentNullException("byteArrayImage", "byteArrayImage is null");

            Petz_dbEntities db = new Petz_dbEntities();
            var client = db.petz_Companies.FirstOrDefault(x => x.company_id == id && x.date_delete == null);
            if (client != null)
                client.company_picture = Compressor.Compress(ImageHelper.ConvertImage(byteArrayImage, System.Drawing.Imaging.ImageFormat.Jpeg));
            db.SaveChanges();
        }

        public byte[] GetCompanyPicture(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            Petz_dbEntities db = new Petz_dbEntities();
            return Compressor.Decompress(db.petz_Companies.Where(x => x.company_id == id && x.date_delete == null).Select(x => x.company_picture).FirstOrDefault());
        }

        public AddressEntity[] GetCompanyAddress(int id)
        {
            AddressController controller = new AddressController();
            List<AddressEntity> arrayEntity = new List<AddressEntity>();

            Petz_dbEntities db = new Petz_dbEntities();
            var address = db.petz_Company_Address
                .Where(x => x.company_id == id)
                .Select(x => x.address_id).ToArray();

            foreach (var a in address)
                arrayEntity.Add(controller.GetAddress(a));

            return arrayEntity.ToArray();
        }

        public WorkEntity[] GetCompanyWork(int id)
        {
            Petz_dbEntities db = new Petz_dbEntities();
            return
                db.petz_Company_Work.Where(x => x.company_id == id)
                    .Select(
                        x =>
                            new WorkEntity()
                            {
                                WeekId = x.work_id,
                                StartTimeMin = x.work_start_time_min,
                                EndTimeMin = x.work_end_time_min
                            })
                    .ToArray();
        }

        public ServiceEntity[] GetCompanyService(int id)
        {
            ServiceController controller = new ServiceController();
            List<ServiceEntity> arrayEntity = new List<ServiceEntity>();

            Petz_dbEntities db = new Petz_dbEntities();
            int[] service = db.petz_Company_Service
                .Where(x => x.company_id == id)
                .Select(x => x.service_id).ToArray();

            foreach (var a in service)
                arrayEntity.Add(controller.GetService(a));

            return arrayEntity.ToArray();
        }

        public WebCamEntity[] GetCompanyWebCam(int id)
        {
            Petz_dbEntities db = new Petz_dbEntities();
            return db.petz_Company_webcam
                .Where(x => x.company_id == id)
                .Select(x => new WebCamEntity()
                {
                    Id = x.webcam_id,
                    Name = x.webcam_name,
                    Url = x.webcam_url
                }).ToArray();
        }

        public EmployeesEntity[] GetCompanyEmployees(int id)
        {
            UsersController controller = new UsersController();
            List<EmployeesEntity> arrayEmployees = new List<EmployeesEntity>();

            Petz_dbEntities db = new Petz_dbEntities();
            var employees = db.petz_Employees
                .Where(x => x.company_id == id)
                .Select(x => new
                {
                    Admin = x.employee_admin,
                    CompanyID = x.company_id,
                    Id = x.employees_id,
                    UserID = x.user_id
                }).ToArray();
            foreach (var employee in employees)
            {
                arrayEmployees.Add(new EmployeesEntity()
                {
                    IsCompanyAdmin = employee.Admin,
                    Id = employee.Id,
                    CompanyId = employee.CompanyID,
                    User = controller.GetUser(employee.UserID)
                });
            }
            return arrayEmployees.ToArray();
        }

        public EmployeesEntity GetEmployees(int id)
        {
            UsersController controller = new UsersController();

            Petz_dbEntities db = new Petz_dbEntities();
            var employee = db.petz_Employees
                .Where(x => x.employees_id == id)
                .Select(x => new
                {
                    Admin = x.employee_admin,
                    CompanyID = x.company_id,
                    Id = x.employees_id,
                    UserID = x.user_id
                }).FirstOrDefault();
            if (employee != null)
            {
                return new EmployeesEntity()
                {
                    IsCompanyAdmin = employee.Admin,
                    Id = employee.Id,
                    CompanyId = employee.CompanyID,
                    User = controller.GetUser(employee.UserID)
                };
            }
            else
                return null;
        }

        public PhoneEntity[] GetCompanyPhone(int id, int? addressId = 0)
        {
            Petz_dbEntities db = new Petz_dbEntities();
            return db.petz_Company_Phone
                .Where(x => x.company_id == id
                            && (addressId == 0 || x.address_id == addressId))
                .Select(x => new PhoneEntity()
                {
                    Id = x.phone_id,
                    Phone = x.company_phone
                }).ToArray();
        }

        public ItemEntity[] GetCompaniesUser(int userId)
        {
            Petz_dbEntities db = new Petz_dbEntities();
            return (from e in db.petz_Employees
                    join c in db.petz_Companies on e.company_id equals c.company_id
                    where e.user_id == userId && e.date_delete == null
                    select new ItemEntity()
                    {
                        Value = e.company_id.ToString(),
                        Text = c.company_name
                    }).ToArray();
        }

        public PetsEntity[] GetCompanyPets(int id, int clientId = 0)
        {
            List<PetsEntity> arrayOfPetsEntity = new List<PetsEntity>();
            ClientsController controller = new ClientsController();
            Petz_dbEntities db = new Petz_dbEntities();
            int[] idsByService = (from s in db.petz_Pet_Scheduling
                    join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                    where a.company_id == id
                          && s.date_delete == null
                          && s.client_id == (clientId <= 0 ? s.client_id : clientId)
                    select s.client_id)
                .Distinct()
                .ToArray();

            int[] idsByFavorite = db.petz_Client_Company
                .Where(x => x.company_id == id
                            && x.client_id == (clientId <= 0 ? x.client_id : clientId))
                .Select(x => x.client_id)
                .Distinct()
                .ToArray();
            int[] ids = idsByService.Concat(idsByFavorite)
                .Distinct()
                .ToArray();
            foreach (int i in ids)
                arrayOfPetsEntity.AddRange(controller.GetPetsClient(i));

            return arrayOfPetsEntity.ToArray();
        }

        public ClientEntity[] GetCompanyClient(int id, int clientId = 0)
        {
            List<ClientEntity> arrayOfClientEntity = new List<ClientEntity>();
            ClientsController controller = new ClientsController();
            Petz_dbEntities db = new Petz_dbEntities();
            int[] idsByService = (from s in db.petz_Pet_Scheduling
                    join a in db.petz_Company_Address on s.company_address_id equals a.company_address_id
                    where a.company_id == id
                          && s.date_delete == null
                          && s.client_id == (clientId <= 0 ? s.client_id : clientId)
                    select s.client_id)
                .Distinct()
                .ToArray();
            int[] idsByFavorite = db.petz_Client_Company
                .Where(x => x.company_id == id
                            && x.client_id == (clientId <= 0 ? x.client_id : clientId))
                .Select(x => x.client_id)
                .Distinct()
                .ToArray();
            int[] ids = idsByService.Concat(idsByFavorite)
                .Distinct()
                .ToArray();
            foreach (int i in ids)
                arrayOfClientEntity.Add(controller.GetClient(i));

            return arrayOfClientEntity.ToArray();
        }
    }
}