using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAppMvc.Migrations
{
    /// <inheritdoc />
    public partial class OriginalLotCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LotCost",
                table: "InventoryItemCosts",
                newName: "OriginalCost");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentCost",
                table: "InventoryItemCosts",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCost",
                table: "InventoryItemCosts");

            migrationBuilder.RenameColumn(
                name: "OriginalCost",
                table: "InventoryItemCosts",
                newName: "LotCost");
        }
    }
}
