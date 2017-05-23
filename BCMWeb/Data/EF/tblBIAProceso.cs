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
    
    public partial class tblBIAProceso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBIAProceso()
        {
            this.tblBCPDocumento = new HashSet<tblBCPDocumento>();
            this.tblBIAAplicacion = new HashSet<tblBIAAplicacion>();
            this.tblBIAClienteProceso = new HashSet<tblBIAClienteProceso>();
            this.tblBIADocumentacion = new HashSet<tblBIADocumentacion>();
            this.tblBIAEntrada = new HashSet<tblBIAEntrada>();
            this.tblBIAGranImpacto = new HashSet<tblBIAGranImpacto>();
            this.tblBIAImpactoFinanciero = new HashSet<tblBIAImpactoFinanciero>();
            this.tblBIAImpactoOperacional = new HashSet<tblBIAImpactoOperacional>();
            this.tblBIAInterdependencia = new HashSet<tblBIAInterdependencia>();
            this.tblBIAMTD = new HashSet<tblBIAMTD>();
            this.tblBIAPersonaClave = new HashSet<tblBIAPersonaClave>();
            this.tblBIAPersonaRespaldoProceso = new HashSet<tblBIAPersonaRespaldoProceso>();
            this.tblBIAProcesoAlterno = new HashSet<tblBIAProcesoAlterno>();
            this.tblBIAProveedor = new HashSet<tblBIAProveedor>();
            this.tblBIARespaldoPrimario = new HashSet<tblBIARespaldoPrimario>();
            this.tblBIARespaldoSecundario = new HashSet<tblBIARespaldoSecundario>();
            this.tblBIARPO = new HashSet<tblBIARPO>();
            this.tblBIARTO = new HashSet<tblBIARTO>();
            this.tblBIAUnidadTrabajoProceso = new HashSet<tblBIAUnidadTrabajoProceso>();
            this.tblBIAWRT = new HashSet<tblBIAWRT>();
            this.tblBIAAmenaza = new HashSet<tblBIAAmenaza>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdDocumentoBia { get; set; }
        public long IdProceso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> NroProceso { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<long> IdUnidadOrganizativa { get; set; }
        public Nullable<bool> Critico { get; set; }
        public Nullable<long> IdEstadoProceso { get; set; }
        public Nullable<System.DateTime> FechaUltimoEstatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPDocumento> tblBCPDocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAAplicacion> tblBIAAplicacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAClienteProceso> tblBIAClienteProceso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIADocumentacion> tblBIADocumentacion { get; set; }
        public virtual tblBIADocumento tblBIADocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAEntrada> tblBIAEntrada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAGranImpacto> tblBIAGranImpacto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAImpactoFinanciero> tblBIAImpactoFinanciero { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAImpactoOperacional> tblBIAImpactoOperacional { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAInterdependencia> tblBIAInterdependencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAMTD> tblBIAMTD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAPersonaClave> tblBIAPersonaClave { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAPersonaRespaldoProceso> tblBIAPersonaRespaldoProceso { get; set; }
        public virtual tblEstadoProceso tblEstadoProceso { get; set; }
        public virtual tblUnidadOrganizativa tblUnidadOrganizativa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAProcesoAlterno> tblBIAProcesoAlterno { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAProveedor> tblBIAProveedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIARespaldoPrimario> tblBIARespaldoPrimario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIARespaldoSecundario> tblBIARespaldoSecundario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIARPO> tblBIARPO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIARTO> tblBIARTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAUnidadTrabajoProceso> tblBIAUnidadTrabajoProceso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAWRT> tblBIAWRT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBIAAmenaza> tblBIAAmenaza { get; set; }
    }
}
