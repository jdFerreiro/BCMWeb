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
    
    public partial class tblEscala
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblEscala()
        {
            this.tblBIAImpactoOperacional = new HashSet<tblBIAImpactoOperacional>();
            this.tblBIAMTD = new HashSet<tblBIAMTD>();
            this.tblBIARPO = new HashSet<tblBIARPO>();
            this.tblBIARTO = new HashSet<tblBIARTO>();
            this.tblBIAWRT = new HashSet<tblBIAWRT>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdEscala { get; set; }
        public long IdTipoEscala { get; set; }
        public short Valor { get; set; }
        public string Descripcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAImpactoOperacional> tblBIAImpactoOperacional { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAMTD> tblBIAMTD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIARPO> tblBIARPO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIARTO> tblBIARTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAWRT> tblBIAWRT { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblTipoEscala tblTipoEscala { get; set; }
    }
}
