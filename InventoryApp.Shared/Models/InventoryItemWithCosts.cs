using InventoryApp.Shared.Data;
using System.ComponentModel;

namespace InventoryApp.Shared.Models
{
    public class InventoryItemWithCosts: InventoryItem
    {
        [DisplayName("Total Cost")]
        public decimal TotalCost { get; set; }

        public InventoryItemWithCosts() { }

        public InventoryItemWithCosts(InventoryItem item)
        {
            Id = item.Id;
            ProductCode = item.ProductCode;
            Name = item.Name;
            Quantity = item.Quantity;
            Costs = item.Costs ?? new List<InventoryItemCost>();

            TotalCost = Costs.Sum(c => c.CurrentCost);
        }
    }
}
