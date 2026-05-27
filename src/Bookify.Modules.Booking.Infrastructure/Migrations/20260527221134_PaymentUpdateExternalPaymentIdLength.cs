using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Modules.Booking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentUpdateExternalPaymentIdLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExternalPaymentId",
                schema: "booking",
                table: "Payments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExternalPaymentId",
                schema: "booking",
                table: "Payments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }
    }
}
