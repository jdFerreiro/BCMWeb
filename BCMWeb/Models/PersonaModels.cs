﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{
    public class PersonaModel
    {
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.FichaResource))]
        public long IdPersona { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.FichaResource) )]
        [StringLength(250, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombre", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(50, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Identificacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionUO", ResourceType = typeof(Resources.FichaResource))]
        public long IdUnidadOrganizativa { get; set; }
        [Display(Name = "captionUO", ResourceType = typeof(Resources.FichaResource))]
        public string UnidadOrganizativa { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionCargo", ResourceType = typeof(Resources.FichaResource))]
        public long IdCargo { get; set; }
        [Display(Name = "captionCargo", ResourceType = typeof(Resources.FichaResource))]
        public string Cargo { get; set; }
        public long IdUsuario { get; set; }
        public IList<PersonaEmail> CorreosElectronicos { get; set; }
        public IList<PersonaDireccion> Direcciones { get; set; }
        public IList<PersonaTelefono> Telefonos { get; set; }

        public PersonaModel()
        {
            this.CorreosElectronicos = new List<PersonaEmail>();
            this.Direcciones = new List<PersonaDireccion>();
            this.Telefonos = new List<PersonaTelefono>();
        }
    }
    public class PersonaEmail
    {
        [Display(Name = "captionEmail", ResourceType = typeof(Resources.FichaResource))]
        public long IdPersonaEmail { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEmail", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        [RegularExpression("^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessageResourceName = "InvalidoErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTipoEmail", ResourceType = typeof(Resources.FichaResource))]
        public long IdTipoEmail { get; set; }
        [Display(Name = "captionTipoEmail", ResourceType = typeof(Resources.FichaResource))]
        public string TipoEmail { get; set; }

    }
    public class PersonaDireccion
    {
        [Display(Name = "captionIdPersonaDireccion", ResourceType = typeof(Resources.FichaResource))]
        public long IdPersonaDireccion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTipoDireccion", ResourceType = typeof(Resources.FichaResource))]
        public long IdTipoDireccion { get; set; }
        [Display(Name = "captionTipoDireccion", ResourceType = typeof(Resources.FichaResource))]
        public string TipoDireccion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionCalleAvenida", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string CalleAvenida { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEdificioCasa", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string EdificioCasa { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionPisoNivel", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string PisoNivel { get; set; }
        [Display(Name = "captionTorreAla", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string TorreAla { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionUrbanizacion", ResourceType = typeof(Resources.FichaResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Urbanizacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionPais", ResourceType = typeof(Resources.FichaResource))]
        public long IdPais { get; set; }
        [Display(Name = "captionPais", ResourceType = typeof(Resources.FichaResource))]
        public string Pais
        {
            get
            {
                return Metodos.GetPais(this.IdPais);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEstado", ResourceType = typeof(Resources.FichaResource))]
        public long IdEstado { get; set; }
        [Display(Name = "captionEstado", ResourceType = typeof(Resources.FichaResource))]
        public string Estado
        {
            get
            {
                return Metodos.GetEstado(this.IdPais, this.IdEstado);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionCiudad", ResourceType = typeof(Resources.FichaResource))]
        public long IdCiudad { get; set; }
        [Display(Name = "captionCiudad", ResourceType = typeof(Resources.FichaResource))]
        public string Ciudad
        {
            get
            {
                return Metodos.GetCiudad(this.IdPais, this.IdEstado, this.IdCiudad);
            }
        }


    }
    public class PersonaTelefono
    {
        [Display(Name = "captionIdTelefono", ResourceType = typeof(Resources.FichaResource))]
        public long IdPersonaTelefono { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTipoTelefono", ResourceType = typeof(Resources.FichaResource))]
        public long IdTipoTelefono { get; set; }
        [Display(Name = "captionTipoTelefono", ResourceType = typeof(Resources.FichaResource))]
        public string TipoTelefono { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionCodigoAreaTelefono", ResourceType = typeof(Resources.FichaResource))]
        public int CodigoArea { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNroTelefono", ResourceType = typeof(Resources.FichaResource))]
        public int NroTelefono { get; set; }
        [Display(Name = "captionNroExtension", ResourceType = typeof(Resources.FichaResource))]
        public int Extension1 { get; set; }
        [Display(Name = "captionNroExtension", ResourceType = typeof(Resources.FichaResource))]
        public int Extension2 { get; set; }
        [Display(Name = "captionNroTelefono", ResourceType = typeof(Resources.FichaResource))]
        public string TelefonoCompleto
        {
            get
            {
                return string.Format("{0}{1}", CodigoArea.ToString().PadLeft(4, '0'), NroTelefono.ToString().PadLeft(6, '0'));
            }
        }
    }
    public class UnidadOrganizativaModel
    {
        public long IdUnidad { get; set; }
        public long IdUnidadPadre { get; set; }
        public string Nombre { get; set; }
    }
    public class CargoModel
    {
        public long IdCargo { get; set; }
        public string Nombre { get; set; }
    }
}