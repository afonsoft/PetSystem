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
    
    public partial class petz_Role_Groups
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_Role_Groups()
        {
            this.petz_Role_Groups_Roles = new HashSet<petz_Role_Groups_Roles>();
            this.petz_Users_Role_Groups = new HashSet<petz_Users_Role_Groups>();
        }
    
        public int role_group_id { get; set; }
        public string role_group_description { get; set; }
        public int insert_user_id { get; set; }
        public Nullable<int> update_user_id { get; set; }
        public System.DateTime date_insert { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
    
        public virtual petz_Users petz_Users { get; set; }
        public virtual petz_Users petz_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Role_Groups_Roles> petz_Role_Groups_Roles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Users_Role_Groups> petz_Users_Role_Groups { get; set; }
    }
}