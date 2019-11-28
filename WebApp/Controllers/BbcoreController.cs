using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class BbcoreController : Controller
    {
        // GET: Bbcore
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReporteMovimientoBbCore(string hc)
        {        
            return View();
        }
       
        public ActionResult ReporteMovimientoBbCore2(string hc)
        {
            // var respuesta = HRA.Negocio.ReporteBL.ObtenerPaciente(hc);
            return View();
        }
        public ActionResult ListarProcedimientos(string hc)
        {
            return View();
        }
    }
}