using AutoMapper;
using Ecommerce.DataService.Repositories.Interfaces;
using Ecommerce.Entities.DbSets;
using Ecommerce.Entities.Dtos.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.DataService.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private User _user;

        private const string _loginProvider = "EcommerceApi";
        private const string _refreshToken = "RefreshToken";
        private const string _token = "Token";


        public AuthManager(IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"Login Attempt for {loginDto.Email}");

            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (_user == null)
            {
                _logger.LogWarning($"User with email: {loginDto.Email}, not found in database.");
                return null;
            }

            bool isValidCredintials = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (!isValidCredintials)
            {
                _logger.LogWarning($"User with email: {loginDto.Email}, entered wrong password.");
                return null;
            }

            var token = await GenerateToken();
            _logger.LogInformation($"Token generated for {loginDto.Email} | Token: {token}");
            return new AuthResponseDto { UserId = _user.Id, Token = token, RefreshToken = await CreateRefreshToken() };
        }

        public async Task<IEnumerable<IdentityError>> Register(UserDto userDto)
        {

            //if (errors.Any())
            //{
            //    foreach (var error in errors)
            //    {
            //        ModelState.AddModelError(error.Code, error.Description);
            //    }
            //    return BadRequest(ModelState);
            //}

            bool adminRoleExists = await _roleManager.RoleExistsAsync(userDto.Role);
            if (!adminRoleExists)
            {
                return new List<IdentityError>
                {
                    new IdentityError
                    {
                        Code = "NotFound",
                        Description = "Role not found"
                    }
                };
            }
            _user = _mapper.Map<User>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
               await _userManager.AddToRoleAsync(_user, userDto.Role);
            }

            return result.Errors;
        }
        public async Task<string> CreateRefreshToken()
        {
            var result0 = await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            var refreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, refreshToken);

            return refreshToken;

        }
        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            var userName = tokenContent.Claims.ToList().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            _user = await _userManager.FindByEmailAsync(userName);

            if (_user == null || _user.Id != request.UserId) return null;

            //await _userManager.token

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);


            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto { UserId = _user.Id, Token = token, RefreshToken = await CreateRefreshToken() };
            }

            await _userManager.UpdateSecurityStampAsync(_user);

            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id)
            }.Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(
                    _configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
