using Microsoft.EntityFrameworkCore;

namespace BillPaymentSystem.Models
{
    public class FaturaContext : DbContext
    {
        public FaturaContext(DbContextOptions<FaturaContext> options) : base(options) { }

        public DbSet<Fatura> Faturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fatura>().Property(f => f.Tutar).HasPrecision(18, 2);

            // Veritabanı ilk oluştuğunda içine eklenecek örnek veriler
            modelBuilder.Entity<Fatura>().HasData(
                new Fatura() { AboneNo = 1, Durum = false, FaturaType = "Elektrik", Name = "Durmuş Durarmış", Tarih = new DateTime(2024, 9, 7), Tutar = 456m, FaturaNo = 1 },
                new Fatura() { AboneNo = 32, Durum = false, FaturaType = "Su", Name = "Aslı Mert", Tarih = new DateTime(2024, 9, 7), Tutar = 654m, FaturaNo = 2 },
                new Fatura() { AboneNo = 123, Durum = false, FaturaType = "İnternet", Name = "Doğukan Gün", Tarih = new DateTime(2024, 9, 7), Tutar = 423m, FaturaNo = 3 }
            );
        }
    }
}