using System;
using System.Collections.Generic;
using promoterplus.serverless.Controllers.Promotions;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class Promoter
    {
        public Promoter()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool IsActive { get; set; }
        public User ModifiedUser { get; set; }
        public virtual ICollection<StockCount> StockCount { get; set; } = new HashSet<StockCount>();
        public virtual ICollection<PromotionPromoter> PromotionPromoter { get; set; } = new HashSet<PromotionPromoter>();
        public virtual ICollection<Participant> Participant { get; set; } = new HashSet<Participant>();
        public virtual ICollection<Traffic> Traffic { get; set; } = new HashSet<Traffic>();
    }
}
