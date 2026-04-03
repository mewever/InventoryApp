using InventoryApp.Shared.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Shared.Models
{
    public class ReceiveInventoryModel
    {
        public InventoryItem InventoryItem { get; set; } = new InventoryItem();
        [DisplayName("Reference Number")]
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DisplayName("Quantity Received")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity Received must be a non-negative value.")]
        [Required(ErrorMessage = "Quantity Received is required.")]
        public int QuantityReceived { get; set; }
        [DisplayName("Amount Paid")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid must be a non-negative value.")]
        [Required(ErrorMessage = "Amount Paid is required.")]
        public decimal AmountPaid { get; set; }
    }
}
