# CurrencyTracker - Döviz Takip ve Analiz Sistemi

## Proje Hakkında
Bu proje, Görsel Programlama dersi final ödevi kapsamında geliştirilmiş bir C# konsol uygulamasıdır. Uygulama, finansal verileri gerçek zamanlı olarak takip etmek amacıyla "Frankfurter API" servisini kullanmaktadır.

Kullanıcı, Türk Lirası (TRY) bazlı güncel döviz kurlarını sistem üzerinden çekebilir, listeleyebilir ve LINQ sorguları kullanarak çeşitli analizler (sıralama, filtreleme, istatistiksel hesaplama) gerçekleştirebilir. Proje, "Hard-coded" (elle yazılmış) veri kullanımından kaçınarak, tamamen dinamik ve internet tabanlı bir veri işleme modeli üzerine kurulmuştur.

## Özellikler
Uygulama aşağıdaki temel fonksiyonları içermektedir:

* **Canlı Veri Çekme:** HttpClient kütüphanesi kullanılarak API üzerinden anlık kur bilgileri çekilir.
* **JSON Veri İşleme:** Gelen veriler System.Text.Json kütüphanesi ile deserialize edilerek nesne tabanlı yapıya (OOP) dönüştürülür.
* **Döviz Listeleme:** Tüm kurlar kod ve değer bazında listelenir.
* **Arama Fonksiyonu:** Kullanıcı tarafından girilen döviz koduna (örn: USD, EUR) göre özel arama yapılır.
* **Filtreleme:** Belirli bir kur değerinin üzerindeki dövizler listelenir (LINQ Where).
* **Sıralama:** Döviz kurları değerlerine göre küçükten büyüğe sıralanır (LINQ OrderBy).
* **İstatistiksel Analiz:** Toplam döviz sayısı, en yüksek kur, en düşük kur ve ortalama kur bilgileri hesaplanır (LINQ Count, Max, Min, Average).

## Teknik Detaylar ve Kullanılan Teknolojiler

Proje geliştirme sürecinde aşağıdaki teknik gereksinimler ve yapılar kullanılmıştır:

* **Dil ve Platform:** C#, .NET Framework / .NET Core
* **Veri Kaynağı:** Frankfurter FREE API (https://api.frankfurter.app)
* **Asenkron Programlama:** Veri çekme işlemi sırasında ana iş parçacığının (UI thread) bloklanmaması için `async` ve `await` yapısı kullanılmıştır.
* **LINQ (Language Integrated Query):** Veri koleksiyonları üzerinde sorgulama yapmak için aktif olarak kullanılmıştır.
* **Veri Yapıları:** API'den gelen veriler önce `Dictionary` yapısında karşılanmış, daha sonra işlenebilirlik açısından `List<Currency>` modeline dönüştürülmüştür.
* **Hata Yönetimi (Exception Handling):** İnternet bağlantısı sorunları veya hatalı veri girişlerine karşı Try-Catch blokları ile önlem alınmıştır.

## Kurulum ve Çalıştırma

1.  Projeyi bilgisayarınıza indirin veya klonlayın.
2.  Visual Studio üzerinde `CurrencyTracker.sln` dosyasını açın.
3.  Projenin derlenmesi (Build) işlemini gerçekleştirin.
4.  Aktif bir internet bağlantınızın olduğundan emin olun.
5.  Uygulamayı başlatın (Start/F5).

## Notlar
Uygulama grafiksel kullanıcı arayüzü (GUI) içermemektedir, istenildiği üzere Konsol (Console Application) tabanlı olarak tasarlanmıştır.
