using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetworkTeknikServis.BLL.Repository;
using NetworkTeknikServis.BLL.Services.Senders;
using NetworkTeknikServis.DAL;
using NetworkTeknikServis.MODELS.Entities;
using NetworkTeknikServis.MODELS.Enums;
using NetworkTeknikServis.MODELS.IdentityModels;
using NetworkTeknikServis.MODELS.Models;
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
                Adress = x.Adress,
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
                    await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Sitemize Hoşgeldiniz" }, customer.Email);
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

        [HttpGet]
        public ActionResult FaultTracking(string id)
        {
            var data = new FaultRepo().GetAll(x => x.OperatorId == id).ToList();
            return View(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetDetail(Guid id)
        {
            List<User> users = new List<User>();
            int sayac = 0;
            try
            {
                var fault = new FaultRepo().GetById(id);
                var faults = new FaultRepo().GetAll();
                var customer = await NewUserStore().FindByIdAsync(fault.CustomerId);
                var allTechnicians = NewRoleManager().FindByName(IdentityRoles.Technician.ToString()).Users.ToList();
                var user = await NewUserStore().FindByIdAsync(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId());

                foreach (var item in allTechnicians)
                {
                    foreach (var item2 in faults)
                    {
                        if (item.UserId != item2.TechnicianId&&item2.haveJob==false)
                            sayac++;
                    }

                    if (sayac == faults.Count)
                    {
                        var x = await NewUserStore().FindByIdAsync(item.UserId);
                        users.Add(x);
                    }
                    sayac = 0;
                }

                

                return Json(new ResponseData()
                {
                    data = new { fault, user, customer,users },
                    success = true,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AssignToTechnician(FaultTrackingViewModel model)
        {
            try
            {
                var teknisyen = await NewUserStore().FindByIdAsync(model.TechnicianID);
                var fault = new FaultRepo().GetById(model.FaultID);
                if (teknisyen != null)
                {
                    
                    fault.TechnicianId = teknisyen.Id;
                    new FaultRepo().Update(fault);
                    TempData["message"] = $"{fault.FaultID} no'lu arıza işlemi {teknisyen.Name + " " + teknisyen.Surname} isimli teknisyene atanmıştır.";
                }
                else
                    throw new Exception("Teknisyen atama işlemi yapılırken bir hata oluştu");


            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }

            return View();
        }



    }
}