using InventoryApp.Shared.Data;
using InventoryApp.Shared.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Models
{
    public class RemoveInventoryModel
    {
        public InventoryItem InventoryItem { get; set; } = new InventoryItem();
        public int CurrentQuantity { get => InventoryItem.Quantity; }
        [DisplayName("Reference Number")]
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DisplayName("Quantity Removed")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Removed must be above zero.")]
        [Required(ErrorMessage = "Quantity Removed is required.")]
        [LessThanOrEqualToOtherProperty("CurrentQuantity", "Quantity Removed must be less than or equal to the current inventory quantity.")]
        public int QuantityRemoved { get; set; }
    }
}
