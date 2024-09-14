using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entities.Dtos.Users
{
    public class UserDto: LoginDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
