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
    
    public partial class tblIncidentes
    {
        public long IdEmpresa { get; set; }
        public long IdIncidente { get; set; }
        public Nullable<int> IdTipoIncidente { get; set; }
        public Nullable<int> IdNaturalezaIncidente { get; set; }
        public Nullable<int> IdFuenteIncidente { get; set; }
        public Nullable<System.DateTime> FechaIncidente { get; set; }
        public string Descripcion { get; set; }
        public string LugarIncidente { get; set; }
        public Nullable<int> Duracion { get; set; }
        public string NombreReportero { get; set; }
        public string NombreSolucionador { get; set; }
        public string Observaciones { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblFuenteIncidente tblFuenteIncidente { get; set; }
        public virtual tblNaturalezaIncidente tblNaturalezaIncidente { get; set; }
        public virtual tblTipoIncidente tblTipoIncidente { get; set; }
    }
}
