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
    
    public partial class tblDispositivoEnvio
    {
        public long IdDispositivo { get; set; }
        public long IdSubModulo { get; set; }
        public Nullable<long> IdEmpresa { get; set; }
        public Nullable<long> IdUsuarioEnvia { get; set; }
        public Nullable<System.DateTime> FechaEnvio { get; set; }
        public Nullable<System.DateTime> FechaDescarga { get; set; }
        public Nullable<bool> Descargado { get; set; }
    
        public virtual tblDispositivo tblDispositivo { get; set; }
    }
}
