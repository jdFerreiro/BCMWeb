//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BCMWeb.Data.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBIADocumento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBIADocumento()
        {
            this.tblBIAComentario = new HashSet<tblBIAComentario>();
            this.tblBIAImpactoFinanciero = new HashSet<tblBIAImpactoFinanciero>();
            this.tblBIAProceso = new HashSet<tblBIAProceso>();
            this.tblDocumento = new HashSet<tblDocumento>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdUnidadOrganizativa { get; set; }
        public Nullable<long> IdCadenaServicio { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAComentario> tblBIAComentario { get; set; }
        public virtual tblUnidadOrganizativa tblUnidadOrganizativa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAImpactoFinanciero> tblBIAImpactoFinanciero { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAProceso> tblBIAProceso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
    }
}
