using Microsoft.EntityFrameworkCore;
using System;

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
                new Fatura() { AboneNo = 1, Durum = false, FaturaType = "Electricity", Name = "Durmuş Durarmış", Tarih = DateTime.Parse("07/09/2024"), Tutar = 456m, FaturaNo = 1 },
                new Fatura() { AboneNo = 32, Durum = false, FaturaType = "Water", Name = "Aslı Mert", Tarih = DateTime.Parse("07/09/2024"), Tutar = 654m, FaturaNo = 2 },
                new Fatura() { AboneNo = 123, Durum = false, FaturaType = "Internet", Name = "Doğukan Gün", Tarih = DateTime.Parse("07/09/2024"), Tutar = 423m, FaturaNo = 3 }
            );
        }
    }
}