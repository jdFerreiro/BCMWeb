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
    
    public partial class tblModulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblModulo()
        {
            this.tblModulo_NivelUsuario = new HashSet<tblModulo_NivelUsuario>();
            this.tblPMTMensajeActualizacion = new HashSet<tblPMTMensajeActualizacion>();
            this.tblPMTResponsableUpdate = new HashSet<tblPMTResponsableUpdate>();
        }
    
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public Nullable<int> IdCodigoModulo { get; set; }
        public long IdModuloPadre { get; set; }
        public short IdTipoElemento { get; set; }
        public string Nombre { get; set; }
        public string Accion { get; set; }
        public string Controller { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string imageRoot { get; set; }
        public bool Activo { get; set; }
        public bool Negocios { get; set; }
        public bool Tecnologia { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblEmpresaModulo tblEmpresaModulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblModulo_NivelUsuario> tblModulo_NivelUsuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPMTMensajeActualizacion> tblPMTMensajeActualizacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPMTResponsableUpdate> tblPMTResponsableUpdate { get; set; }
    }
}
