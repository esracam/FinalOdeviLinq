using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json; // JSON verisini okumak için gerekli
using System.Threading.Tasks;

namespace CurrencyTracker
{
    // --- ZORUNLU MODEL SINIFLARI ---
    class CurrencyResponse
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    class Currency
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }

    class Program
    {
        // İnternet bağlantısı için HttpClient nesnesi
        static readonly HttpClient client = new HttpClient();

        // async Task Main: İnternetten veri bekleyeceğimiz için asenkron yapı şart
        static async Task Main(string[] args)
        {
            List<Currency> currencies = new List<Currency>();

            Console.WriteLine("Veriler sunucudan çekiliyor, lütfen bekleyiniz...");

            try
            {
                // API ADRESİ (ZORUNLU)
                string url = "https://api.frankfurter.app/latest?from=TRY";

                // 1. Veriyi internetten string olarak indir
                string jsonString = await client.GetStringAsync(url);

                // 2. JSON verisini C# nesnesine çevir (Deserialize)
                // PropertyNameCaseInsensitive = true: Büyük/küçük harf uyumsuzluğunu yoksay
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = JsonSerializer.Deserialize<CurrencyResponse>(jsonString, options);

                // 3. Dictionary yapısını List<Currency> yapısına çevir (LINQ Select ile)
                // API bize "USD": 0.03 şeklinde veriyor, biz bunu Currency nesnesine çeviriyoruz.
                if (response != null && response.Rates != null)
                {
                    currencies = response.Rates.Select(k => new Currency
                    {
                        Code = k.Key,
                        Rate = k.Value
                    }).ToList();
                }

                Console.WriteLine("Veriler başarıyla yüklendi!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.WriteLine("İnternet bağlantınızı kontrol edin.");
                return;
            }

            // --- MENU DÖNGÜSÜ ---
            while (true)
            {
                Console.WriteLine("\n===== CurrencyTracker =====");
                Console.WriteLine("1. Tüm dövizleri listele");
                Console.WriteLine("2. Koda göre döviz ara");
                Console.WriteLine("3. Belirli bir değerden büyük dövizleri listele");
                Console.WriteLine("4. Dövizleri değere göre sırala");
                Console.WriteLine("5. İstatistiksel özet göster");
                Console.WriteLine("0. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        // 1️⃣ Tüm Dövizleri Listele (LINQ Select kullanımı)
                        // Sadece listeyi ekrana basıyoruz
                        Console.WriteLine("\n--- Tüm Dövizler ---");
                        var tumListe = currencies.Select(c => $"{c.Code}: {c.Rate}");
                        foreach (var item in tumListe)
                        {
                            Console.WriteLine(item);
                        }
                        break;

                    case "2":
                        // 2️⃣ Koda Göre Döviz Ara (LINQ Where)
                        Console.Write("Aranacak Döviz Kodu (Örn: USD, EUR): ");
                        string kod = Console.ReadLine().ToUpper(); // Büyük harfe çevir

                        var bulunan = currencies
                                      .Where(c => c.Code == kod)
                                      .FirstOrDefault(); // İlk bulduğunu getir, yoksa null

                        if (bulunan != null)
                            Console.WriteLine($"\nSONUÇ: 1 TRY = {bulunan.Rate} {bulunan.Code}");
                        else
                            Console.WriteLine("\nBöyle bir döviz kodu bulunamadı.");
                        break;

                    case "3":
                        // 3️⃣ Belirli Değerden Büyükler (LINQ Where)
                        Console.Write("Minimum Kur Değeri Girin (Örn: 0.5): ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal minDeger))
                        {
                            var buyukler = currencies.Where(c => c.Rate > minDeger).ToList();

                            Console.WriteLine($"\n{minDeger} değerinden büyük olanlar:");
                            if (buyukler.Count > 0)
                            {
                                foreach (var c in buyukler)
                                    Console.WriteLine($"{c.Code}: {c.Rate}");
                            }
                            else
                            {
                                Console.WriteLine("Kriteri sağlayan döviz yok.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Geçersiz sayı girdiniz.");
                        }
                        break;

                    case "4":
                        // 4️⃣ Sıralama (LINQ OrderBy)
                        Console.WriteLine("\n--- Değere Göre Sıralı (Küçükten Büyüğe) ---");
                        var siraliListe = currencies.OrderBy(c => c.Rate).ToList();

                        foreach (var c in siraliListe)
                        {
                            Console.WriteLine($"{c.Code}: {c.Rate}");
                        }
                        break;

                    case "5":
                        // 5️⃣ İstatistik (LINQ Count, Max, Min, Average)
                        Console.WriteLine("\n--- İstatistiksel Özet ---");

                        int adet = currencies.Count();
                        decimal enYuksek = currencies.Max(c => c.Rate);
                        decimal enDusuk = currencies.Min(c => c.Rate);
                        decimal ortalama = currencies.Average(c => c.Rate);

                        // En yüksek ve en düşüğün kime ait olduğunu bulalım (Ekstra puan için)
                        var enYuksekDoviz = currencies.First(c => c.Rate == enYuksek);
                        var enDusukDoviz = currencies.First(c => c.Rate == enDusuk);

                        Console.WriteLine($"Toplam Döviz Sayısı : {adet}");
                        Console.WriteLine($"En Yüksek Kur       : {enYuksek} ({enYuksekDoviz.Code})");
                        Console.WriteLine($"En Düşük Kur        : {enDusuk} ({enDusukDoviz.Code})");
                        Console.WriteLine($"Ortalama Kur        : {ortalama:F4}");
                        break;

                    case "0":
                        Console.WriteLine("Çıkış yapılıyor...");
                        return;

                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }
            }
        }
    }
}