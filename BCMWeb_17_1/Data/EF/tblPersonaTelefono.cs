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
    
    public partial class tblPersonaTelefono
    {
        public long IdEmpresa { get; set; }
        public long IdPersona { get; set; }
        public long IdPersonaTelefono { get; set; }
        public long IdTipoTelefono { get; set; }
        public string Telefono { get; set; }
    
        public virtual tblPersona tblPersona { get; set; }
        public virtual tblTipoTelefono tblTipoTelefono { get; set; }
    }
}