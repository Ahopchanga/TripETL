﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TripETL.Data;

#nullable disable

namespace TripETL.Data.Migrations
{
    [DbContext(typeof(TripDbContext))]
    [Migration("20240504121707_InsertCsvData")]
    partial class InsertCsvData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TripETL.Domain.Entities.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DOLocationId")
                        .HasColumnType("int")
                        .HasColumnName("DOLocationID");

                    b.Property<decimal>("FareAmount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("fare_amount");

                    b.Property<int>("PULocationId")
                        .HasColumnType("int")
                        .HasColumnName("PULocationID");

                    b.Property<int>("PassengerCount")
                        .HasColumnType("int")
                        .HasColumnName("passenger_count");

                    b.Property<string>("StoreAndFwdFlag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("store_and_fwd_flag");

                    b.Property<decimal>("TipAmount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("tip_amount");

                    b.Property<DateTime>("TpepDropoffDatetime")
                        .HasColumnType("datetime2")
                        .HasColumnName("tpep_dropoff_datetime");

                    b.Property<DateTime>("TpepPickupDatetime")
                        .HasColumnType("datetime2")
                        .HasColumnName("tpep_pickup_datetime");

                    b.Property<decimal>("TripDistance")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("trip_distance");

                    b.HasKey("Id");

                    b.HasIndex("TpepPickupDatetime", "TpepDropoffDatetime", "PassengerCount")
                        .IsUnique();

                    b.ToTable("Trips");
                });
#pragma warning restore 612, 618
        }
    }
}
