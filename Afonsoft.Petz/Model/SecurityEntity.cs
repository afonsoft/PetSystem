using System;
using System.Xml.Serialization;

namespace Afonsoft.Petz.Model
{
    [Serializable]
    public class UserCredentials
    {
        [XmlElement(Order = 1, IsNullable = false)]
        public string Username { get; set; }

        [XmlElement(Order = 2, IsNullable = false)]
        public string Password { get; set; }

        [XmlElement(Order = 3, IsNullable = false)]
        public string IpAddress { get; set; }
    }

    public class ClientCredentials
    {
        [XmlElement(Order = 1, IsNullable = false)]
        public string Email { get; set; }

        [XmlElement(Order = 2, IsNullable = false)]
        public string Password { get; set; }
    }


    [Serializable]
    public class ResponseMessage
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}