using BCMWeb.Models;
using System;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class PlanTrabajoController : Controller
    {
        [SessionExpire]
        [HandleError]
        public ActionResult Index(long IdModulo)
        {
            FirstModuloSelected firstModulo = Metodos.GetFirstUserModulo(IdModulo);

            return RedirectToAction(firstModulo.Action, firstModulo.Controller, new { modId = firstModulo.IdModulo });
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Iniciativas(long modId)
        {

            Session["modId"] = modId;

            IniciativaModel model = new IniciativaModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Auditoria.RegistarAccion(eTipoAccion.Mostrar);
            Session["GridViewData"] = Metodos.GetIniciativas();
            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult ExportIniciativas(IniciativaModel model)
        {
            string _modId = Session["modId"] .ToString();
            long modId = long.Parse(_modId);
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);

            Session["GridViewData"] = Metodos.GetIniciativas();
            return GridViewExportIniciativas.FormatConditionsExportFormatsInfo[GridViewExportFormat.Xlsx](GridViewExportIniciativas.FormatConditionsExportGridViewSettings, Metodos.GetIniciativas());

        }
        [SessionExpire]
        [HandleError]
        public ActionResult IniciativaPartialView()
        {
            Session["GridViewData"] = Metodos.GetIniciativas();
            return PartialView("IniciativaPartialView", Metodos.GetIniciativas());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIniciativaAddNewPartial(IniciativaModel Iniciativa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Metodos.InsertIniciativa(Iniciativa);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = Resources.ErrorPageResource.AllErrors;
                ViewData["EditableIniciativa"] = Iniciativa;
            }
            Session["GridViewData"] = Metodos.GetIniciativas();
            return PartialView("IniciativaPartialView", Metodos.GetIniciativas());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIniciativaUpdatePartial(IniciativaModel Iniciativa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Metodos.UpdateIniciativa(Iniciativa);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = Resources.ErrorPageResource.AllErrors;
                ViewData["EditableIniciativa"] = Iniciativa;
            }

            Session["GridViewData"] = Metodos.GetIniciativas();
            return PartialView("IniciativaPartialView", Metodos.GetIniciativas());
        }
        [SessionExpire]
        [HandleError]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIniciativaDeletePartial(int IdIniciativa)
        {
            if (IdIniciativa > 0)
            {
                try
                {
                    Metodos.DeleteIniciativa(IdIniciativa);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            Session["GridViewData"] = Metodos.GetIniciativas();
            return PartialView("IniciativaPartialView", Metodos.GetIniciativas());
        }
    }
}