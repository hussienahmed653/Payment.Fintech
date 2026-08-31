using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Customer.Infrastructure.Context.Migrations;
/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Merchants",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ContactFirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                ContactLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                BusinessName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                BusinessType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DailyLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CreatedByGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedByGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Merchants", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Merchants_Email",
            table: "Merchants",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Merchants");
    }
}
