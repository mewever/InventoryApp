using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryApp.Shared.Data
{
    [DisplayName("History Item Cost")]
    public class HistoryItemCost
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("HistoryItem")]
        public int HistoryItemId { get; set; }
        [ForeignKey("InventoryItemCost")]
        public int InventoryItemCostId { get; set; }
        [DisplayName("Quantity Change")]
        public int QuantityChange { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        [DisplayName("Unit Value")]
        public decimal UnitValue { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        [DisplayName("Value Change")]
        public decimal ValueChange { get; set; }

        [JsonIgnore]
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public HistoryItem? HistoryItem { get; set; } = null;
        [JsonIgnore]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public InventoryItemCost? InventoryItemCost { get; set; } = null;
    }
}
