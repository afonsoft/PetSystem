using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Web.Mvc;
using Afonsoft.Petz.API.Model;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Afonsoft.Petz.API.json
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://api.fonsoft.com.br/json/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        [AcceptVerbs("GET")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetUsersTest()
        {
            var users = new UserEntity[]
                        {
                            new UserEntity()
                            {
                                Email = "teste@teste.com",
                                ID = 1,
                                IsAdmin = false,
                                Login = "teste",
                                Nome = "teste",
                                LastIpAddress = "127.0.0.1"
                            },
                            new UserEntity()
                            {
                                Email = "teste1@teste.com",
                                ID = 1,
                                IsAdmin = false,
                                Login = "teste1",
                                Nome = "teste1",
                                LastIpAddress = "127.0.0.1"
                            }
                        };
            return JsonConvert.SerializeObject(users);
        }
    }
}