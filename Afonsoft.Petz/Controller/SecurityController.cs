using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class SecurityController
    {
        private readonly Random _random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public int AuthenticateUser(String userName, String password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName", "UserName is null");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", "Password is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var entity = db.petz_Users
                            .Where(x => x.user_login == userName
                                    && x.user_password == password
                                    && x.date_delete == null)
                            .Select(x => x.user_id)
                            .ToArray();
                if (entity.Length == 1)
                    return entity[0];
                else
                    return -1;
            }
        }

        public UserEntity GetUser(int id)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var entity = db.petz_Users
                            .Where(x => x.user_id == id)
                            .Select(x => new UserEntity()
                            {
                                Email = x.user_email,
                                Id = x.user_id,
                                IsSystemAdmin = x.user_admin,
                                Name = x.user_name,
                                UserName = x.user_login
                            }
                            ).ToArray();
                if (entity.Length == 1)
                    return entity[0];
                else
                    return null;
            }
        }
        public ClientEntity GetClient(int id)
        {
            ClientsController controller = new ClientsController();
            return controller.GetClient(id);
        }

        public int AuthenticateClient(String email, String password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email", "Email is null");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", "Password is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var entity = db.petz_Clients
                            .Where(x => x.client_email == email
                                    && x.client_password == password
                                    && x.date_delete == null)
                            .Select(x => x.client_id)
                            .ToArray();

                if (entity.Length == 1)
                    return entity[0];
                else
                    return -1;
            }
        }

        public bool IsAdmin(int companyId, int userId)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Employees
                            .Where(x => x.company_id == companyId
                            && x.user_id == userId)
                            .Select(x => x.employee_admin)
                            .FirstOrDefault();
            }
        }
    }
}