﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RabbitEntityConsumer.Models;

namespace RabbitEntityConsumer.Migrations
{
    [DbContext(typeof(CarsDBContext))]
    [Migration("20200728110841_AddedReports")]
    partial class AddedReports
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarFactories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("CarFactories");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarFeatureCarModel", b =>
                {
                    b.Property<int>("CarFeatureId")
                        .HasColumnType("int");

                    b.Property<int>("CarModelId")
                        .HasColumnType("int");

                    b.HasKey("CarFeatureId", "CarModelId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CarModelId");

                    b.ToTable("CarFeatureCarModel");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarFeatures", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nchar(3)")
                        .IsFixedLength(true)
                        .HasMaxLength(3);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("CarFeatures");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarModels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("CarModels");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarProductCarFeature", b =>
                {
                    b.Property<int>("CarProductId")
                        .HasColumnType("int");

                    b.Property<int>("InstalledFeatureId")
                        .HasColumnType("int");

                    b.HasKey("CarProductId", "InstalledFeatureId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("InstalledFeatureId");

                    b.ToTable("CarProductCarFeature");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarProducts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarModelId")
                        .HasColumnType("int");

                    b.Property<int>("FactoryId")
                        .HasColumnType("int");

                    b.Property<string>("Vin")
                        .IsRequired()
                        .HasColumnName("VIN")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<short>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("CarModelId");

                    b.HasIndex("FactoryId");

                    b.HasIndex("Vin")
                        .IsUnique()
                        .HasName("VIN_Unique");

                    b.ToTable("CarProducts");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.Cities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.Countries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.Manufacturers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.Reports", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AddedToDatabase")
                        .HasColumnType("bit");

                    b.Property<string>("ReportData")
                        .HasColumnName("ReportData")
                        .HasColumnType("xml");

                    b.Property<DateTime>("RequestedDateTime")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarFactories", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.Cities", "City")
                        .WithMany("CarFactories")
                        .HasForeignKey("CityId")
                        .HasConstraintName("FK_CityFactory")
                        .IsRequired();

                    b.HasOne("RabbitEntityConsumer.Models.Manufacturers", "Manufacturer")
                        .WithMany("CarFactories")
                        .HasForeignKey("ManufacturerId")
                        .HasConstraintName("FK_ManufacturerCarFactory")
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarFeatureCarModel", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.CarFeatures", "CarFeature")
                        .WithMany("CarFeatureCarModel")
                        .HasForeignKey("CarFeatureId")
                        .HasConstraintName("FK_CarFeatureCarModel_CarFeature")
                        .IsRequired();

                    b.HasOne("RabbitEntityConsumer.Models.CarModels", "CarModel")
                        .WithMany("CarFeatureCarModel")
                        .HasForeignKey("CarModelId")
                        .HasConstraintName("FK_CarFeatureCarModel_CarModel")
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarModels", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.Manufacturers", "Manufacturer")
                        .WithMany("CarModels")
                        .HasForeignKey("ManufacturerId")
                        .HasConstraintName("FK_ManufacturerCarModel")
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarProductCarFeature", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.CarProducts", "CarProduct")
                        .WithMany("CarProductCarFeature")
                        .HasForeignKey("CarProductId")
                        .HasConstraintName("FK_CarProductCarFeature_CarProduct")
                        .IsRequired();

                    b.HasOne("RabbitEntityConsumer.Models.CarFeatures", "InstalledFeature")
                        .WithMany("CarProductCarFeature")
                        .HasForeignKey("InstalledFeatureId")
                        .HasConstraintName("FK_CarProductCarFeature_CarFeature")
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.CarProducts", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.CarModels", "CarModel")
                        .WithMany("CarProducts")
                        .HasForeignKey("CarModelId")
                        .HasConstraintName("FK_CarModelProduct")
                        .IsRequired();

                    b.HasOne("RabbitEntityConsumer.Models.CarFactories", "Factory")
                        .WithMany("CarProducts")
                        .HasForeignKey("FactoryId")
                        .HasConstraintName("FK_FactoryProduct")
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.Cities", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.Countries", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK_CountryCity")
                        .IsRequired();
                });

            modelBuilder.Entity("RabbitEntityConsumer.Models.Manufacturers", b =>
                {
                    b.HasOne("RabbitEntityConsumer.Models.Countries", "Country")
                        .WithMany("Manufacturers")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK_CountryManufacturer")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
