using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Enums;
using Ecommerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataService.Repositories.Interfaces
{
    public interface IOrderRepository: IGenericRepository<Order>
    {
        Task<Result<Order>> CreateOrder(List<OrderItem> items, ClaimsPrincipal userClaim);
        Task<Result<Order>> CancelOrderAsync(Guid id, ClaimsPrincipal userClaim);
        Task<Result<Order>> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status, ClaimsPrincipal userClaim);
    }
}
