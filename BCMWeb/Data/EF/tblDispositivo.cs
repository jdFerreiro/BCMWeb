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
    
    public partial class tblDispositivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblDispositivo()
        {
            this.tblDispositivoConexion = new HashSet<tblDispositivoConexion>();
            this.tblDispositivoEnvio = new HashSet<tblDispositivoEnvio>();
        }
    
        public long IdDispositivo { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
        public string IdUnicoDispositivo { get; set; }
        public string nombre { get; set; }
        public string fabricante { get; set; }
        public string modelo { get; set; }
        public string plataforma { get; set; }
        public string version { get; set; }
        public string tipo { get; set; }
        public string token { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDispositivoConexion> tblDispositivoConexion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblDispositivoEnvio> tblDispositivoEnvio { get; set; }
    }
}
