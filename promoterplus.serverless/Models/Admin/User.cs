using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using promoterplus.serverless.Models.Lookups;
using promoterplus.serverless.Models.Promotions;

namespace promoterplus.serverless.Models.Admin
{
    public partial class User
    {
        public User()
        {
            Age = new HashSet<Age>();
            BuyingPower = new HashSet<BuyingPower>();
            Checkin = new HashSet<Checkin>();
            Client = new HashSet<Client>();
            Feedback = new HashSet<Feedback>();
            Gender = new HashSet<Gender>();
            Location = new HashSet<Location>();
            Media = new HashSet<Media>();
            Mediatype = new HashSet<Mediatype>();
            Participant = new HashSet<Participant>();
            ParticipantType = new HashSet<ParticipantType>();
            Product = new HashSet<Product>();
            Promoter = new HashSet<Promoter>();
            Promotion = new HashSet<Promotion>();
            PromotionProduct = new HashSet<PromotionProduct>();
            PromotionPromoter = new HashSet<PromotionPromoter>();
            PromotionPromoterMedia = new HashSet<PromotionPromoterMedia>();
            Race = new HashSet<Race>();
            Repetitiontype = new HashSet<RepetitionType>();
            StockCount = new HashSet<StockCount>();
            Traffic = new HashSet<Traffic>();
            UserclientModifiedUser = new HashSet<UserClient>();
            UserclientUser = new HashSet<UserClient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        public string Latitude { get; set; }
                [NotMapped]
        public string Longitude { get; set; }

        public ICollection<Age> Age { get; set; }
        public ICollection<BuyingPower> BuyingPower { get; set; }
        public ICollection<Checkin> Checkin { get; set; }
        public ICollection<Client> Client { get; set; }
        public ICollection<Feedback> Feedback { get; set; }
        public ICollection<Gender> Gender { get; set; }
        public ICollection<Location> Location { get; set; }
        public ICollection<Media> Media { get; set; }
        public ICollection<Mediatype> Mediatype { get; set; }
        public ICollection<Participant> Participant { get; set; }
        public ICollection<ParticipantType> ParticipantType { get; set; }
        public ICollection<Product> Product { get; set; }
        public ICollection<Promoter> Promoter { get; set; }
        public ICollection<Promotion> Promotion { get; set; }
        public ICollection<PromotionProduct> PromotionProduct { get; set; }
        public ICollection<PromotionPromoter> PromotionPromoter { get; set; }
        public ICollection<PromotionPromoterMedia> PromotionPromoterMedia { get; set; }
        public ICollection<Race> Race { get; set; }
        public ICollection<RepetitionType> Repetitiontype { get; set; }
        public ICollection<StockCount> StockCount { get; set; }
        public ICollection<Traffic> Traffic { get; set; }
        public ICollection<UserClient> UserclientModifiedUser { get; set; }
        public ICollection<UserClient> UserclientUser { get; set; }
    }
}
