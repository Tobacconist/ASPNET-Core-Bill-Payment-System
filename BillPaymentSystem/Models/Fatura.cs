using System.ComponentModel.DataAnnotations;

namespace BillPaymentSystem.Models
{
    public class Fatura
    {
        public const decimal GunlukGecikmeOrani = 0.05m;

        [Key]
        public int FaturaNo { get; set; }
        public int AboneNo { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FaturaType { get; set; } = string.Empty;
        public DateTime Tarih { get; set; }
        public bool Durum { get; set; }
        public decimal Tutar { get; set; }

        public int GecikmeGunu(DateTime odemeTarihi) =>
            Math.Max(0, (odemeTarihi.Date - Tarih.Date).Days);

        public decimal GecikmeliTutar(DateTime odemeTarihi) =>
            Tutar + Tutar * GunlukGecikmeOrani * GecikmeGunu(odemeTarihi);
    }
}
