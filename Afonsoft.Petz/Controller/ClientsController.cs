using Afonsoft.Petz.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using Afonsoft.Petz.Model;
using Afonsoft.Petz.Library;

namespace Afonsoft.Petz.Controller
{
    public class ClientsController
    {
        public void CreateClient(ClientEntity clientEntity, string password)
        {
            if (clientEntity == null)
                throw new ArgumentNullException("clientEntity", "clientEntity is null or invalid");

            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password", "Password is null or invalid");

            if (String.IsNullOrEmpty(clientEntity.Email))
                throw new ArgumentNullException("clientEntity.Email", "clientEntity.Email is null or invalid");

            if (String.IsNullOrEmpty(clientEntity.Name))
                throw new ArgumentNullException("clientEntity.Name", "clientEntity.Name is null or invalid");

            if (String.IsNullOrEmpty(clientEntity.Document))
                throw new ArgumentNullException("clientEntity.Document", "clientEntity.Document is null or invalid");

            if(!clientEntity.Document.IsCpf())
                throw new ArgumentNullException("clientEntity.Document", "clientEntity.Document is not a valid CPF");

            var clientAlreadyExists = GetClient(clientEntity.Email);

            if (clientAlreadyExists == null)
            {
                using (Petz_dbEntities db = new Petz_dbEntities())
                {
                    petz_Clients client = new petz_Clients
                    {
                        client_birthday = clientEntity.Birthday,
                        client_document = clientEntity.Document.Replace(".","").Replace("-",""),
                        client_email = clientEntity.Email,
                        client_name = clientEntity.Name,
                        client_nickname = clientEntity.NickName,
                        client_password = password
                    };


                    if (clientEntity.Sex == EnumSex.Female)
                        client.client_sex = "F";
                    if (clientEntity.Sex == EnumSex.Male)
                        client.client_sex = "M";
                    if (clientEntity.Sex == EnumSex.Other)
                        client.client_sex = null;

                    client.date_insert = DateTime.Now;

                    db.petz_Clients.Add(client);
                    db.SaveChanges();

                    if (clientEntity.Phones != null)
                    {
                        foreach (var phone in clientEntity.Phones)
                        {
                            petz_Client_Phone p = new petz_Client_Phone
                            {
                                client_phone = phone.Phone,
                                client_id = client.client_id
                            };
                            db.petz_Client_Phone.Add(p);
                            db.SaveChanges();
                        }
                    }

                    if (clientEntity.Address != null)
                    {
                        foreach (var address in clientEntity.Address)
                        {
                            petz_Address a = new petz_Address
                            {
                                address_complement = address.Complement,
                                address_latitude = address.Latitude,
                                address_longitude = address.Longitude,
                                address_name = address.Name,
                                address_nickname = "",
                                address_number = address.Number,
                                address_zip = address.ZipCode,
                                date_insert = DateTime.Now,
                                insert_client_id = client.client_id,
                                state_id = address.State.Id
                            };
                            db.petz_Address.Add(a);

                            petz_Client_Address p = new petz_Client_Address
                            {
                                address_id = a.address_id,
                                client_id = client.client_id
                            };
                            db.petz_Client_Address.Add(p);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                throw new Exception("The Client (" + clientEntity.Email + ") already exists");
            }
        }

        public void ChangePassword(String oldPassword, String newPassword, int clientId, int? userId = null)
        {

            if (clientId <= 0)
                throw new ArgumentOutOfRangeException("clientId", "ClientID is null");

            if (string.IsNullOrEmpty(oldPassword))
                throw new ArgumentOutOfRangeException("oldPassword", "OldPassword is null");

            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentOutOfRangeException("newPassword", "NewPassword is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var upd = db.petz_Clients.FirstOrDefault(x => x.client_id == clientId);
                if (upd != null)
                {
                    if (upd.client_password == oldPassword)
                    {
                        upd.client_password = newPassword;
                        upd.date_update = DateTime.Now;

                        if (userId != null && userId > 0)
                            upd.update_user_id = userId;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("clientId", "clientId is null");
                    }
                }
                else
                    throw new ArgumentOutOfRangeException("oldPassword", "Old Password is invalid");
            }
        }

        public byte[] GetPetClientPicture(int clientId, int petId)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                bool isPetClient = db.petz_Client_Pet.Count(x => x.client_id == clientId && x.pet_id == petId) > 0;
                if (isPetClient)
                {
                    return Compressor.Decompress(db.petz_Pets.Where(x => x.pet_id == petId && x.date_delete == null).Select(x => x.pet_picture).FirstOrDefault());
                }
                else
                {
                    throw new ArgumentOutOfRangeException("petId", "This Pet is not of this client");
                }
            }
        }

        public ClientEntity GetClient(String email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email", "email is null");

            ClientEntity client;
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                client = db.petz_Clients
                            .Where(x => x.client_email == email
                                    && x.date_delete == null)
                            .Select(x => new ClientEntity()
                            {
                                Id = x.client_id,
                                Email = x.client_email,
                                Document = x.client_document,
                                Facebook = x.client_profile_facebook,
                                Name = x.client_name,
                                NickName = x.client_nickname,
                                Birthday = x.client_birthday,
                                Sex = (x.client_sex == null ? EnumSex.Other : (x.client_sex == "F" ? EnumSex.Female : EnumSex.Male)),
                                Rating = x.client_rating ?? 0
                            }).FirstOrDefault();
            }

            if (client != null)
            {
                client.Phones = GetClientPhone(client.Id);
                client.Address = GetClientAddress(client.Id);
            }
            return client;
        }

        public ClientEntity GetClient(Int32 id)
        {
            ClientEntity client;
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                client = db.petz_Clients
                            .Where(x => x.client_id == id)
                            .Select(x => new ClientEntity()
                            {
                                Id = x.client_id,
                                Email = x.client_email,
                                Document = x.client_document,
                                Facebook = x.client_profile_facebook,
                                Name = x.client_name,
                                NickName = x.client_nickname,
                                Birthday = x.client_birthday,
                                Sex = (x.client_sex == null ? EnumSex.Other : (x.client_sex == "F" ? EnumSex.Female : EnumSex.Male)),
                                Rating = x.client_rating ?? 0
                            }).FirstOrDefault();
            }
            if (client != null)
            {
                client.Phones = GetClientPhone(id);
                client.Address = GetClientAddress(id);
            }
            return client;
        }

        public void SetClient(ClientEntity clientEntity, int? userId = null)
        {
            if (clientEntity == null)
                throw new ArgumentNullException("clientEntity", "clientEntity is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var clientUpd = db.petz_Clients.FirstOrDefault(x => x.client_id == clientEntity.Id);

                if (clientUpd != null)
                {
                    clientUpd.client_birthday = clientEntity.Birthday;
                    clientUpd.client_document = clientEntity.Document;
                    clientUpd.client_email = clientEntity.Email;
                    clientUpd.client_name = clientEntity.Name;
                    clientUpd.client_nickname = clientEntity.NickName;

                    if (clientEntity.Sex == EnumSex.Female)
                        clientUpd.client_sex = "F";
                    if (clientEntity.Sex == EnumSex.Male)
                        clientUpd.client_sex = "M";
                    if (clientEntity.Sex == EnumSex.Other)
                        clientUpd.client_sex = null;
                    clientUpd.date_update = DateTime.Now;

                    if (userId != null && userId > 0)
                        clientUpd.update_user_id = userId;
                }

                db.SaveChanges();

                if (clientEntity.Phones != null)
                {
                    foreach (var phone in clientEntity.Phones)
                    {
                        petz_Client_Phone p;
                        if (phone.Id <= 0)
                        {
                            p = new petz_Client_Phone
                            {
                                client_phone = phone.Phone,
                                client_id = clientEntity.Id
                            };
                            db.petz_Client_Phone.Add(p);
                            db.SaveChanges();
                        }
                        else
                        {
                            p = db.petz_Client_Phone.FirstOrDefault(x => x.phone_id == phone.Id && x.client_id == clientEntity.Id);
                            if (p != null)
                            {
                                p.client_phone = phone.Phone;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                if (clientEntity.Address != null)
                {
                    foreach (var address in clientEntity.Address)
                    {
                        petz_Address a;
                        if (address.Id <= 0)
                        {
                            a = new petz_Address
                            {
                                address_complement = address.Complement,
                                address_latitude = address.Latitude,
                                address_longitude = address.Longitude,
                                address_name = address.Name,
                                address_nickname = "",
                                address_number = address.Number,
                                address_zip = address.ZipCode,
                                date_insert = DateTime.Now,
                                insert_client_id = clientEntity.Id,
                                state_id = address.State.Id
                            };
                            db.petz_Address.Add(a);

                            petz_Client_Address p = new petz_Client_Address
                            {
                                address_id = a.address_id,
                                client_id = clientEntity.Id
                            };
                            db.petz_Client_Address.Add(p);
                            db.SaveChanges();
                        }else
                        {
                            a = db.petz_Address.FirstOrDefault(x => x.address_id == address.Id);
                            if (a != null)
                            {
                                a.address_complement = address.Complement;
                                a.address_latitude = address.Latitude;
                                a.address_longitude = address.Longitude;
                                a.address_name = address.Name;
                                a.address_nickname = "";
                                a.address_number = address.Number;
                                a.address_zip = address.ZipCode;
                                a.date_insert = DateTime.Now;
                                a.insert_client_id = clientEntity.Id;
                                a.state_id = address.State.Id;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        public void SetClientPicture(int id, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes", "bytes is null");

            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            if (bytes.Length > 512000)  // 512KB = 500 * 1024
                throw new Exception("File too large! Limit of 512KB!");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var client = db.petz_Clients.FirstOrDefault(x => x.client_id == id && x.date_delete == null);
                if (client != null)
                    client.client_picture = Compressor.Compress(ImageHelper.ConvertImage(bytes, System.Drawing.Imaging.ImageFormat.Jpeg));
                db.SaveChanges();
            }
        }

        public byte[] GetClientPicture(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return Compressor.Decompress(db.petz_Clients.Where(x => x.client_id == id && x.date_delete == null).Select(x=>x.client_picture).FirstOrDefault());
            }
        }

        public void SetClientPhone(int id, PhoneEntity[] phones)
        {
            if (phones == null)
                throw new ArgumentNullException("phones", "phones is null");

            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                foreach (var phone in phones)
                {
                    petz_Client_Phone p;
                    if (phone.Id <= 0)
                    {
                        p = new petz_Client_Phone
                        {
                            client_phone = phone.Phone,
                            client_id = id
                        };
                        db.petz_Client_Phone.Add(p);
                        db.SaveChanges();
                    }
                    else
                    {
                        p = db.petz_Client_Phone.FirstOrDefault(x => x.phone_id == phone.Id && x.client_id == id);
                        if (p != null)
                        {
                            p.client_phone = phone.Phone;
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("This phone (" + phone.Id + ") is not this client (" + id + ")");
                        }
                    }
                }
            }
        }

        public void DeleteClientPhone(int id, PhoneEntity phone)
        {
            if (phone == null)
                throw new ArgumentNullException("phone", "phone is null");

            if (phone.Id <= 0)
                throw new ArgumentNullException("phone.Id", "phone.ID is null");

            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {

                petz_Client_Phone p = db.petz_Client_Phone.FirstOrDefault(x => x.phone_id == phone.Id && x.client_id == id);
                if (p != null)
                {
                    db.petz_Client_Phone.Remove(p);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("This phone (" + phone.Id + ") is not this client (" + id + ")");
                }
            }
        }

        public AddressEntity[] GetClientAddress(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return (from c in db.petz_Clients
                        join ca in db.petz_Client_Address on c.client_id equals ca.client_id
                        join a in db.petz_Address on ca.address_id equals a.address_id
                        join s in db.petz_States on a.state_id equals s.state_id
                        join co in db.petz_Countries on s.country_id equals co.country_id
                        where c.client_id == id
                        && c.date_delete == null
                        && a.date_delete == null
                        select new AddressEntity()
                        {
                            Id = a.address_id,
                            Address = a.address_name,
                            Complement = a.address_complement,
                            Name = a.address_nickname,
                            Latitude = a.address_latitude,
                            Longitude = a.address_longitude,
                            Number = a.address_number,
                            ZipCode = a.address_zip,
                            State = new StatesEntity()
                            {
                                Id = s.state_id,
                                Name = s.state_name,
                                Abbreviation = s.state_abbreviation,
                                Country = new CountriesEntity()
                                {
                                    Id = co.country_id,
                                    Name = co.country_name
                                }
                            }
                        }).ToArray();
            }
        }

        public void SetPetsClient(PetsEntity petEntity, int? userId = null)
        {
           

            if (petEntity == null)
                throw new ArgumentNullException("petEntity", "petEntity is null");

            if (petEntity.SubSpecies == null || petEntity.SubSpecies.Id <= 0)
                throw new ArgumentNullException("petEntity.SubSpecies", "petEntity.SubSpecies is null");

            if (petEntity.Size == null || petEntity.Size.Id <= 0)
                throw new ArgumentNullException("petEntity.Size", "petEntity.Size is null");

            if (petEntity.ClientId <= 0)
                throw new ArgumentNullException("petEntity.ClientId", "ClientID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (petEntity.Id <= 0)
                {
                    //Novo
                    petz_Pets pet = new petz_Pets();

                    if (userId != null && userId > 0)
                        pet.insert_user_id = userId.Value;
                    else
                        pet.insert_client_id = petEntity.ClientId;

                    pet.pet_birthday = petEntity.Birthday;
                    pet.pet_color = petEntity.Color;
                    pet.pet_document = petEntity.Document;
                    pet.pet_name = petEntity.Name;
                    pet.pet_nickname = petEntity.NickName;
                    pet.pet_profile_facebook = petEntity.Facebook;
                    pet.pet_weight = petEntity.Weight;
                    pet.size_id = petEntity.Size.Id;
                    pet.sub_species_id = petEntity.SubSpecies.Id;
                    if (petEntity.Breed != null)
                        pet.breed_id = petEntity.Breed.Id;
                    if (petEntity.Sex == EnumSex.Female)
                        pet.pet_sex = "F";
                    if (petEntity.Sex == EnumSex.Male)
                        pet.pet_sex = "M";
                    if (petEntity.Sex == EnumSex.Other)
                        pet.pet_sex = null;
                    pet.date_insert = DateTime.Now;

                    db.petz_Pets.Add(pet);
                    db.SaveChanges();

                    int petId = pet.pet_id;
                    if (petEntity.ClientId > 0)
                    {
                        db.petz_Client_Pet.Add(new petz_Client_Pet() { pet_id = petId, client_id = petEntity.ClientId });
                        db.SaveChanges();
                    }
                }
                else
                {
                    //update
                    Boolean isPetClient = false;
                    if (petEntity.ClientId > 0)
                        isPetClient = db.petz_Client_Pet.Count(x => x.pet_id == petEntity.Id && x.client_id == petEntity.ClientId) == 1;

                    if (isPetClient)
                    {
                        //Update
                        petz_Pets pet = db.petz_Pets.FirstOrDefault(x => x.pet_id == petEntity.Id);
                        if (pet != null)
                        {
                            if (userId != null && userId > 0)
                                pet.update_user_id = userId.Value;
                            else
                                pet.update_client_id = petEntity.ClientId;

                            pet.pet_birthday = petEntity.Birthday;
                            pet.pet_color = petEntity.Color;
                            pet.pet_document = petEntity.Document;
                            pet.pet_name = petEntity.Name;
                            pet.pet_nickname = petEntity.NickName;
                            pet.pet_profile_facebook = petEntity.Facebook;
                            pet.pet_weight = petEntity.Weight;
                            pet.size_id = petEntity.Size.Id;
                            pet.sub_species_id = petEntity.SubSpecies.Id;
                            if (petEntity.Breed != null)
                                pet.breed_id = petEntity.Breed.Id;
                            if (petEntity.Sex == EnumSex.Female)
                                pet.pet_sex = "F";
                            if (petEntity.Sex == EnumSex.Male)
                                pet.pet_sex = "M";
                            if (petEntity.Sex == EnumSex.Other)
                                pet.pet_sex = null;
                            pet.date_update = DateTime.Now;

                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("petEntity.Id", "This Pet is not of this client");
                    }
                }
            }
        }

        public void DeletePetsClient(PetsEntity petEntity, int? userId = null)
        {
            if (petEntity == null)
                throw new ArgumentNullException("petEntity", "petEntity is null");

            if (petEntity.Id <= 0)
                throw new ArgumentNullException("petEntity.Id", "petEntity.ID is null");

            if (petEntity.ClientId <= 0)
                throw new ArgumentNullException("petEntity.ClientId", "ClientID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                //update
                Boolean isPetClient = false;
                if (petEntity.ClientId > 0)
                    isPetClient = db.petz_Client_Pet.Count(x => x.pet_id == petEntity.Id && x.client_id == petEntity.ClientId) == 1;

                if (isPetClient)
                {
                    petz_Pets pet = db.petz_Pets.FirstOrDefault(x => x.pet_id == petEntity.Id);
                    if (pet != null)
                    {
                        if (userId != null && userId > 0)
                            pet.update_user_id = userId.Value;
                        else
                            pet.update_client_id = petEntity.ClientId;
                        pet.date_delete = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("petEntity.Id", "This Pet is not of this client");
                }
            }
        }

        public PetsEntity[] GetPetsClient(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            using (var db = new Petz_dbEntities())
            {
                return (from pc in db.petz_Client_Pet
                        join p in db.petz_Pets on pc.pet_id equals p.pet_id
                        join s in db.petz_Sub_Species on p.sub_species_id equals s.sub_species_id
                        join ss in db.petz_Species on s.species_id equals ss.species_id
                        join sz in db.petz_Size on p.size_id equals sz.size_id
                        where pc.client_id == id
                        && p.date_delete == null
                        select new PetsEntity()
                        {
                            ClientId = pc.client_id,
                            Birthday = p.pet_birthday,
                            Color = p.pet_color,
                            Document = p.pet_document,
                            Id = p.pet_id,
                            Facebook = p.pet_profile_facebook,
                            Name = p.pet_name,
                            NickName = p.pet_nickname,
                            Weight = p.pet_weight,
                            Size = new SizeEntity()
                            {
                                Id = sz.size_id,
                                Name = sz.size_name,
                                Description = sz.size_description
                            },
                            Sex = (p.pet_sex == null ? EnumSex.Other : (p.pet_sex == "F" ? EnumSex.Female : EnumSex.Male)),
                            SubSpecies = new SubSpeciesEntity()
                            {
                                Id = s.sub_species_id,
                                Name = s.sub_species_name,
                                Species = new SpeciesEntity()
                                {
                                    Id = ss.species_id,
                                    Name = ss.species_name
                                }
                            },
                            Breed = db.petz_Breed
                                        .Where(b => b.breed_id == (p.breed_id ?? 0))
                                        .Select(b => new BreedEntity()
                                        {
                                            Id = b.breed_id,
                                            Name = b.breed_name,
                                            UrlReference = b.breed_url_ref,
                                            SubSpecies = new SubSpeciesEntity()
                                            {
                                                Id = s.sub_species_id,
                                                Name = s.sub_species_name,
                                                Species = new SpeciesEntity()
                                                {
                                                    Id = ss.species_id,
                                                    Name = ss.species_name
                                                }
                                            }
                                        }).FirstOrDefault(),
                            Rating = p.pet_rating ?? 0
                        }).ToArray();                    
            }
        }

        public CompaniesEntity[] GetClientCompanies(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            CompaniesController controller = new CompaniesController();
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                int[] company = db.petz_Client_Company
                            .Where(x => x.client_id == id)
                            .Select(x => x.company_id)
                            .ToArray();

                List<CompaniesEntity> array = new List<CompaniesEntity>();
                foreach (int c in company)
                    array.Add(controller.GetCompany(c));

                return array.ToArray();
            }
        }

        public void SetClientCompanies(int clientId, int conpanyId)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (conpanyId <= 0)
                throw new ArgumentNullException("conpanyId", "ConpanyID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                petz_Client_Company clientCompany = new petz_Client_Company
                {
                    client_id = clientId,
                    company_id = conpanyId
                };
                db.petz_Client_Company.Add(clientCompany);
                db.SaveChanges();
            }
        }

        public void DeleteFavoriteCompanies(int clientId, int conpanyId)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (conpanyId <= 0)
                throw new ArgumentNullException("conpanyId", "ConpanyID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var clientCompany = db.petz_Client_Company.FirstOrDefault(x => x.company_id == conpanyId && x.client_id == clientId);
                db.petz_Client_Company.Remove(clientCompany);
                db.SaveChanges();
            }
        }

        public PhoneEntity[] GetClientPhone(int id)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Client_Phone
                    .Where(x => x.client_id == id)
                    .Select(x => new PhoneEntity()
                    {
                        Id = x.phone_id,
                        Phone = x.client_phone
                    }).ToArray();
            }
        }

        public HistoricEntity[] GetPetHistoric(int clientId, int petId)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (petId <= 0)
                throw new ArgumentNullException("petId", "PetID is null");

            bool petClient;
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                petClient = db.petz_Client_Pet.Count(x => x.client_id == clientId && x.pet_id == petId) == 1;
            }

            if (petClient)
            {
                PetsController controller = new PetsController();
                return controller.GetPetHistoric(petId);
            }
            else
                throw new Exception("Pet (" + petId + ") not found!");
        }

        public VaccinationEntity[] GetPetVaccination(int clientId, int petId)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (petId <= 0)
                throw new ArgumentNullException("petId", "PetID is null");

            bool petClient;
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                petClient = db.petz_Client_Pet.Count(x => x.client_id == clientId && x.pet_id == petId) == 1;
            }
            
            if (petClient)
            {
                PetsController controller = new PetsController();
                return controller.GetPetVaccination(petId);
            }
            else
                throw new Exception("Pet ("+ petId + ") not found!");
        }

        public void DeleteScheduling(int clientId, int schedulingId)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (schedulingId <= 0)
                throw new ArgumentNullException("schedulingId", "SchedulingID is null");

            bool schedulingClient;
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                schedulingClient = db.petz_Pet_Scheduling.Count(x => x.client_id == clientId && x.scheduling_id == schedulingId) == 1;
            }

            if (schedulingClient)
            {
                SchedulingController controller = new SchedulingController();
                controller.DeleteSheduling(schedulingId);
            }else
            {
                throw new Exception("Scheduling (" + schedulingId + ") not found!");
            }
        }

        public void SetPetClientPicture(int clientId, int petId, byte[] byteArrayImage)
        {
            if (clientId <= 0)
                throw new ArgumentNullException("clientId", "ClientID is null");

            if (petId <= 0)
                throw new ArgumentNullException("petId", "PetID is null");


            if (byteArrayImage.Length > 512000)  // 512KB = 500 * 1024
                throw new Exception("File too large! Limit of 512KB!");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                Boolean isPetClient = db.petz_Client_Pet.Count(x => x.pet_id == petId && x.client_id == clientId) == 1;
                if (isPetClient)
                {
                    petz_Pets pet = db.petz_Pets.FirstOrDefault(x => x.pet_id == petId);
                    if (pet != null)
                    {
                        pet.update_client_id = clientId;
                        pet.pet_picture = Compressor.Compress(ImageHelper.ConvertImage(byteArrayImage, System.Drawing.Imaging.ImageFormat.Jpeg));
                    }
                    db.SaveChanges();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("petId", "This Pet is not of this client");
                }
            }
        }
    }
}