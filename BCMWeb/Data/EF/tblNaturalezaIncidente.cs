//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BCMWeb.Data.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblNaturalezaIncidente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblNaturalezaIncidente()
        {
            this.tblCulture_NaturalezaIncidente = new HashSet<tblCulture_NaturalezaIncidente>();
            this.tblIncidentes = new HashSet<tblIncidentes>();
        }
    
        public int IdNaturalezaIncidente { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCulture_NaturalezaIncidente> tblCulture_NaturalezaIncidente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblIncidentes> tblIncidentes { get; set; }
    }
}
