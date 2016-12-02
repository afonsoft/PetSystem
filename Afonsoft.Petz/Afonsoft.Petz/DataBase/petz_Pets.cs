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
    
    public partial class petz_Pets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_Pets()
        {
            this.petz_Client_Pet = new HashSet<petz_Client_Pet>();
            this.petz_Pet_Historic = new HashSet<petz_Pet_Historic>();
            this.petz_Pet_Scheduling = new HashSet<petz_Pet_Scheduling>();
            this.petz_Pet_Vaccination = new HashSet<petz_Pet_Vaccination>();
            this.petz_Pet_Vaccination_Scheduling = new HashSet<petz_Pet_Vaccination_Scheduling>();
            this.petz_Rating = new HashSet<petz_Rating>();
        }
    
        public int pet_id { get; set; }
        public string pet_name { get; set; }
        public string pet_nickname { get; set; }
        public string pet_document { get; set; }
        public string pet_color { get; set; }
        public string pet_sex { get; set; }
        public Nullable<double> pet_weight { get; set; }
        public Nullable<int> size_id { get; set; }
        public Nullable<System.DateTime> pet_birthday { get; set; }
        public byte[] pet_picture { get; set; }
        public string pet_profile_facebook { get; set; }
        public int sub_species_id { get; set; }
        public Nullable<int> breed_id { get; set; }
        public Nullable<double> pet_rating { get; set; }
        public Nullable<int> insert_client_id { get; set; }
        public Nullable<int> update_client_id { get; set; }
        public Nullable<int> insert_user_id { get; set; }
        public Nullable<int> update_user_id { get; set; }
        public System.DateTime date_insert { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
    
        public virtual petz_Breed petz_Breed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Client_Pet> petz_Client_Pet { get; set; }
        public virtual petz_Clients petz_Clients { get; set; }
        public virtual petz_Clients petz_Clients1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Historic> petz_Pet_Historic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Scheduling> petz_Pet_Scheduling { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Vaccination> petz_Pet_Vaccination { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Vaccination_Scheduling> petz_Pet_Vaccination_Scheduling { get; set; }
        public virtual petz_Size petz_Size { get; set; }
        public virtual petz_Sub_Species petz_Sub_Species { get; set; }
        public virtual petz_Users petz_Users { get; set; }
        public virtual petz_Users petz_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Rating> petz_Rating { get; set; }
    }
}
