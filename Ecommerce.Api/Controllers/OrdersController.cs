using AutoMapper;
using Ecommerce.DataService.Repositories;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Order;
using Ecommerce.Entities.Dtos.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        public OrdersController(IMapper mapper, IOrderRepository ordersRepository, ILogger<OrdersController> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Product>> PostOrder([FromBody] List<CreateOrderItemDto> createOrderItems)
        {
            try
            {
                var product = _mapper.Map<List<OrderItem>>(createOrderItems);

                var result = await _ordersRepository.CreateOrder(product, HttpContext.User);

                return (result.IsSuccess) ? Created() : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostOrder)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("UpdateOrderStatus/{id}")]
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            try
            {
                var result = await _ordersRepository.UpdateOrderStatusAsync(id, updateOrderStatusDto.Status, HttpContext.User);
                return (result.IsSuccess) ? NoContent() : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateOrderStatus)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("CancelOrder/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            try
            {
                var result = await _ordersRepository.CancelOrderAsync(id, HttpContext.User) ;
                return (result.IsSuccess) ? NoContent() : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CancelOrder)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
