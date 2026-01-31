Üretim Takip ve Agregasyon Sistemi (Traceability Case Study)
Bu proje; üretim hatlarında ürünlerin tekilleştirilmesi, GS1 standartlarında etiketlenmesi ve bu ürünlerin koli/palet hiyerarşisinde (aggregation) takip edilmesini sağlayan bir altyapı sunar. Sistem, yüksek veri bütünlüğü gerektiren endüstriyel süreçler göz önüne alınarak geliştirilmiştir.

🏗 Mimari Açıklama (Clean Architecture)
Proje, bağımlılıkların içe (Domain katmanına) doğru olduğu Clean Architecture prensipleriyle kurgulanmıştır. Bu sayede iş mantığı; veritabanı, UI veya dış araçlardan bağımsız hale getirilmiştir.

Core (Domain): Sistemin kalbidir. Entity'ler, Enum'lar ve kural setleri burada yer alır. Herhangi bir dış kütüphane bağımlılığı yoktur.

Application: İş akışlarının (Service katmanı) yönetildiği yerdir. DTO'lar, validasyonlar ve interface tanımları burada bulunur.

Infrastructure: Veritabanı erişimi (EF Core), repository implementasyonları ve loglama gibi dış dünya araçlarının yapılandırıldığı katmandır.

Web API: Uygulamanın giriş kapısıdır. Sadece isteği alır ve Application katmanına iletir.

🛠 Kurulum Adımları
Veritabanı Yapılandırması: WebAPI projesi altındaki appsettings.json dosyasında yer alan ConnectionStrings bölümünü kendi MSSQL Server adresinize göre güncelleyin.

Migration Uygulama: Package Manager Console üzerinden Default Project olarak Infrastructure katmanını seçin ve şu komutu çalıştırın:

Bash

Update-Database
(Not: Eğer migration kullanmak istemezseniz, kaynak kodla birlikte iletilen SQL script'ini Management Studio üzerinden manuel çalıştırabilirsiniz.)

Çalıştırma: Visual Studio üzerinden WebAPI projesini Start edin. Sistem ayağa kalktığında Swagger arayüzü otomatik olarak açılacaktır.

📝 Varsayımlar ve Kritik Kararlar
Proje geliştirilirken endüstriyel standartlar gereği aşağıdaki varsayımlar üzerine odaklanılmıştır:

Unique Değer Üretimi: Seri numarası ve SSCC kodları üretilirken, sadece DB kısıtlarına güvenilmemiş; uygulama katmanında asenkron AnyAsync kontrolleriyle çakışmaların (collision) önüne geçilmiştir.

GS1 Standartları: SSCC kodlarının 18 haneli olduğu ve son hanesinin bir kontrol basamağı (Check Digit) olduğu varsayılmıştır. Sistem bu haneyi algoritmik olarak hesaplar.

Hiyerarşik Silme: Bir paketleme birimi (Koli/Palet) silindiğinde, içindeki ürünlerin fiziksel olarak silinmediği, sadece üzerindeki "paket kimliğinin" boşa çıkarılarak tekrar paketlenebilir hale geldiği varsayılmıştır.

Performans: İş emri oluşturma sırasında seri numaralarının toplu (Bulk) şekilde üretilmesi tercih edilerek veritabanı maliyeti minimize edilmiştir.

Donanım Hazırlığı: Gerçek senaryoda fiziksel bir PLC veya yazıcı bağlı olmasa da, mimari bu cihazlardan gelecek sinyalleri (IAutomationService gibi) karşılayacak esneklikte bırakılmıştır.

📂 Proje Katmanları
Core: Domain modelleri ve temel arayüzler.

Infrastructure: DbContext, Migrations ve Repository implementasyonları.

Application: DTO modelleri, Business servisleri ve FluentValidation kuralları.

WebAPI: REST endpoint'leri ve Middleware yapılandırması.
