using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryApp.Shared.Data
{
    [DisplayName("History Item")]
    public class HistoryItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }
        public DateTime Timestamp { get; set; }
        [MaxLength(50)]
        [DisplayName("Reference Number")]
        public string ReferenceNumber { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        [DisplayName("Quantity Change")]
        public int QuantityChange { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        [DisplayName("Value Change")]
        public decimal ValueChange { get; set; }

        public List<HistoryItemCost>? Costs { get; set; }

        [JsonIgnore]
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public InventoryItem? InventoryItem { get; set; }
    }
}
