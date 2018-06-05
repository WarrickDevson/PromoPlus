using System;
using System.ComponentModel.DataAnnotations.Schema;
using promoterplus.serverless.Models.Admin;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class StockCount
    {
        public int Id { get; set; }
        public int PromotionProductId { get; set; }
        public int? PromoterId { get; set; }
        public int Count { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public int ProductId { get; set; }
        public User ModifiedUser { get; set; }
        public Promoter Promoter { get; set; }
        public PromotionProduct PromotionProduct { get; set; }
    }
}
