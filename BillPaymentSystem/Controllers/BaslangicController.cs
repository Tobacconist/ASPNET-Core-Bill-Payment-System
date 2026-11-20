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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Odeme(int FaturaNo, DateTime OdemeTarihi)
        {
            var fatura = _context.Faturas.FirstOrDefault(p => p.FaturaNo == FaturaNo);

            if (fatura == null)
            {
                TempData["Hata"] = "Bu numaraya ait fatura bulunamadı.";
            }
            else if (fatura.Durum)
            {
                TempData["Hata"] = "Bu fatura daha önce ödenmiş.";
            }
            else if (OdemeTarihi.Date < fatura.Tarih.Date)
            {
                TempData["Hata"] = "Ödeme tarihi fatura tarihinden önce olamaz.";
            }
            else
            {
                fatura.Tutar = fatura.GecikmeliTutar(OdemeTarihi);
                fatura.Durum = true;
                _context.SaveChanges();

                var gecikme = fatura.GecikmeGunu(OdemeTarihi);
                TempData["Basari"] = gecikme > 0
                    ? $"{fatura.Tutar:C2} ödendi ({gecikme} gün gecikme cezası dahil)."
                    : $"{fatura.Tutar:C2} ödendi.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}