using Ecommerce.Entities.DbSets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities.Dtos.Store
{
    public class CreateStoreDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
