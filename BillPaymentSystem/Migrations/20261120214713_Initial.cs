using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillPaymentSystem.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faturas",
                columns: table => new
                {
                    FaturaNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboneNo = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FaturaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: false),
                    Tutar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturas", x => x.FaturaNo);
                });

            migrationBuilder.InsertData(
                table: "Faturas",
                columns: new[] { "FaturaNo", "AboneNo", "Durum", "FaturaType", "Name", "Tarih", "Tutar" },
                values: new object[,]
                {
                    { 1, 1, false, "Elektrik", "Durmuş Durarmış", new DateTime(2024, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 456m },
                    { 2, 32, false, "Su", "Aslı Mert", new DateTime(2024, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 654m },
                    { 3, 123, false, "İnternet", "Doğukan Gün", new DateTime(2024, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 423m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faturas");
        }
    }
}
