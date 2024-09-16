using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.Dtos.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public ILogger<AccountController> _logger { get; }

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email}");

            try
            {
                var errors = await _authManager.Register(userDto);
                return errors.Any() ? BadRequest(errors) : Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)} - User Registration Attempt {userDto.Email}");

                return Problem($"Internal Server Error in The {nameof(Register)}. Please Try Again Later.", statusCode: 500);
            }


        }


        // POST: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"Login Attempt for {loginDto.Email}");

            try
            {
                var authResponse = await _authManager.Login(loginDto);

                return authResponse != null ? Ok(authResponse) : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");

                return Problem($"Internal Server Error in The {nameof(Login)}. Please Try Again Later.", statusCode: 500);
            }


        }


        // POST: api/Account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            _logger.LogInformation($"Refresh Token Attempt for {request.Token}");

            try
            {
                var authResponse = await _authManager.VerifyRefreshToken(request);

                return authResponse != null ? Ok(authResponse) : Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(RefreshToken)}");

                return Problem($"Internal Server Error in The {nameof(RefreshToken)}. Please Try Again Later.", statusCode: 500);
            }

        }
    }
}
