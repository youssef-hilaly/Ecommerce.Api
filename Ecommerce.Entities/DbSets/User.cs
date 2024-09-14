using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Entities.DbSets
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
