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
    
    public partial class tblPersona
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPersona()
        {
            this.tblBCPReanudacionPersonaClave = new HashSet<tblBCPReanudacionPersonaClave>();
            this.tblBCPRecuperacionPersonaClave = new HashSet<tblBCPRecuperacionPersonaClave>();
            this.tblBIAPersonaRespaldoProceso = new HashSet<tblBIAPersonaRespaldoProceso>();
            this.tblDocumentoAprobacion = new HashSet<tblDocumentoAprobacion>();
            this.tblDocumentoCertificacion = new HashSet<tblDocumentoCertificacion>();
            this.tblDocumentoPersonaClave = new HashSet<tblDocumentoPersonaClave>();
            this.tblPBEPruebaEjecucionEjercicio = new HashSet<tblPBEPruebaEjecucionEjercicio>();
            this.tblPBEPruebaPlanificacion = new HashSet<tblPBEPruebaPlanificacion>();
            this.tblPersonaCorreo = new HashSet<tblPersonaCorreo>();
            this.tblPersonaDireccion = new HashSet<tblPersonaDireccion>();
            this.tblPersonaTelefono = new HashSet<tblPersonaTelefono>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public long IdUnidadOrganizativa { get; set; }
        public long IdCargo { get; set; }
        public long IdUsuario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPReanudacionPersonaClave> tblBCPReanudacionPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRecuperacionPersonaClave> tblBCPRecuperacionPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAPersonaRespaldoProceso> tblBIAPersonaRespaldoProceso { get; set; }
        public virtual tblCargo tblCargo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoAprobacion> tblDocumentoAprobacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoCertificacion> tblDocumentoCertificacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoPersonaClave> tblDocumentoPersonaClave { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPBEPruebaEjecucionEjercicio> tblPBEPruebaEjecucionEjercicio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPBEPruebaPlanificacion> tblPBEPruebaPlanificacion { get; set; }
        public virtual tblUnidadOrganizativa tblUnidadOrganizativa { get; set; }
        public virtual tblUsuario tblUsuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPersonaCorreo> tblPersonaCorreo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPersonaDireccion> tblPersonaDireccion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPersonaTelefono> tblPersonaTelefono { get; set; }
    }
}
