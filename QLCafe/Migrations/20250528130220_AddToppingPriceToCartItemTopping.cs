using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCafe.Migrations
{
    /// <inheritdoc />
    public partial class AddToppingPriceToCartItemTopping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ToppingPrice",
                table: "CartItemToppings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToppingPrice",
                table: "CartItemToppings");
        }
    }
}
