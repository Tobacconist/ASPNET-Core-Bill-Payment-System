using BillPaymentSystem.Models;

namespace BillPaymentSystem.Tests;

public class FaturaGecikmeTests
{
    private static Fatura ElektrikFaturasi(decimal tutar) => new()
    {
        FaturaNo = 1,
        AboneNo = 1042,
        Name = "Selin Aydoğan",
        FaturaType = "Elektrik",
        Tarih = new DateTime(2024, 9, 7),
        Tutar = tutar
    };

    [Fact]
    public void FaturaTarihindenOnceOdemede_GecikmeYokTutarDegismez()
    {
        var fatura = ElektrikFaturasi(456m);
        var odeme = new DateTime(2024, 9, 1);

        Assert.Equal(0, fatura.GecikmeGunu(odeme));
        Assert.Equal(456m, fatura.GecikmeliTutar(odeme));
    }

    [Fact]
    public void FaturaTarihiniGunuOdemede_GecikmeYokTutarDegismez()
    {
        var fatura = ElektrikFaturasi(456m);
        var odeme = new DateTime(2024, 9, 7);

        Assert.Equal(0, fatura.GecikmeGunu(odeme));
        Assert.Equal(456m, fatura.GecikmeliTutar(odeme));
    }

    [Fact]
    public void AyniGunSaatFarki_GecikmeSayilmaz()
    {
        var fatura = ElektrikFaturasi(456m);
        var odeme = new DateTime(2024, 9, 7, 23, 59, 0);

        Assert.Equal(0, fatura.GecikmeGunu(odeme));
        Assert.Equal(456m, fatura.GecikmeliTutar(odeme));
    }

    [Fact]
    public void BirGunGecikme_YuzdeBesEklenir()
    {
        var fatura = ElektrikFaturasi(200m);
        var odeme = new DateTime(2024, 9, 8);

        Assert.Equal(1, fatura.GecikmeGunu(odeme));
        Assert.Equal(210m, fatura.GecikmeliTutar(odeme));
    }

    [Fact]
    public void OnGunGecikme_GunlukYuzdeBesBasitFaizOlarakEklenir()
    {
        var fatura = ElektrikFaturasi(200m);
        var odeme = new DateTime(2024, 9, 17);

        Assert.Equal(10, fatura.GecikmeGunu(odeme));
        Assert.Equal(300m, fatura.GecikmeliTutar(odeme));
    }

    [Fact]
    public void YuzGunGecikme_CezaAnaparayiAsabilir()
    {
        var fatura = ElektrikFaturasi(456m);
        var odeme = fatura.Tarih.AddDays(100);

        Assert.Equal(100, fatura.GecikmeGunu(odeme));
        Assert.Equal(2736m, fatura.GecikmeliTutar(odeme));
    }

    [Fact]
    public void GecikmeGunu_AyVeYilSinirindaDogruSayilir()
    {
        var fatura = ElektrikFaturasi(100m);
        fatura.Tarih = new DateTime(2024, 12, 30);

        Assert.Equal(3, fatura.GecikmeGunu(new DateTime(2025, 1, 2)));
        Assert.Equal(60, fatura.GecikmeGunu(new DateTime(2025, 2, 28)));
    }

    [Fact]
    public void GecikmeliTutar_FaturayiDegistirmez()
    {
        var fatura = ElektrikFaturasi(456m);

        fatura.GecikmeliTutar(new DateTime(2024, 9, 20));

        Assert.Equal(456m, fatura.Tutar);
        Assert.False(fatura.Durum);
    }

    [Theory]
    [InlineData("123.45", 1, "129.62")]
    [InlineData("123.45", 3, "141.97")]
    [InlineData("0.99", 1, "1.04")]
    [InlineData("1999.99", 7, "2699.99")]
    public void KurusluTutarlarda_SonucIkiHaneyeYuvarlanir(string tutar, int gecikmeGunu, string beklenen)
    {
        var fatura = ElektrikFaturasi(decimal.Parse(tutar, System.Globalization.CultureInfo.InvariantCulture));
        var odeme = fatura.Tarih.AddDays(gecikmeGunu);

        var sonuc = fatura.GecikmeliTutar(odeme);

        Assert.Equal(decimal.Parse(beklenen, System.Globalization.CultureInfo.InvariantCulture), sonuc);
    }
}
