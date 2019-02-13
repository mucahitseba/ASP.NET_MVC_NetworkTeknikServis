using Microsoft.AspNet.Identity.EntityFramework;
using NetworkTeknikServis.MODELS.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTeknikServis.DAL
{
    public class MyContext : IdentityDbContext<User>
    {
        public DateTime InstanceDate { get; set; }
        public MyContext():base("name=MyCon")
        {
            this.InstanceDate = DateTime.Now;
        }
    }
}
