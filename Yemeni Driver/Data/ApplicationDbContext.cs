using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DriverAndRequest> DriversAndRequests { get; set; }
        public DbSet<CancelRequest> CancelRequests { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vehicle
            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.VehicleId);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.ApplicationUser)
                .WithOne(d => d.Vehicle)
                .HasForeignKey<ApplicationUser>(v => v.VehicleId);

            // Driver
           

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(d => d.Vehicle)
                .WithOne(v => v.ApplicationUser)
                .HasForeignKey<Vehicle>(v => v.VehicleId);

            modelBuilder.Entity<DriverAndRequest>()
                .HasKey(dr => new { dr.RequestId, dr.ApplicationUserId });

            modelBuilder.Entity<DriverAndRequest>()
                .HasOne(dr => dr.Request)
                .WithMany(r => r.DriverAndRequests)
                .HasForeignKey(dr => dr.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<DriverAndRequest>()
                .HasOne(dr => dr.ApplicationUser)
                .WithMany(d => d.DriverAndRequests)
                .HasForeignKey(dr => dr.ApplicationUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(d => d.Trip)
                .WithOne(t => t.ApplicationUser)
                .HasForeignKey<Trip>(t => t.ApplicationUserId);

          


            // Request
            modelBuilder.Entity<Request>()
                .HasKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.ApplicationUser)
                .WithMany(p => p.Requests)
                .HasForeignKey(r => r.ApplicationUserId);

            modelBuilder.Entity<Request>()
                .HasMany(r => r.DriverAndRequests)
                .WithOne(dr => dr.Request)
                .HasForeignKey(dr => dr.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Trip)
                .WithOne(t => t.Request)
                .HasForeignKey<Trip>(t => t.RequestId);

            // CancelRequest
            modelBuilder.Entity<CancelRequest>()
                .HasKey(cr => new { cr.RequestId, cr.ApplicationUserId});

            modelBuilder.Entity<CancelRequest>()
                .HasOne(cr => cr.Request)
                .WithOne(r => r.CancelRequest)
                .HasForeignKey<CancelRequest>(r => r.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull); ;

            modelBuilder.Entity<CancelRequest>()
                .HasOne(cr => cr.ApplicationUser)
                .WithOne(p => p.CancelRequest)
                .HasForeignKey <CancelRequest>(cr => cr.ApplicationUserId)
                .OnDelete(DeleteBehavior.ClientSetNull); ;

            modelBuilder.Entity<CancelRequest>()
                .HasOne(cr => cr.ApplicationUser)
                .WithOne(d => d.CancelRequest)
                .HasForeignKey<CancelRequest>(cr => cr.ApplicationUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);;

            // Trip
            modelBuilder.Entity<Trip>()
                .HasKey(t => t.TripId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Request)
                .WithOne(r => r.Trip)
                .HasForeignKey<Trip>(t => t.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.ApplicationUser)
                .WithOne(d => d.Trip)
                .HasForeignKey<Trip>(t => t.ApplicationUserId);

            // Passenger

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.Requests)
                .WithOne(r => r.ApplicationUser)
                .HasForeignKey(r => r.ApplicationUserId);




          

            // Other configurations...

            //ApplicationUser

          
           
         

            base.OnModelCreating(modelBuilder);
        }

    }
}
