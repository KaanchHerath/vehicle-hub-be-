﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using reservation_system_be.Models;

namespace reservation_system_be.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<AdditionalFeatures> AdditionalFeatures { get; set; }
        public DbSet<VehicleInsurance> VehicleInsurances { get; set; }
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
        public DbSet<VehicleLog> VehicleLogs { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleMake> VehicleMake { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AdminNotification> AdminNotifications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<CustomerReservation> CustomerReservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the composite key for Wishlist
            modelBuilder.Entity<Wishlist>().HasKey(w => new { w.VehicleId, w.CustomerId });


            var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
            timeOnly => timeOnly.ToTimeSpan(),
            timeSpan => TimeOnly.FromTimeSpan(timeSpan));

            modelBuilder.Entity<Reservation>()
                .Property(e => e.StartTime)
                .HasColumnType("time")
                .HasConversion(timeOnlyConverter);

            modelBuilder.Entity<Reservation>()
                .Property(e => e.EndTime)
                .HasColumnType("time")
                .HasConversion(timeOnlyConverter);
        }
    }
}
