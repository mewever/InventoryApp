using InventoryApp.Shared.Data;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace InventoryApp.Shared.Models
{
    public class InventoryItemWithHistory: InventoryItem
    {
        [DisplayName("Total Cost")]
        public decimal TotalCost{ get; set; }

        public List<InventoryHistoryListItem> ListItems { get; set; } = [];

        public InventoryItemWithHistory() { }

        public InventoryItemWithHistory(InventoryItem item)
        {
            Id = item.Id;
            ProductCode = item.ProductCode;
            Name = item.Name;
            Quantity = item.Quantity;
            foreach (var history in item.History ?? [])
            {
                InventoryHistoryListItem listItem = new InventoryHistoryListItem()
                {
                    HistoryDate = history.Timestamp,
                    ReferenceNumber = history.ReferenceNumber,
                    HistoryQuantity = history.QuantityChange,
                    HistoryCost = history.ValueChange
                };
                if (history.Costs?.Count == 0)
                {
                    ListItems.Add(listItem);
                }
                else
                {
                    foreach (var historyCost in history.Costs ?? [])
                    {
                        var cost = item.Costs?.FirstOrDefault(c => c.Id == historyCost.InventoryItemCostId);
                        listItem.CostDate = cost?.ReceivedTimestamp ?? DateTime.MinValue;
                        listItem.CostQuantity = historyCost.QuantityChange;
                        listItem.CostUnitCost = historyCost.UnitValue;
                        ListItems.Add(listItem);
                        listItem = new InventoryHistoryListItem();
                    }
                }
                TotalCost = item.Costs?.Sum(c => c.CurrentCost) ?? 0;
            }
        }
    }
}
