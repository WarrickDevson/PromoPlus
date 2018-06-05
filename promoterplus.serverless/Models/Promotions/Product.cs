using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class Product
    {
        public Product()
        {
            Participant = new HashSet<Participant>();
        }

        public int Id { get; set; }
        public int? ClientId { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public Client Client { get; set; }
        public User ModifiedUser { get; set; }
        public virtual ICollection<Participant> Participant { get; set; }
        public virtual ICollection<PromotionProduct> PromotionProduct { get; set; }
    }
}
