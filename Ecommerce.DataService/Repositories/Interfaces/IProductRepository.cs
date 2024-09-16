using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Product;
using Ecommerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataService.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetAllStoreProductsAsync(Guid storeId);
        Task<Result<Product>> AddProductToStoreAsync(Product product, ClaimsPrincipal userClaim);
        Task<Result<Product>> UpdateProductToStoreAsync(UpdateProductDto product, ClaimsPrincipal userClaim);
        Task<Result<Product>> DeleteProductToStoreAsync(Guid id, ClaimsPrincipal userClaim);
    }
}
