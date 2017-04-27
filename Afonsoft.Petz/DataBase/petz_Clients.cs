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
    
    public partial class petz_Clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_Clients()
        {
            this.petz_Address = new HashSet<petz_Address>();
            this.petz_Address1 = new HashSet<petz_Address>();
            this.petz_Client_Address = new HashSet<petz_Client_Address>();
            this.petz_Client_Company = new HashSet<petz_Client_Company>();
            this.petz_Client_Pet = new HashSet<petz_Client_Pet>();
            this.petz_Client_Phone = new HashSet<petz_Client_Phone>();
            this.petz_Pet_Scheduling = new HashSet<petz_Pet_Scheduling>();
            this.petz_Pet_Scheduling1 = new HashSet<petz_Pet_Scheduling>();
            this.petz_Pet_Scheduling2 = new HashSet<petz_Pet_Scheduling>();
            this.petz_Pet_Vaccination = new HashSet<petz_Pet_Vaccination>();
            this.petz_Pet_Vaccination1 = new HashSet<petz_Pet_Vaccination>();
            this.petz_Pet_Vaccination_Scheduling = new HashSet<petz_Pet_Vaccination_Scheduling>();
            this.petz_Pet_Vaccination_Scheduling1 = new HashSet<petz_Pet_Vaccination_Scheduling>();
            this.petz_Pets = new HashSet<petz_Pets>();
            this.petz_Pets1 = new HashSet<petz_Pets>();
            this.petz_Rating = new HashSet<petz_Rating>();
            this.petz_Rating1 = new HashSet<petz_Rating>();
        }
    
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_nickname { get; set; }
        public string client_email { get; set; }
        public string client_document { get; set; }
        public string client_phone { get; set; }
        public string client_cellphone { get; set; }
        public Nullable<System.DateTime> client_birthday { get; set; }
        public byte[] client_picture { get; set; }
        public string client_profile_facebook { get; set; }
        public string client_password { get; set; }
        public Nullable<double> client_rating { get; set; }
        public Nullable<int> update_user_id { get; set; }
        public System.DateTime date_insert { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
        public string client_sex { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Address> petz_Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Address> petz_Address1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Client_Address> petz_Client_Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Client_Company> petz_Client_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Client_Pet> petz_Client_Pet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Client_Phone> petz_Client_Phone { get; set; }
        public virtual petz_Users petz_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Scheduling> petz_Pet_Scheduling { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Scheduling> petz_Pet_Scheduling1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Scheduling> petz_Pet_Scheduling2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Vaccination> petz_Pet_Vaccination { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Vaccination> petz_Pet_Vaccination1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Vaccination_Scheduling> petz_Pet_Vaccination_Scheduling { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Vaccination_Scheduling> petz_Pet_Vaccination_Scheduling1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pets> petz_Pets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pets> petz_Pets1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Rating> petz_Rating { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Rating> petz_Rating1 { get; set; }
    }
}