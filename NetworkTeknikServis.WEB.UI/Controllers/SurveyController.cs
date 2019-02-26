using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetworkTeknikServis.BLL.Repository;
using NetworkTeknikServis.BLL.Services.Senders;
using NetworkTeknikServis.MODELS.Entities;
using NetworkTeknikServis.MODELS.Enums;
using static NetworkTeknikServis.BLL.Identity.MembershipTools;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        [HttpGet]
        public ActionResult Index(string code)
        {
            var fault = new FaultRepo().GetAll(x => x.SurveyCode == code).FirstOrDefault();
            var customerId = HttpContext.User.Identity.GetUserId();

            if (customerId == null)
            {
                TempData["Message"] = $"Anketi doldurabilmek için giriş yapmalısınız. ";
                return RedirectToAction("Index", "Account");
            }
            else if (fault.CustomerId != customerId)
            {
                TempData["Message"] = $"Bu anket sizin arızanıza tanımlanmamıştır. ";
                return RedirectToAction("Index", "Account");
            }

            else if (fault.AnketYapildimi == true)
            {
                TempData["message"] = $"{fault.FaultID} numaralı arıza için daha önceden anket yapılmıştır.";
                return RedirectToAction("Index", "Account");
            }
            else
                return View(fault);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmSurvey(Fault model)
        {
            var fault = new FaultRepo().GetById(model.FaultID);
            var customer = await NewUserStore().FindByIdAsync(fault.CustomerId);
            fault.DavranisPuani = model.DavranisPuani;
            fault.OMNetHakkindakiGorusler = model.OMNetHakkindakiGorusler;
            fault.OMNetHizmetPuanı = model.OMNetHizmetPuanı;
            fault.TeknisyenDavranisPuani = model.TeknisyenDavranisPuani;
            fault.TeknisyenBilgiPuani = model.TeknisyenBilgiPuani;
            fault.AnketYapildimi = true;

            new FaultRepo().Update(fault);

            string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                     (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            var emailService = new EmailService();
            var body = $"Merhaba <b>{customer.Name} {customer.Surname}</b><br>Memnuniyet anketimizi doldurup bizi bilgilendirdiğiniz için teşekkür ederiz.";
            await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Memnuniyet Anketi" }, customer.Email);
            return RedirectToAction("Index","Account");
        }
    }
}