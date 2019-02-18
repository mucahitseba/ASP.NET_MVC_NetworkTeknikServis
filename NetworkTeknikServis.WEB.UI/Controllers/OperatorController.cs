using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetworkTeknikServis.BLL.Repository;
using NetworkTeknikServis.MODELS.ViewModels;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    [Authorize(Roles = "Operator")]
    public class OperatorController : Controller
    {
        // GET: Operator
        public ActionResult Index()
        {
            var data = new FaultRepo().GetAll(x => x.AssignedOperator == false).Select(x => new FaultViewModel()
            {
                Adress =x.Adress,
                FaultPath = x.FaultPath,
                FaultDescription = x.FaultDescription,
                AssignedOperator = x.AssignedOperator,
                CustomerId = x.CustomerId,
                FaultID = x.FaultID,
                FaultNotifyDate = x.FaultNotifyDate,
                FaultResultDate = x.FaultResultDate,
                InvoicePath = x.InvoicePath,
                FaultState = x.FaultState,
                OperatorId = x.OperatorId,
                TechnicianId = x.TechnicianId,
            }).ToList();
            return View(data);
        }
    }
}