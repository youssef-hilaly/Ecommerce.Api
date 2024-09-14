using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities.Dtos.Product
{
    public class CreateProductDto: BaseProductDto
    {
        public Guid StoreId { get; set; }
    }
}
