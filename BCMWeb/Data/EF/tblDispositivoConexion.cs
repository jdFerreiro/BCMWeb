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
    
    public partial class tblDispositivoConexion
    {
        public long IdEmpresa { get; set; }
        public long IdDispositivo { get; set; }
        public long IdUsuario { get; set; }
        public System.DateTime fechaConexion { get; set; }
        public string DireccionIP { get; set; }
    
        public virtual tblDispositivo tblDispositivo { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblUsuario tblUsuario { get; set; }
    }
}
