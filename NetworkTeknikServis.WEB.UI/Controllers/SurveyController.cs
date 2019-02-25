using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetworkTeknikServis.BLL.Repository;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult Index(string code)
        {
            var fault = new FaultRepo().GetAll(x => x.SurveyCode == code).FirstOrDefault();

            if (fault.AnketYapildimi == true)
            {
                TempData["message"] = $"{fault.FaultID} numaralı arıza için daha önceden anket yapılmıştır.";
                return RedirectToAction("Index", "Account");
            }
            return View(fault);
        }
    }
}