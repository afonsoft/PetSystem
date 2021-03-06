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
    
    public partial class petz_States
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_States()
        {
            this.petz_Address = new HashSet<petz_Address>();
        }
    
        public int state_id { get; set; }
        public int country_id { get; set; }
        public string state_name { get; set; }
        public string state_abbreviation { get; set; }
        public int insert_user_id { get; set; }
        public Nullable<int> update_user_id { get; set; }
        public System.DateTime date_insert { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Address> petz_Address { get; set; }
        public virtual petz_Countries petz_Countries { get; set; }
        public virtual petz_Users petz_Users { get; set; }
        public virtual petz_Users petz_Users1 { get; set; }
    }
}
