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
    
    public partial class tblPMTProgramacionDocumentos
    {
        public long IdPMTProgramacion { get; set; }
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
    
        public virtual tblDocumento tblDocumento { get; set; }
        public virtual tblModulo tblModulo { get; set; }
        public virtual tblPMTProgramacion tblPMTProgramacion { get; set; }
    }
}
