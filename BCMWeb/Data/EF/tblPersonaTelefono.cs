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
    
    public partial class tblPersonaTelefono
    {
        public long IdEmpresa { get; set; }
        public long IdPersona { get; set; }
        public long IdPersonaTelefono { get; set; }
        public long IdTipoTelefono { get; set; }
        public int CodigoArea { get; set; }
        public int NroTelefono { get; set; }
        public int Extension1 { get; set; }
        public int Extension2 { get; set; }
        public string Telefono { get; set; }
    
        public virtual tblPersona tblPersona { get; set; }
        public virtual tblTipoTelefono tblTipoTelefono { get; set; }
    }
}
