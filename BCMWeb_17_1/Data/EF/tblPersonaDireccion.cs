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
    
    public partial class tblPersonaDireccion
    {
        public long IdEmpresa { get; set; }
        public long IdPersona { get; set; }
        public long IdPersonaDireccion { get; set; }
        public long IdTipoDireccion { get; set; }
        public string Ubicacion { get; set; }
        public long IdPais { get; set; }
        public long IdEstado { get; set; }
        public long IdCiudad { get; set; }
    
        public virtual tblCiudad tblCiudad { get; set; }
        public virtual tblEstado tblEstado { get; set; }
        public virtual tblPais tblPais { get; set; }
        public virtual tblPersona tblPersona { get; set; }
        public virtual tblTipoDireccion tblTipoDireccion { get; set; }
    }
}