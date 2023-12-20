using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YemeniDriver.Models;
using YemeniDriver.Models;

namespace YemeniDriver.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DriverRating> DriverRatings { get; set; }
        public DbSet<DriverReview> DriverReviews { get; set; }

        public DbSet<PassengerRating> PassengerRatings { get; set; }
        public DbSet<PassengerReview> PassengerReviews { get; set; }

        //public DbSet<CancelRequest> CancelRequests { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationship: Request -> ApplicationUser (Driver)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Driver)
                .WithMany(u => u.DriverRequests)
                .HasForeignKey(r => r.DriverID) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related requests

            // Define relationship: Request -> ApplicationUser (Passenger)
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Passenger)
                .WithMany(u => u.PassengerRequests)
                .HasForeignKey(r => r.PassengerId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related requests

            // Define relationship: Request -> Trip
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Trip)
                .WithOne(t => t.Request)
                .HasForeignKey<Trip>(t => t.RequestId) // Foreign key linking to Request.RequestId
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there is a related trip

            // Define relationship: Trip -> ApplicationUser (Driver)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver)
                .WithMany(u => u.DriverTrips)
                .HasForeignKey(t => t.DriverId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related trips

            // Define relationship: Trip -> ApplicationUser (Passenger)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Passenger)
                .WithMany(u => u.PassengerTrips)
                .HasForeignKey(t => t.PassengerId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related trips


            modelBuilder.Entity<Trip>()
               .HasMany(t => t.DriverRatings)
               .WithOne(dr => dr.Trip)
               .HasForeignKey(dr => dr.TripId);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.PassengerRatings)
                .WithOne(pr => pr.Trip)
                .HasForeignKey(pr => pr.TripId);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.DriverReviews)
                .WithOne(dr => dr.Trip)
                .HasForeignKey(dr => dr.TripId);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.PassengerReviews)
                .WithOne(pr => pr.Trip)
                .HasForeignKey(pr => pr.TripId);
            // Define relationship: Vehicle -> ApplicationUser (Driver)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Driver)
                .WithOne(u => u.Vehicle)
                .HasForeignKey<ApplicationUser>(u => u.VehicleId) // Foreign key linking to Vehicle.VehicleId
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there is a related vehicle

            // Define relationship: ApplicationUser (Driver) -> Trip (DriverTrips)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.DriverTrips)
                .WithOne(t => t.Driver)
                .HasForeignKey(t => t.DriverId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related driver trips

            // Define relationship: ApplicationUser (Passenger) -> Trip (PassengerTrips)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.PassengerTrips)
                .WithOne(t => t.Passenger)
                .HasForeignKey(t => t.PassengerId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related passenger trips

            // Define relationship: ApplicationUser (Driver) -> Request (DriverRequests)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.DriverRequests)
                .WithOne(r => r.Driver)
                .HasForeignKey(r => r.DriverID) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related driver requests

            // Define relationship: ApplicationUser (Passenger) -> Request (PassengerRequests)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.PassengerRequests)
                .WithOne(r => r.Passenger)
                .HasForeignKey(r => r.PassengerId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there are related passenger requests

            // Define relationship: ApplicationUser (Driver) -> Vehicle
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Vehicle)
                .WithOne(v => v.Driver)
                .HasForeignKey<Vehicle>(v => v.DriverId) // Foreign key linking to ApplicationUser.Id
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if there is a related vehicle



        }





    }
}
