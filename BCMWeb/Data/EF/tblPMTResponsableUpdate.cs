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
    
    public partial class tblPMTResponsableUpdate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPMTResponsableUpdate()
        {
            this.tblPMTResponsableUpdate_Correo = new HashSet<tblPMTResponsableUpdate_Correo>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public long IdMensaje { get; set; }
        public System.DateTime FechaActualizar { get; set; }
        public System.DateTime FechaUltimoEnvio { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblModulo tblModulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPMTResponsableUpdate_Correo> tblPMTResponsableUpdate_Correo { get; set; }
    }
}
