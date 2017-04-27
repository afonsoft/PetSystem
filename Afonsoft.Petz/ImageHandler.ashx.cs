using Afonsoft.Petz.Controller;
using Afonsoft.Petz.Library;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Web;
using System.Web.Services;

namespace Afonsoft.Petz
{
    /// <summary>
    /// Class para recuperar imagem da base
    /// </summary>
    [WebService(Namespace = "http://pet.afonsoft.com.br/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.ContentType = "text/calendar";
            try
            {
                string typeOfImage = context.Request.QueryString["type"];
                string token = context.Request.QueryString["token"];
                int id = Int32.Parse(context.Request.QueryString["ID"]);
                byte[] byteArrayImage = new byte[0];

                string ipAddress = context.Request.ServerVariables["REMOTE_ADDR"].Replace(".", "").Replace(":", "");
                if (String.IsNullOrEmpty(ipAddress))
                    if (context.Request.UserHostAddress != null)
                        ipAddress = context.Request.UserHostAddress.Replace(".", "").Replace(":", "");

                if (id > 0 && !string.IsNullOrEmpty(typeOfImage) && !string.IsNullOrEmpty(token))
                {
                    ValidSecurityToken(token, ipAddress);
                    typeOfImage = typeOfImage.Trim().ToUpper();
                    switch (typeOfImage)
                    {
                        case "CLIENT":
                            ClientsController client = new ClientsController();
                            byteArrayImage = client.GetClientPicture(id);
                            break;
                        case "PET":
                            PetsController pet = new PetsController();
                            byteArrayImage = pet.GetPetPicture(id);
                            break;
                        case "USER":
                            UsersController user = new UsersController();
                            byteArrayImage = user.GetUserPicture(id);
                            break;
                        case "COMPANY":
                            CompaniesController company = new CompaniesController();
                            byteArrayImage = company.GetCompanyPicture(id);
                            break;
                    }

                    if (byteArrayImage != null)
                    {
                        Bitmap bitmap = new Bitmap(ImageHelper.ByteToImage(byteArrayImage));
                        bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    }
                    else
                    {
                        Bitmap bitmap = new Bitmap(Image.FromFile(context.Server.MapPath("~/Images/photo.png")));
                        bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    }
                }
                else
                {
                    Bitmap bitmap = new Bitmap(ImageHelper.DrawTextImage("Invalid QueryString", Color.Red, Color.WhiteSmoke));
                    bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                Bitmap bitmap = new Bitmap(ImageHelper.DrawTextImage(ex.Treatment(true), Color.Red, Color.WhiteSmoke));
                bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
        }

        private void ValidSecurityToken(string securityToken, string ipAddress)
        {
            if (string.IsNullOrEmpty(securityToken))
                throw new Exception("SecurityToken is invalid");

            //SessionId|ID|yyyyMMddHHmmss|IpAddress
            string[] variableToken;
            try
            {
                variableToken = Cryptographic.Decryptor(securityToken).Split('|');
            }
            catch (Exception ex)
            {
                throw new Exception("SecurityToken is invalid - " + ex.Message, ex);
            }

            if (variableToken.Length <= 3)
                throw new Exception("SecurityToken is invalid");

            string clientIdOrUserId = variableToken[1].Trim();
            //string sessionId = variableToken[0].Trim();
            string date = variableToken[2].Trim();
            string ipSecurity = variableToken[3].Trim().Replace(".", "").Replace(":", "").Replace("\0", "");
            
            if (ipSecurity != ipAddress && ipAddress != "1" && ipAddress != "127001")
                throw new Exception("Ip is invalid. Your IP: " + ipAddress + " and Ip Security: " + ipSecurity);

            int id;
            if (!int.TryParse(clientIdOrUserId, out id))
                throw new Exception("Client or User is invalid");


            if (DateTime.Now.AddMinutes(-1) > DateTime.ParseExact(date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                throw new Exception("Session expired");
        }


        public bool IsReusable
        {
            get { return false; }
        }
    }
}