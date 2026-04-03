using InventoryApp.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Shared.ApiModels
{
    public class RemoveInventoryRequest
    {
        public int InventoryItemId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Removed must be above zero.")]
        [Required(ErrorMessage = "Quantity Removed is required.")]
        public int QuantityRemoved { get; set; }
    }
}
