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
    
    public partial class tblAuditoriaProcesoCritico
    {
        public long IdProceso { get; set; }
        public long IdAuditoriaProcesoCritico { get; set; }
        public System.DateTime FechaActualizacion { get; set; }
        public bool EstadoAnterior { get; set; }
        public bool EstadoActual { get; set; }
        public long IdEmpresa { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
    }
}
