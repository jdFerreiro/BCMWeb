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
    
    public partial class tblModuloAnexo
    {
        public long IdEmpresa { get; set; }
        public long IdModulo { get; set; }
        public bool Negocios { get; set; }
        public long IdAnexo { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> IsFolder { get; set; }
        public byte[] Data { get; set; }
        public Nullable<System.DateTime> LastWriteTime { get; set; }
    
        public virtual tblEmpresa tblEmpresa { get; set; }
    }
}
