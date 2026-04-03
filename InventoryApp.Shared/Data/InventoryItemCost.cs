using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InventoryApp.Shared.Data
{
    [Index(nameof(InventoryItemId), nameof(ReceivedTimestamp), nameof(CurrentQuantity))]
    public class InventoryItemCost
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }
        [DisplayName("Received Timestamp")]
        public DateTime ReceivedTimestamp { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        [DisplayName("Original Cost")]
        public decimal OriginalCost { get; set; } // Total cost of the lot at time of receipt
        [DisplayName("Original Quantity")]
        public int OriginalQuantity { get; set; } // Quantity of the lot at time of receipt
        [Column(TypeName = "decimal(12,2)")]
        [DisplayName("Current Cost")]
        public decimal CurrentCost { get; set; } // Total cost of the remaining quantity in the lot
        [DisplayName("Current Quantity")]
        public int? CurrentQuantity { get; set; } // Remaining quantity in the lot (nullable to allow for fully consumed lots)

        [JsonIgnore]
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public InventoryItem? InventoryItem { get; set; } = null;
    }
}
