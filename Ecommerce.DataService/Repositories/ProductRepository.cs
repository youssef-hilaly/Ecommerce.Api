using AutoMapper;
using Ecommerce.DataService.Data;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Product;
using Ecommerce.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataService.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context, UserManager<User> userManager, IMapper mapper) : base(context, userManager, mapper)
        {
        }

        public async Task<List<Product>> GetAllStoreProductsAsync(Guid storeId)
        {

            return await _context.Set<Product>().Where(x => x.StoreId == storeId).ToListAsync();
        }

        public async Task<Result<Product>> AddProductToStoreAsync(Product product, ClaimsPrincipal userClaim)
        {
            var result = await IsStoreOwner(product.StoreId, userClaim);
            if (!result.IsSuccess)
            {
                return result;
            }

            return await base.AddAsync(product);
        }
        public async Task<Result<Product>> UpdateProductToStoreAsync(UpdateProductDto updateProductDto, ClaimsPrincipal userClaim)
        {
            var product = await GetAsync(updateProductDto.Id);
            if (product == null) return Result<Product>.Failure("Product not found");

            // modify the product object that tracked by EF
            _mapper.Map(updateProductDto, product);

            var result = await IsStoreOwner(product.StoreId, userClaim);
            if (!result.IsSuccess) return result;

            await base.UpdateAsync(product);

            return Result<Product>.Success();
        }

        public async Task<Result<Product>> DeleteProductToStoreAsync(Guid id, ClaimsPrincipal userClaim)
        {
            var product = await GetAsync(id);
            if (product == null) return Result<Product>.Failure("Product not found");

            var result = await IsStoreOwner(product.StoreId, userClaim);
            if (!result.IsSuccess) return result;

            await base.DeleteAsync(product.Id);

            return Result<Product>.Success();
        }

        private async Task<Result<Product>> IsStoreOwner(Guid storeId, ClaimsPrincipal userClaim)
        {
            var userId = await GetUserIdByUserClaim(userClaim);

            if (userId == "")
            {
                return Result<Product>.Failure("User not found");
            }

            var store = await _context.Set<Store>().Where(x => x.Id == storeId).FirstOrDefaultAsync();

            if (store == null)
            {
                return Result<Product>.Failure("Store not found");
            }

            if (store.UserId != userId)
            {
                return Result<Product>.Failure("User is not the store owner");
            }

            return Result<Product>.Success();
        }

        private async Task<string> GetUserIdByUserClaim(ClaimsPrincipal userClaim)
        {
            var loggedInUser = await _userManager.GetUserAsync(userClaim);
            if (loggedInUser == null)
            {
                return "";
            }

            return loggedInUser.Id;

        }
    }
}
