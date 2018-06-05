using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class PromotionPromoter
    {
        public PromotionPromoter()
        {
            PromotionPromoterMedia = new HashSet<PromotionPromoterMedia>();
            Checkin = new HashSet<Checkin>();
        }

        public int Id { get; set; }
        public int PromotionId { get; set; }
        public int PromoterId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public User ModifiedUser { get; set; }
        public Promoter Promoter { get; set; }
        public Promotion Promotion { get; set; }
        public ICollection<Checkin> Checkin { get; set; }
        public ICollection<PromotionPromoterMedia> PromotionPromoterMedia { get; set; }

    }
}
