using InventoryApp.Shared.Data;

namespace InventoryApp.Shared.Models
{
    public class HistoryItemWithCost: HistoryItem
    {
        public List<HistoryItemCost> Costs { get; set; } = new List<HistoryItemCost>();
    }
}
