using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altavix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesWithTbPrefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Users_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Users_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Users_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Users_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryEntityProductEntity_Categories_CategoriesId",
                table: "CategoryEntityProductEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryEntityProductEntity_Products_ProductEntityId",
                table: "CategoryEntityProductEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserCreatorId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryEntityProductEntity",
                table: "CategoryEntityProductEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "tbUsers");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "tbRoles");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "tbProducts");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "tbProductImages");

            migrationBuilder.RenameTable(
                name: "CategoryEntityProductEntity",
                newName: "tbCategoryProduct");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "tbCategories");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "tbUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "tbUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "tbUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "tbUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "tbRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Id",
                table: "tbUsers",
                newName: "IX_tbUsers_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_UserCreatorId",
                table: "tbProducts",
                newName: "IX_tbProducts_UserCreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_ProductId",
                table: "tbProductImages",
                newName: "IX_tbProductImages_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryEntityProductEntity_ProductEntityId",
                table: "tbCategoryProduct",
                newName: "IX_tbCategoryProduct_ProductEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Title",
                table: "tbCategories",
                newName: "IX_tbCategories_Title");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "tbUserRoles",
                newName: "IX_tbUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "tbUserLogins",
                newName: "IX_tbUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "tbUserClaims",
                newName: "IX_tbUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "tbRoleClaims",
                newName: "IX_tbRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbUsers",
                table: "tbUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbRoles",
                table: "tbRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbProducts",
                table: "tbProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbProductImages",
                table: "tbProductImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbCategoryProduct",
                table: "tbCategoryProduct",
                columns: new[] { "CategoriesId", "ProductEntityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbCategories",
                table: "tbCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbUserTokens",
                table: "tbUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbUserRoles",
                table: "tbUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbUserLogins",
                table: "tbUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbUserClaims",
                table: "tbUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbRoleClaims",
                table: "tbRoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbCategoryProduct_tbCategories_CategoriesId",
                table: "tbCategoryProduct",
                column: "CategoriesId",
                principalTable: "tbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbCategoryProduct_tbProducts_ProductEntityId",
                table: "tbCategoryProduct",
                column: "ProductEntityId",
                principalTable: "tbProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbProductImages_tbProducts_ProductId",
                table: "tbProductImages",
                column: "ProductId",
                principalTable: "tbProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbProducts_tbUsers_UserCreatorId",
                table: "tbProducts",
                column: "UserCreatorId",
                principalTable: "tbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbRoleClaims_tbRoles_RoleId",
                table: "tbRoleClaims",
                column: "RoleId",
                principalTable: "tbRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbUserClaims_tbUsers_UserId",
                table: "tbUserClaims",
                column: "UserId",
                principalTable: "tbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbUserLogins_tbUsers_UserId",
                table: "tbUserLogins",
                column: "UserId",
                principalTable: "tbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbUserRoles_tbRoles_RoleId",
                table: "tbUserRoles",
                column: "RoleId",
                principalTable: "tbRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbUserRoles_tbUsers_UserId",
                table: "tbUserRoles",
                column: "UserId",
                principalTable: "tbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbUserTokens_tbUsers_UserId",
                table: "tbUserTokens",
                column: "UserId",
                principalTable: "tbUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbCategoryProduct_tbCategories_CategoriesId",
                table: "tbCategoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_tbCategoryProduct_tbProducts_ProductEntityId",
                table: "tbCategoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_tbProductImages_tbProducts_ProductId",
                table: "tbProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_tbProducts_tbUsers_UserCreatorId",
                table: "tbProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_tbRoleClaims_tbRoles_RoleId",
                table: "tbRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_tbUserClaims_tbUsers_UserId",
                table: "tbUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_tbUserLogins_tbUsers_UserId",
                table: "tbUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_tbUserRoles_tbRoles_RoleId",
                table: "tbUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_tbUserRoles_tbUsers_UserId",
                table: "tbUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_tbUserTokens_tbUsers_UserId",
                table: "tbUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbUserTokens",
                table: "tbUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbUsers",
                table: "tbUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbUserRoles",
                table: "tbUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbUserLogins",
                table: "tbUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbUserClaims",
                table: "tbUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbRoles",
                table: "tbRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbRoleClaims",
                table: "tbRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbProducts",
                table: "tbProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbProductImages",
                table: "tbProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbCategoryProduct",
                table: "tbCategoryProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbCategories",
                table: "tbCategories");

            migrationBuilder.RenameTable(
                name: "tbUserTokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "tbUsers",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "tbUserRoles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "tbUserLogins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "tbUserClaims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "tbRoles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "tbRoleClaims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "tbProducts",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "tbProductImages",
                newName: "ProductImages");

            migrationBuilder.RenameTable(
                name: "tbCategoryProduct",
                newName: "CategoryEntityProductEntity");

            migrationBuilder.RenameTable(
                name: "tbCategories",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_tbUsers_Id",
                table: "Users",
                newName: "IX_Users_Id");

            migrationBuilder.RenameIndex(
                name: "IX_tbUserRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_tbUserLogins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tbUserClaims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tbRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_tbProducts_UserCreatorId",
                table: "Products",
                newName: "IX_Products_UserCreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_tbProductImages_ProductId",
                table: "ProductImages",
                newName: "IX_ProductImages_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_tbCategoryProduct_ProductEntityId",
                table: "CategoryEntityProductEntity",
                newName: "IX_CategoryEntityProductEntity_ProductEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_tbCategories_Title",
                table: "Categories",
                newName: "IX_Categories_Title");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryEntityProductEntity",
                table: "CategoryEntityProductEntity",
                columns: new[] { "CategoriesId", "ProductEntityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Users_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Users_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Users_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Users_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryEntityProductEntity_Categories_CategoriesId",
                table: "CategoryEntityProductEntity",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryEntityProductEntity_Products_ProductEntityId",
                table: "CategoryEntityProductEntity",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserCreatorId",
                table: "Products",
                column: "UserCreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
