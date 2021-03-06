﻿
namespace CarsharingSystem.Data
{
    using System.Data.Entity;

    using CarsharingSystem.Data.Migrations;
    using CarsharingSystem.Model;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public IDbSet<Driver> Drivers { get; set; }

        public IDbSet<Passenger> Passengers { get; set; }

        public IDbSet<Travel> Travels { get; set; }

        public IDbSet<Vehicle> Vehicles { get; set; }

        public IDbSet<DrivingLicense> DrivingLicenses { get; set; }

        public IDbSet<TravelPassenger> TravelPassengers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>()
            .HasKey(d => d.DrivingLicenseId);

            modelBuilder.Entity<DrivingLicense>()
              .HasRequired(lu => lu.Driver)
              .WithOptional(pi => pi.DrivingLicense);



            modelBuilder.Entity<TravelPassenger>().HasKey(si => new { si.TravelId, si.PassengerId });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
