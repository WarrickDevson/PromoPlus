using System;
using System.Collections.Generic;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Models
{
    public partial class Traffic
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public int PromoterId { get; set; }
        public int? GenderId { get; set; }
        public int? AgeId { get; set; }
        public int? RaceId { get; set; }
        public int? BuyingPowerId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public Age Age { get; set; }
        public BuyingPower BuyingPower { get; set; }
        public Promoter Promoter { get; set; }
        public Promotion Promotion { get; set; }
        public Gender Gender { get; set; }
        public User ModifiedUser { get; set; }
        public Race Race { get; set; }
    }
}
