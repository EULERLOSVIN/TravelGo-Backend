using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartureTimeToQueueVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idDepartureTime",
                table: "QueueVehicle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueueVehicle_idDepartureTime",
                table: "QueueVehicle",
                column: "idDepartureTime");

            migrationBuilder.AddForeignKey(
                name: "FK_QueueVehicle_DepartureTime",
                table: "QueueVehicle",
                column: "idDepartureTime",
                principalTable: "DepartureTime",
                principalColumn: "idDepartureTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueueVehicle_DepartureTime",
                table: "QueueVehicle");

            migrationBuilder.DropIndex(
                name: "IX_QueueVehicle_idDepartureTime",
                table: "QueueVehicle");

            migrationBuilder.DropColumn(
                name: "idDepartureTime",
                table: "QueueVehicle");
        }
    }
}
