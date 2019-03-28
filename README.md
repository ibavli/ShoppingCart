# ShoppingCart


Projeyi çalıştırdığımızda local mssql veritabanımıza **ShoppingCartib** isminde bir database oluşturuyor. Veritabanı oluşma anında da;

- 6 adet örnek kategori
- 14 adet örnek ürün
- 8 adet örnek kampanya
- 1 adet kupon
- 3 adet alışveriş sepeti oluşuyor.

Tüm bu oluşanları ve süreci ekrana yazdırıyoruz.


- Sepete ilk olarak kampanyaları uyguluyoruz. Bunun için de **_shoppingCartService.ApplyDiscounts(shoppingCart);** Metoduna sepetimizi gönderiyoruz. Bu metod ilk olarak kendisine gönderilen sepeti **ValidCampaigns(shoppingCart)** metoduna gönderiyor. Bu metot sayesinde o sepete uygun olan, koşulları sağlayan tüm kampanyaların analizini yapıp, bu kampanyaların listesini dönüyoruz.
- Bu metottan dönen kampanya listesinde kampanya yok ise, direkt olarak sepetin kendisini dönüyoruz.
- Eğer sepete uyumlu kampanya/kampanyalar varsa, bu kampanyaları **PrepareNewShoppingCart(validCampaigns, shoppingCart)** metoduna gönderiyoruz. Bu metot sepetteki her bir ürün için, kampanyalar içerisinden ona en iyi uygulanan indirimi analiz ediyor ve fiyatını güncelliyor.
- Sepetteki fiyat güncellemesinin bitmesinin ardından yeni sepet döndürülüyor.
- Yeni sepet ekrana yazdırılıyor.
- Daha sonra sepete kupon uygulanıyor. Bu işlem için **_shoppingCartService .ApplyCoupon(coupon, totalCartAmount)** metoduna gidiyoruz. Metot burada sepet tutarı kupon indirimi için uygun ise indirimi yapıyor ve yeni sepet tutarını döndürüyor. Ekrana bilgisi yazdırılıyor.
- Son işlem olarak deliveryCost değeri hesaplanması için **_shoppingCartService.DeliveryCostCalculator(shoppingCart:shoppingCart, costPerDelivery:2, costPerProduct:2)** metoduna parametreleri gönderiyoruz.Bu metot formüle göre bize hesapladığı değeri döndürüyor. **(costPerDelivery ve costPerProduct biz gönderiyoruz.)**

Bu işlem farklı senaryolar oluşturulmuş tüm sepetler için yaptırılıyor.

**Sepet1 =>** birden fazla kategoriye, onlara ait birden fazla kampanyaları içeriyor.

**Sepet2 =>** hiçbir kampanya içermeyen sepet.

**Sepet3 =>** sadece bir çeşit kategori bulunan ve ona ait birden fazla kampanya olan sepet


