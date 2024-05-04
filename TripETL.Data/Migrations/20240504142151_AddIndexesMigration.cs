using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripETL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Trips_PULocationID",
                table: "Trips",
                column: "PULocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_tip_amount",
                table: "Trips",
                column: "tip_amount");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_tpep_pickup_datetime_tpep_dropoff_datetime",
                table: "Trips",
                columns: new[] { "tpep_pickup_datetime", "tpep_dropoff_datetime" });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_trip_distance",
                table: "Trips",
                column: "trip_distance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_PULocationID",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_tip_amount",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_tpep_pickup_datetime_tpep_dropoff_datetime",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_trip_distance",
                table: "Trips");
        }
    }
}
