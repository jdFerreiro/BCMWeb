using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        // GET: Menu
        [SessionExpire]
        [HandleError]
        public ActionResult Index()
        {
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, Resources.BCMWebPublic.labelAppSlogan);
            ModulosUserModel model = new ModulosUserModel();
            model.IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            model.ModulosPrincipales = Metodos.GetModulosPrincipalesEmpresaUsuario();
            return View(model);
        }

        [HttpPost]
        [SessionExpire]
        [HandleError]
        public ActionResult Index(ModulosUserModel model) 
        {
            ViewBag.Title = string.Format("{0} - {1}", Resources.BCMWebPublic.labelAppTitle, Resources.BCMWebPublic.labelAppSlogan);
            Session["IdEmpresa"] = model.IdEmpresa;
            model.ModulosPrincipales = Metodos.GetModulosPrincipalesEmpresaUsuario();
            return View(model);
        }

        [SessionExpire]
        [HandleError]
        public ActionResult ComboBoxEmpresaPartial()
        {
            return PartialView();
        }
    }
}