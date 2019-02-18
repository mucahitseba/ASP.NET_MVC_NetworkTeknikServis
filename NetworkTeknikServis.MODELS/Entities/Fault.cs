using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTeknikServis.MODELS.Enums;
using NetworkTeknikServis.MODELS.IdentityModels;

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
        public DateTime FaultNotifyDate { get; set; }=DateTime.Now;

        [DisplayName("Arıza Sonuçlanma Tarihi")]
        public DateTime? FaultResultDate { get; set; }

        [DisplayName("Arıza Durumu")] public FaultState FaultState { get; set; } = FaultState.Uncompleted;
        [DisplayName("Operatöre Atanmış mı ?")]
        public bool AssignedOperator { get; set; } = false;
        [DisplayName("Ürün Resmi Ekleyiniz :")]
        public string FaultPath { get; set; }
        //todo view modelyapcaksın bu alanı resim için.
        [DisplayName("Fatura Resmini Ekleyiniz")]
        public string InvoicePath { get; set; }
        public string FaultDescription { get; set; }
        public string Adress { get; set; }



    }
}
