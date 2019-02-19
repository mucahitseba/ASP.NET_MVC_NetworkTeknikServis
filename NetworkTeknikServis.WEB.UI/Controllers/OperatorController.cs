using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetworkTeknikServis.BLL.Repository;
using NetworkTeknikServis.BLL.Services.Senders;
using NetworkTeknikServis.MODELS.ViewModels;
using static NetworkTeknikServis.BLL.Identity.MembershipTools;

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

        public async Task<ActionResult> ConfirmFault(Guid id)
        {
            try
            {
                var fault = new FaultRepo().GetById(id);
                var customer = await NewUserStore().FindByIdAsync(fault.CustomerId);
                var user = await NewUserStore().FindByIdAsync(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId());
                if (user != null)
                {
                    fault.AssignedOperator = true;
                    fault.OperatorId = user.Id;
                    new FaultRepo().Update(fault);

                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                     (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var emailService = new EmailService();
                    var body = $"Merhaba <b>{customer.Name} {customer.Surname}</b><br>Arıza talebiniz onaylanmıştır.";
                    await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Sitemize Hoşgeldiniz" },customer.Email);
                }

                TempData["Message"] = $"Arıza talebi onaylanmıştır";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}