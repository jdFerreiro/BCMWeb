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
    
    public partial class tblPBEPruebaEjecucionRecurso
    {
        public long IdEmpresa { get; set; }
        public long IdPlanificacion { get; set; }
        public long IdEjercicio { get; set; }
        public long IdRecurso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public string Responsable { get; set; }
    
        public virtual tblPBEPruebaEjecucionEjercicio tblPBEPruebaEjecucionEjercicio { get; set; }
    }
}
