using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BadmintonReservationData
{
    public partial class NET1711_231_1_BadmintonReservationContext : DbContext
    {
        public NET1711_231_1_BadmintonReservationContext()
        {
        }

        public NET1711_231_1_BadmintonReservationContext(DbContextOptions<NET1711_231_1_BadmintonReservationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingDetail> BookingDetails { get; set; } = null!;
        public virtual DbSet<BookingType> BookingTypes { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Court> Courts { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<DateType> DateTypes { get; set; } = null!;
        public virtual DbSet<Frame> Frames { get; set; } = null!;
        public virtual DbSet<Holiday> Holidays { get; set; } = null!;
        public virtual DbSet<Operation> Operations { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PurchasedHoursMonthly> PurchasedHoursMonthlies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true).Build();
            return config["ConnectionStrings:DefaultConnectionStringDB"];
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in this.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                var now = DateTime.Now;
                entry.Property("UpdatedDate").CurrentValue = now;
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedDate").IsModified = false;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = now;
                }
            }

            var numberChange = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            this.ChangeTracker.Clear();
            return numberChange;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookingTypeId).HasColumnName("booking_type_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.PaymentType).HasColumnName("payment_type");

                entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");

                entity.Property(e => e.PromotionAmount).HasColumnName("promotion_amount");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.BookingType)
                    .WithMany()
                    .HasForeignKey(d => d.BookingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_booking_type_FK");

                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_customer_FK");

                entity.HasOne(d => d.Payment)
                    .WithMany()
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_payment_FK");
            });

            modelBuilder.Entity<BookingDetail>(entity =>
            {
                entity.ToTable("booking_detail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookDate)
                    .HasColumnType("datetime")
                    .HasColumnName("book_date");

                entity.Property(e => e.BookingId).HasColumnName("booking_id");

                entity.Property(e => e.CheckinTime)
                    .HasColumnType("date")
                    .HasColumnName("checkin_time");

                entity.Property(e => e.CheckoutTime)
                    .HasColumnType("date")
                    .HasColumnName("checkout_time");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.FrameId).HasColumnName("frame_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TimeFrom)
                    .HasColumnName("time_from");

                entity.Property(e => e.TimeTo)
                    .HasColumnName("time_to");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                //entity.HasOne(d => d.Booking)
                //    .WithMany(p => p.BookingDetails)
                //    .HasForeignKey(d => d.BookingId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("booking_detail_booking_FK");

                entity.HasOne(d => d.Frame)
                    .WithMany()
                    .HasForeignKey(d => d.FrameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("booking_detail_frame_FK");
            });

            modelBuilder.Entity<BookingType>(entity =>
            {
                entity.ToTable("booking_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PromotionAmount).HasColumnName("promotion_amount");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("company");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .HasColumnName("description");

                entity.Property(e => e.Location)
                    .HasMaxLength(400)
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Slogan)
                    .HasMaxLength(300)
                    .HasColumnName("slogan");
            });

            modelBuilder.Entity<Court>(entity =>
            {
                entity.ToTable("court");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Amentities)
                                  .HasMaxLength(512)
                                  .HasColumnName("amentities");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.CloseHours).HasColumnName("close_hours");

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .HasColumnName("name");

                entity.Property(e => e.CourtType).HasColumnName("court_type");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.Property(e => e.OpeningHours).HasColumnName("opening_hours");

                entity.Property(e => e.SurfaceType).HasColumnName("surface_type");

                entity.Property(e => e.TotalBooking)
                    .HasMaxLength(512)
                    .HasColumnName("total_booking");

            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");
                
                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .HasColumnName("full_name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");

                entity.Property(e => e.TotalHoursMonthly).HasColumnName("total_hours_monthly");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<DateType>(entity =>
            {
                entity.ToTable("date_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<Frame>(entity =>
            {
                entity.ToTable("frame");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourtId).HasColumnName("court_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Label)
                    .HasMaxLength(512)
                    .HasColumnName("label");

                entity.Property(e => e.Note)
                    .HasMaxLength(512)
                    .HasColumnName("note");

                entity.Property(e => e.TimeFrom)
                    .HasColumnName("time_from");

                entity.Property(e => e.TimeTo)
                    .HasColumnName("time_to");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.Court)
                    .WithMany()
                    .HasForeignKey(d => d.CourtId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("frame_court_FK");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.ToTable("holiday");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.ToTable("operation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.OpenTimeFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("open_time_from");

                entity.Property(e => e.OpenTimeTo)
                    .HasColumnType("datetime")
                    .HasColumnName("open_time_to");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.ThirdPartyPaymentId).HasColumnName("third_party_payment_id");

                entity.Property(e => e.ThirdPartyResponse).HasColumnName("third_party_response");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");
            });

            modelBuilder.Entity<PurchasedHoursMonthly>(entity =>
            {
                entity.ToTable("purchased_hours_monthly");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AmountHour).HasColumnName("amount_hour");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.PurchasedHoursMonthlies)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("purchased_hours_monthly_customer_FK");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PurchasedHoursMonthlies)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("purchased_hours_monthly_payment_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
