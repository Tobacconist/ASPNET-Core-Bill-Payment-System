using System.ComponentModel.DataAnnotations;

namespace BillPaymentSystem.Models
{
    public class Fatura
    {
        public const decimal GunlukGecikmeOrani = 0.05m;

        [Key]
        public int FaturaNo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Abone numarası pozitif olmalı.")]
        public int AboneNo { get; set; }
        [Required(ErrorMessage = "Müşteri adı zorunlu."), StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Fatura türü zorunlu."), StringLength(50)]
        public string FaturaType { get; set; } = string.Empty;
        public DateTime Tarih { get; set; }
        public bool Durum { get; set; }
        [Range(0.01, 1_000_000, ErrorMessage = "Tutar 0,01 ile 1.000.000 arasında olmalı.")]
        public decimal Tutar { get; set; }

        public int GecikmeGunu(DateTime odemeTarihi) =>
            Math.Max(0, (odemeTarihi.Date - Tarih.Date).Days);

        public decimal GecikmeliTutar(DateTime odemeTarihi) =>
            Tutar + Tutar * GunlukGecikmeOrani * GecikmeGunu(odemeTarihi);
    }
}
