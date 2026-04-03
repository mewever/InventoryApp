using InventoryApp.Shared.ApiModels;
using InventoryApp.Shared.Data;
using InventoryApp.Shared.Models;

namespace InventoryAppMvc.Services
{
    public interface IInventoryItemsService
    {
        public Task<IEnumerable<InventoryItem>> Get();
        public Task<InventoryItem?> Get(int id);
        public Task<int> Add(InventoryItem item);
        public Task Update(InventoryItem item);
        public Task Delete(InventoryItem item);
        public Task ReceiveInventory(ReceiveInventoryRequest request);
        public Task RemoveInventory(RemoveInventoryRequest request);
        public Task<InventoryItemWithCosts> GetCosts(int id);
        public Task<InventoryItemWithHistory> GetHistory(int id);
    }
}
