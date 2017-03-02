using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class DocumentosController : Controller
    {
        public ActionResult Ficha(long modId)
        {

            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;
            Session["IdTipoDocumento"] = IdTipoDocumento;
            long IdDocumento  = long.Parse(Session["IdDocumento"].ToString());
            int  IdClaseDocumento = int.Parse(Session["IdClaseDocumento"].ToString());
            int IdVersion  = int.Parse(Session["IdVersion"].ToString());

            DocumentoModel Documento = new DocumentoModel();
            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            if (IdDocumento == -1)
            {
                Documento = new DocumentoModel
                {
                    Anexos = new List<DocumentoAnexoModel>(),
                    Aprobaciones = new List<DocumentoAprobacionModel>(),
                    Auditoria = new List<DocumentoAuditoriaModel>(),
                    Certificaciones = new List<DocumentoCertificacionModel>(),
                    Contenido = new List<DocumentoContenidoModel>(),
                    Entrevistas = new List<DocumentoEntrevistaModel>(),
                    Estatus = "Cargando",
                    FechaCreacion = DateTime.UtcNow,
                    FechaEstadoDocumento = DateTime.UtcNow,
                    FechaUltimaModificacion = DateTime.UtcNow,
                    IdDocumento = IdDocumento,
                    IdEstatus = 1,
                    IdPersonaResponsable = 0,
                    NroDocumento = Metodos.GetNextNroDocumento(IdClaseDocumento, IdTipoDocumento),
                    NroVersion = Metodos.GetNextVersion(IdClaseDocumento, IdTipoDocumento, IdVersion),
                    PersonasClave = new List<DocumentoPersonaClaveModel>(),
                    RequiereCertificacion = true,
                    VersionOriginal = IdVersion
                };

            }
            else
            {
                Documento = Metodos.GetDocumento(IdDocumento, IdTipoDocumento);
            }

            Documento.Negocios = (IdClaseDocumento == 1);
            Documento.IdTipoDocumento = IdTipoDocumento;

            DocumentosModel model = new DocumentosModel
            {
                Documentos = (new List<DocumentoModel>()),
                EditDocumento = true,
                IdClaseDocumento = IdClaseDocumento,
                IdDocumentoSelected = 0,
                IdEmpresa = long.Parse(Session["IdEmpresa"].ToString()),
                IdModulo = IdModulo
            };

            model.Documentos.Add(Documento);
            return View(model);
        }
    }
}