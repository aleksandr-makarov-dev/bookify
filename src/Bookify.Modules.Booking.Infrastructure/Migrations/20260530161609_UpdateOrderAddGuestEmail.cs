using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Modules.Booking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderAddGuestEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                schema: "booking",
                table: "Orders",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestEmail",
                schema: "booking",
                table: "Orders");
        }
    }
}
