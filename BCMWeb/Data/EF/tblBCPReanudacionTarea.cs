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
    
    public partial class tblBCPReanudacionTarea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBCPReanudacionTarea()
        {
            this.tblBCPReanudacionTareaActividad = new HashSet<tblBCPReanudacionTareaActividad>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdDocumentoBCP { get; set; }
        public long IdBCPReanudacionTarea { get; set; }
        public string Nombre { get; set; }
    
        public virtual tblBCPDocumento tblBCPDocumento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBCPReanudacionTareaActividad> tblBCPReanudacionTareaActividad { get; set; }
    }
}
