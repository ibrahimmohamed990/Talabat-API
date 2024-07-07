using Microsoft.Extensions.Logging;
using Store.Data.Context;
using Store.Data.Entities;
using System.Text.Json;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                bool flag = false;
                if(context.ProductBrands != null && !context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands != null)
                        await context.ProductBrands.AddRangeAsync(brands);
                    //context.SaveChanges();
                    flag = true;
                }
                if (context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if(types != null)
                        await context.ProductTypes.AddRangeAsync(types);
                    //context.SaveChanges();
                    flag = true;
                }
                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if(products != null)
                        await context.Products.AddRangeAsync(products);
                    //context.SaveChanges();
                    flag = true;
                }
                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                    if (deliveryMethods != null)
                        await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    //context.SaveChanges();
                    flag = true;
                }
                if (flag && context != null) context.SaveChanges();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreDbContext>();
                logger.LogError(ex.Message);
            }

        }

    }
}
