using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class AdministracionModel : ModulosUserModel
    {
        public long IdModuloActualiza { get; set; }
        public long IdEmpresaSelected { get; set; }
        public IList<ModuloModel> Modulos { get; set; }
    }
    public class AuditoriaAdminModel : ModulosUserModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public long? IdUsuario { get; set; }
        public TablasGeneralesModel Usuarios { get; set; }
        public IQueryable<AuditoriaModel> Data { get; set; }
    }
}