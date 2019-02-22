using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTeknikServis.MODELS.ViewModels
{
    public class FaultTrackingViewModel
    {
        public Guid FaultID { get; set; } = Guid.NewGuid();
        public string TechnicianID { get; set; }
    }
}
