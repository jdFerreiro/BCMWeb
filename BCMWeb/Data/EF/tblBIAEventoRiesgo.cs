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
    
    public partial class tblBIAEventoRiesgo
    {
        public long IdEventoRiesgo { get; set; }
        public long IdEmpresa { get; set; }
        public short Probabilidad { get; set; }
        public short Impacto { get; set; }
        public short Control { get; set; }
        public long Severidad { get; set; }
        public long IdEstadoRiesgo { get; set; }
        public long IdFuenteRiesgo { get; set; }
    }
}
