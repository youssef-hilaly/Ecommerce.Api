using AutoMapper;
using Ecommerce.DataService.Data;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ecommerce.Entities.Enums;


namespace Ecommerce.DataService.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, UserManager<User> userManager, IMapper mapper) : base(context, userManager, mapper)
        {
        }

        public async Task<Result<Order>> CreateOrder(List<OrderItem> items, ClaimsPrincipal userClaim)
        {
            if(items.Count == 0) return Result<Order>.Failure("Items list is empty");

            var userId = await GetUserIdByUserClaim(userClaim);
            if (userId == "") return Result<Order>.Failure("User not found");


            decimal totalPrice = 0;

            List<Product> products = new List<Product>();

            foreach (var item in items)
            {
                var product = await _context.Set<Product>().Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                if (product == null) return Result<Order>.Failure("Product not found");

                products.Add(product);
                totalPrice += product.Price * item.Quantity;
            }

            // check if all the products are from the same store
            var storeId = products[0].StoreId;
            foreach (var product in products)
            {
                if (storeId != product.StoreId)
                {
                    return Result<Order>.Failure("All products must be from the same store");
                }
            }

            var result = await AddAsync(new Order
            {
                UserId = userId,
                StoreId = storeId,
                TotalPrice = totalPrice
            });

            if (!result.IsSuccess) return result;

            foreach (var item in items)
            {
                await _context.AddAsync(new OrderItem
                {
                    OrderId = result.Data.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return Result<Order>.Success();
        }

        public async Task<Result<Order>> CancelOrderAsync(Guid id, ClaimsPrincipal userClaim)
        {
            var order = await GetAsync(id);
            if (order == null) return Result<Order>.Failure("Order not found");

            var userId = await GetUserIdByUserClaim(userClaim);
            if (userId == "") return Result<Order>.Failure("User not found");

            if (order.UserId != userId) return Result<Order>.Failure("User is not the order owner");

            if(order.Status == OrderStatusEnum.Delivered) return Result<Order>.Failure("Order is delivered");

            order.Status = OrderStatusEnum.Canceled;
            await _context.SaveChangesAsync();
            return Result<Order>.Success();

        }

        public async Task<Result<Order>> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status, ClaimsPrincipal userClaim)
        {
            var order = await GetAsync(id);
            if (order == null) return Result<Order>.Failure("Order not found");

            var storeOwnerId = await GetUserIdByUserClaim(userClaim);
            if (storeOwnerId == "") return Result<Order>.Failure("User not found");

            var store = await _context.Set<Store>().Where(x => x.Id == order.StoreId).FirstOrDefaultAsync();
            if (store == null) return Result<Order>.Failure("Store not found");

            if (store.UserId != storeOwnerId) return Result<Order>.Failure("User is not the store owner");

            if(order.Status == OrderStatusEnum.Canceled) return Result<Order>.Failure("Order is canceled");

            if(order.Status == OrderStatusEnum.Delivered) return Result<Order>.Failure("Order is delivered");

            order.Status = status;
            await _context.SaveChangesAsync();
            return Result<Order>.Success();
        }

        private async Task<string> GetUserIdByUserClaim(ClaimsPrincipal userClaim)
        {
            var loggedInUser = await _userManager.GetUserAsync(userClaim);
            if (loggedInUser == null) return "";

            return loggedInUser.Id;
        }
    }
}
