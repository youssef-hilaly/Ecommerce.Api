using AutoMapper;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Product;
using Ecommerce.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        public ProductsController(IMapper mapper, IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return _mapper.Map<List<GetProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetProducts)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductDto>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.GetAsync(id);
                if (product == null) return NotFound();

                return _mapper.Map<GetProductDto>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "StoreOwner")]
        public async Task<ActionResult<Product>> PostProduct([FromBody] CreateProductDto createProduct)
        {
            try
            {
                var product = _mapper.Map<Product>(createProduct);

                var result = await _productRepository.AddProductToStoreAsync(product, HttpContext.User);

                return (result.IsSuccess) ? Created() : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> PutProduct(UpdateProductDto updateProductDto)
        {
            try
            {
                var product = await _productRepository.GetAsync(updateProductDto.Id);
                if (product == null) return NotFound();

                // modify the product object that tracked by EF
                _mapper.Map(updateProductDto, product);

                var result = await _productRepository.UpdateProductToStoreAsync(product, HttpContext.User);
                return (result.IsSuccess) ? NoContent() : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PutProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productRepository.DeleteProductToStoreAsync(id, HttpContext.User);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }
    }
}
