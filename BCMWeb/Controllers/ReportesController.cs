using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BCMWeb.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        [SessionExpire]
        [HandleError]
        public ActionResult Index(long IdModulo)
        {


            return View();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Cuadro(long IdModulo)
        {

            ReporteModel model = new ReporteModel();
            
            return View();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Tabla(long IdModulo)
        {

            ReporteModel model = new ReporteModel();

            return View();
        }
        [SessionExpire]
        [HandleError]
        public ActionResult Grafico(long IdModulo)
        {

            ReporteModel model = new ReporteModel();

            return View();
        }
    }
}