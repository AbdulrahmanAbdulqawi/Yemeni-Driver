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
                .HasOne(v => v.Driver)
                .WithMany(d => d.Vehicles)
                .HasForeignKey(v => v.VehicleId);

            // Driver
           

            modelBuilder.Entity<Driver>()
                .HasMany(d => d.Vehicles)
                .WithOne(v => v.Driver)
                .HasForeignKey(v => v.VehicleId);

            modelBuilder.Entity<DriverAndRequest>()
                .HasKey(dr => new { dr.RequestId, dr.DriverId });

            modelBuilder.Entity<DriverAndRequest>()
                .HasOne(dr => dr.Request)
                .WithMany(r => r.DriverAndRequests)
                .HasForeignKey(dr => dr.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<DriverAndRequest>()
                .HasOne(dr => dr.Driver)
                .WithMany(d => d.DriverAndRequests)
                .HasForeignKey(dr => dr.DriverId);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.Trip)
                .WithOne(t => t.Driver)
                .HasForeignKey<Trip>(t => t.DriverId);

          


            // Request
            modelBuilder.Entity<Request>()
                .HasKey(r => r.RequestId);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Passenger)
                .WithMany(p => p.Requests)
                .HasForeignKey(r => r.UserId);

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
                .HasKey(cr => new { cr.RequestId, cr.UserId, cr.DriverId });

            modelBuilder.Entity<CancelRequest>()
                .HasOne(cr => cr.Request)
                .WithOne(r => r.CancelRequest)
                .HasForeignKey<CancelRequest>(r => r.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull); ;

            modelBuilder.Entity<CancelRequest>()
                .HasOne(cr => cr.Passenger)
                .WithOne(p => p.CancelRequest)
                .HasForeignKey <CancelRequest>(cr => cr.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull); ;

            modelBuilder.Entity<CancelRequest>()
                .HasOne(cr => cr.Driver)
                .WithOne(d => d.CancelRequest)
                .HasForeignKey<CancelRequest>(cr => cr.DriverId)
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
                .HasOne(t => t.Driver)
                .WithOne(d => d.Trip)
                .HasForeignKey<Trip>(t => t.DriverId);

            // Passenger

            modelBuilder.Entity<Passenger>()
                .HasMany(p => p.Requests)
                .WithOne(r => r.Passenger)
                .HasForeignKey(r => r.UserId);




          

            // Other configurations...

            //ApplicationUser
            modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);

          
           
         

            base.OnModelCreating(modelBuilder);
        }

    }
}
