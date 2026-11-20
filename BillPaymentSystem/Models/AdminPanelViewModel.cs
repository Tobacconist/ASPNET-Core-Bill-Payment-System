namespace BillPaymentSystem.Models
{
    public class AdminPanelViewModel
    {
        public List<Fatura> Faturalar { get; set; } = new();
        public Fatura YeniFatura { get; set; } = new() { Tarih = DateTime.Today };
    }
}
