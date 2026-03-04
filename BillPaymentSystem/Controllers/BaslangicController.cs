using Microsoft.AspNetCore.Mvc;
using BillPaymentSystem.Models;
using System;
using System.Linq;

namespace BillPaymentSystem.Controllers
{
    public class BaslangicController : Controller
    {
        private readonly FaturaContext _context;

        public BaslangicController(FaturaContext context)
        {
            _context = context;
        }

        // --- ADMİN PANELİ İŞLEMLERİ ---

        // Faturaları Listeleme
        public IActionResult AdminPanel()
        {
            var faturalistesi = _context.Faturas.ToList();
            return View(faturalistesi);
        }

        // Fatura Silme
        [HttpPost]
        public IActionResult AdminPanel(int FaturaNo)
        {
            var fatura = _context.Faturas.Find(FaturaNo);
            if (fatura != null)
            {
                _context.Faturas.Remove(fatura);
                _context.SaveChanges();
            }
            var faturalistesi = _context.Faturas.ToList();
            return View("AdminPanel", faturalistesi);
        }

        // Yeni Fatura Ekleme
        [HttpPost]
        public IActionResult YeniFaturaEkle(Fatura fatura)
        {
            if (ModelState.IsValid)
            {
                _context.Faturas.Add(fatura);
                _context.SaveChanges();
                return RedirectToAction("AdminPanel");
            }
            return View(fatura);
        }

        // --- MÜŞTERİ ÖDEME İŞLEMİ ---

        // Fatura Ödeme ve Gecikme Cezası Hesaplama
        public IActionResult Odeme(int FaturaNo, DateTime OdemeTarihi)
        {
            var fatura = _context.Faturas.FirstOrDefault(p => p.FaturaNo == FaturaNo);
            double odenenTutar = 0;

            if (fatura != null && !fatura.Durum)
            {
                DateTime GerekenTarih = fatura.Tarih;
                double gecikmeGun = (OdemeTarihi - GerekenTarih).TotalDays;

                // Eğer ödeme tarihi geçmişse günlük %5 oranında ceza uygula
                if (gecikmeGun > 0)
                {
                    double Oran = 5;
                    double gecikmeTutar = fatura.Tutar * Oran * gecikmeGun;
                    fatura.Tutar += (int)gecikmeTutar;
                }

                fatura.Durum = true; // Ödendi olarak işaretle
                odenenTutar = fatura.Tutar;
                _context.SaveChanges();
            }
            // İşlem bitince müşterinin kendi sayfasına yönlendir
            return RedirectToAction("Index", "Home");
        }
    }
}