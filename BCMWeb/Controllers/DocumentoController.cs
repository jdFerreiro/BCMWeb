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
        [SessionExpire]
        public ActionResult Index(long IdModulo)
        {

            string _IdModulo = IdModulo.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));

            Session["modId"] = IdModulo;

            DocumentosModel model = new DocumentosModel();
            model.EditDocumento = false;
            model.IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            model.IdDocumentoSelected = 0;
            model.IdClaseDocumento = 1;
            model.IdModulo = IdModulo;
            model.IdModuloActual = IdModulo;
            model.Documentos = Metodos.GetDocumentosModulo(IdTipoDocumento, (model.IdClaseDocumento == 1));
            model.ModulosPrincipales = Metodos.GetModulosPrincipalesEmpresaUsuario();
            model.PageTitle = "Relación de Documentos";

            Auditoria.RegistarAccion(eTipoAccion.AccesoModuloWeb);

            return View(model);
        }
        [HttpPost]
        [SessionExpire]
        public ActionResult Index(DocumentosModel model)
        {
            string _IdModulo = model.IdModulo.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));
            model.Documentos = Metodos.GetDocumentosModulo(IdTipoDocumento, (model.IdClaseDocumento == 1));
            model.PageTitle = "Relación de Documentos";

            return View(model);
        }
        [SessionExpire]
        public ActionResult EditarDocumento(long IdDocumento, int IdClaseDocumento, long IdModulo, int IdVersion = 0)
        {

            Session["IdDocumento"] = IdDocumento;
            Session["IdClaseDocumento"] = IdClaseDocumento;
            Session["IdVersion"] = IdVersion;
            Session["OnlyVisible"] = false;

            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            return RedirectToAction(firstModulo.Action, firstModulo.Controller, new { modId = firstModulo.IdModulo });

        }
        [SessionExpire]
        public ActionResult EliminarDocumento(long IdDocumento, long IdModulo)
        {
            string _IdModulo = IdModulo.ToString();
            int IdTipoDocumento = int.Parse(_IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2)));

            Metodos.EliminarDocumento(IdDocumento, IdTipoDocumento);
            return RedirectToAction("Index", new { IdModulo = IdModulo });
        }
        [SessionExpire]
        public ActionResult VerDocumento(long IdDocumento, int IdClaseDocumento, long IdModulo, int IdVersion = 0)
        {

            Session["IdDocumento"] = IdDocumento;
            Session["IdClaseDocumento"] = IdClaseDocumento;
            Session["IdVersion"] = IdVersion;
            Session["OnlyVisible"] = true;

            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            return RedirectToAction(firstModulo.Action, firstModulo.Controller, new { modId = firstModulo.IdModulo });

        }
        public ActionResult NuevaVersionDocumento(long IdDocumento, int IdClaseDocumento, long IdModulo, int IdVersionActual, int NroVersion)
        {
            int IdVersion = NroVersion++;
            string _IdModulo = IdModulo.ToString();
            Session["IdTipoDocumento"] = _IdModulo.Substring(0, (_IdModulo.Length == 7 ? 1 : 2));
            long IdNuevoDocumento = Metodos.GenerarNuevaVersionDocumento(IdDocumento, IdVersionActual, NroVersion);

            return RedirectToAction("EditarDocumento", new { IdDocumento = IdNuevoDocumento, IdClaseDocumento, IdModulo, IdVersion });
        }
    }
}   