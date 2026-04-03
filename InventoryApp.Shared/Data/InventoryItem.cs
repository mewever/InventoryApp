using InventoryApp.Shared.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryApp.Shared.Data
{
    [DisplayName("Inventory Item")]
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        [DisplayName("Product Code")]
        public string ProductCode { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public List<InventoryItemCost>? Costs { get; set; }
        public List<HistoryItem>? History { get; set; }
    }
}
