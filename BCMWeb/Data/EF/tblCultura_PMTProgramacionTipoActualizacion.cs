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
    
    public partial class tblCultura_PMTProgramacionTipoActualizacion
    {
        public string Culture { get; set; }
        public short IdTipoActualizacion { get; set; }
        public string Descripcion { get; set; }
    
        public virtual tblPMTProgramacionTipoActualizacion tblPMTProgramacionTipoActualizacion { get; set; }
    }
}
