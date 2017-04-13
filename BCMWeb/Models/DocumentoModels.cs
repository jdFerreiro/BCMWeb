using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{

    public class DocumentosModel : ModulosUserModel
    {
        public long IdDocumentoSelected { get; set; }
        public IList<DocumentoModel> Documentos { get; set; }

        public DocumentosModel()
        {
            Documentos = new List<DocumentoModel>();
        }
    }
    public class DocumentoModel : ModulosUserModel
    {
        [Display(Name = "captionDocumento", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdDocumento { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTipoDocumento", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdTipoDocumento { get; set; }
        [Display(Name = "captionNroDocumento", ResourceType = typeof(Resources.DocumentoResource))]
        public long NroDocumento { get; set; }
        [Display(Name = "captionFechaCreacion", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "captionFechaUltimaModificacion", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaUltimaModificacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEstatus", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdEstatus { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEstatus", ResourceType = typeof(Resources.DocumentoResource))]
        public string Estatus { get; set; }
        [Display(Name = "captionFechaEstatus", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaEstadoDocumento { get; set; }
        public bool Negocios { get; set; }
        [Display(Name = "captionVersion", ResourceType = typeof(Resources.DocumentoResource))]
        public int NroVersion { get; set; }
        [Display(Name = "captionVersionAnterior", ResourceType = typeof(Resources.DocumentoResource))]
        public int VersionOriginal { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionResponsable", ResourceType = typeof(Resources.DocumentoResource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public long IdPersonaResponsable { get; set; }
        [Display(Name = "captionResponsable", ResourceType = typeof(Resources.DocumentoResource))]
        public PersonaModel Responsable
        {
            get
            {
                return Metodos.GetPersonaById(IdPersonaResponsable);
            }
        }
        [Display(Name = "captionRequiereCertificacion", ResourceType = typeof(Resources.DocumentoResource))]
        public bool RequiereCertificacion { get; set; }
        public bool Editable { get; set; }
        public bool Eliminable { get; set; }
        public string RutaDocumentoPDF { get; set; }
        public string Version { get; set; }
        public bool HasVersion { get; set; }
        public List<DocumentoAnexoModel> Anexos { get; set; }
        public List<DocumentoAprobacionModel> Aprobaciones { get; set; }
        public List<DocumentoAuditoriaModel> Auditoria { get; set; }
        public List<DocumentoCertificacionModel> Certificaciones { get; set; }
        public List<DocumentoContenidoModel> Contenido { get; set; }
        public List<DocumentoEntrevistaModel> Entrevistas { get; set; }
        public List<DocumentoPersonaClaveModel> PersonasClave { get; set; }
        public List<DocumentoProcesoModel> Procesos { get; set; }
        public bool Updated { get; set; }
        public DocumentoModel()
        {
            this.Anexos = new List<DocumentoAnexoModel>();
            this.Aprobaciones = new List<DocumentoAprobacionModel>();
            this.Auditoria = new List<DocumentoAuditoriaModel>();
            this.Certificaciones = new List<DocumentoCertificacionModel>();
            this.Contenido = new List<DocumentoContenidoModel>();
            this.Entrevistas = new List< DocumentoEntrevistaModel>();
            this.PersonasClave = new List<DocumentoPersonaClaveModel>();
            this.Procesos = new List<DocumentoProcesoModel>();
            this.Updated = false;
    }

}
    public class DocumentoAnexoModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdAnexo { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
    }
    public class DocumentoAprobacionModel : ModulosUserModel
    {
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdAprobacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaAprobacion", ResourceType = typeof(Resources.DocumentoResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public Nullable<DateTime> FechaAprobacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionAprobador", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdPersona { get; set; }
        [Display(Name = "captionAprobador", ResourceType = typeof(Resources.DocumentoResource))]
        public PersonaModel Persona
        {
            get
            {
                PersonaModel _persona =  Metodos.GetPersonaById(this.IdPersona);
                if ((_persona == null || string.IsNullOrEmpty(_persona.Nombre)) && this.IdPersona > 0)
                {
                    _persona = Metodos.ConvertUsuarioToPersona(this.IdPersona);
                }
                return _persona;
            }
        }
        [Display(Name = "captionAprobado", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Aprobado { get; set; }
        [Display(Name = "captionProcesado", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Procesado { get; set; }
        public bool Responsable { get; set; }
    }
    public class DocumentoAuditoriaModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdAuditoria { get; set; }
        [Display(Name = "captionFechaRegistro", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaRegistro { get; set; }
        [Display(Name = "captionDireccionIP", ResourceType = typeof(Resources.DocumentoResource))]
        public string DireccionIP { get; set; }
        [Display(Name = "captionMensaje", ResourceType = typeof(Resources.DocumentoResource))]
        public string Mensaje { get; set; }
        [Display(Name = "captionAccion", ResourceType = typeof(Resources.DocumentoResource))]
        public string Accion { get; set; }
        [Display(Name = "captionUsuario", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdUsuario { get; set; }
        [Display(Name = "captionUsuario", ResourceType = typeof(Resources.DocumentoResource))]
        public PersonaModel Usuario { get; set; }
        public bool Negocios { get; set; }

    }
    public class DocumentoCertificacionModel : ModulosUserModel
    {
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdCertificacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaCertificacion", ResourceType = typeof(Resources.DocumentoResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime? FechaCertificacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionCertificador", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdPersona { get; set; }
        [Display(Name = "captionCertificador", ResourceType = typeof(Resources.DocumentoResource))]
        public PersonaModel Persona
        {
            get
            {
                PersonaModel _persona = Metodos.GetPersonaById(this.IdPersona);
                if ((_persona == null || string.IsNullOrEmpty(_persona.Nombre)) && this.IdPersona > 0)
                {
                    _persona = Metodos.ConvertUsuarioToPersona(this.IdPersona);
                }
                return _persona;
            }
        }
        [Display(Name = "captionCertificado", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Certificado { get; set; }
        [Display(Name = "captionProcesado", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Procesado { get; set; }
        public bool Responsable { get; set; }
    }
    public class DocumentoContenidoModel : ModulosUserModel
    {
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public byte[] Contenido { get; set; }
        [Display(Name = "captionFechaCreacion", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "captionFechaUltimaModificacion", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaUltimaModificacion { get; set; }
        public string UniqueId
        {
            get
            {
                string uId = this.IdDocumento.ToString().Trim() + this.IdModulo.ToString().Trim();
                return uId;
            }
        }
        public string DocExtension { get; set; }
        public DevExpress.XtraRichEdit.DocumentFormat docFormat
        {
            get
            {
                switch (this.DocExtension)
                {
                    case "docx":
                        return DevExpress.XtraRichEdit.DocumentFormat.OpenXml;
                    case "doc":
                        return DevExpress.XtraRichEdit.DocumentFormat.Doc;
                    case "html":
                        return DevExpress.XtraRichEdit.DocumentFormat.Html;
                    case "rtf":
                        return DevExpress.XtraRichEdit.DocumentFormat.Rtf;
                    default:
                        return DevExpress.XtraRichEdit.DocumentFormat.PlainText;
                }
            }
        }
        public bool Saved { get; set; }
        public string Accion { get; set; }
        public bool CanSave { get; set; }
    }
    public class DocumentoEntrevistaModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdEntrevista { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaInicio", ResourceType = typeof(Resources.DocumentoResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime Inicio { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionFechaFinal", ResourceType = typeof(Resources.DocumentoResource))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "InvalidoErrorFemale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public DateTime Final { get; set; }
        [Display(Name = "captionParticipantes", ResourceType = typeof(Resources.DocumentoResource))]
        public List<DocumentoEntrevistaPersonaModel> Personas { get; set; }

        public DocumentoEntrevistaModel()
        {
            Personas = new List<DocumentoEntrevistaPersonaModel>();
        }
    }
    public class DocumentoEntrevistaPersonaModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdEntrevista { get; set; }
        public long IdPersona { get; set; }
        [Display(Name = "captionEntrevistador", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Entrevistador { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreParticipante", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(1500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEmpresaParticipante", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(1500, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Empresa { get; set; }
    }
    public class DocumentoPersonaClaveModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdPersona { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombrePersonaClave", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionIdentificacionPersonaClave", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Cédula { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTelefonoOficina", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string TelefonoOficina { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTelefonoHabitacion", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string TelefonoHabitacion { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionTelefonoCelular", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string TelefonoCelular { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionEmailPersonaClave", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        [RegularExpression("^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", ErrorMessageResourceName = "RegexErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDireccion", ResourceType = typeof(Resources.DocumentoResource))]
        public string DireccionHabitacion { get; set; }
        [Display(Name = "captionPrincipalPersonaClave", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Principal { get; set; }
        public bool Responsable { get; set; }

    }
    public class DocumentoProcesoModel
    {
        [Display(Name = "captionProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdProceso { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreProceso", ResourceType = typeof(Resources.DocumentoResource))]
        [StringLength(450, ErrorMessageResourceName = "StringLengthErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource), MinimumLength = 5)]
        public string Nombre { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionDescripcionProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public string Descripcion { get; set; }
        [Display(Name = "captionNroProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public int NroProceso { get; set; }
        [Display(Name = "captionFechaCreacionProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "captionCriticoProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public bool Critico { get; set; }
        [Display(Name = "captionEstatusProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdEstatus { get; set; }
        [Display(Name = "captionEstatusProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public string Estatus
        {
            get
            {
                return Metodos.GetEstatusProceso(this.IdEstatus);
            }
        }
        [Display(Name = "captionFechaEstatusProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public DateTime FechaEstatus { get; set; }
    }
    public class DocumentoDiagrama : DocumentoModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "RequiredErrorMale", ErrorMessageResourceType = typeof(Resources.ErrorResource))]
        [Display(Name = "captionNombreProceso", ResourceType = typeof(Resources.DocumentoResource))]
        public long IdProceso { get; set; }
        public string NroProceso { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Interdependencias { get; set; }
        public string Clientes_Productos { get; set; }
        public string Entradas { get; set; }
        public string Proveedores { get; set; }
        public string PersonalClave { get; set; }
        public string Tecnologia { get; set; }
    }
}