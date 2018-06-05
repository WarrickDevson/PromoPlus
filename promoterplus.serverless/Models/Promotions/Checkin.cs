using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class Checkin
    {
        public int Id { get; set; }
        public int PromotionPromoterId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public User ModifiedUser { get; set; }
        public PromotionPromoter PromotionPromoter { get; set; }
    }
}
