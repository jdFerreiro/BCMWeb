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
    
    public partial class tblPMTProgramacion
    {
        public long IdPMTProgramación { get; set; }
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public long IdDocumento { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public long IdEstado { get; set; }
        public bool Negocios { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
    }
}
