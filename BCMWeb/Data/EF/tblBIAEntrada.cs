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
    
    public partial class tblBIAEntrada
    {
        public long IdEmpresa { get; set; }
        public long IdDocumentoBIA { get; set; }
        public long IdProceso { get; set; }
        public long IdEntrada { get; set; }
        public string Unidad { get; set; }
        public string Evento { get; set; }
        public string Responsable { get; set; }
    
        public virtual tblBIAProceso tblBIAProceso { get; set; }
    }
}
