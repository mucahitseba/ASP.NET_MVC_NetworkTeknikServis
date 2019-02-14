using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult FooterPartial()
        {
            return PartialView(viewName: "Partial/_FooterPartial");
        }
        public PartialViewResult HeaderPartial()
        {
            return PartialView(viewName: "Partial/_HeaderPartial");
        }
    }
}