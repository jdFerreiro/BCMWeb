using BCMWeb.APIModels;
using BCMWeb.Data.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BCMWeb.APIController
{
    [Authorize]
    [RoutePrefix("api/documento")]
    public class tblDocumentoController : ApiController
    {
        private Entities db = new Entities();
        private string Culture;

        public tblDocumentoController()
        {
            string[] _userLanguages = HttpContext.Current.Request.UserLanguages;
            Culture = (_userLanguages == null || _userLanguages.Count() == 0 ? "es-VE" : _userLanguages[0]);
        }

        [Route("getall")]
        [HttpGet]
        public async Task<IList<DocumentoModel>> getall()
        {
            List<DocumentoModel> _documentos = new List<DocumentoModel>();
            List<tblDocumento> docs = await db.tblDocumento.ToListAsync();
            _documentos = docs.AsEnumerable()
                .Select(x => new DocumentoModel
                {
                    Id = x.IdDocumento,
                    IdTipoDocumento = x.IdTipoDocumento,
                    Negocios = x.Negocios,
                    NroDocumento = x.NroDocumento,
                    NroVersion = x.NroVersion,
                    PdfRoute = GetRouteFile(x),
                    TipoDocumento = db.tblModulo.FirstOrDefault(m => m.IdEmpresa == x.IdEmpresa
                                        && m.IdCodigoModulo == x.IdTipoDocumento
                                        && m.IdModuloPadre == 0).Nombre,
                    VersionOriginal = x.VersionOriginal ?? 1,
                })
                .ToList();

            return _documentos.Where(x => !string.IsNullOrEmpty(x.PdfRoute)).ToList();
        }

        [Route("getbyuser/{id:long}/{idEmpresa:long}")]
        [HttpGet]
        public async Task<IList<DocumentoModel>> getbyuser(long id, long idEmpresa)
        {
            List<DocumentoModel> _documentos = new List<DocumentoModel>();
            List<tblDocumento> docs = await db.tblDocumento
                .Where(x => x.IdPersonaResponsable == id || x.tblDocumentoCertificacion.Where(cx => cx.tblPersona.IdUsuario == id).Count() > 0)
                .ToListAsync();

            _documentos = docs.AsEnumerable()
                .Select(x => new DocumentoModel
                {
                    Id = x.IdDocumento,
                    IdTipoDocumento = x.IdTipoDocumento,
                    Negocios = x.Negocios,
                    NroDocumento = x.NroDocumento,
                    NroVersion = x.NroVersion,
                    PdfRoute = GetRouteFile(x),
                    TipoDocumento = db.tblModulo.FirstOrDefault(m => m.IdEmpresa == x.IdEmpresa
                                        && m.IdCodigoModulo == x.IdTipoDocumento
                                        && m.IdModuloPadre == 0).Nombre,
                    VersionOriginal = x.VersionOriginal ?? 1,
                })
                .ToList();

            return _documentos.Where(x => !string.IsNullOrEmpty(x.PdfRoute)).ToList();
        }
        private string GetRouteFile(tblDocumento x)
        {
            eSystemModules Modulo = (eSystemModules)x.IdTipoDocumento;
            eEstadoDocumento EstadoDocumento = (eEstadoDocumento)x.IdEstadoDocumento;

            string _CodigoInforme = string.Format("{0}_{1}_{2}_{3}.{4}", Modulo.ToString(), x.NroDocumento.ToString("#000"), (EstadoDocumento == eEstadoDocumento.Certificado ? x.FechaEstadoDocumento.ToString("MM-yyyy") : DateTime.Now.ToString("MM-yyyy")), x.VersionOriginal ?? 1, x.NroVersion);
            string _FileName = string.Format("{0}.pdf", _CodigoInforme.Replace("-", "_"));
            string _pathFile = string.Format("https:\\www.bcmweb.net\\PDFDocs\\{1}", _FileName);

            if (!File.Exists(_pathFile))
                _pathFile = string.Empty;

            return _pathFile;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}