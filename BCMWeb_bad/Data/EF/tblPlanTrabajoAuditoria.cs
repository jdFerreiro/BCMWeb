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
    
    public partial class tblPlanTrabajoAuditoria
    {
        public long IdEmpresa { get; set; }
        public long IdAccion { get; set; }
        public long IdActividad { get; set; }
        public long IdCambioEstado { get; set; }
        public System.DateTime FechaCambioEstado { get; set; }
        public short Estado { get; set; }
        public long IdUsuarioCambioEstado { get; set; }
    
        public virtual tblPlanTrabajo tblPlanTrabajo { get; set; }
        public virtual tblPlanTrabajoAccion tblPlanTrabajoAccion { get; set; }
        public virtual tblPlanTrabajoEstatus tblPlanTrabajoEstatus { get; set; }
        public virtual tblUsuario tblUsuario { get; set; }
    }
}