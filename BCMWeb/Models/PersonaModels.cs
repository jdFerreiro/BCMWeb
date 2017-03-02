using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class PersonaModel
    {
        public long IdPersona { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public long IdUnidadOrganizativa { get; set; }
        public string UnidadOrganizativa { get; set; }
        public long IdCargo { get; set; }
        public string Cargo { get; set; }
        public long IdUsuario { get; set; }
        public List<PersonaEmail> CorreosElectronicos { get; set; }
        public List<PersonaDireccion> Direcciones { get; set; }
        public List<PersonaTelefono> Telefonos { get; set; }
    }
    public class PersonaEmail
    {
        public long IdPersonaEmail { get; set; }
        public string Email { get; set; }
        public long IdTipoEmail { get; set; }
        public string TipoEmail { get; set; }

    }
    public class PersonaDireccion
    {
        public long IdPersonaDireccion { get; set; }
        public long IdTipoDireccion { get; set; }
        public string TipoDireccion { get; set; }
        public string CalleAvenida { get; set; }
        public string EdificioCasa { get; set; }
        public string PisoNivel { get; set; }
        public string TorreAla { get; set; }
        public string Urbanizacion { get; set; }
        public long IdPais { get; set; }
        public string Pais { get; set; }
        public long IdEstado { get; set; }
        public string Estado { get; set; }
        public long IdCiudad { get; set; }
        public string Ciudad { get; set; }

    }
    public class PersonaTelefono
    {
        public long IdPersonaTelefono { get; set; }
        public long IdTipoTelefono { get; set; }
        public string TipoTelefono { get; set; }
        public int CodigoArea { get; set; }
        public int NroTelefono { get; set; }
        public int Extension1 { get; set; }
        public int Extension2 { get; set; }
        public string TelefonoCompleto
        {
            get
            {
                return string.Format("{0}{1}", CodigoArea.ToString().PadLeft(4, '0'), NroTelefono.ToString().PadLeft(6, '0'));
            }
        }
    }
}