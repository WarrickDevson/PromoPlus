using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public class PromotionProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PromotionId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public User ModifiedUser { get; set; }

        public virtual Product Product { get; set; }

        public virtual Promotion Promotion { get; set; }

        public virtual ICollection<StockCount> StockCount { get; set; }
    }
}
