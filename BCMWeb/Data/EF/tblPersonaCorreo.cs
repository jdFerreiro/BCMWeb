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
    
    public partial class tblPersonaCorreo
    {
        public long IdEmpresa { get; set; }
        public long IdPersona { get; set; }
        public long IdPersonaCorreo { get; set; }
        public string Correo { get; set; }
        public long IdTipoCorreo { get; set; }
    
        public virtual tblPersona tblPersona { get; set; }
        public virtual tblTipoCorreo tblTipoCorreo { get; set; }
    }
}
