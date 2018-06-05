using System;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class PromotionPromoterMedia
    {
        public int Id { get; set; }
        public int PromotionPromoterId { get; set; }
        public int MediaId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public Media Media { get; set; }
        public User ModifiedUser { get; set; }
        public PromotionPromoter PromotionPromoter { get; set; }
    }
}
