using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class AdministracionController : Controller
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
        public ActionResult ActualizaTablas(long modId)
        {

            Session["modId"] = modId;

            AdministracionModel model = new AdministracionModel();
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            long IdModulo = IdTipoDocumento * 1000000;

            model.IdModulo = IdModulo;
            model.IdModuloActual = modId;
            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(modId);
            model.Modulos = Metodos.GetModulosPrincipalesEmpresaUsuario(model.IdEmpresa);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            Auditoria.RegistarAccion(eTipoAccion.Actualizar);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult ActualizaTablas(AdministracionModel model)
        {

            string _modId = model.IdModuloActual.ToString();
            int IdTipoDocumento = int.Parse(_modId.Substring(0, (_modId.Length == 7 ? 1 : 2)));

            model.Perfil = Metodos.GetPerfilData();
            model.PageTitle = Metodos.GetModuloName(model.IdModuloActual);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            Auditoria.RegistarAccion(eTipoAccion.Actualizar);
            model.Modulos = Metodos.GetModulosPrincipalesEmpresaUsuario(model.IdEmpresaSelected);
            if (model.IdModuloActualiza > 0)
                Metodos.ProcesarDocumentosModulo(model.IdEmpresaSelected, model.IdModuloActualiza);

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ComboBoxEmpresa(long IdEmpresa)
        {
            AdministracionModel model = new AdministracionModel();
            model.IdEmpresaSelected = IdEmpresa;
            return PartialView(model);
        }
        public ActionResult ComboBoxModulo(long IdEmpresa, long IdModulo)
        {
            AdministracionModel model = new AdministracionModel();
            model.IdEmpresa = IdEmpresa;
            model.IdModuloActualiza = IdModulo;
            return PartialView(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Auditar(long modId)
        {
            string _modId = modId.ToString();
            int IdTipoDocumento = int.Parse(_modId.Length == 7 ? _modId.Substring(0, 1) : _modId.Substring(0, 2));
            long IdModulo = IdTipoDocumento * 1000000;

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            AuditoriaAdminModel model = new AuditoriaAdminModel();
            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            model.IdModulo = IdModulo;
            model.Perfil = Metodos.GetPerfilData();
            model.IdModuloActual = modId;
            model.FechaDesde = DateTime.MinValue.AddYears(99);
            model.FechaHasta = DateTime.UtcNow.AddMinutes(Minutos);
            model.Data = Metodos.GetAuditoria(model.FechaDesde
                                                , model.FechaHasta
                                                , (model.IdUsuario < 0 ? null : model.IdUsuario));
            model.FechaDesde = model.Data.Min(x => x.FechaRegistro);
            model.IdUsuario = -1;

            Session["Data"] = model.Data;
            Session["IdModulo"] = modId;
            Session["IdUsuarioAuditoria"] = model.IdUsuario;

            return View(model);
        }
        [SessionExpire]
        [HandleError]
        [HttpPost]
        public ActionResult Auditar(AuditoriaAdminModel model)
        {
            string _modId = model.IdModuloActual.ToString();
            int IdTipoDocumento = int.Parse(_modId.Length == 7 ? _modId.Substring(0, 1) : _modId.Substring(0, 2));
            long IdModulo = IdTipoDocumento * 1000000;

            string UserTimeZone = Session["UserTimeZone"].ToString();
            int Horas = int.Parse(UserTimeZone.Split(':').First());
            int Minutos = (Math.Abs(Horas) * 60) + int.Parse(UserTimeZone.Split(':').Last());
            if (Horas < 0) Minutos *= -1;

            model.PageTitle = Metodos.GetModuloName(model.IdModuloActual);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            model.IdModulo = IdModulo;
            model.Perfil = Metodos.GetPerfilData();
            if (model.FechaHasta == DateTime.MinValue)
                model.FechaHasta = DateTime.MaxValue;

            DateTime fechaDesde = (model.FechaDesde != DateTime.MinValue ? model.FechaDesde.AddMinutes(Minutos * -1) : model.FechaDesde);
            DateTime fechaHasta = (model.FechaHasta != DateTime.MaxValue ? model.FechaHasta.AddMinutes(Minutos * -1) : model.FechaHasta);

            model.Data = Metodos.GetAuditoria(fechaDesde, fechaHasta, (model.IdUsuario < 0 ? null : model.IdUsuario));
            Session["Data"] = model.Data;
            Session["IdUsuarioAuditoria"] = model.IdUsuario;
            return View(model);
        }
        [SessionExpire]
        [HandleError]
        public ActionResult AuditoriaPartialView()
        {

            return PartialView();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult ComboBoxUsuariosPartialView()
        {

            AuditoriaAdminModel model = new AuditoriaAdminModel();
            string _modId = Session["IdModulo"].ToString();
            long modId = long.Parse(_modId);
            int IdTipoDocumento = int.Parse(_modId.Length == 7 ? _modId.Substring(0, 1) : _modId.Substring(0, 2));
            long IdModulo = IdTipoDocumento * 1000000;

            model.PageTitle = Metodos.GetModuloName(modId);
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, model.PageTitle);
            model.IdModulo = IdModulo;
            model.Perfil = Metodos.GetPerfilData();
            model.IdModuloActual = modId;
            model.IdUsuario = long.Parse(Session["IdUsuarioAuditoria"].ToString());

            return PartialView(model);
        }
    }
}