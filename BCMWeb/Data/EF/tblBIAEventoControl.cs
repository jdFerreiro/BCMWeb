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
    
    public partial class tblBIAEventoControl
    {
        public long IdEmpresa { get; set; }
        public long IdDocumentoBIA { get; set; }
        public long IdProceso { get; set; }
        public long IdAmenaza { get; set; }
        public long IdAmenazaEvento { get; set; }
        public long IdEventoControl { get; set; }
        public string Descripcion { get; set; }
        public bool Implantado { get; set; }
    
        public virtual tblBIAAmenazaEvento tblBIAAmenazaEvento { get; set; }
    }
}
