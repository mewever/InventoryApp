using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAppMvc.Migrations
{
    /// <inheritdoc />
    public partial class FixHistoryItemCostForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryItemCosts_HistoryItemCosts_InventoryItemCostId",
                table: "HistoryItemCosts");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryItemCosts_InventoryItemCosts_InventoryItemCostId",
                table: "HistoryItemCosts",
                column: "InventoryItemCostId",
                principalTable: "InventoryItemCosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryItemCosts_InventoryItemCosts_InventoryItemCostId",
                table: "HistoryItemCosts");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryItemCosts_HistoryItemCosts_InventoryItemCostId",
                table: "HistoryItemCosts",
                column: "InventoryItemCostId",
                principalTable: "HistoryItemCosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
