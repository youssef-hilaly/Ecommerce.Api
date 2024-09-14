using Ecommerce.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities.DbSets
{
    public class Order : BaseEntity
    {
        [ForeignKey(nameof(Client))]
        public string UserId { get; set; }
        public User Client { get; set; }

        [ForeignKey(nameof(Store))]
        public Guid StoreId { get; set; }
        public Store Store { get; set; }

        public OrderStatusEnum Status { get; set; }

        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
