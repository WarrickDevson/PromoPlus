using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class Promotion
    {
        public Promotion()
        {
            ProductIds = new List<int>();
            PromoterIds = new List<int>();
        }

        public int Id { get; set; }
        public int ClientId { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public List<int> ProductIds { get; set; }
        [NotMapped]
        public List<int> PromoterIds { get; set; }

        public virtual Client Client { get; set; }
        public virtual Location Location { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual ICollection<Participant> Participant { get; set; } = new HashSet<Participant>();

        public virtual ICollection<Traffic> Traffic { get; set; } = new HashSet<Traffic>();
        public virtual ICollection<PromotionProduct> PromotionProduct { get; set; } = new HashSet<PromotionProduct>();
        public virtual ICollection<PromotionPromoter> PromotionPromoter { get; set; } = new HashSet<PromotionPromoter>();
    }
}
