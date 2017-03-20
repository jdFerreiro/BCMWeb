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
        public ActionResult Index()
        {
            ModulosUserModel model = new ModulosUserModel();
            model.IdEmpresa = long.Parse(Session["IdEmpresa"].ToString());
            model.ModulosPrincipales = Metodos.GetModulosPrincipalesEmpresaUsuario();
            return View(model);
        }

        [HttpPost]
        [SessionExpire]
        public ActionResult Index(ModulosUserModel model) 
        {
            Session["IdEmpresa"] = model.IdEmpresa;
            model.ModulosPrincipales = Metodos.GetModulosPrincipalesEmpresaUsuario();
            return View(model);
        }

        [SessionExpire]
        public ActionResult ComboBoxEmpresaPartial()
        {
            return PartialView();
        }
    }
}