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
    
    public partial class tblBIAUnidadTrabajoPersonas
    {
        public long IdEmpresa { get; set; }
        public long IdUnidadTrabajo { get; set; }
        public long IdUnidadTrabajoProceso { get; set; }
        public long IdUnidadPersona { get; set; }
        public long IdClienteProceso { get; set; }
        public string Nombre { get; set; }
        public long IdDocumentoBIA { get; set; }
        public long IdProceso { get; set; }
    
        public virtual tblBIAClienteProceso tblBIAClienteProceso { get; set; }
        public virtual tblBIAUnidadTrabajoProceso tblBIAUnidadTrabajoProceso { get; set; }
    }
}
