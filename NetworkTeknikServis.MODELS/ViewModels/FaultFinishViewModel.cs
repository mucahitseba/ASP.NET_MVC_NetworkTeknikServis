using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTeknikServis.MODELS.Enums;

namespace NetworkTeknikServis.MODELS.ViewModels
{
    public class FaultFinishViewModel
    {
        public Guid FaultID { get; set; } = Guid.NewGuid();
        
        public FaultState faultState { get; set; }
    }
}
