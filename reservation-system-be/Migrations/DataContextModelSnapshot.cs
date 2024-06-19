﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using reservation_system_be.Data;

#nullable disable

namespace reservation_system_be.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("reservation_system_be.Models.AdditionalFeatures", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ABS")
                        .HasColumnType("bit");

                    b.Property<bool>("AcFront")
                        .HasColumnType("bit");

                    b.Property<bool>("AirbagDriver")
                        .HasColumnType("bit");

                    b.Property<bool>("AirbagPassenger")
                        .HasColumnType("bit");

                    b.Property<bool>("AirbagSide")
                        .HasColumnType("bit");

                    b.Property<bool>("AlloyWheels")
                        .HasColumnType("bit");

                    b.Property<bool>("AutomaticHeadlights")
                        .HasColumnType("bit");

                    b.Property<bool>("Bluetooth")
                        .HasColumnType("bit");

                    b.Property<bool>("ElectricMirrors")
                        .HasColumnType("bit");

                    b.Property<bool>("FogLights")
                        .HasColumnType("bit");

                    b.Property<bool>("KeylessEntry")
                        .HasColumnType("bit");

                    b.Property<bool>("NavigationSystem")
                        .HasColumnType("bit");

                    b.Property<bool>("ParkingSensor")
                        .HasColumnType("bit");

                    b.Property<bool>("PowerWindow")
                        .HasColumnType("bit");

                    b.Property<bool>("RearWindowWiper")
                        .HasColumnType("bit");

                    b.Property<bool>("SecuritySystem")
                        .HasColumnType("bit");

                    b.Property<bool>("Sunroof")
                        .HasColumnType("bit");

                    b.Property<bool>("TintedGlass")
                        .HasColumnType("bit");

                    b.Property<int>("VehicleModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleModelId")
                        .IsUnique();

                    b.ToTable("AdditionalFeatures");
                });

            modelBuilder.Entity("reservation_system_be.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContactNo")
                        .HasColumnType("int");

                    b.Property<string>("DrivingLicenseNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OtpExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetOtp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("reservation_system_be.Models.CustomerReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("ReservationId")
                        .HasColumnType("int");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ReservationId")
                        .IsUnique();

                    b.HasIndex("VehicleId");

                    b.ToTable("CustomerReservations");
                });

            modelBuilder.Entity("reservation_system_be.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContactNo")
                        .HasColumnType("int");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("reservation_system_be.Models.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerReservationId")
                        .HasColumnType("int");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Feedback_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Feedback_Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("RatingNo")
                        .HasColumnType("int");

                    b.Property<string>("Service_Review")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vehicle_Review")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerReservationId")
                        .IsUnique();

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("reservation_system_be.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<int>("CustomerReservationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerReservationId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("reservation_system_be.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CustomerReservationId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Generated_DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VehicleInsuranceID")
                        .HasColumnType("int");

                    b.Property<int?>("VehicleMaintenanceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerReservationId");

                    b.HasIndex("VehicleInsuranceID");

                    b.HasIndex("VehicleMaintenanceId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("reservation_system_be.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("reservation_system_be.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("reservation_system_be.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ChassisNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Colour")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("CostPerDay")
                        .HasColumnType("real");

                    b.Property<float>("CostPerExtraKM")
                        .HasColumnType("real");

                    b.Property<string>("DashboardImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("FrontImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InteriorImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<string>("RearImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Transmission")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleModelId")
                        .HasColumnType("int");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("VehicleModelId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<int>("ReservationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId")
                        .IsUnique();

                    b.ToTable("VehicleAvailability");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleInsurance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InsuranceNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("VehicleInsurances");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerReservationId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EndMileage")
                        .HasColumnType("int");

                    b.Property<int>("ExtraKM")
                        .HasColumnType("int");

                    b.Property<int>("Penalty")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerReservationId")
                        .IsUnique();

                    b.ToTable("VehicleLogs");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleMaintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentMileage")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("VehicleMaintenances");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleMake", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VehicleMake");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EngineCapacity")
                        .HasColumnType("int");

                    b.Property<string>("Fuel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatingCapacity")
                        .HasColumnType("int");

                    b.Property<int>("VehicleMakeId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VehicleMakeId");

                    b.ToTable("VehicleModels");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("DepositAmount")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("reservation_system_be.Models.Wishlist", b =>
                {
                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.HasKey("VehicleId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("reservation_system_be.Models.AdditionalFeatures", b =>
                {
                    b.HasOne("reservation_system_be.Models.VehicleModel", "VehicleModel")
                        .WithOne("AdditionalFeatures")
                        .HasForeignKey("reservation_system_be.Models.AdditionalFeatures", "VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleModel");
                });

            modelBuilder.Entity("reservation_system_be.Models.CustomerReservation", b =>
                {
                    b.HasOne("reservation_system_be.Models.Customer", "Customer")
                        .WithMany("CusReservation")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("reservation_system_be.Models.Reservation", "Reservation")
                        .WithOne("CustomerReservation")
                        .HasForeignKey("reservation_system_be.Models.CustomerReservation", "ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("reservation_system_be.Models.Vehicle", "Vehicle")
                        .WithMany("CusReservation")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Reservation");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("reservation_system_be.Models.Feedback", b =>
                {
                    b.HasOne("reservation_system_be.Models.CustomerReservation", "CustomerReservation")
                        .WithOne("Feedback")
                        .HasForeignKey("reservation_system_be.Models.Feedback", "CustomerReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerReservation");
                });

            modelBuilder.Entity("reservation_system_be.Models.Invoice", b =>
                {
                    b.HasOne("reservation_system_be.Models.CustomerReservation", "CustomerReservation")
                        .WithMany("Invoices")
                        .HasForeignKey("CustomerReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerReservation");
                });

            modelBuilder.Entity("reservation_system_be.Models.Notification", b =>
                {
                    b.HasOne("reservation_system_be.Models.CustomerReservation", "CustomerReservation")
                        .WithMany("Notifications")
                        .HasForeignKey("CustomerReservationId");

                    b.HasOne("reservation_system_be.Models.VehicleInsurance", "VehicleInsurance")
                        .WithMany()
                        .HasForeignKey("VehicleInsuranceID");

                    b.HasOne("reservation_system_be.Models.VehicleMaintenance", "VehicleMaintenance")
                        .WithMany()
                        .HasForeignKey("VehicleMaintenanceId");

                    b.Navigation("CustomerReservation");

                    b.Navigation("VehicleInsurance");

                    b.Navigation("VehicleMaintenance");
                });

            modelBuilder.Entity("reservation_system_be.Models.Payment", b =>
                {
                    b.HasOne("reservation_system_be.Models.Invoice", "Invoice")
                        .WithOne("Payment")
                        .HasForeignKey("reservation_system_be.Models.Payment", "InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("reservation_system_be.Models.Reservation", b =>
                {
                    b.HasOne("reservation_system_be.Models.Employee", "Employee")
                        .WithMany("Reservations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("reservation_system_be.Models.Vehicle", b =>
                {
                    b.HasOne("reservation_system_be.Models.Employee", "Employee")
                        .WithMany("Vehicles")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("reservation_system_be.Models.VehicleModel", "VehicleModel")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("reservation_system_be.Models.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("VehicleModel");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleAvailability", b =>
                {
                    b.HasOne("reservation_system_be.Models.Reservation", "Reservation")
                        .WithOne("VehicleAvailability")
                        .HasForeignKey("reservation_system_be.Models.VehicleAvailability", "ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleInsurance", b =>
                {
                    b.HasOne("reservation_system_be.Models.Vehicle", "Vehicle")
                        .WithMany("VehicleInsurance")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleLog", b =>
                {
                    b.HasOne("reservation_system_be.Models.CustomerReservation", "CustomerReservation")
                        .WithOne("VehicleLog")
                        .HasForeignKey("reservation_system_be.Models.VehicleLog", "CustomerReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerReservation");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleMaintenance", b =>
                {
                    b.HasOne("reservation_system_be.Models.Vehicle", "Vehicle")
                        .WithMany("VehicleMaintenance")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleModel", b =>
                {
                    b.HasOne("reservation_system_be.Models.VehicleMake", "VehicleMake")
                        .WithMany("VehicleModels")
                        .HasForeignKey("VehicleMakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleMake");
                });

            modelBuilder.Entity("reservation_system_be.Models.Wishlist", b =>
                {
                    b.HasOne("reservation_system_be.Models.Customer", "Customer")
                        .WithMany("Wishlist")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("reservation_system_be.Models.Vehicle", "Vehicle")
                        .WithMany("Wishlist")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("reservation_system_be.Models.Customer", b =>
                {
                    b.Navigation("CusReservation");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("reservation_system_be.Models.CustomerReservation", b =>
                {
                    b.Navigation("Feedback");

                    b.Navigation("Invoices");

                    b.Navigation("Notifications");

                    b.Navigation("VehicleLog");
                });

            modelBuilder.Entity("reservation_system_be.Models.Employee", b =>
                {
                    b.Navigation("Reservations");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("reservation_system_be.Models.Invoice", b =>
                {
                    b.Navigation("Payment");
                });

            modelBuilder.Entity("reservation_system_be.Models.Reservation", b =>
                {
                    b.Navigation("CustomerReservation");

                    b.Navigation("VehicleAvailability");
                });

            modelBuilder.Entity("reservation_system_be.Models.Vehicle", b =>
                {
                    b.Navigation("CusReservation");

                    b.Navigation("VehicleInsurance");

                    b.Navigation("VehicleMaintenance");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleMake", b =>
                {
                    b.Navigation("VehicleModels");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleModel", b =>
                {
                    b.Navigation("AdditionalFeatures");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("reservation_system_be.Models.VehicleType", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
