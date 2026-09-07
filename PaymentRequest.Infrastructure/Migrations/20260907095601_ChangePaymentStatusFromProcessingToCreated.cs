using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentRequest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePaymentStatusFromProcessingToCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaymentRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Created",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Processing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PaymentRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Processing",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Created");
        }
    }
}
