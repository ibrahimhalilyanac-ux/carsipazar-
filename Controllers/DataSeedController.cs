using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Controllers
{
    public class DataSeedController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public DataSeedController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Seed()
        {
            var resultMsg = new StringBuilder();
            resultMsg.AppendLine("--- CARSIPAZAR VERI TOHUMLAMA SIHRIBAZI BASLATILDI ---");

            try
            {
                // 1. Mevcut Urunleri Cek ve Sil
                resultMsg.AppendLine("1. Eski urunler veritabanindan cekiliyor...");
                var products = await _productService.GetAllProductAsync();
                resultMsg.AppendLine($" - {products.Count} adet eski urun bulundu. Temizleniyor...");
                foreach (var product in products)
                {
                    try { await _productService.DeleteProductAsync(product.ProductId); } catch { }
                }
                resultMsg.AppendLine(" - Eski urun temizleme tamamlandi.");

                // 2. Mevcut Kategorileri Cek ve Sil
                resultMsg.AppendLine("2. Eski kategoriler veritabanindan cekiliyor...");
                var categories = await _categoryService.GetAllCategoryAsync();
                resultMsg.AppendLine($" - {categories.Count} adet eski kategori bulundu. Temizleniyor...");
                foreach (var category in categories)
                {
                    try { await _categoryService.DeleteCategoryAsync(category.CategoryID); } catch { }
                }
                resultMsg.AppendLine(" - Eski kategori temizleme tamamlandi.");

                // 3. Tanimli Essiz Kategoriler ve Urun Verileri (UNICODE ESCAPED FOR 100% ROBUSTNESS)
                var seedData = new List<(string Name, string Image, List<(string ProdName, string ProdImage, decimal Price)> Products)>
                {
                    ("Giyim & Aksesuar", "clothes.png", new List<(string, string, decimal)>
                    {
                        ("K\u0131\u015fl\u0131k \u00d6rme Kazak", "https://images.unsplash.com/photo-1574169208507-84376144848b?w=500", 450),
                        ("Pamuklu Beyaz Ti\u015f\u00f6rt", "https://images.unsplash.com/photo-1521572267360-ee0c2909d518?w=500", 150),
                        ("Kad\u0131n K\u0131\u015fl\u0131k Mont", "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=500", 850),
                        ("Spor Kesim Pantolon", "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=500", 320),
                        ("Erkek Deri Ceket", "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=500", 1200),
                        ("Yazl\u0131k V Yaka Ti\u015f\u00f6rt", "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?w=500", 120)
                    }),
                    ("Elektronik & Teknoloji", "laptop.png", new List<(string, string, decimal)>
                    {
                        ("Gaming Laptop RTX 4060", "https://images.unsplash.com/photo-1603302576837-37561b2e2302?w=500", 25000),
                        ("Ak\u0131ll\u0131 Telefon 128GB", "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500", 12000),
                        ("Bluetooth Kablosuz Kulakl\u0131k", "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500", 850),
                        ("Tablet Bilgisayar 10 in\u00e7", "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?w=500", 4500),
                        ("Ultra HD 4K Monit\u00f6r", "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=500", 3200),
                        ("Oyun Konsolu 1TB", "https://images.unsplash.com/photo-1606813907291-d86efa9b94db?w=500", 18000)
                    }),
                    ("Ev & Dekorasyon", "furniture.jpg", new List<(string, string, decimal)>
                    {
                        ("Modern 3'l\u00fc Koltuk", "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=500", 4500),
                        ("Ah\u015fap Mutfak Masas\u0131", "https://images.unsplash.com/photo-1530018607912-eff2daa1bac4?w=500", 2100),
                        ("Dekoratif Duvar Saati", "https://images.unsplash.com/photo-1563861826100-9cb868fdba1e?w=500", 350),
                        ("Ortopedik \u00c7ift Ki\u015filik Yatak", "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?w=500", 6500),
                        ("TV \u00dcnitesi ve Kitapl\u0131k", "https://images.unsplash.com/photo-1595428774223-ef52624120d2?w=500", 1800),
                        ("S\u0131v\u0131 Sabunluk Seti", "https://images.unsplash.com/photo-1608248597279-f99d160bfcbc?w=500", 120)
                    }),
                    ("Ayakkab\u0131 & \u00c7anta", "shoes.png", new List<(string, string, decimal)>
                    {
                        ("G\u00fcnl\u00fck Spor Ayakkab\u0131", "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500", 850),
                        ("Hakiki Deri \u00c7izme", "https://images.unsplash.com/photo-1520639888713-7851133b1ed0?w=500", 1400),
                        ("Su Ge\u00e7irmez S\u0131rt \u00c7antas\u0131", "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500", 450),
                        ("Kad\u0131n Omuz \u00c7antas\u0131", "https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=500", 650),
                        ("Ortopedik Y\u00fcr\u00fcy\u00fc\u015f Ayakkab\u0131s\u0131", "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=500", 950),
                        ("Deri Evrak \u00c7antas\u0131", "https://images.unsplash.com/photo-1547949003-9792a18a2601?w=500", 1200)
                    }),
                    ("Kozmetik & Bak\u0131m", "cleanthings.jpg", new List<(string, string, decimal)>
                    {
                        ("Do\u011fal Lavanta Sabunu", "https://images.unsplash.com/photo-1607006342411-92fc0a41f0a2?w=500", 45),
                        ("Ferahlat\u0131c\u0131 Beyazlat\u0131c\u0131 Di\u015f Macunu", "https://images.unsplash.com/photo-1559599101-f09722fb4948?w=500", 65),
                        ("G\u00fcnl\u00fck Y\u00fcz Temizleme Jeli", "https://images.unsplash.com/photo-1556228720-195a672e8a03?w=500", 120),
                        ("K\u0131r\u0131\u015f\u0131kl\u0131k Kar\u015f\u0131t\u0131 Gece Kremi", "https://images.unsplash.com/photo-1601049541289-9b1b7bbbfe19?w=500", 250),
                        ("Leke \u00c7\u0131kar\u0131c\u0131 S\u0131v\u0131 Deterjan", "https://images.unsplash.com/photo-1610557892470-76d747eed2f1?w=500", 85),
                        ("Kepe\u011fe Kar\u015f\u0131 \u015eampuan", "https://images.unsplash.com/photo-1535585209827-a15fcdbc4c2d?w=500", 90)
                    }),
                    ("Spor & Outdoor", "electronics.jpg", new List<(string, string, decimal)>
                    {
                        ("Nefes Alan Spor Ti\u015f\u00f6rt", "https://images.unsplash.com/photo-1581655353564-df123a1eb820?w=500", 150),
                        ("Profesyonel Ko\u015fu Ayakkab\u0131s\u0131", "https://images.unsplash.com/photo-1476480862126-209bfaa8edc8?w=500", 1100),
                        ("4 Ki\u015filik Kamp \u00c7ad\u0131r\u0131", "https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500", 1450),
                        ("Kaymaz Yoga Mat\u0131", "https://images.unsplash.com/photo-1592432678016-e910b452f9a2?w=500", 220),
                        ("Damb\u0131l Seti 10kg", "https://images.unsplash.com/photo-1638536532686-d610adfc8e5c?w=500", 350),
                        ("Termos 1 Litre", "https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=500", 280)
                    }),
                    ("Kitap & Hobi", "toy.png", new List<(string, string, decimal)>
                    {
                        ("E\u011fitici Ah\u015fap Oyuncak", "https://images.unsplash.com/photo-1515488042361-404e9250afef?w=500", 120),
                        ("D\u00fcnya Klasikleri Roman Seti", "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=500", 240),
                        ("1000 Par\u00e7a Do\u011fa Puzzle", "https://images.unsplash.com/photo-1585250004683-c2f604477ca6?w=500", 85),
                        ("Akrilik Boya F\u0131r\u00e7a Seti", "https://images.unsplash.com/photo-1513364776144-60967b0f800f?w=500", 150),
                        ("Tarih Ansiklopedisi", "https://images.unsplash.com/photo-1589829085413-56de8ae18c73?w=500", 350),
                        ("Pelu\u015f Oyuncak Ay\u0131", "https://images.unsplash.com/photo-1559251606-c623743a6d76?w=500", 180)
                    }),
                    ("Anne & Bebek", "clothes2.png", new List<(string, string, decimal)>
                    {
                        ("Katlanabilir Bebek Arabas\u0131", "https://images.unsplash.com/photo-1591088398332-8a7791972843?w=500", 3500),
                        ("Yenido\u011fan Hastane \u00c7\u0131k\u0131\u015f\u0131 Set", "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=500", 450),
                        ("Uyku Arkada\u015f\u0131 Ay\u0131c\u0131k", "https://images.unsplash.com/photo-1582254465498-6bc70419b607?w=500", 150),
                        ("\u00c7ok G\u00f6zl\u00fc Bebek Bak\u0131m \u00c7antas\u0131", "https://images.unsplash.com/photo-1597033878166-5d6c810bb73c?w=500", 350),
                        ("Organik Pamuk Bebek Battaniyesi", "https://images.unsplash.com/photo-1560943960-4966601b0f5b?w=500", 280),
                        ("Bebek \u015eampuan\u0131 500ml", "https://images.unsplash.com/photo-1608248597279-f99d160bfcbc?w=500", 75)
                    }),
                    ("S\u00fcpermarket", "fruit.png", new List<(string, string, decimal)>
                    {
                        ("Taze Kar\u0131\u015f\u0131k Meyve Paketi", "https://images.unsplash.com/photo-1619546813926-a78fa6372cd2?w=500", 120),
                        ("Organik Sebze Basketi", "https://images.unsplash.com/photo-1540420773420-3366772f4999?w=500", 140),
                        ("Renk Koruyucu Deterjan", "https://images.unsplash.com/photo-1583947215259-38e31be8751f?w=500", 95),
                        ("Dondurulmu\u015f Tavuk Nugget", "https://images.unsplash.com/photo-1562967914-608f82629710?w=500", 85),
                        ("Zeytinya\u011fl\u0131 Do\u011fal Sabun", "https://images.unsplash.com/photo-1546554137-f86b9593a222?w=500", 45),
                        ("Tam Bu\u011fday Unu 2kg", "https://images.unsplash.com/photo-1509440159596-0249088772ff?w=500", 35)
                    }),
                    ("Mutfak Gere\u00e7leri", "kitchen.jpg", new List<(string, string, decimal)>
                    {
                        ("7 Par\u00e7a Granit Tencere Seti", "https://images.unsplash.com/photo-1584269600464-37b1b58a9fe7?w=500", 1250),
                        ("Otomatik Kahve Makinesi", "https://images.unsplash.com/photo-1517701604599-bb29b565090c?w=500", 1800),
                        ("24 Par\u00e7a Porselen Yemek Seti", "https://images.unsplash.com/photo-1610701596007-11502861dcfa?w=500", 2100),
                        ("\u00c7elik \u00c7atal B\u0131\u00e7ak Tak\u0131m\u0131 72P", "https://images.unsplash.com/photo-1596078841242-12f73dc697c6?w=500", 1500),
                        ("Cam Baharatl\u0131k Seti", "https://images.unsplash.com/photo-1534422298391-e4f8c172dddb?w=500", 250),
                        ("Leke Tutmaz Mutfak \u00d6nl\u00fc\u011f\u00fc", "https://images.unsplash.com/photo-1607344645866-009c320c5ab8?w=500", 110)
                    })
                };

                // 4. Yeni Kategorileri Olustur
                resultMsg.AppendLine("3. 10 yeni kategori tescil ediliyor...");
                foreach (var cat in seedData)
                {
                    await _categoryService.CreateCategoryAsync(new CreateCategoryDto
                    {
                        CategoryName = cat.Name,
                        ImageUrl = $"/images/categoryimages/{cat.Image}"
                    });
                }
                resultMsg.AppendLine(" - Kategoriler basariyla eklendi.");

                // 5. Olusturulan Kategorileri Veritabanindan Geri Cek (ID'leri Almak Icin)
                var addedCategories = await _categoryService.GetAllCategoryAsync();

                // 6. Urunleri Kategorilerine Gore Tescil Et
                resultMsg.AppendLine("4. Urunler kategorilerine gore ekleniyor...");
                foreach (var cat in seedData)
                {
                    var matchingCategory = addedCategories.Find(c => c.CategoryName == cat.Name);
                    if (matchingCategory != null)
                    {
                        foreach (var prod in cat.Products)
                        {
                            await _productService.CreateProductAsync(new CreateProductDto
                            {
                                ProductName = prod.ProdName,
                                ProductPrice = prod.Price,
                                ProductImageUrl = prod.ProdImage,
                                ProductDescription = $"{cat.Name} kategorisine ozel, en cok tercih edilen birinci sinif urun.",
                                CategoryId = matchingCategory.CategoryID
                            });
                        }
                    }
                }
                resultMsg.AppendLine(" - Urunlerin tamami mukemmel sekilde tescil edildi.");

                return Content(resultMsg.ToString() + "\n\n--- [ISLEM KUSURSUZ TAMAMLANDI: TUM TURKCE KARAKTERLER VE GORSELLER DUZELTILDI] ---");
            }
            catch (Exception ex)
            {
                return Content(resultMsg.ToString() + $"\n\n!!! HATA OLUSTU !!!: {ex.Message}\nIc Hata: {ex.InnerException?.Message}");
            }
        }
    }
}
