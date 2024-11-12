using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductReposoitory
    {


        Task<IReadOnlyList<Product>> GetProductsAsync(string ?brand,
            string?type,string ? sort);
        Task<IReadOnlyList<string>> GetBrandsAsync();
        Task<IReadOnlyList<string>> GetTypesAsync();
        Task<Product?> GetProductByIdAsync(int id);

        void AddProduct(Product product);

        void updateProduct(Product product);


        void DeleteProduct(Product product);

        bool ProductExists(int id);

        Task<bool> SaveChangesAsync();




    }
}
