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
    
    public partial class petz_Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_Service()
        {
            this.petz_Company_Service = new HashSet<petz_Company_Service>();
        }
    
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int insert_user_id { get; set; }
        public Nullable<int> update_user_id { get; set; }
        public System.DateTime date_insert { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
        public int service_estimated_time { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Company_Service> petz_Company_Service { get; set; }
        public virtual petz_Users petz_Users { get; set; }
        public virtual petz_Users petz_Users1 { get; set; }
    }
}
