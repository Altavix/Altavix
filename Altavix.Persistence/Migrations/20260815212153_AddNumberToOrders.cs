using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altavix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "OrderNumbers",
                startValue: 10000L);

            migrationBuilder.AddColumn<long>(
                name: "Number",
                table: "tbOrders",
                type: "bigint",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR OrderNumbers")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.CreateIndex(
                name: "IX_tbOrders_Number",
                table: "tbOrders",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbOrders_Number",
                table: "tbOrders");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "tbOrders");

            migrationBuilder.DropSequence(
                name: "OrderNumbers");
        }
    }
}
