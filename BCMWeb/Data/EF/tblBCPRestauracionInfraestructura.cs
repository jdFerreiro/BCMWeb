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
    
    public partial class tblBCPRestauracionInfraestructura
    {
        public long IdEmpresa { get; set; }
        public long IdDocumentoBCP { get; set; }
        public long IdBCPRestauracionInfraestructura { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
    
        public virtual tblBCPDocumento tblBCPDocumento { get; set; }
    }
}
