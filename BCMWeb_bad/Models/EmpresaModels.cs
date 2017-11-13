using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class EmpresaModel
    {
        public long IdEmpresa { get; set; }
        public string NombreFiscal { get; set; }
        public string NombreComercial { get; set; }
        public string RegistroFiscal { get; set; }
        public string Direccion { get; set; }
        public long IdEstatus{ get; set; }
        public DateTime FechaUltimoEstado { get; set; }
        public string LogoUrl { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}