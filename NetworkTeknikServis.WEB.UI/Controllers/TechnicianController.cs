using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    [Authorize(Roles = "Technician")]
    public class TechnicianController : Controller
    {
        // GET: Technician
        public ActionResult Index()
        {
            return View();
        }
    }
}