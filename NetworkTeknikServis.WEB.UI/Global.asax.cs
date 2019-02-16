using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using NetworkTeknikServis.BLL.Identity;
using NetworkTeknikServis.MODELS.Enums;
using NetworkTeknikServis.MODELS.IdentityModels;

namespace NetworkTeknikServis.WEB.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var roller = Enum.GetNames(typeof(IdentityRoles));
            var roleManager = MembershipTools.NewRoleManager();
            foreach (var rol in roller)
            {
                if (!roleManager.RoleExists(rol))
                    roleManager.Create(new Role()
                    {
                        Name = rol
                    });
            }
        }
    }
}
