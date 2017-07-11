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
    
    public partial class tblBIAAmenaza
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdProceso { get; set; }
        public long IdAmenaza { get; set; }
        public long IdDocumentoBIA { get; set; }
        public long IdTipoDocumento { get; set; }
        public string Descripcion { get; set; }
        public string Evento { get; set; }
        public string TipoControlImplantado { get; set; }
        public string ControlesImplantar { get; set; }
        public Nullable<short> Probabilidad { get; set; }
        public Nullable<short> Impacto { get; set; }
        public Nullable<short> Control { get; set; }
        public Nullable<short> Severidad { get; set; }
        public string Fuente { get; set; }
        public Nullable<short> Estado { get; set; }
    
        public virtual tblBIAProceso tblBIAProceso { get; set; }
        public virtual tblControlRiesgo tblControlRiesgo { get; set; }
        public virtual tblDocumento tblDocumento { get; set; }
        public virtual tblEstadoRiesgo tblEstadoRiesgo { get; set; }
        public virtual tblFuenteRiesgo tblFuenteRiesgo { get; set; }
        public virtual tblImpactoRiesgo tblImpactoRiesgo { get; set; }
        public virtual tblProbabilidadRiesgo tblProbabilidadRiesgo { get; set; }
        public virtual tblSeveridadRiesgo tblSeveridadRiesgo { get; set; }
    }
}
