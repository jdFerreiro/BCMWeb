using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // DXCOMMENT: Pass a data model for GridView

            ViewBag.PageTitle = Resources.BCMWebPublic.labelAppSlogan;
            return View();
        }
        
        public ActionResult GridViewPartialView() 
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView("GridViewPartialView");
        }
        public ActionResult About()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.AboutPageTitle;
            return View();
        }
        public ActionResult BSMMovil()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.BSMMovilPageTitle;
            return View();
        }
        public ActionResult Confidencialidad()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.ConfidencialidadPageTitle;
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.ContactPageTitle;
            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.FAQPageTitle;
            return View();
        }
        public ActionResult ProcedimientoBCP()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.ProcedimientoBCPPageTitle;
            return View();
        }
        public ActionResult ProcedimientoBIA()
        {
            ViewBag.PageTitle = Resources.BCMWebPublic.ProcedimientoBIAPageTitle;
            return View();
        }
    }
}

public enum HeaderViewRenderMode { Full, Title }