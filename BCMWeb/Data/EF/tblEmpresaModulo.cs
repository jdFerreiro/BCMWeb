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
    
    public partial class tblEmpresaModulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblEmpresaModulo()
        {
            this.tblDispositivoEnvio1 = new HashSet<tblDispositivoEnvio1>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDispositivoEnvio1> tblDispositivoEnvio1 { get; set; }
        public virtual tblModulo tblModulo { get; set; }
    }
}
