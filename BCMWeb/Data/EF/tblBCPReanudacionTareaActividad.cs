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
    
    public partial class tblBCPReanudacionTareaActividad
    {
        public long IdEmpresa { get; set; }
        public long IdDocumentoBCP { get; set; }
        public long IdBCPReanudacionTarea { get; set; }
        public long IdBCPReanudacionTareaActividad { get; set; }
        public string Descripcion { get; set; }
        public bool Procesado { get; set; }
    
        public virtual tblBCPReanudacionTarea tblBCPReanudacionTarea { get; set; }
    }
}
