using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Library;
using Afonsoft.Petz.Model;

namespace Afonsoft.Petz.ws
{
    [WebService(Namespace = "http://webservice.afonsoft.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Service : WebService
    {
        readonly string _baseUrl = HttpContext.Current.Request.Url.Scheme + "://" +
                                   HttpContext.Current.Request.Url.Authority +
                                   HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

        // ReSharper disable once InconsistentNaming
        public AuthHeader authHeader = new AuthHeader();

        #region Authenticate

        [WebMethod(Description = "Method to perform authentication on WebService (Efetuar o login no WebService )")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.Out)]
        public ResponseMessage Authenticate(ClientCredentials clientCredentials)
        {
            authHeader = new AuthHeader();
            ResponseMessage securityReply = new ResponseMessage();

            authHeader.SecurityToken = "";
            securityReply.Message = "";

            try
            {
                SecurityController controller = new SecurityController();
                authHeader.SessionId = controller.RandomString(8);

                if (clientCredentials == null)
                {
                    securityReply.Message = "UserCredentials is null or invalid";
                    securityReply.Success = false;
                    return securityReply;
                }
                if (String.IsNullOrEmpty(clientCredentials.Email))
                {
                    securityReply.Message = "Email is invalid";
                    securityReply.Success = false;
                    return securityReply;
                }
                if (String.IsNullOrEmpty(clientCredentials.Password))
                {
                    securityReply.Message = "Password is invalid";
                    securityReply.Success = false;
                    return securityReply;
                }
                string ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].Replace(".", "").Replace(":", "");
                if (String.IsNullOrEmpty(ipAddress))
                    if (Context.Request.UserHostAddress != null)
                        ipAddress = Context.Request.UserHostAddress.Replace(".", "").Replace(":", "");

                if (String.IsNullOrEmpty(ipAddress))
                {
                    securityReply.Message = "Ip Address is invalid";
                    securityReply.Success = false;
                    return securityReply;
                }

                int id = controller.AuthenticateClient(clientCredentials.Email, clientCredentials.Password);
                if (id > 0)
                {
                    //SessionId|ID|yyyyMMddHHmmss|IpAddress|C
                    authHeader.SecurityToken =
                        Cryptographic.Encryptor(authHeader.SessionId + "|" + id + "|" +
                                                DateTime.Now.AddMinutes(20).ToString("yyyyMMddHHmmss") + "|" + ipAddress +
                                                "|C");
                    securityReply.Message = "Authentication successfully";
                    securityReply.Success = true;
                    return securityReply;
                }
                else
                {
                    securityReply.Message = "username or Password is invalid";
                    securityReply.Success = false;
                    return securityReply;
                }
            }
            catch (Exception ex)
            {
                securityReply.Success = false;
                securityReply.Message = ex.Message;
                securityReply.Exception = ex.Treatment();
                return securityReply;
            }
        }

        #endregion

        #region SignOut

        [WebMethod(Description = "Method to make the SignOut")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SignOut()
        {
            ResponseMessage securityReply = new ResponseMessage {Message = "User is no logged"};

            if (authHeader != null)
            {
                authHeader = new AuthHeader
                {
                    SessionId = "",
                    SecurityToken = ""
                };
                securityReply.Success = true;
                securityReply.Message = "SignOut successfully";
            }
            securityReply.Success = false;

            return securityReply;
        }

        #endregion

        #region GetStates

        [WebMethod(Description = "Method to take a states (Estados Brasileiro)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public StatesEntity[] GetStates(int id)
        {
            ValidSecurityToken(authHeader);
            StatesEntity[] states;
            StatesController controller = new StatesController();
            if (id < 0)
                CacheHelper.Add("GetStates", controller.GetStates(0), DateTime.Now.AddDays(1));

            if (CacheHelper.Exists("GetStates") == false)
                CacheHelper.Add("GetStates", controller.GetStates(0), DateTime.Now.AddDays(1));
            CacheHelper.Get("GetStates", out states);

            return states.Where(x => x.Id == (id <= 0 ? x.Id : id)).ToArray();
        }

        #endregion

        #region CreateClient

        [WebMethod(Description = "Method to create a client (Criar um novo Cliente)")]
        public ResponseMessage CreateClient(ClientEntity clientEntity, String password)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                ClientsController controller = new ClientsController();
                controller.CreateClient(clientEntity, password);
                replay.Success = true;
                replay.Message = "Client create successfully";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;
        }

        #endregion

        #region ChangePassword

        [WebMethod(Description = "Method to Change a Password of client (Alterar a senha do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage ChangePassword(String password, String newPassword1, String newPassword2)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);

                if (newPassword1 != newPassword2)
                    throw new ArgumentOutOfRangeException("password", "New Password not match");

                ClientsController controller = new ClientsController();
                controller.ChangePassword(password, newPassword1, id);
                replay.Success = true;
                replay.Message = "Password Change successfully";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;
        }

        #endregion

        #region GetClientInformation

        [WebMethod(Description = "Method to take client in the session (Recuperar as informações do Cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ClientEntity GetClientInformation()
        {
            int id = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            return controller.GetClient(id);
        }

        #endregion

        #region SetClientInformation

        [WebMethod(Description = "Method to set a client Information (Atualizar as informações do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetClientInformation(ClientEntity clientEntity)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();

                if (clientEntity == null)
                    throw new ArgumentNullException("clientEntity", "clientEntity is null");

                if (clientEntity.Id != id)
                    throw new ArgumentNullException("clientEntity.Id", "clientEntity.ID is invalid");

                controller.SetClient(clientEntity);
                replay.Success = true;
                replay.Message = "Update information successfully";
                return replay;
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region GetClientPicture

        [WebMethod(Description = "Method to take picture of client  (Recuperar a foto do Cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ImageEntity GetClientPicture()
        {
            int id = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            ImageEntity entity = new ImageEntity
            {
                Id = id,
                Url = "",
                Binary = controller.GetClientPicture(id)
            };
            if (entity.Binary != null)
                entity.Url =
                    HttpUtility.HtmlDecode(_baseUrl + "ImageHandler.ashx?ID=" + id + "&type=CLIENT&token=" +
                                           authHeader.SecurityToken);
            return entity;
        }

        #endregion

        #region SetClientPicture

        [WebMethod(
             Description =
                 "Method to set a client picture (Base64) (Alterar a foto do cliete por Byte ou pela url da foto)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetClientPicture(ImageEntity entity)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                if (entity == null)
                    throw new ArgumentNullException("entity", "Entity is null");

                if (entity.Binary == null || String.IsNullOrEmpty(entity.Url))
                    throw new ArgumentNullException("entity.Url", "Entity.URL is null");

                if (entity.Binary == null || !String.IsNullOrEmpty(entity.Url))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        entity.Binary = webClient.DownloadData(entity.Url);
                    }
                }

                if (entity.Binary == null)
                    throw new ArgumentNullException("entity", "Entity.Binary is null");

                ClientsController controller = new ClientsController();
                controller.SetClientPicture(id, entity.Binary);
                replay.Success = true;
                replay.Message = "Upload picture successfully";
                return replay;
            }
            catch (Exception ex)
            {

                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region SetClientPhone

        [WebMethod(Description = "Method to set a client phone (Alterar os telefones do cliete)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetClientPhone(PhoneEntity[] phones)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                controller.SetClientPhone(id, phones);
                replay.Success = true;
                replay.Message = "Phones update successfully";
                return replay;
            }
            catch (Exception ex)
            {

                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region DeleteClientPhone

        [WebMethod(Description = "Method to delete a client phone (delete o telefone do cliete)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage DeleteClientPhone(PhoneEntity phone)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                controller.DeleteClientPhone(id, phone);
                replay.Success = true;
                replay.Message = "phone delete successfully";
                return replay;
            }
            catch (Exception ex)
            {

                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region GetClientAddress

        [WebMethod(Description = "Method to take client Address (Recuperar os endereços do Cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public AddressEntity[] GetClientAddress()
        {
            int id = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            return controller.GetClientAddress(id);
        }

        #endregion

        #region SetClientAddress

        [WebMethod(Description = "Method to set a client Address (Adicionar ou atualizar um endereço do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetClientAddress(AddressEntity addressEntity)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                AddressController controller = new AddressController();
                controller.SetClientAddress(addressEntity, id);
                replay.Success = true;
                replay.Message = "Address insert or update successfully";
                return replay;
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region GetClientPets

        [WebMethod(Description = "Method to take user pets (Recuperar os pets do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public PetsEntity[] GetClientPets()
        {
            int clientId = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            return controller.GetPetsClient(clientId);
        }

        #endregion

        #region SetClientPets

        [WebMethod(Description = "Method to set user pets (Adicionar ou atualizar um pet do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetClientPets(PetsEntity petsEntity)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                if (petsEntity.ClientId == id)
                {
                    controller.SetPetsClient(petsEntity);
                    replay.Success = true;
                    replay.Message = "Pet insert or update successfully";
                }
                else
                {
                    replay.Success = false;
                    replay.Message = "This Pet is not of this client";
                }
                return replay;
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region GetPetHistoric

        [WebMethod(Description = "Method to get pets historic (Recuperar o historico do pet)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public HistoricEntity[] GetPetHistoric(int petId)
        {
            int clientId = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            return controller.GetPetHistoric(clientId, petId);
        }

        #endregion

        #region GetPetVaccination

        [WebMethod(Description = "Method to get pet Vaccination (Recuperar as vacinas do pet)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public VaccinationEntity[] GetPetVaccination(int petId)
        {
            int clientId = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            return controller.GetPetVaccination(clientId, petId);
        }

        #endregion

        #region DeleteClientPets

        [WebMethod(Description = "Method to delete user pets (Remover um pet do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage DeleteClientPets(PetsEntity petsEntity)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                controller.DeletePetsClient(petsEntity, id);
                replay.Success = true;
                replay.Message = "Pet delete successfully";
                return replay;
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region GetPetSpecies

        [WebMethod(Description = "Method to take pets Species (Recuperar as Especies)", EnableSession = true)]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public SpeciesEntity[] GetPetSpecies(String name = "")
        {
            ValidSecurityToken(authHeader);
            SpeciesController controller = new SpeciesController();
            SpeciesEntity[] species;

            if (name == "clean")
                CacheHelper.Add("GetSpecies", controller.GetSpecies(), DateTime.Now.AddDays(1));

            if (CacheHelper.Exists("GetSpecies") == false)
                CacheHelper.Add("GetSpecies", controller.GetSpecies(), DateTime.Now.AddDays(1));
            CacheHelper.Get("GetSpecies", out species);

            if (string.IsNullOrEmpty(name))
                return species;
            else
                return species.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToArray();
        }

        #endregion

        #region GetPetSubSpecies

        [WebMethod(Description = "Method to take pets Sub Species (Recuperar as Sub Especies)", EnableSession = true)]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public SubSpeciesEntity[] GetPetSubSpecies(int? speciesId = 0, String name = "")
        {
            ValidSecurityToken(authHeader);
            SpeciesController controller = new SpeciesController();
            SubSpeciesEntity[] subSpecies;

            if (speciesId < 0)
                CacheHelper.Add("GetSubSpecies", controller.GetSubSpecies(), DateTime.Now.AddDays(1));

            if (CacheHelper.Exists("GetSubSpecies") == false)
                CacheHelper.Add("GetSubSpecies", controller.GetSubSpecies(), DateTime.Now.AddDays(1));
            CacheHelper.Get("GetSubSpecies", out subSpecies);

            if (!string.IsNullOrEmpty(name) && speciesId <= 0)
                return subSpecies.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToArray();
            else if (string.IsNullOrEmpty(name) && speciesId > 0)
                return subSpecies.Where(x => x.Id == speciesId).ToArray();
            else if (!string.IsNullOrEmpty(name) && speciesId > 0)
                return subSpecies.Where(x => x.Id == speciesId && x.Name.ToLower().Contains(name.ToLower())).ToArray();

            return subSpecies;
        }

        #endregion

        #region GetPetBreed

        [WebMethod(Description = "Method to take pets Breed (recuperar as Raças de uma Sub Especie)",
             EnableSession = true)]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public BreedEntity[] GetPetBreed(int? subSpeciesId = 0, String name = "")
        {
            ValidSecurityToken(authHeader);
            SpeciesController controller = new SpeciesController();
            BreedEntity[] breed;

            if (subSpeciesId < 0)
                CacheHelper.Add("GetBreed", controller.GetBreed(), DateTime.Now.AddDays(1));

            if (CacheHelper.Exists("GetBreed") == false)
                CacheHelper.Add("GetBreed", controller.GetBreed(), DateTime.Now.AddDays(1));
            CacheHelper.Get("GetBreed", out breed);

            if (!string.IsNullOrEmpty(name) && subSpeciesId <= 0)
                return breed.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToArray();
            else if (string.IsNullOrEmpty(name) && subSpeciesId > 0)
                return breed.Where(x => x.Id == subSpeciesId).ToArray();
            else if (!string.IsNullOrEmpty(name) && subSpeciesId > 0)
                return breed.Where(x => x.Id == subSpeciesId && x.Name.ToLower().Contains(name.ToLower())).ToArray();

            return breed;
        }

        #endregion

        #region GetPetSize

        [WebMethod(Description = "Method to take pets Size (recuperar os tamanhos dos pets)", EnableSession = true)]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public SizeEntity[] GetPetSize(int? id = 0, String name = "")
        {
            ValidSecurityToken(authHeader);
            PetsController controller = new PetsController();
            SizeEntity[] size;

            if (id < 0)
                CacheHelper.Add("GetPetsSize", controller.GetPetsSize(), DateTime.Now.AddDays(1));

            if (CacheHelper.Exists("GetPetsSize") == false)
                CacheHelper.Add("GetPetsSize", controller.GetPetsSize(), DateTime.Now.AddDays(1));
            CacheHelper.Get("GetPetsSize", out size);

            if (!string.IsNullOrEmpty(name) && id <= 0)
                return size.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToArray();
            else if (string.IsNullOrEmpty(name) && id > 0)
                return size.Where(x => x.Id == id).ToArray();
            else if (!string.IsNullOrEmpty(name) && id > 0)
                return size.Where(x => x.Id == id && x.Name.ToLower().Contains(name.ToLower())).ToArray();

            return size;
        }

        #endregion

        #region GetCompanies

        [WebMethod(Description = "Method to take Companies (Listar os PetShop)", EnableSession = true)]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public CompaniesEntity[] GetCompanies(int? id = 0, String name = "")
        {
            ValidSecurityToken(authHeader);
            CompaniesController controller = new CompaniesController();
            CompaniesEntity[] companies;
            if (CacheHelper.Exists("GetCompanies") == false)
                CacheHelper.Add("GetCompanies", controller.GetCompanies(), DateTime.Now.AddDays(1));
            CacheHelper.Get("GetCompanies", out companies);

            if (!string.IsNullOrEmpty(name) && id <= 0)
                return companies.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToArray();
            else if (string.IsNullOrEmpty(name) && id > 0)
                return companies.Where(x => x.Id == id).ToArray();
            else if (!string.IsNullOrEmpty(name) && id > 0)
                return companies.Where(x => x.Id == id && x.Name.ToLower().Contains(name.ToLower())).ToArray();

            return companies;

        }

        #endregion

        #region GetFavoriteCompanies

        [WebMethod(Description = "Method to take Favorite Companies (Listar os PetShop favoritos do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public CompaniesEntity[] GetFavoriteCompanies()
        {
            int clientId = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            return controller.GetClientCompanies(clientId);
        }

        #endregion

        #region SetFavoriteCompanies

        [WebMethod(Description = "Method to Set a Favorite Companies (Adicionar o PetShop ao favorito)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetFavoriteCompanies(int conpanyId)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                controller.SetClientCompanies(clientId, conpanyId);
                replay.Success = true;
                replay.Message = "Favorite Company Change successfully";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;

        }

        #endregion

        #region DeleteFavoriteCompanies

        [WebMethod(Description = "Method to Set a Favorite Companies (remover o PetShop ao favorito)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage DeleteFavoriteCompanies(int conpanyId)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                controller.DeleteFavoriteCompanies(clientId, conpanyId);
                replay.Success = true;
                replay.Message = "Favorite Company remove successfully";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;

        }

        #endregion

        #region GetScheduling

        [WebMethod(Description = "Method to take Scheduling (Listar os agendamentos do cliente)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public SchedulingEntity[] GetScheduling()
        {
            int clientId = ValidSecurityToken(authHeader);
            SchedulingController controller = new SchedulingController();
            return controller.GetSchedulingClient(clientId);
        }

        #endregion

        #region DeleteScheduling

        [WebMethod(Description = "Method to delete a Scheduling (remover um agendamento)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage DeleteScheduling(int schedulingId)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                ClientsController controller = new ClientsController();
                controller.DeleteScheduling(clientId, schedulingId);
                replay.Success = true;
                replay.Message = "Scheduling remove successfully";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;

        }

        #endregion

        #region SetConfirmScheduling

        [WebMethod(Description = "Method to Confirm a Scheduling (confirmar um agendamento feito ou alterado pela loja)"
         )]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetConfirmScheduling(int schedulingId)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                SchedulingController controller = new SchedulingController();
                controller.SetConfirmShedulingByClient(schedulingId, clientId);
                replay.Success = true;
                replay.Message = "Scheduling Confirmed";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;
        }

        #endregion

        #region SetScheduling

        [WebMethod(Description = "Method to Set a Scheduling (Adicionar um agendamento)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetScheduling(SchedulingEntityInsertOrUpdate insertOrUpdate)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                SchedulingController controller = new SchedulingController();

                if (insertOrUpdate == null)
                    insertOrUpdate = new SchedulingEntityInsertOrUpdate();
                insertOrUpdate.ClientId = clientId;

                controller.SetSheduling(insertOrUpdate);
                replay.Success = true;
                replay.Message = "Scheduling Change successfully";
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;

        }

        #endregion

        #region GetCalender

        [WebMethod(Description = "Method to take a calender from company (Listar o calendário de um petshop)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public CompanyCalenderEntity[] GetCalender(int companyId)
        {
            ValidSecurityToken(authHeader);
            SchedulingController controller = new SchedulingController();
            return controller.GetCompanyCalender(companyId);
        }

        #endregion

        #region GetCompanyPicture

        [WebMethod(Description = "Method to take picture of company in Byte (Recuperar a foto da CIA em Byte)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ImageEntity GetCompanyPicture(int id)
        {
            ValidSecurityToken(authHeader);
            CompaniesController controller = new CompaniesController();
            ImageEntity entity = new ImageEntity
            {
                Id = id,
                Url = "",
                Binary = controller.GetCompanyPicture(id)
            };
            if (entity.Binary != null)
                entity.Url =
                    HttpUtility.HtmlDecode(_baseUrl + "ImageHandler.ashx?ID=" + id + "&type=COMPANY&token=" +
                                           authHeader.SecurityToken);
            return entity;
        }


        #endregion

        #region GetPetPicture

        [WebMethod(Description = "Method to take picture of PET in Byte (Recuperar a foto do PET em Byte)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ImageEntity GetPetPicture(int id)
        {
            int clientId = ValidSecurityToken(authHeader);
            ClientsController controller = new ClientsController();
            ImageEntity entity = new ImageEntity
            {
                Id = id,
                Url = "",
                Binary = controller.GetPetClientPicture(clientId, id)
            };
            if (entity.Binary != null)
                entity.Url =
                    HttpUtility.HtmlDecode(_baseUrl + "ImageHandler.ashx?ID=" + id + "&type=PET&token=" +
                                           authHeader.SecurityToken);
            return entity;
        }

        #endregion

        #region SetPetPicture

        [WebMethod(
             Description = "Method to set a pet picture (Base64) (Alterar a foto do pet por Byte ou pela url da foto)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetPetPicture(ImageEntity entity)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int id = ValidSecurityToken(authHeader);
                if (entity == null)
                    throw new ArgumentNullException("entity", "Entity is null");

                if (entity.Binary == null || String.IsNullOrEmpty(entity.Url))
                    throw new ArgumentNullException("entity.Url", "Entity.URL is null");

                if (entity.Binary == null || !String.IsNullOrEmpty(entity.Url))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        entity.Binary = webClient.DownloadData(entity.Url);
                    }
                }

                if (entity.Binary == null)
                    throw new ArgumentNullException("entity.Binary", "Entity.Binary is null");

                ClientsController controller = new ClientsController();
                controller.SetPetClientPicture(id, entity.Id, entity.Binary);
                replay.Success = true;
                replay.Message = "Upload picture successfully";
                return replay;
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }


        #endregion

        #region ValidSecurityToken

        // ReSharper disable once ParameterHidesMember
        private int ValidSecurityToken(AuthHeader authHeader)
        {
            if (authHeader != null)
            {
                if (string.IsNullOrEmpty(authHeader.SecurityToken))
                    throw new SoapException("SecurityToken is invalid", SoapException.ClientFaultCode,
                        Context.Request.Url.AbsoluteUri);

                //SessionId|ID|yyyyMMddHHmmss|IpAddress
                string[] variableToken;
                try
                {
                    variableToken = Cryptographic.Decryptor(authHeader.SecurityToken).Split('|');
                }
                catch (Exception ex)
                {
                    throw new SoapException("SecurityToken is invalid - " + ex.Message, SoapException.ClientFaultCode,
                        Context.Request.Url.AbsoluteUri);
                }

                if (variableToken.Length <= 2)
                    throw new SoapException("SecurityToken is invalid", SoapException.ClientFaultCode,
                        Context.Request.Url.AbsoluteUri);

                string clientIdOrUserId = variableToken[1].Trim();
                string sessionId = variableToken[0].Trim();
                string date = variableToken[2].Trim();
                string ipSecurity = variableToken[3].Trim().Replace(".", "").Replace(":", "").Replace("\0", "");
                string ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].Replace(".", "").Replace(":", "");
                if (String.IsNullOrEmpty(ipAddress))
                    if (Context.Request.UserHostAddress != null)
                        ipAddress = Context.Request.UserHostAddress.Replace(".", "").Replace(":", "");

                if (ipSecurity != ipAddress && ipAddress != "1" && ipAddress != "127001")
                    throw new SoapException("Ip is invalid. Your IP: " + ipAddress + " and Ip Security: " + ipSecurity,
                        SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);

                int id;
                if (!int.TryParse(clientIdOrUserId, out id))
                    throw new SoapException("Client or User is invalid", SoapException.ClientFaultCode,
                        Context.Request.Url.AbsoluteUri);

                if (authHeader.SessionId != sessionId)
                    throw new SoapException("SessionId is invalid", SoapException.ClientFaultCode,
                        Context.Request.Url.AbsoluteUri);

                if (DateTime.Now.AddMinutes(-1) >
                    DateTime.ParseExact(date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                    throw new SoapException("Session expired", SoapException.ClientFaultCode,
                        Context.Request.Url.AbsoluteUri);

                return id;

            }
            throw new SoapException("User is no logged", SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);
        }

        #endregion

        #region SetRatingCompany

        [WebMethod(Description = "Method to set a rating Company (Classificar a compania)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetRatingCompany(int id, int value, string comments)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                bool isCompanyClient = new ClientsController().GetClientCompanies(clientId).Count(x => x.Id == id) > 0;

                if (isCompanyClient)
                {
                    new RatingController().SetCompanyRating(id, clientId, value, comments);
                    replay.Success = true;
                    replay.Message = "Set Rating successfully";
                }
                else
                {
                    replay.Success = false;
                    replay.Message = "Set Rating falid, Company is not your favorite";
                }
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
            }
            return replay;
        }

        #endregion

        #region SetPetPicture

        [WebMethod(Description = "Method to set a rating Employees (Classificar o funcionário)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public ResponseMessage SetRatingEmployees(int id, int value, string comments)
        {
            ResponseMessage replay = new ResponseMessage();
            try
            {
                int clientId = ValidSecurityToken(authHeader);
                var companies = new ClientsController().GetClientCompanies(clientId);

                bool isCompanyClient = companies.Count(c => c.Employees.Any(e => e.Id == id)) > 0;
                if (isCompanyClient)
                {
                    new RatingController().SetUserRating(id, clientId, value, comments);
                    replay.Success = true;
                    replay.Message = "Set Rating successfully";
                    return replay;
                }
                else
                {
                    replay.Success = false;
                    replay.Message = "Set Rating falid, User is not your favorite Company Employees";
                    return replay;
                }
            }
            catch (Exception ex)
            {
                replay.Success = false;
                replay.Message = ex.Message;
                replay.Exception = ex.Treatment();
                return replay;
            }
        }

        #endregion

        #region GetCompanyRatingHistoric

        [WebMethod(Description = "Method to take a hitoric of Rating for Company (Recuperar a classificação)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public RatingHistoric[] GetCompanyRatingHistoric(int id)
        {
            ValidSecurityToken(authHeader);
            RatingController controller = new RatingController();
            return controller.GetCompanyRatingHistoric(id);
        }

        #endregion

        #region GetUserRatingHistoric

        [WebMethod(Description = "Method to take a hitoric of Rating for User (Recuperar a classificação)")]
        [SoapHeader("authHeader", Direction = SoapHeaderDirection.In)]
        public RatingHistoric[] GetUserRatingHistoric(int id)
        {
            ValidSecurityToken(authHeader);
            RatingController controller = new RatingController();
            return controller.GetUserRatingHistoric(id);
        }

        #endregion

    }

    [Serializable]
    public class AuthHeader : SoapHeader
    {
        public string SessionId { get; set; }

        public string SecurityToken { get; set; }
    }
}