using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookingApp.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace BookingApp.Data
{
    public class AppContextDB : IdentityDbContext<User>
    {
        public AppContextDB(DbContextOptions<AppContextDB> options) : base(options) { }

        public DbSet<Accommodation> Accommodations { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Amenity> Amenity { get; set; }

        public DbSet<Bookmark> Bookmark { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // إجبار جميع خصائص DateTime أن تكون بتوقيت UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }

                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime?, DateTime?>(
                            v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()) : v,
                            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
                        ));
                    }
                }
            }
        }
    }
}
