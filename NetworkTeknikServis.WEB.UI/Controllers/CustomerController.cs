using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetworkTeknikServis.BLL.Helpers;
using NetworkTeknikServis.BLL.Repository;
using NetworkTeknikServis.MODELS.Entities;
using NetworkTeknikServis.MODELS.Enums;
using NetworkTeknikServis.MODELS.ViewModels;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult FaultCreate(FaultViewModel model)
        {
            //o anki sistemdeki kullanıcının idsini verir. 
            var MusteriId = HttpContext.User.Identity.GetUserId();

            //if (!ModelState.IsValid)
            //{
            //    //Gelen model valid degiilse bu sayfaya yönlendirilip hatalar gösterilicek.
            //    return RedirectToAction("Index", "Musteri", model);
            //}


            try
            {
                #region ArızaResimİşlemi
                if (model.PostedFileFault != null &&
                    model.PostedFileFault.ContentLength > 0)
                {
                    var file = model.PostedFileFault;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extName = Path.GetExtension(file.FileName);
                    fileName = StringHelper.UrlFormatConverter(fileName);
                    fileName += StringHelper.GetCode();
                    var klasoryolu = Server.MapPath("~/Ariza/");
                    var dosyayolu = Server.MapPath("~/Ariza/") + fileName + extName;

                    if (!Directory.Exists(klasoryolu))
                        Directory.CreateDirectory(klasoryolu);
                    file.SaveAs(dosyayolu);

                    WebImage img = new WebImage(dosyayolu);
                    img.Resize(250, 250, false);
                    img.AddTextWatermark("FİTech");
                    img.Save(dosyayolu);
                    var oldPath = model.FaultPath;
                    model.FaultPath = "/Ariza/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }


                #endregion

                #region FaturaResimİşlemleri
                if (model.PostedFileInvoice != null &&
                    model.PostedFileInvoice.ContentLength > 0)
                {
                    var file = model.PostedFileInvoice;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extName = Path.GetExtension(file.FileName);
                    fileName = StringHelper.UrlFormatConverter(fileName);
                    fileName += StringHelper.GetCode();
                    var klasoryolu = Server.MapPath("~/Fatura/");
                    var dosyayolu = Server.MapPath("~/Fatura/") + fileName + extName;

                    if (!Directory.Exists(klasoryolu))
                        Directory.CreateDirectory(klasoryolu);
                    file.SaveAs(dosyayolu);

                    WebImage img = new WebImage(dosyayolu);
                    img.Resize(250, 250, false);
                    img.AddTextWatermark("FİTech");
                    img.Save(dosyayolu);
                    var oldPath = model.InvoicePath;
                    model.InvoicePath = "/Fatura/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }


                #endregion

                var data = new Fault
                {
                    CustomerId = MusteriId,
                    FaultPath=model.FaultPath,
                    InvoicePath = model.InvoicePath,
                    Adress = model.Adress,
                    FaultDescription = model.FaultDescription,
                    AssignedOperator = false,
                    FaultState = FaultState.Uncompleted,
                    FaultID=model.FaultID,
                };
                new FaultRepo().Insert(data);
                var Log = new FaultLog
                {
                    TechnicianId = data.TechnicianId,
                    CustomerId = data.CustomerId,
                    Operation = "Arıza kaydı oluşturuldu",
                    FaultId = data.FaultID,
                    OperationDescription = data.FaultDescription
                };
                new FaultLogRepo().Insert(Log);
                TempData["Message"] = $"{model.FaultID} no'lu kayıt başarıyla eklenmiştir";
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Index",
                    ControllerName = "Musteri",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Musteri",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            
        }

        public async Task<ActionResult> GetMyRecords()
        {
            var musteriid = HttpContext.User.Identity.GetUserId();
            var data = new FaultRepo().GetAll(x => x.CustomerId == musteriid).ToList();
            return View(data);

        }

    }
}