using AutoMapper;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using static System.Formats.Asn1.AsnWriter;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository _storesRepository;
        private readonly ILogger<StoresController> _logger;
        private readonly IMapper _mapper;
        public StoresController(IMapper mapper, IStoreRepository storesRepository, ILogger<StoresController> logger)
        {
            _storesRepository = storesRepository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetStoreDto>>> GetStores()
        {
            try
            {
                var stores = await _storesRepository.GetAllAsync();
                return _mapper.Map<List<GetStoreDto>>(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetStores)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }


        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            try
            {
                bool success = await _storesRepository.DeleteAsync(id);
                return (success) ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteStore)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }

        }

        // POST: api/Stores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> PostStore(CreateStoreDto createStore)
        {
            try
            {
                var store = _mapper.Map<Store>(createStore);

                var result = await _storesRepository.AddAsync(store);

                return (result.IsSuccess) ? Created() : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostStore)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
