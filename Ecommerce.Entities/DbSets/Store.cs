using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Entities.DbSets
{
    public class Store : BaseEntity
    {
        public string Name { get; set; }

        [ForeignKey(nameof(Owner))]
        public string UserId { get; set; }
        public User Owner { get; set; }
    }
}
