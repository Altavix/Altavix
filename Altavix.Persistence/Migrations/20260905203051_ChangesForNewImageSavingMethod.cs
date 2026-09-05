using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altavix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangesForNewImageSavingMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageContent",
                table: "tbProductImages",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "tbProductImages",
                newName: "ImageContent");
        }
    }
}
