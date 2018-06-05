using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class Media
    {
        public Media()
        {
            PromotionPromoterMedia = new HashSet<PromotionPromoterMedia>();
        }

        public int Id { get; set; }
        public int? MediaTypeId { get; set; }
        public byte[] MediaContent { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public Mediatype MediaType { get; set; }
        public User ModifiedUser { get; set; }
        public ICollection<PromotionPromoterMedia> PromotionPromoterMedia { get; set; }
    }
}
