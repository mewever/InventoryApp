using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Shared.ApiModels
{
    public class ReceiveInventoryRequest
    {
        public int InventoryItemId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Quantity Received must be a non-negative value.")]
        [Required(ErrorMessage = "Quantity Received is required.")]
        public int QuantityReceived { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Amount Paid must be a non-negative value.")]
        [Required(ErrorMessage = "Amount Paid is required.")]
        public decimal AmountPaid { get; set; }

    }
}
