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
    
    public partial class tblVicepresidencia
    {
        public long IdEmpresa { get; set; }
        public long IdVicepresidencia { get; set; }
        public string Nombre { get; set; }
        public string CalleAvenida { get; set; }
        public string EdificioCasa { get; set; }
        public string PisoNivel { get; set; }
        public string TorreAla { get; set; }
        public string Urbanizacion { get; set; }
        public long IdCiudad { get; set; }
        public long IdEstado { get; set; }
        public long IdPais { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblPais tblPais { get; set; }
    }
}
