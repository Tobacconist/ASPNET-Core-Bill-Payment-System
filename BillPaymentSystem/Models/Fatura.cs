using System;
using System.ComponentModel.DataAnnotations;

namespace BillPaymentSystem.Models
{
    public class Fatura
    {
        [Key]
        public int FaturaNo { get; set; }  // Her fatura için benzersiz kimlik [cite: 189]
        public int AboneNo { get; set; }   // Müşteri hesap numarası [cite: 190]
        public string Name { get; set; }   // Müşteri adı [cite: 191]
        public string FaturaType { get; set; } // Fatura türü (Elektrik, Su vb.) [cite: 192]
        public DateTime Tarih { get; set; } // Fatura kesim tarihi [cite: 193]
        public bool Durum { get; set; }    // Ödeme durumu (false = ödenmedi, true = ödendi) [cite: 194]
        public int Tutar { get; set; }     // Fatura tutarı [cite: 195]
    }
}