using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BillPaymentSystem.Models;

namespace BillPaymentSystem.Controllers
{
    public class BaslangicController : Controller
    {
        private readonly FaturaContext _context;

        public BaslangicController(FaturaContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult AdminPanel()
        {
            return View(PanelModeli());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Sil(int FaturaNo)
        {
            var fatura = _context.Faturas.Find(FaturaNo);
            if (fatura != null)
            {
                _context.Faturas.Remove(fatura);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(AdminPanel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult YeniFaturaEkle([Bind("AboneNo,Name,FaturaType,Tarih,Tutar")] Fatura fatura)
        {
            if (!ModelState.IsValid)
            {
                var model = PanelModeli();
                model.YeniFatura = fatura;
                return View(nameof(AdminPanel), model);
            }

            _context.Faturas.Add(fatura);
            _context.SaveChanges();
            return RedirectToAction(nameof(AdminPanel));
        }

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

        private AdminPanelViewModel PanelModeli() =>
            new() { Faturalar = _context.Faturas.OrderBy(f => f.FaturaNo).ToList() };
    }
}
