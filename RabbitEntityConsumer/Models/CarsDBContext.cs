using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RabbitEntityConsumer.Models
{
    public partial class CarsDBContext : DbContext
    {
        public CarsDBContext()
        {
        }

        public CarsDBContext(DbContextOptions<CarsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CarFactories> CarFactories { get; set; }
        public virtual DbSet<CarFeatureCarModel> CarFeatureCarModel { get; set; }
        public virtual DbSet<CarFeatures> CarFeatures { get; set; }
        public virtual DbSet<CarModels> CarModels { get; set; }
        public virtual DbSet<CarProductCarFeature> CarProductCarFeature { get; set; }
        public virtual DbSet<CarProducts> CarProducts { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Manufacturers> Manufacturers { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Integrated Security=true;Database=CarsDB;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarFactories>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CarFactories)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CityFactory");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.CarFactories)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManufacturerCarFactory");
            });

            modelBuilder.Entity<CarFeatureCarModel>(entity =>
            {
                entity.HasKey(e => new { e.CarFeatureId, e.CarModelId })
                    .IsClustered(false);

                entity.HasOne(d => d.CarFeature)
                    .WithMany(p => p.CarFeatureCarModel)
                    .HasForeignKey(d => d.CarFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarFeatureCarModel_CarFeature");

                entity.HasOne(d => d.CarModel)
                    .WithMany(p => p.CarFeatureCarModel)
                    .HasForeignKey(d => d.CarModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarFeatureCarModel_CarModel");
            });

            modelBuilder.Entity<CarFeatures>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CarModels>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.CarModels)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManufacturerCarModel");
            });

            modelBuilder.Entity<CarProductCarFeature>(entity =>
            {
                entity.HasKey(e => new { e.CarProductId, e.InstalledFeatureId })
                    .IsClustered(false);

                entity.HasOne(d => d.CarProduct)
                    .WithMany(p => p.CarProductCarFeature)
                    .HasForeignKey(d => d.CarProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarProductCarFeature_CarProduct");

                entity.HasOne(d => d.InstalledFeature)
                    .WithMany(p => p.CarProductCarFeature)
                    .HasForeignKey(d => d.InstalledFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarProductCarFeature_CarFeature");
            });

            modelBuilder.Entity<CarProducts>(entity =>
            {
                entity.HasIndex(e => e.Vin)
                    .HasName("VIN_Unique")
                    .IsUnique();

                entity.Property(e => e.Vin)
                    .IsRequired()
                    .HasColumnName("VIN")
                    .HasMaxLength(255);

                entity.HasOne(d => d.CarModel)
                    .WithMany(p => p.CarProducts)
                    .HasForeignKey(d => d.CarModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarModelProduct");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.CarProducts)
                    .HasForeignKey(d => d.FactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FactoryProduct");
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryCity");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Manufacturers>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Manufacturers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryManufacturer");
            });

            modelBuilder.Entity<Reports>(entity =>
            {
                entity.ToTable("Reports");

                entity.Property(e => e.RequestedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ReportData)
                    .HasColumnName("ReportData")
                    .HasColumnType("xml");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
