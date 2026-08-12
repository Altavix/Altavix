using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Altavix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersWithItemsDeliveryAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbDeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbDeliveryMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ordered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Paid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Processing = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Shipped = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cancelled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ClientMobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ClientEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    City = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CityRef = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPriceCoin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbOrders_tbDeliveryMethods_DeliveryMethodId",
                        column: x => x.DeliveryMethodId,
                        principalTable: "tbDeliveryMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbOrders_tbPaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "tbPaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbOrders_tbUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "tbUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tbOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPriceCoin = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ordered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pending = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyToShip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Shipped = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cancelled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbOrderItems_tbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "tbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbOrderItems_tbProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tbProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbOrderItems_OrderId",
                table: "tbOrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tbOrderItems_ProductId",
                table: "tbOrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tbOrders_ClientId",
                table: "tbOrders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tbOrders_DeliveryMethodId",
                table: "tbOrders",
                column: "DeliveryMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_tbOrders_PaymentMethodId",
                table: "tbOrders",
                column: "PaymentMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbOrderItems");

            migrationBuilder.DropTable(
                name: "tbOrders");

            migrationBuilder.DropTable(
                name: "tbDeliveryMethods");

            migrationBuilder.DropTable(
                name: "tbPaymentMethods");
        }
    }
}
