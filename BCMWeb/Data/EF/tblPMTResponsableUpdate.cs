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
