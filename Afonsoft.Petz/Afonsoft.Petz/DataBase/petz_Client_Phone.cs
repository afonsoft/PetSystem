//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Afonsoft.Petz.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class petz_Client_Phone
    {
        public int phone_id { get; set; }
        public int client_id { get; set; }
        public string client_phone { get; set; }
    
        public virtual petz_Clients petz_Clients { get; set; }
    }
}
