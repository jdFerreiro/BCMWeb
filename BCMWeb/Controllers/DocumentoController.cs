using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class DocumentoController : Controller
    {
        // GET: Documento
        public ActionResult Index(long IdModulo)
        {

            string _IdModulo = IdModulo.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));

            DocumentosModel model = new DocumentosModel();
            model.EditDocumento = false;
            model.IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            model.IdDocumentoSelected = 0;
            model.IdClaseDocumento = 1;
            model.IdModulo = IdModulo;
            model.Documentos = Metodos.GetDocumentosModulo(IdTipoDocumento, (model.IdClaseDocumento == 1));
            model.ModulosPrincipales = Metodos.GetModulosPrincipalesEmpresaUsuario();

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(DocumentosModel model)
        {
            string _IdModulo = model.IdModulo.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));
            model.Documentos = Metodos.GetDocumentosModulo(IdTipoDocumento, (model.IdClaseDocumento == 1));
            return View(model);
        }
        public ActionResult EditarDocumento(long IdDocumento, int IdClaseDocumento, long IdModulo, int IdVersion = 0)
        {

            Session["IdDocumento"] = IdDocumento;
            Session["IdClaseDocumento"] = IdClaseDocumento;
            Session["IdVersion"] = IdVersion;

            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            return RedirectToAction(firstModulo.Action, firstModulo.Controller, new { modId = firstModulo.IdModulo });

        }
        public ActionResult EliminarDocumento(long IdDocumento, long IdModulo)
        {
            string _IdModulo = IdModulo.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));

            Metodos.EliminarDocumento(IdDocumento, IdTipoDocumento);
            return View("Index", new { IdModulo });
        }
    }
}