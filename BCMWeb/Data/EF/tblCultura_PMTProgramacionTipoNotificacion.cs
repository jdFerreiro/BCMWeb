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
    
    public partial class tblCultura_PMTProgramacionTipoNotificacion
    {
        public string Cultura { get; set; }
        public short IdTipoNotificacion { get; set; }
        public string Descripcion { get; set; }
    
        public virtual tblPMTProgramacionTipoNotificacion tblPMTProgramacionTipoNotificacion { get; set; }
    }
}
