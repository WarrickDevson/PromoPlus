using System;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;

namespace promoterplus.serverless.Models.Promotions
{
    public partial class Participant
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public int PromoterId { get; set; }
        public int ProductId { get; set; }
        public int ParticipantTypeId { get; set; }
        public int? RepetitionTypeId { get; set; }
        public int? GenderId { get; set; }
        public int? AgeId { get; set; }
        public int? RaceId { get; set; }
        public int? BuyingPowerId { get; set; }
        public int? FeedbackId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        public Age Age { get; set; }
        public BuyingPower BuyingPower { get; set; }
        public Feedback Feedback { get; set; }
        public Gender Gender { get; set; }
        public User ModifiedUser { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public Product Product { get; set; }
        public Promoter Promoter { get; set; }
        public Promotion Promotion { get; set; }
        public Race Race { get; set; }
        public RepetitionType RepetitionType { get; set; }
    }
}
