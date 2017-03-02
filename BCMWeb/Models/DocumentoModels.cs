using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCMWeb.Models
{

    public class DocumentosModel : ModulosUserModel
    {
        public long IdDocumentoSelected { get; set; }
        public List<DocumentoModel> Documentos { get; set; }
    }
    public class DocumentoModel 
    {
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long NroDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public long IdEstatus { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaEstadoDocumento { get; set; }
        public bool Negocios { get; set; }
        public int NroVersion { get; set; }
        public int VersionOriginal { get; set; }
        public long IdPersonaResponsable { get; set; }
        public PersonaModel Responsable
        {
            get
            {

            }
        }
        public bool RequiereCertificacion { get; set; }
        public IEnumerable<DocumentoAnexoModel> Anexos { get; set; }
        public IEnumerable<DocumentoAprobacionModel> Aprobaciones { get; set; }
        public IEnumerable<DocumentoAuditoriaModel> Auditoria { get; set; }
        public IEnumerable<DocumentoCertificacionModel> Certificaciones { get; set; }
        public IEnumerable<DocumentoContenidoModel> Contenido { get; set; }
        public IEnumerable<DocumentoEntrevistaModel> Entrevistas { get; set; }
        public IEnumerable<DocumentoPersonaClaveModel> PersonasClave { get; set; }
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
    public class DocumentoAprobacionModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdAprobacion { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public long IdPersona { get; set; }
        public bool Aprobado { get; set; }
        public bool Procesado { get; set; }
    }
    public class DocumentoAuditoriaModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdAuditoria { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DireccionIP { get; set; }
        public string Mensaje { get; set; }
        public string Accion { get; set; }
        public long IdUsuario { get; set; }
        public bool Negocios { get; set; }

    }
    public class DocumentoCertificacionModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdCertificacion { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public long IdPersona { get; set; }
        public bool Certificado { get; set; }
        public bool Procesado { get; set; }
    }
    public class DocumentoContenidoModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdModulo { get; set; }
        public byte[] Contenido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
    }
    public class DocumentoEntrevistaModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdEntrevista { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Final { get; set; }
        public IEnumerable<DocumentoEntrevistaPersonaModel> Personas { get; set; }
    }
    public class DocumentoEntrevistaPersonaModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdEntrevista { get; set; }
        public long IdPersona { get; set; }
        public bool Entrevistador { get; set; }
        public string Nombre { get; set; }
        public string Empresa { get; set; }
    }
    public class DocumentoPersonaClaveModel
    {
        public long IdEmpresa { get; set; }
        public long IdDocumento { get; set; }
        public long IdTipoDocumento { get; set; }
        public long IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Cédula { get; set; }
        public string TelefonoOficina { get; set; }
        public string TelefonoHabitacion { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
        public string DireccionHabitacion { get; set; }
        public bool Principal { get; set; }
    }
}