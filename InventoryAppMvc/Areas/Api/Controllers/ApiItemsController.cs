using InventoryApp.Shared.ApiModels;
using InventoryApp.Shared.Data;
using InventoryApp.Shared.Models;
using InventoryAppMvc.Data;
using InventoryAppMvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppMvc.Areas.Api.Controllers
{
    [Route("api/items")]
    [ApiController]
    //[Authorize]
    public class ApiItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IInventoryItemsService _inventoryItemService;
        private readonly ILogger<ApiItemsController> _logger;

        public ApiItemsController(ApplicationDbContext context, IInventoryItemsService inventoryItemService, ILogger<ApiItemsController> logger)
        {
            _context = context;
            _inventoryItemService = inventoryItemService;
            _logger = logger;
        }

        // GET: api/Items/List
        [HttpGet("list")]
        public async Task<ApiResponse<IEnumerable<InventoryItem>>> List()
        {
            try
            {
                var items = await _context.InventoryItems.ToArrayAsync();
                return ApiResponse<IEnumerable<InventoryItem>>.Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving inventory items.");
                return ApiResponse<IEnumerable<InventoryItem>>.Fail("An unexpected error occurred while retrieving inventory items.");
            }
        }

        // POST: api/Items/Details
        [HttpPost("details")]
        public async Task<ApiResponse<InventoryItem>> Details([FromBody]InventoryItemDetailsRequest request)
        {
            try
            {
                var inventoryItem = await _context.InventoryItems.FindAsync(request.Id);
                if (inventoryItem == null)
                {
                    return ApiResponse<InventoryItem>.Fail("An unknown inventory item ID was provided.");
                }

                return ApiResponse<InventoryItem>.Success(inventoryItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving inventory item details for ID {id}.", request.Id);
                return ApiResponse<InventoryItem>.Fail("An unexpected error occurred while retrieving inventory item details.");
            }
        }

        // POST: api/Items/Add
        [HttpPost("add")]
        public async Task<ApiResponse<AddInventoryItemResponse>> Add([FromBody]InventoryItem inventoryItem)
        {
            try
            {
                _context.Add(inventoryItem);
                await _context.SaveChangesAsync();
                AddInventoryItemResponse response = new AddInventoryItemResponse()
                {
                    Id = inventoryItem.Id
                };
                return ApiResponse<AddInventoryItemResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding an inventory item.");
                return ApiResponse<AddInventoryItemResponse>.Fail("An unexpected error occurred while creating an inventory item.");
            }
        }

        // POST: api/Items/Update
        [HttpPost("update")]
        public async Task<ApiResponse> Update([FromBody]InventoryItem inventoryItem)
        {
            try
            {
                _context.Update(inventoryItem);
                await _context.SaveChangesAsync();
                return ApiResponse.Success();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!InventoryItemExists(inventoryItem.Id))
                {
                    return ApiResponse.Fail("The inventory item no longer exists.");
                }
                else
                {
                    _logger.LogError(ex, "An unexpected database error occurred while updating inventory item details for ID {id}.", inventoryItem.Id);
                    return ApiResponse.Fail("An unexpected database error occurred while updating an inventory item.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating inventory item details for ID {id}.", inventoryItem.Id);
                return ApiResponse.Fail("An unexpected error occurred while updating an inventory item.");
            }
        }

        // POST: api/Items/Delete
        [HttpPost("delete")]
        public async Task<ApiResponse> DeleteConfirmed([FromBody]InventoryItemDeleteRequest request)
        {
            try
            {
                var inventoryItem = await _context.InventoryItems.FindAsync(request.Id);
                if (inventoryItem == null)
                {
                    return ApiResponse.Fail("An unknown inventory item ID was provided.");
                }
                _context.InventoryItems.Remove(inventoryItem);

                await _context.SaveChangesAsync();
                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting inventory item details for ID {id}.", request.Id);
                return ApiResponse.Fail("An unexpected error occurred while deleting an inventory item.");
            }
        }

        // POST: api/Items/Receive
        [HttpPost("receive")]
        public async Task<ApiResponse> Receive([FromBody]ReceiveInventoryRequest request)
        {
            try
            {
                await _inventoryItemService.ReceiveInventory(request);

                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while receiving inventory for inventory item with ID {id}.", request.InventoryItemId);
                return ApiResponse.Fail("An unexpected error occurred while receiving inventory for an inventory item.");
            }
        }

        // POST: api/Items/Remove
        [HttpPost("remove")]
        public async Task<ApiResponse> Remove([FromBody]RemoveInventoryRequest request)
        {
            try
            {
                await _inventoryItemService.RemoveInventory(request);

                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while removing inventory for inventory item with ID {id}.", request.InventoryItemId);
                return ApiResponse.Fail("An unexpected error occurred while removing inventory for an inventory item.");
            }
        }

        // POST: api/Items/Costs
        [HttpPost("costs")]
        public async Task<ApiResponse<InventoryItemWithCosts>> Costs([FromBody]InventoryItemCostsRequest request)
        {
            try
            {
                var inventoryItemWithCosts = await _inventoryItemService.GetCosts(request.Id);
                if (inventoryItemWithCosts == null)
                {
                    return ApiResponse<InventoryItemWithCosts>.Fail("An unexpected error occurred while retreiving costs for an inventory item.");
                }
                return ApiResponse<InventoryItemWithCosts>.Success(inventoryItemWithCosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving costs for an inventory item with ID {id}.", request.Id);
                return ApiResponse<InventoryItemWithCosts>.Fail("An unexpected error occurred while retrieving costs for an inventory item.");
            }
        }

        // POST: api/Items/History/5
        [HttpPost("history")]
        public async Task<ApiResponse<InventoryItemWithHistory>> History([FromBody] InventoryItemHistoryRequest request)
        {
            try
            {
                var inventoryItemWithHistory = await _inventoryItemService.GetHistory(request.Id);
                if (inventoryItemWithHistory == null)
                {
                    return ApiResponse<InventoryItemWithHistory>.Fail("An unexpected error occurred while retreiving history for an inventory item.");
                }
                return ApiResponse<InventoryItemWithHistory>.Success(inventoryItemWithHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving history for an inventory item with ID {id}.", request.Id);
                return ApiResponse<InventoryItemWithHistory>.Fail("An unexpected error occurred while retrieving history for an inventory item.");
            }
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItems.Any(e => e.Id == id);
        }
    }
}
