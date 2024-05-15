using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BarelySliced.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherForeCast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TemperatureC = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Date);
                });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Date", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { new DateOnly(2024, 5, 16), "Warm", 25 },
                    { new DateOnly(2024, 5, 17), "Hot", 30 },
                    { new DateOnly(2024, 5, 18), "Cool", 15 },
                    { new DateOnly(2024, 5, 19), "Chilly", 10 },
                    { new DateOnly(2024, 5, 20), "Freezing", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
