using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Library;
using Afonsoft.Petz.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class PetsController
    {

        public void SetPetPicture(int id, byte[] byteArrayImage)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            if (byteArrayImage == null)
                throw new ArgumentNullException("byteArrayImage", "byteArrayImage is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var client = db.petz_Pets.FirstOrDefault(x => x.pet_id == id && x.date_delete == null);
                if (client != null)
                    client.pet_picture = Compressor.Compress(ImageHelper.ConvertImage(byteArrayImage, System.Drawing.Imaging.ImageFormat.Jpeg));
                db.SaveChanges();
            }
        }

        public byte[] GetPetPicture(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return Compressor.Decompress(db.petz_Pets.Where(x => x.pet_id == id && x.date_delete == null).Select(x => x.pet_picture).FirstOrDefault());
            }
        }

        public PetsEntity GetPet(int id)
        {
            if (id <= 0)
                return null;

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return (from p in db.petz_Pets
                        join s in db.petz_Sub_Species on p.sub_species_id equals s.sub_species_id
                        join ss in db.petz_Species on s.species_id equals ss.species_id
                        join sz in db.petz_Size on p.size_id equals sz.size_id
                        where p.pet_id == id
                        && p.date_delete == null
                        select new PetsEntity()
                        {
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
                        }).FirstOrDefault();
            }
        }

        public SizeEntity[] GetPetsSize()
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Size
                    .Select(x => new SizeEntity()
                    {
                        Description = x.size_description,
                        Id = x.size_id,
                        Name = x.size_name
                    }).ToArray();
            }
        }

        public HistoricEntity[] GetPetHistoric(PetsEntity petsEntity)
        {
            if (petsEntity == null)
                throw new ArgumentNullException("petsEntity", "petsEntity is null");

            if (petsEntity.Id <= 0)
                throw new ArgumentNullException("petsEntity.Id", "petsEntity.ID is null");

            return GetPetHistoric(petsEntity.Id);
        }

        public HistoricEntity[] GetPetHistoric(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            var arrayOfHistoricEntity = new List<HistoricEntity>();
            CompaniesController companyController = new CompaniesController();
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var arrayHistoric = db.petz_Pet_Historic.Where(x => x.pet_id == id).ToArray();
                foreach (var hist in arrayHistoric)
                {
                    HistoricEntity entity = new HistoricEntity {Comments = hist.history_comments};
                    if (hist.history_date != null) entity.Date = hist.history_date.Value;
                    entity.Id = hist.history_id;
                    entity.Employee = companyController.GetEmployees(hist.employees_id);
                    arrayOfHistoricEntity.Add(entity);
                }
            }
            return arrayOfHistoricEntity.ToArray();
        }

        public VaccinationEntity[] GetPetVaccination(PetsEntity petsEntity)
        {
            if (petsEntity == null)
                throw new ArgumentNullException("petsEntity", "petsEntity is null");

            if (petsEntity.Id <= 0)
                throw new ArgumentNullException("petsEntity.Id", "petsEntity.ID is null");

            return GetPetVaccination(petsEntity.Id);
        }

        public VaccinationEntity[] GetPetVaccination(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id", "ID is null");

            List<VaccinationEntity> arrayOfVaccinationEntity = new List<VaccinationEntity>();
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var arrayVaccination = db.petz_Pet_Vaccination.Where(x => x.pet_id == id).ToArray();

                foreach (var vaccination in arrayVaccination)
                {
                    VaccinationEntity entity = new VaccinationEntity
                    {
                        Comments = vaccination.vaccination_comments,
                        Id = vaccination.vaccination_id
                    };
                    if (vaccination.vaccination_date != null) entity.Date = vaccination.vaccination_date.Value;
                    arrayOfVaccinationEntity.Add(entity);
                }
            }
            return arrayOfVaccinationEntity.ToArray();
        }
    }
}