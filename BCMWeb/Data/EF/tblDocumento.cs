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
    
    public partial class tblDocumento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblDocumento()
        {
            this.tblAuditoria = new HashSet<tblAuditoria>();
            this.tblBCPDocumento = new HashSet<tblBCPDocumento>();
            this.tblBIAAmenaza = new HashSet<tblBIAAmenaza>();
            this.tblBIADocumento = new HashSet<tblBIADocumento>();
            this.tblBIAPersonaClave = new HashSet<tblBIAPersonaClave>();
            this.tblDocumentoAprobacion = new HashSet<tblDocumentoAprobacion>();
            this.tblDocumentoCertificacion = new HashSet<tblDocumentoCertificacion>();
            this.tblDocumentoAnexo = new HashSet<tblDocumentoAnexo>();
            this.tblDocumentoContenido = new HashSet<tblDocumentoContenido>();
            this.tblDocumentoEntrevista = new HashSet<tblDocumentoEntrevista>();
            this.tblDocumentoPersonaClave = new HashSet<tblDocumentoPersonaClave>();
            this.tblPMTProgramacionDocumentos = new HashSet<tblPMTProgramacionDocumentos>();
            this.tblPPEFrecuencia = new HashSet<tblPPEFrecuencia>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long NroDocumento { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }
        public long IdEstadoDocumento { get; set; }
        public System.DateTime FechaEstadoDocumento { get; set; }
        public bool Negocios { get; set; }
        public int NroVersion { get; set; }
        public Nullable<int> VersionOriginal { get; set; }
        public long IdPersonaResponsable { get; set; }
        public bool RequiereCertificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAuditoria> tblAuditoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPDocumento> tblBCPDocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAAmenaza> tblBIAAmenaza { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIADocumento> tblBIADocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAPersonaClave> tblBIAPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoAprobacion> tblDocumentoAprobacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoCertificacion> tblDocumentoCertificacion { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblEstadoDocumento tblEstadoDocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoAnexo> tblDocumentoAnexo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoContenido> tblDocumentoContenido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoEntrevista> tblDocumentoEntrevista { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDocumentoPersonaClave> tblDocumentoPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPMTProgramacionDocumentos> tblPMTProgramacionDocumentos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPPEFrecuencia> tblPPEFrecuencia { get; set; }
    }
}
