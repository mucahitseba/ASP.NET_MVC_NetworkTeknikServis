using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTeknikServis.MODELS.Enums;

namespace NetworkTeknikServis.MODELS.Entities
{
    [Table("Faults")]
    public class Fault
    {
        [Key]
        [DisplayName("Arıza ID")]
        public Guid FaultID { get; set; } = Guid.NewGuid();
        [DisplayName("Müşteri ID")]
        [Required]
        public string CustomerId { get; set; } = String.Empty;
        [DisplayName("Operatör ID")]
        public string OperatorId { get; set; } = String.Empty;
        [DisplayName("Teknisyen ID")]
        public string TechnicianId { get; set; } = String.Empty;
        [DisplayName("Arıza Bildirim Tarihi")]
        public DateTime FaultNotifyDate { get; set; }
        [DisplayName("Arıza Sonuçlanma Tarihi")]
        public DateTime? FaultResultDate { get; set; }
        [DisplayName("Arıza Durumu")]
        public FaultState FaultState { get; set; }
        [DisplayName("Operatöre Atanmış mı ?")]
        public bool AssignedOperator { get; set; } = false;

    }
}
