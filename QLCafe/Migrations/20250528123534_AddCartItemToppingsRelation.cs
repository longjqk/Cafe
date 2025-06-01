using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCafe.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItemToppingsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ToppingPrice",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CartItemToppings",
                columns: table => new
                {
                    CartItemToppingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartItemId = table.Column<int>(type: "int", nullable: false),
                    ToppingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemToppings", x => x.CartItemToppingId);
                    table.ForeignKey(
                        name: "FK_CartItemToppings_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "CartItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItemToppings_Toppings_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Toppings",
                        principalColumn: "ToppingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemToppings_CartItemId",
                table: "CartItemToppings",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemToppings_ToppingId",
                table: "CartItemToppings",
                column: "ToppingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemToppings");

            migrationBuilder.DropColumn(
                name: "ToppingPrice",
                table: "CartItems");
        }
    }
}
