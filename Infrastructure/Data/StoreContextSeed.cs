using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {

        public async static Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {

                var productsData = await File
                    .ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");


                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products == null)
                {
                    Console.WriteLine("No products were found in the JSON file.");
                    return;
                    
                }

                Console.WriteLine($"{products.Count} products");    
                context.Products.AddRange(products);

                await context.SaveChangesAsync();

            }
            if (!context.DeliveryMethods.Any())
            {

                var dmData = await File
                    .ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");


                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                if (methods == null)
                {
                    Console.WriteLine("No methods were found in the JSON file.");
                    return;

                }

                context.DeliveryMethods.AddRange(methods);

                await context.SaveChangesAsync();

            }
        }
    }

}
