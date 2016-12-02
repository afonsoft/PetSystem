using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class AddressController
    {
        public AddressEntity GetAddress(int id)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return (from a in db.petz_Address
                        join s in db.petz_States on a.state_id equals s.state_id
                        join co in db.petz_Countries on s.country_id equals co.country_id
                        where a.address_id == id
                        && a.date_delete == null
                        select new AddressEntity()
                        {
                            Id = a.address_id,
                            Name = a.address_nickname,
                            Address = a.address_name,
                            Complement = a.address_complement,
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
                        }).FirstOrDefault();
            }
        }

        public void SetClientAddress(AddressEntity addressEntity, int clientId, int? userId = null)
        {
            if (addressEntity == null)
                throw new ArgumentNullException(nameof(addressEntity), "addressEntity is null");

            if (addressEntity.State == null || addressEntity.State.Id <= 0)
                throw new ArgumentNullException(nameof(addressEntity.State), "addressEntity.States is null");

            if (clientId == 0 && userId == null)
                throw new ArgumentNullException(nameof(clientId), "clientId of User or Client is null");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                if (addressEntity.Id <= 0)
                {
                    //Novo
                    petz_Address address = new petz_Address();

                    if (userId != null && userId > 0)
                        address.insert_user_id = userId.Value;
                    else
                        address.insert_client_id = clientId;

                    address.date_insert = DateTime.Now;
                    address.address_name = addressEntity.Address;
                    address.address_number = addressEntity.Number;
                    address.address_nickname = addressEntity.Name;
                    address.address_zip = addressEntity.ZipCode;
                    address.address_complement = addressEntity.Complement;
                    address.address_latitude = addressEntity.Latitude;
                    address.address_longitude = addressEntity.Longitude;
                    address.state_id = addressEntity.State.Id;

                    db.petz_Address.Add(address);
                    db.SaveChanges();

                    int addressId = address.address_id;
                    if (clientId > 0)
                    {
                        db.petz_Client_Address.Add(new petz_Client_Address() {address_id = addressId, client_id = clientId });
                        db.SaveChanges();
                    }
                }
                else
                {
                    Boolean isAddressClient = false;
                    if (clientId > 0)
                        isAddressClient = db.petz_Client_Address.Count(x => x.address_id == addressEntity.Id && x.client_id == clientId) == 1;
                    
                    if (isAddressClient)
                    {
                        //Update
                        petz_Address address = db.petz_Address.FirstOrDefault(x => x.address_id == addressEntity.Id);
                        if (address != null)
                        {
                            if (userId != null && userId > 0)
                            {
                                address.update_user_id = userId.Value;
                            }
                            else
                                address.update_client_id = clientId;

                            address.date_update = DateTime.Now;
                            address.address_name = addressEntity.Address;
                            address.address_number = addressEntity.Number;
                            address.address_nickname = addressEntity.Name;
                            address.address_zip = addressEntity.ZipCode;
                            address.address_complement = addressEntity.Complement;
                            address.address_latitude = addressEntity.Latitude;
                            address.address_longitude = addressEntity.Longitude;
                            address.state_id = addressEntity.State.Id;
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(nameof(addressEntity.Id), "This address is not of this client");
                        }
                    }else
                    {
                        throw new ArgumentOutOfRangeException(nameof(addressEntity.Id), "This address is not of this client");
                    }
                }
            }
        }
    }
}