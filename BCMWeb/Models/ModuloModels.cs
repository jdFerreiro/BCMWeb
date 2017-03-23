using BCMWeb.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{

   public class ModulosUserModel
    {
        public long IdEmpresa { get; set; }
        public int IdClaseDocumento { get; set; }
        public long IdModulo { get; set; }
        public bool EditDocumento { get; set; }
        public long IdModuloActual { get; set; }
        public string ModuloActual
        {
            get
            {
                if (IdModuloActual > 0)
                    return Metodos.GetNombreModulo(IdModuloActual);
                return string.Empty;
            }
        }
        public IList<ModuloModel> ModulosPrincipales { get; set; }
        public SubModulosData SubModulos
        {
            get
            {
                if (this.IdModulo.ToString().StartsWith("99"))
                {
                    return new SubModulosData(this.IdModuloActual);
                }
                else
                {
                    return new SubModulosData(this.IdModulo);
                }
            }
        }
        public string returnPage { get; set; }
        public string PageTitle { get; set; }
        public string UserTimezone { get; set; }
    }
   public class ModuloModel
    {
        private long _IdModuloPadre;
        private List<ModuloModel> _SubModulos = new List<ModuloModel>();

        public long IdModulo { get; set; }
        public long IdModuloPadre
        {
            get
            {
                return _IdModuloPadre;
            }
            set
            {
                _IdModuloPadre = value;
                if (_IdModuloPadre > 0)
                {
                    using (Entities db = new Entities())
                    {
                        _SubModulos = (from m in db.tblModulo
                                       where m.IdModuloPadre == _IdModuloPadre
                                       select new ModuloModel
                                       {
                                           Action = m.Accion,
                                           Activo = m.Activo,
                                           CodigoModulo = (int)m.IdCodigoModulo,
                                           Controller = m.Controller,
                                           Descripcion = m.Descripcion,
                                           IdModulo = m.IdModulo,
                                           IdModuloPadre = m.IdModuloPadre,
                                           IdTipoElemento = m.IdTipoElemento,
                                           ImageRoot = m.imageRoot,
                                           Negocios = m.Negocios,
                                           Nombre = m.Nombre,
                                           Tecnologia = m.Tecnologia,
                                           Titulo = m.Titulo
                                       }).ToList();
                    }
                }
            }
        }
        public int CodigoModulo { get; set; }
        public short IdTipoElemento { get; set; }
        public string Nombre { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string ImageRoot { get; set; }
        public bool Activo { get; set; }
        public bool Negocios { get; set; }
        public bool Tecnologia { get; set; }
        public List<ModuloModel> SubModulos
        {
            get
            {
                return _SubModulos;
            }
        }
    }
}