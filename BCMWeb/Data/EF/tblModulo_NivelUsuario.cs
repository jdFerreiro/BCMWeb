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
    
    public partial class tblModulo_NivelUsuario
    {
        public long IdEmpresa { get; set; }
        public long IdNivelUsuario { get; set; }
        public long IdModulo { get; set; }
        public bool Actualizar { get; set; }
        public bool Eliminar { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
        public virtual tblModulo tblModulo { get; set; }
        public virtual tblNivelUsuario tblNivelUsuario { get; set; }
    }
}
