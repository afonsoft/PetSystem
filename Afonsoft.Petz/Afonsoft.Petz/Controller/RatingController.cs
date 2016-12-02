using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class RatingController
    {
        public Boolean SetClientRating(int clientId, int insertByUserId, int value, string comments = "")
        {
            if (clientId <= 0)
                throw new ArgumentNullException(nameof(clientId), "ClientId is null");

            if (insertByUserId <= 0)
                throw new ArgumentNullException(nameof(insertByUserId), "InsertByUserId is null");

            if (value < 0 && value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value min 0 and max 5");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (db.petz_Rating.Count(x => x.insert_user_id == insertByUserId && x.rating_client_id == clientId) <= 0)
                {
                    petz_Rating rating = new petz_Rating
                    {
                        date_insert = DateTime.Now,
                        rating_comments = comments,
                        rating_value = value,
                        insert_user_id = insertByUserId,
                        rating_client_id = clientId
                    };
                    db.petz_Rating.Add(rating);
                    db.SaveChanges();

                    var upd = db.petz_Clients.FirstOrDefault(x => x.client_id == clientId);
                    if (upd != null)
                    {
                        upd.client_rating = GetClientRatingValue(clientId);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            return false;
        }

        public Boolean SetUserRating(int userId, int insertByClientId, int value, string comments = "")
        {
            if (userId <= 0)
                throw new ArgumentNullException(nameof(userId), "UserId is null");

            if (insertByClientId <= 0)
                throw new ArgumentNullException(nameof(insertByClientId), "InsertByClientId is null");

            if (value < 0 && value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value min 0 and max 5");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (db.petz_Rating.Count(x => x.insert_client_id == insertByClientId && x.rating_user_id == userId) <= 0)
                {
                    petz_Rating rating = new petz_Rating
                    {
                        date_insert = DateTime.Now,
                        rating_comments = comments,
                        rating_value = value,
                        insert_client_id = insertByClientId,
                        rating_user_id = userId
                    };
                    db.petz_Rating.Add(rating);
                    db.SaveChanges();

                    var upd = db.petz_Users.FirstOrDefault(x => x.user_id == userId);
                    if (upd != null)
                    {
                        upd.user_rating = GetUserRatingValue(userId);
                        db.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
        }

        public Boolean SetCompanyRating(int companyId, int insertByClientId, int value, string comments = "")
        {
            if (companyId <= 0)
                throw new ArgumentNullException(nameof(companyId), "CompanyId is null");

            if (insertByClientId <= 0)
                throw new ArgumentNullException(nameof(insertByClientId), "InsertByClientId is null");

            if (value < 0 && value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value min 0 and max 5");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (db.petz_Rating.Count(x => x.insert_client_id == insertByClientId && x.rating_company_id == companyId) <= 0)
                {
                    petz_Rating rating = new petz_Rating
                    {
                        date_insert = DateTime.Now,
                        rating_comments = comments,
                        rating_value = value,
                        insert_client_id = insertByClientId,
                        rating_company_id = companyId
                    };
                    db.petz_Rating.Add(rating);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public Boolean SetPetRating(int petId, int insertByUserId, int value, string comments = "")
        {
            if (petId <= 0)
                throw new ArgumentNullException(nameof(petId), "PetId is null");

            if (insertByUserId <= 0)
                throw new ArgumentNullException(nameof(insertByUserId), "InsertByUserId is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (db.petz_Rating.Count(x => x.insert_user_id == insertByUserId && x.rating_pet_id == petId) <= 0)
                {
                    petz_Rating rating = new petz_Rating
                    {
                        date_insert = DateTime.Now,
                        rating_comments = comments,
                        rating_value = value,
                        insert_user_id = insertByUserId,
                        rating_pet_id = petId
                    };
                    db.petz_Rating.Add(rating);
                    db.SaveChanges();

                    var upd = db.petz_Pets.FirstOrDefault(x => x.pet_id == petId);
                    if (upd != null)
                    {
                        upd.pet_rating = GetPetRatingValue(petId);
                        db.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
        }

        public RatingHistoric[] GetClientRatingHistoric(int id)
        {
            UsersController controller = new UsersController();
            return GetClientRating(id)
                .Select(x => new RatingHistoric()
                {
                    Comments = x.Comments,
                    Date = x.Date,
                    Id = x.Id,
                    InsertByName = controller.GetUser(x.InsertByUserId).Name,
                    RatingValue = x.RatingValue
                }).ToArray();
        }

        public RatingHistoric[] GetCompanyRatingHistoric(int id)
        {
            ClientsController controller = new ClientsController();
            return GetCompanyRating(id)
                .Select(x => new RatingHistoric()
                {
                    Comments = x.Comments,
                    Date = x.Date,
                    Id = x.Id,
                    InsertByName = controller.GetClient(x.InsertByClientId).Name,
                    RatingValue = x.RatingValue
                }).ToArray();
        }

        public RatingHistoric[] GetUserRatingHistoric(int id)
        {
            ClientsController controller = new ClientsController();
            return GetUserRating(id)
                .Select(x => new RatingHistoric()
                {
                    Comments = x.Comments,
                    Date = x.Date,
                    Id = x.Id,
                    InsertByName = controller.GetClient(x.InsertByClientId).Name,
                    RatingValue = x.RatingValue
                }).ToArray();
        }

        public RatingHistoric[] GetPetRatingHistoric(int id)
        {
            ClientsController controller = new ClientsController();
            return GetPetRating(id)
                .Select(x => new RatingHistoric()
                {
                    Comments = x.Comments,
                    Date = x.Date,
                    Id = x.Id,
                    InsertByName = controller.GetClient(x.InsertByClientId).Name,
                    RatingValue = x.RatingValue
                }).ToArray();
        }

        public RatingEntity[] GetClientRating(int id)
        {
            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Rating
                        .Where(x => x.rating_client_id == id)
                        .Select(x => new RatingEntity()
                        {
                            Comments = x.rating_comments,
                            Date = x.date_insert.Value,
                            Id = x.rating_id,
                            InsertByClientId = x.insert_client_id.HasValue ? x.insert_client_id.Value : 0,
                            InsertByUserId = x.insert_user_id.HasValue ? x.insert_user_id.Value : 0,
                            RatingClientId = x.rating_client_id.HasValue ? x.rating_client_id.Value : 0,
                            RatingPetId = x.rating_pet_id.HasValue ? x.rating_pet_id.Value : 0,
                            RatingUserId = x.rating_user_id.HasValue ? x.rating_user_id.Value : 0,
                            RatingValue = x.rating_value.Value
                        })
                        .OrderBy(x => x.Date)
                        .ToArray();
            }
        }

        public RatingEntity[] GetUserRating(int id)
        {
            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Rating
                        .Where(x => x.rating_user_id == id)
                        .Select(x => new RatingEntity()
                        {
                            Comments = x.rating_comments,
                            Date = x.date_insert.Value,
                            Id = x.rating_id,
                            InsertByClientId = x.insert_client_id.HasValue ? x.insert_client_id.Value : 0,
                            InsertByUserId = x.insert_user_id.HasValue ? x.insert_user_id.Value : 0,
                            RatingClientId = x.rating_client_id.HasValue ? x.rating_client_id.Value : 0,
                            RatingPetId = x.rating_pet_id.HasValue ? x.rating_pet_id.Value : 0,
                            RatingUserId = x.rating_user_id.HasValue ? x.rating_user_id.Value : 0,
                            RatingValue = x.rating_value.Value
                        })
                        .OrderBy(x => x.Date)
                        .ToArray();
            }
        }

        public RatingEntity[] GetCompanyRating(int id)
        {
            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Rating
                        .Where(x => x.rating_company_id == id)
                        .Select(x => new RatingEntity()
                        {
                            Comments = x.rating_comments,
                            Date = x.date_insert.Value,
                            Id = x.rating_id,
                            InsertByClientId = x.insert_client_id.HasValue ? x.insert_client_id.Value : 0,
                            InsertByUserId = x.insert_user_id.HasValue ? x.insert_user_id.Value : 0,
                            RatingClientId = x.rating_client_id.HasValue ? x.rating_client_id.Value : 0,
                            RatingPetId = x.rating_pet_id.HasValue ? x.rating_pet_id.Value : 0,
                            RatingUserId = x.rating_user_id.HasValue ? x.rating_user_id.Value : 0,
                            RatingValue = x.rating_value.Value
                        })
                        .OrderBy(x => x.Date)
                        .ToArray();
            }
        }

        public RatingEntity[] GetPetRating(int id)
        {
            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Rating
                        .Where(x => x.rating_pet_id == id)
                        .Select(x => new RatingEntity()
                        {
                            Comments = x.rating_comments,
                            Date = x.date_insert.Value,
                            Id = x.rating_id,
                            InsertByClientId = x.insert_client_id.HasValue ? x.insert_client_id.Value : 0,
                            InsertByUserId = x.insert_user_id.HasValue ? x.insert_user_id.Value : 0,
                            RatingClientId = x.rating_client_id.HasValue ? x.rating_client_id.Value : 0,
                            RatingPetId = x.rating_pet_id.HasValue ? x.rating_pet_id.Value : 0,
                            RatingUserId = x.rating_user_id.HasValue ? x.rating_user_id.Value : 0,
                            RatingValue = x.rating_value.Value
                        })
                        .OrderBy(x=>x.Date)
                        .ToArray();
            }
        }

        public double GetClientRatingValue(int id)
        {
            try
            {
                var itens = GetClientRating(id);
                double total = itens.Count();
                double value = itens.Select(x => x.RatingValue).Sum();
                return value / total;
            }
            catch
            {
                return 0;
            }
        }

        public double GetUserRatingValue(int id)
        {
            try
            {
                var itens = GetUserRating(id);
                double total = itens.Count();
                double value = itens.Select(x => x.RatingValue).Sum();
                return value / total;
            }
            catch
            {
                return 0;
            }
        }

        public double GetCompanyRatingValue(int id)
        {
            try
            {
                var itens = GetCompanyRating(id);
                double total = itens.Count();
                double value = itens.Select(x => x.RatingValue).Sum();
                return value / total;
            }
            catch
            {
                return 0;
            }
        }

        public double GetPetRatingValue(int id)
        {
            try
            {
                var itens = GetPetRating(id);
                double total = itens.Count();
                double value = itens.Select(x => x.RatingValue).Sum();
                return value / total;
            }
            catch
            {
                return 0;
            }
        }
    }
}