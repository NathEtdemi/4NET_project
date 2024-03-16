using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using project_API.Domain;

namespace project_API
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) :
        base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brand>()
                .HasMany<CarModel>(x => x.CarModels)
                .WithOne()
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarModel>()
                .HasOne<Brand>()
                .WithMany(x => x.CarModels)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarModel>()
                .HasMany<Vehicle>(x => x.Vehicles)
                .WithOne()
                .HasForeignKey(x => x.CarModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasOne<CarModel>()
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.CarModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasMany<Maintenance>(x => x.Maintenances)
                .WithOne()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Maintenance>()
                .HasOne<Vehicle>()
                .WithMany(x => x.Maintenances)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Brand>().HasKey(x => x.Id);
            modelBuilder.Entity<CarModel>().HasKey(x => x.Id);
            modelBuilder.Entity<Vehicle>().HasKey(x => x.Id);
            modelBuilder.Entity<Maintenance>().HasKey(x => x.Id);


        }
    }
}
