using Microsoft.EntityFrameworkCore;
using reservation_system_be.Models;

namespace reservation_system_be.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base (options){ }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleInsurance> VehicleInsurances { get; set; }
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
        public DbSet<VehicleLog> VehicleLogs { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<FeedbackType> FeedbackTypes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<CustomerReservation> CustomersReservation { get; set; }
        public DbSet<CustomerWishlist> CustomersWishlists { get;set; }
        public DbSet<WishlistVehicle> WishlistsVehicles { get;set; }
    }
}
