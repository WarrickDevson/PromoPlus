using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using promoterplus.serverless.Models.Admin;
using promoterplus.serverless.Models.Lookups;
using promoterplus.serverless.Models.Promotions;
using Client = promoterplus.serverless.Models.Admin.Client;
using Feedback = promoterplus.serverless.Models.Lookups.Feedback;
using Gender = promoterplus.serverless.Models.Lookups.Gender;
using Location = promoterplus.serverless.Models.Promotions.Location;
using Participant = promoterplus.serverless.Models.Promotions.Participant;
using Product = promoterplus.serverless.Models.Promotions.Product;
using Promoter = promoterplus.serverless.Models.Promotions.Promoter;
using Promotion = promoterplus.serverless.Models.Promotions.Promotion;
using Race = promoterplus.serverless.Models.Lookups.Race;
using User = promoterplus.serverless.Models.Admin.User;

namespace promoterplus.serverless.Models
{
    public partial class PromoterPlusContext : DbContext
    {
        public virtual DbSet<Age> Age { get; set; }
        public virtual DbSet<BuyingPower> BuyingPower { get; set; }
        public virtual DbSet<Checkin> Checkin { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<Mediatype> Mediatype { get; set; }
        public virtual DbSet<Participant> Participant { get; set; }
        public virtual DbSet<ParticipantType> ParticipantType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Promoter> Promoter { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionProduct> PromotionProduct { get; set; }
        public virtual DbSet<PromotionPromoter> PromotionPromoter { get; set; }
        public virtual DbSet<PromotionPromoterMedia> PromotionPromoterMedia { get; set; }
        public virtual DbSet<Race> Race { get; set; }
        public virtual DbSet<RepetitionType> RepetitionType { get; set; }
        public virtual DbSet<StockCount> StockCount { get; set; }
        public virtual DbSet<Traffic> Traffic { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserClient> UserClient { get; set; }

        public PromoterPlusContext(DbContextOptions<PromoterPlusContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Age>(entity =>
            {
                entity.ToTable("age");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Age_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Age)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Age_User");
            });

            modelBuilder.Entity<BuyingPower>(entity =>
            {
                entity.ToTable("buyingpower");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_BuyingPower_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.BuyingPower)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuyingPower_User");
            });

            modelBuilder.Entity<Checkin>(entity =>
            {
                entity.ToTable("checkin");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_CheckIn_User_idx");

                entity.HasIndex(e => e.PromotionPromoterId)
                    .HasName("FK_CheckIn_PromotionPromoter_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.Latitude).HasMaxLength(45);

                entity.Property(e => e.Longitude).HasMaxLength(45);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionPromoterId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Checkin)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CheckIn_User");

                entity.HasOne(d => d.PromotionPromoter)
                    .WithMany(p => p.Checkin)
                    .HasForeignKey(d => d.PromotionPromoterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CheckIn_PromotionPromoter");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Client_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Client_User");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Feedback_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Feedback)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_User");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Gender_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Gender)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Gender_User");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Location_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.Label).HasMaxLength(100);

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Location_User");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.ToTable("media");

                entity.HasIndex(e => e.MediaTypeId)
                    .HasName("FK_Media_MediaType_idx");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Media_User_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.MediaContent).HasColumnName("Media");

                entity.Property(e => e.MediaTypeId).HasColumnType("int(11)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.MediaType)
                    .WithMany(p => p.Media)
                    .HasForeignKey(d => d.MediaTypeId)
                    .HasConstraintName("FK_Media_MediaType");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Media)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_User");
            });

            modelBuilder.Entity<Mediatype>(entity =>
            {
                entity.ToTable("mediatype");

                entity.HasIndex(e => e.Description)
                    .HasName("Fk_MediaTypeUser");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Media_Type_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Mediatype)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_Type");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("participant");

                entity.HasIndex(e => e.AgeId)
                    .HasName("FK_Participant_Age");

                entity.HasIndex(e => e.BuyingPowerId)
                    .HasName("FK_Participant_BuyingPower");

                entity.HasIndex(e => e.FeedbackId)
                    .HasName("FK_Participant_Feedback");

                entity.HasIndex(e => e.GenderId)
                    .HasName("FK_Participant_Gender");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Participant_User");

                entity.HasIndex(e => e.ParticipantTypeId)
                    .HasName("FK_Participant_ParticipantType");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_Participant_Product");

                entity.HasIndex(e => e.PromoterId)
                    .HasName("FK_Participant_Promoter");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("FK_Participant_Promotion");

                entity.HasIndex(e => e.RaceId)
                    .HasName("FK_Participant_Race");

                entity.HasIndex(e => e.RepetitionTypeId)
                    .HasName("FK_Participant_RepetitionType");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AgeId).HasColumnType("int(11)");

                entity.Property(e => e.BuyingPowerId).HasColumnType("int(11)");

                entity.Property(e => e.FeedbackId).HasColumnType("int(11)");

                entity.Property(e => e.GenderId).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.ParticipantTypeId).HasColumnType("int(11)");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.PromoterId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionId).HasColumnType("int(11)");

                entity.Property(e => e.RaceId).HasColumnType("int(11)");

                entity.Property(e => e.RepetitionTypeId).HasColumnType("int(11)");

                entity.HasOne(d => d.Age)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.AgeId)
                    .HasConstraintName("FK_Participant_Age");

                entity.HasOne(d => d.BuyingPower)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.BuyingPowerId)
                    .HasConstraintName("FK_Participant_BuyingPower");

                entity.HasOne(d => d.Feedback)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.FeedbackId)
                    .HasConstraintName("FK_Participant_Feedback");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Participant_Gender");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participant_User");

                entity.HasOne(d => d.ParticipantType)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.ParticipantTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participant_ParticipantType");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participant_Product");

                entity.HasOne(d => d.Promoter)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.PromoterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participant_Promoter");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participant_Promotion");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.RaceId)
                    .HasConstraintName("FK_Participant_Race");

                entity.HasOne(d => d.RepetitionType)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.RepetitionTypeId)
                    .HasConstraintName("FK_Participant_RepetitionType");
            });

            modelBuilder.Entity<ParticipantType>(entity =>
            {
                entity.ToTable("participanttype");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_SampleType_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.ParticipantType)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SampleType_User");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_Product_Client");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Product_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClientId).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.Label).HasMaxLength(100);

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_User");
            });

            modelBuilder.Entity<Promoter>(entity =>
            {
                entity.ToTable("promoter");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Promoter_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Promoter)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_Promoter_User");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.ToTable("promotion");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_Promotion_Client");

                entity.HasIndex(e => e.LocationId)
                    .HasName("FK_Promotion_Location");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Promotion_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClientId).HasColumnType("int(11)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.LocationId).HasColumnType("int(11)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promotion_User");
            });

            modelBuilder.Entity<PromotionProduct>(entity =>
            {
                entity.ToTable("promotionproduct");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_PromotionProduct_User");

                entity.HasIndex(e => e.ProductId)
                    .HasName("FK_PromotionProduct_Product");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("FK_PromotionProduct_Promotion");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.ProductId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PromotionProduct)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionProduct_User");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PromotionProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionProduct_Product");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionProduct)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionProduct_Promotion");
            });

            modelBuilder.Entity<PromotionPromoter>(entity =>
            {
                entity.ToTable("promotionpromoter");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_PromotionPromoter_User");

                entity.HasIndex(e => e.PromoterId)
                    .HasName("FK_PromotionPromoter_Promoter");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("FK_PromotionPromoter_Promotion");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.PromoterId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PromotionPromoter)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_PromotionPromoter_User");

                entity.HasOne(d => d.Promoter)
                    .WithMany(p => p.PromotionPromoter)
                    .HasForeignKey(d => d.PromoterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionPromoter_Promoter");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionPromoter)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionPromoter_Promotion");
            });

            modelBuilder.Entity<PromotionPromoterMedia>(entity =>
            {
                entity.ToTable("promotionpromotermedia");

                entity.HasIndex(e => e.MediaId)
                    .HasName("FK_PromotionPromoterMedia_Media_idx");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_PromotionPromoterMedia_User_idx");

                entity.HasIndex(e => e.PromotionPromoterId)
                    .HasName("FK_PromotionPromoterMedia_PromotionPromoter_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.MediaId).HasColumnType("int(11)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionPromoterId).HasColumnType("int(11)");

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.PromotionPromoterMedia)
                    .HasForeignKey(d => d.MediaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionPromoterMedia_Media");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.PromotionPromoterMedia)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionPromoterMedia_User");

                entity.HasOne(d => d.PromotionPromoter)
                    .WithMany(p => p.PromotionPromoterMedia)
                    .HasForeignKey(d => d.PromotionPromoterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionPromoterMedia_PromotionPromoter");
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("race");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Race_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Race)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Race_User");
            });

            modelBuilder.Entity<RepetitionType>(entity =>
            {
                entity.ToTable("repetitiontype");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_RepetitionType_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Repetitiontype)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RepetitionType_User");
            });

            modelBuilder.Entity<StockCount>(entity =>
            {
                entity.ToTable("stockcount");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_StockCount_User");

                entity.HasIndex(e => e.PromoterId)
                    .HasName("FK_StockCount_Promoter");

                entity.HasIndex(e => e.PromotionProductId)
                    .HasName("FK_StockCount_PromotionProduct");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Count).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.PromoterId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionProductId).HasColumnType("int(11)");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.StockCount)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockCount_User");

                entity.HasOne(d => d.Promoter)
                    .WithMany(p => p.StockCount)
                    .HasForeignKey(d => d.PromoterId)
                    .HasConstraintName("FK_StockCount_Promoter");

                entity.HasOne(d => d.PromotionProduct)
                    .WithMany(p => p.StockCount)
                    .HasForeignKey(d => d.PromotionProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockCount_PromotionProduct");
            });

            

            modelBuilder.Entity<Traffic>(entity =>
            {
                entity.ToTable("traffic");

                entity.HasIndex(e => e.AgeId)
                    .HasName("FK_Traffic_Age");

                entity.HasIndex(e => e.BuyingPowerId)
                    .HasName("FK_Traffic_BuyingPower");

                entity.HasIndex(e => e.GenderId)
                    .HasName("FK_Traffic_Gender");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_Traffic_User");

                entity.HasIndex(e => e.RaceId)
                    .HasName("FK_Traffic_Race");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AgeId).HasColumnType("int(11)");

                entity.Property(e => e.BuyingPowerId).HasColumnType("int(11)");

                entity.Property(e => e.GenderId).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.PromoterId).HasColumnType("int(11)");

                entity.Property(e => e.PromotionId).HasColumnType("int(11)");

                entity.Property(e => e.RaceId).HasColumnType("int(11)");

                entity.HasOne(d => d.Age)
                    .WithMany(p => p.Traffic)
                    .HasForeignKey(d => d.AgeId)
                    .HasConstraintName("FK_Traffic_Age");

                entity.HasOne(d => d.BuyingPower)
                    .WithMany(p => p.Traffic)
                    .HasForeignKey(d => d.BuyingPowerId)
                    .HasConstraintName("FK_Traffic_BuyingPower");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Traffic)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Traffic_Gender");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.Traffic)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Traffic_User");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.Traffic)
                    .HasForeignKey(d => d.RaceId)
                    .HasConstraintName("FK_Traffic_Race");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.ToTable("userclient");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_UserClient_Client");

                entity.HasIndex(e => e.ModifiedUserId)
                    .HasName("FK_UserClient_User1");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_UserClient_User");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClientId).HasColumnType("int(11)");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(1)");

                entity.Property(e => e.ModifiedUserId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Userclient)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserClient_Client");

                entity.HasOne(d => d.ModifiedUser)
                    .WithMany(p => p.UserclientModifiedUser)
                    .HasForeignKey(d => d.ModifiedUserId)
                    .HasConstraintName("FK_UserClient_User1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserclientUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserClient_User");
            });
        }
    }
}
