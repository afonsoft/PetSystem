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
    
    public partial class petz_Status
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_Status()
        {
            this.petz_Company_Status_Color = new HashSet<petz_Company_Status_Color>();
            this.petz_Pet_Scheduling = new HashSet<petz_Pet_Scheduling>();
        }
    
        public int status_id { get; set; }
        public string status_name { get; set; }
        public string status_description { get; set; }
        public string default_background_color { get; set; }
        public string default_border_color { get; set; }
        public string default_text_color { get; set; }
        public int insert_user_id { get; set; }
        public Nullable<int> update_user_id { get; set; }
        public Nullable<System.DateTime> date_delete { get; set; }
        public System.DateTime date_insert { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Company_Status_Color> petz_Company_Status_Color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Scheduling> petz_Pet_Scheduling { get; set; }
        public virtual petz_Users petz_Users { get; set; }
        public virtual petz_Users petz_Users1 { get; set; }
    }
}
