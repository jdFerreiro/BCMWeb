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
    
    public partial class tblPMTFrecuenciaParticipante
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdFrecuencia { get; set; }
        public long IdCargo { get; set; }
    
        public virtual tblCargo tblCargo { get; set; }
        public virtual tblPMTFrecuencia tblPMTFrecuencia { get; set; }
    }
}
