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
    
    public partial class tblBCPRespuestaAccion
    {
        public long IdEmpresa { get; set; }
        public long IdDocumentoBCP { get; set; }
        public long IdBCPRespuestaAccion { get; set; }
        public long IdPersona { get; set; }
        public short NivelEscala { get; set; }
        public string NombreEscala { get; set; }
    
        public virtual tblBCPDocumento tblBCPDocumento { get; set; }
    }
}
