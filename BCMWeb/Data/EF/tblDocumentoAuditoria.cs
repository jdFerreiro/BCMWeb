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
    
    public partial class tblDocumentoAuditoria
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdAuditoria { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string DireccionIP { get; set; }
        public string Mensaje { get; set; }
        public string Accion { get; set; }
        public long IdUsuario { get; set; }
        public bool Negocios { get; set; }
    
        public virtual tblDocumento tblDocumento { get; set; }
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblUsuario tblUsuario { get; set; }
    }
}
