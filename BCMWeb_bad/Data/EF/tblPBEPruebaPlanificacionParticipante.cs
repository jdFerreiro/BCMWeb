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
    
    public partial class tblPBEPruebaPlanificacionParticipante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPBEPruebaPlanificacionParticipante()
        {
            this.tblPBEPruebaPlanificacionEjercicioParticipante = new HashSet<tblPBEPruebaPlanificacionEjercicioParticipante>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdPlanificacion { get; set; }
        public long IdParticipante { get; set; }
        public string Empresa { get; set; }
        public string Nombre { get; set; }
        public bool Responsable { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
    
        public virtual tblPBEPruebaPlanificacion tblPBEPruebaPlanificacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPBEPruebaPlanificacionEjercicioParticipante> tblPBEPruebaPlanificacionEjercicioParticipante { get; set; }
    }
}