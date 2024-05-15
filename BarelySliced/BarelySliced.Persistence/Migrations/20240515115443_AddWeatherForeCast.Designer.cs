﻿// <auto-generated />
using System;
using BarelySliced.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BarelySliced.Persistence.Migrations
{
    [DbContext(typeof(SliverDbContext))]
    [Migration("20240515115443_AddWeatherForeCast")]
    partial class AddWeatherForeCast
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BarelySliced.Domain.WeatherForecast", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("int");

                    b.HasKey("Date");

                    b.ToTable("WeatherForecasts");

                    b.HasData(
                        new
                        {
                            Date = new DateOnly(2024, 5, 16),
                            Summary = "Warm",
                            TemperatureC = 25
                        },
                        new
                        {
                            Date = new DateOnly(2024, 5, 17),
                            Summary = "Hot",
                            TemperatureC = 30
                        },
                        new
                        {
                            Date = new DateOnly(2024, 5, 18),
                            Summary = "Cool",
                            TemperatureC = 15
                        },
                        new
                        {
                            Date = new DateOnly(2024, 5, 19),
                            Summary = "Chilly",
                            TemperatureC = 10
                        },
                        new
                        {
                            Date = new DateOnly(2024, 5, 20),
                            Summary = "Freezing",
                            TemperatureC = 5
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
