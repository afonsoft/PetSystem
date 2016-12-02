using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class SpeciesController
    {
        public SpeciesEntity[] GetSpecies(String name = "")
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    return db.petz_Species
                            .Where(x => x.species_name.Contains(name))
                            .Select(x => new SpeciesEntity()
                            {
                                Id = x.species_id,
                                Name = x.species_name
                            }).ToArray();
                }else
                {
                    return db.petz_Species
                           .Select(x => new SpeciesEntity()
                           {
                               Id = x.species_id,
                               Name = x.species_name
                           }).ToArray();
                }
            }
        }

        public SubSpeciesEntity[] GetSubSpecies(int? speciesId = 0, int? id = 0, String name = "")
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var subSpecies = db.petz_Sub_Species
                                .Where(x => (x.species_id == speciesId || speciesId == 0)
                                        && (x.sub_species_id == id || id == 0))
                                .Select(x => new SubSpeciesEntity()
                                {
                                    Id = x.sub_species_id,
                                    Name = x.sub_species_name,
                                    Species = new SpeciesEntity() { Id = x.species_id, Name = x.petz_Species.species_name }
                                }).ToArray();

                if (!string.IsNullOrEmpty(name))
                    return subSpecies.Where(x => x.Name.Contains(name)).ToArray();
                else
                    return subSpecies;
            }
        }

        public BreedEntity[] GetBreed(int? subSpeciesId = 0, int? id = 0, String name = "")
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var breed = db.petz_Breed
                            .Where(x=> (x.sub_species_id == subSpeciesId || subSpeciesId == 0)
                                    && (x.breed_id == id || id == 0))
                            .Select(x => new BreedEntity()
                            {
                                Name = x.breed_name,
                                Id = x.breed_id,
                                UrlReference = x.breed_url_ref,
                                SubSpecies = new SubSpeciesEntity()
                                {
                                    Id = x.sub_species_id,
                                    Name = x.petz_Sub_Species.sub_species_name,
                                    Species = new SpeciesEntity()
                                    {
                                        Id = x.petz_Sub_Species.species_id,
                                        Name = x.petz_Sub_Species.petz_Species.species_name
                                    }
                                }
                            }).ToArray();

                if (!string.IsNullOrEmpty(name))
                    return breed.Where(x => x.Name.Contains(name)).ToArray();
                else
                    return breed;
            }
        }
    }
}