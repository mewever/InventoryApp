using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Shared.Models
{
    public class InventoryHistoryListItem
    {
        public DateTime? HistoryDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public int HistoryQuantity { get; set; }
        public decimal HistoryCost { get; set; }
        public DateTime CostDate { get; set; } = DateTime.MinValue;
        public int CostQuantity { get; set; }
        public decimal CostUnitCost { get; set; }
    }
}
