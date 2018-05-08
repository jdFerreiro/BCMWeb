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
    
    public partial class tblBCPDocumento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBCPDocumento()
        {
            this.tblBCPReanudacionPersonaClave = new HashSet<tblBCPReanudacionPersonaClave>();
            this.tblBCPReanudacionTarea = new HashSet<tblBCPReanudacionTarea>();
            this.tblBCPRecuperacionPersonaClave = new HashSet<tblBCPRecuperacionPersonaClave>();
            this.tblBCPRecuperacionRecurso = new HashSet<tblBCPRecuperacionRecurso>();
            this.tblBCPRespuestaRecurso = new HashSet<tblBCPRespuestaRecurso>();
            this.tblBCPRestauracionEquipo = new HashSet<tblBCPRestauracionEquipo>();
            this.tblBCPRestauracionInfraestructura = new HashSet<tblBCPRestauracionInfraestructura>();
            this.tblBCPRestauracionMobiliario = new HashSet<tblBCPRestauracionMobiliario>();
            this.tblBCPRestauracionOtro = new HashSet<tblBCPRestauracionOtro>();
            this.tblBCPRespuestaAccion = new HashSet<tblBCPRespuestaAccion>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdDocumentoBCP { get; set; }
        public Nullable<long> IdDocumento { get; set; }
        public Nullable<long> IdTipoDocumento { get; set; }
        public Nullable<long> IdDocumentoBIA { get; set; }
        public Nullable<long> IdProceso { get; set; }
        public Nullable<long> IdUnidadOrganizativa { get; set; }
        public string Proceso { get; set; }
        public string SubProceso { get; set; }
        public string Responsable { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPReanudacionPersonaClave> tblBCPReanudacionPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPReanudacionTarea> tblBCPReanudacionTarea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRecuperacionPersonaClave> tblBCPRecuperacionPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRecuperacionRecurso> tblBCPRecuperacionRecurso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRespuestaRecurso> tblBCPRespuestaRecurso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRestauracionEquipo> tblBCPRestauracionEquipo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRestauracionInfraestructura> tblBCPRestauracionInfraestructura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRestauracionMobiliario> tblBCPRestauracionMobiliario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRestauracionOtro> tblBCPRestauracionOtro { get; set; }
        public virtual tblBIAProceso tblBIAProceso { get; set; }
        public virtual tblDocumento tblDocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPRespuestaAccion> tblBCPRespuestaAccion { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
    }
}
