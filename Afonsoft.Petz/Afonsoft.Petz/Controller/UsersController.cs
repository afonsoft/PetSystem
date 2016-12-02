using System;
using System.Linq;
using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using Afonsoft.Petz.Library;

namespace Afonsoft.Petz.Controller
{
    /// <summary>
    /// Informações do Usuário
    /// </summary>
    public class UsersController
    {
        public UserEntity GetUser(String userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName), "UserName is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Users
                        .Where(x => x.user_login == userName
                                && x.date_delete == null)
                        .Select(x => new UserEntity()
                        {
                            Email = x.user_email,
                            Id = x.user_id,
                            IsSystemAdmin = x.user_admin,
                            UserName = x.user_login,
                            Name = x.user_name,
                            LastIpAddress = x.user_last_ip_address,
                            Rating = x.user_rating ?? 0
                        }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Atualizar a senha do usuário
        /// </summary>
        public Boolean SerUserPassword(int id, string oldPwd, string newPwd)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var user = db.petz_Users.FirstOrDefault(x => x.user_id == id  && x.user_password == oldPwd);
                if (user == null)
                    throw new ArgumentOutOfRangeException(nameof(oldPwd), "Password inválido.");

                user.user_password = newPwd;
                db.SaveChanges();
                return true;
            }
        }

        public UserEntity GetUser(int id)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Users
                        .Where(x => x.user_id == id)
                        .Select(x => new UserEntity()
                        {
                            Email = x.user_email,
                            Id = x.user_id,
                            IsSystemAdmin = x.user_admin,
                            UserName = x.user_login,
                            Name = x.user_name,
                            LastIpAddress = x.user_last_ip_address,
                            Rating = x.user_rating ?? 0
                        }).FirstOrDefault();
            }
        }

        public void SetUserPicture(int id, byte[] byteArrayImage)
        {
            if (byteArrayImage == null)
                throw new ArgumentNullException(nameof(byteArrayImage), "byteArrayImage is null");

            if (id <= 0)
                throw new ArgumentNullException(nameof(id), "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var user = db.petz_Users.FirstOrDefault(x => x.user_id == id);
                if (user != null)
                    user.user_picture = Compressor.Compress(ImageHelper.ConvertImage(byteArrayImage, System.Drawing.Imaging.ImageFormat.Jpeg));
                db.SaveChanges();
            }
        }

        public byte[] GetUserPicture(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id), "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return Compressor.Decompress(db.petz_Users.Where(x => x.user_id == id ).Select(x => x.user_picture).FirstOrDefault());
            }
        }
    }
}