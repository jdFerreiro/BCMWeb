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
    
    public partial class tblPMTResponsableUpdate_Correo
    {
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public long IdMensaje { get; set; }
        public long IdUsuario { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
    
        public virtual tblPMTResponsableUpdate tblPMTResponsableUpdate { get; set; }
        public virtual tblUsuario tblUsuario { get; set; }
    }
}
