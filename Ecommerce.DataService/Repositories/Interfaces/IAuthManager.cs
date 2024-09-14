using Ecommerce.Entities.Dtos.Users;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.DataService.Repositories.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserDto userDto);
        Task<AuthResponseDto> Login(LoginDto userDto);
        Task<string> CreateRefreshToken();
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
    }
}
