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
    
    public partial class petz_Company_Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public petz_Company_Address()
        {
            this.petz_Pet_Scheduling = new HashSet<petz_Pet_Scheduling>();
        }
    
        public int company_id { get; set; }
        public int address_id { get; set; }
        public int company_address_id { get; set; }
    
        public virtual petz_Address petz_Address { get; set; }
        public virtual petz_Companies petz_Companies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<petz_Pet_Scheduling> petz_Pet_Scheduling { get; set; }
    }
}
