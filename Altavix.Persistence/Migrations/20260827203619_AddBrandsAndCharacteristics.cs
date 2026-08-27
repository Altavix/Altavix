using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altavix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandsAndCharacteristics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "tbProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "tbProducts",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "InStock",
                table: "tbProducts",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "tbBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbCharacteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCharacteristics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbProductCharacteristics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacteristicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProductCharacteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbProductCharacteristics_tbCharacteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "tbCharacteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbProductCharacteristics_tbProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tbProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbProducts_BrandId",
                table: "tbProducts",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_tbProductCharacteristics_CharacteristicId",
                table: "tbProductCharacteristics",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_tbProductCharacteristics_ProductId",
                table: "tbProductCharacteristics",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbProducts_tbBrands_BrandId",
                table: "tbProducts",
                column: "BrandId",
                principalTable: "tbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbProducts_tbBrands_BrandId",
                table: "tbProducts");

            migrationBuilder.DropTable(
                name: "tbBrands");

            migrationBuilder.DropTable(
                name: "tbProductCharacteristics");

            migrationBuilder.DropTable(
                name: "tbCharacteristics");

            migrationBuilder.DropIndex(
                name: "IX_tbProducts_BrandId",
                table: "tbProducts");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "tbProducts");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "tbProducts");

            migrationBuilder.DropColumn(
                name: "InStock",
                table: "tbProducts");
        }
    }
}
