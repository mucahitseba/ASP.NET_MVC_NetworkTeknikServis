﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NetworkTeknikServis.BLL.Helpers;
using NetworkTeknikServis.BLL.Services.Senders;
using NetworkTeknikServis.MODELS.IdentityModels;
using NetworkTeknikServis.MODELS.ViewModels;
using static NetworkTeknikServis.BLL.Identity.MembershipTools;

namespace NetworkTeknikServis.WEB.UI.Controllers
{
    [RequireHttps]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated && HttpContext.GetOwinContext().Authentication.User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();

                var rm = model.RegisterViewModel;

                var user = await userManager.FindByNameAsync(rm.UserName);
                if (user != null)
                {
                    ModelState.AddModelError("UserName", "Bu kullanıcı adı daha önceden alınmıştır");
                    return View("Index", model);
                }

                var newUser = new User()
                {
                    UserName = rm.UserName,
                    Email = rm.Email,
                    Name = rm.Name,
                    Surname = rm.Surname,
                    ActivationCode = StringHelper.GetCode()
                };
                var result = await userManager.CreateAsync(newUser, rm.Password);
                if (result.Succeeded)
                {
                    if (userStore.Users.Count() == 1)
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "User");
                    }
                    //todo kullanıcıya mail gönderilsin
                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                     (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var emailService = new EmailService();
                    var body = $"Merhaba <b>{newUser.Name} {newUser.Surname}</b><br>Hesabınızı aktif etmek için aşadıdaki linke tıklayınız<br> <a href='{SiteUrl}/account/activation?code={newUser.ActivationCode}' >Aktivasyon Linki </a> ";
                    await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Sitemize Hoşgeldiniz" }, newUser.Email);
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    return View("Index", model);
                }

                TempData["Message"] = "Kaydınız alınlıştır. Lütfen giriş yapınız";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(RegisterLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", model);

                var userManager = NewUserManager();
                var user = await userManager.FindAsync(model.LoginViewModel.UserName, model.LoginViewModel.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    return View("Index", model);
                }
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                authManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = model.LoginViewModel.RememberMe
                }, userIdentity);

                return RedirectToAction("Index","Account");
                //if (HttpContext.GetOwinContext().Authentication.User.IsInRole("Admin"))
                //    return RedirectToAction("Index", "Admin");
                //else if (HttpContext.GetOwinContext().Authentication.User.IsInRole("Operator"))
                //    return RedirectToAction("Index", "Operator");
                //else if (HttpContext.GetOwinContext().Authentication.User.IsInRole("Technician"))
                //    return RedirectToAction("Index", "Technician");
                //else if (HttpContext.GetOwinContext().Authentication.User.IsInRole("Customer"))
                //    return RedirectToAction("Index", "Customer");
                //else
                //    return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index","Account");
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> UserProfile()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();

            try
            {
                var userManager = NewUserManager();
                var user = await userManager.FindByIdAsync(id);

                var data = new ProfilePasswordViewModel()
                {
                    UserProfileViewModel = new UserProfileViewModel()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName
                    }
                };
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> UpdateProfile(ProfilePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            try
            {
                var userManager = NewUserManager();
                var user = await userManager.FindByIdAsync(model.UserProfileViewModel.Id);

                user.Name = model.UserProfileViewModel.Name;
                user.Surname = model.UserProfileViewModel.Surname;
                if (user.Email != model.UserProfileViewModel.Email)
                {
                    //todo tekrardan aktivasyon gönderilmeli ve kullanıcı rolü aktif olamayan bir kullanıcı rolüne atanmalı!
                }
                user.Email = model.UserProfileViewModel.Email;
                user.PhoneNumber = model.UserProfileViewModel.PhoneNumber;
                await userManager.UpdateAsync(user);
                TempData["Message"] = "Profiliniz güncellenmiştir";
                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ProfilePasswordViewModel model)
        {
            try
            {
                var userManager = NewUserManager();
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var data = new ProfilePasswordViewModel()
                {
                    UserProfileViewModel = new UserProfileViewModel()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Surname = user.Surname,
                        UserName = user.UserName
                    }
                };
                model.UserProfileViewModel = data.UserProfileViewModel;
                if (!ModelState.IsValid)
                {
                    model.ChangePasswordViewModel = new ChangePasswordViewModel();
                    return View("UserProfile", model);
                }


                var result = await userManager.ChangePasswordAsync(
                    HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId(),
                    model.ChangePasswordViewModel.OldPassword, model.ChangePasswordViewModel.NewPassword);

                if (result.Succeeded)
                {
                    //todo kullanıcıyı bilgilendiren bir mail atılır
                    return RedirectToAction("Logout", "Account");
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    model.ChangePasswordViewModel = new ChangePasswordViewModel();
                    return View("UserProfile", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Activation(string code)
        {
            try
            {
                var userStore = NewUserStore();
                var user = userStore.Users.FirstOrDefault(x => x.ActivationCode == code);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        ViewBag.Message = $"<span class='alert alert-success'>Bu hesap daha önce aktive edilmiştir.</span>";
                    }
                    else
                    {
                        user.EmailConfirmed = true;

                        userStore.Context.SaveChanges();
                        ViewBag.Message = $"<span class='alert alert-success'>Aktivasyon işleminiz başarılı</span>";
                    }
                }
                else
                {
                    ViewBag.Message = $"<span class='alert alert-danger'>Aktivasyon başarısız</span>";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "<span class='alert alert-danger'>Aktivasyon işleminde bir hata oluştu</span>";
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();
                var user = await userStore.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, $"{model.Email} mail adresine kayıtlı bir üyeliğe erişilemedi");
                    return View(model);
                }

                var newPassword = StringHelper.GetCode().Substring(0, 6);
                await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(newPassword));
                var result = userStore.Context.SaveChanges();
                if (result == 0)
                {
                    TempData["Model"] = new ErrorViewModel()
                    {
                        Text = $"Bir hata oluştu",
                        ActionName = "RecoverPassword",
                        ControllerName = "Account",
                        ErrorCode = 500
                    };
                    return RedirectToAction("Error", "Home");
                }

                var emailService = new EmailService();
                var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Hesabınızın parolası sıfırlanmıştır<br> Yeni parolanız: <b>{newPassword}</b> <p>Yukarıdaki parolayı kullanarak sistemize giriş yapabilirsiniz.</p>";
                emailService.Send(new IdentityMessage() { Body = body, Subject = $"{user.UserName} Şifre Kurtarma" }, user.Email);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "RecoverPassword",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}