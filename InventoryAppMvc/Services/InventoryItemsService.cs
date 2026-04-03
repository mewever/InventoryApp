using InventoryApp.Shared.ApiModels;
using InventoryApp.Shared.Data;
using InventoryApp.Shared.Models;
using InventoryAppMvc.Data;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace InventoryAppMvc.Services
{
    public class InventoryItemsService(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        ILogger<InventoryItemsService> logger) : IInventoryItemsService
    {
        public async Task<IEnumerable<InventoryItem>> Get()
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    return await dbContext.InventoryItems.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving inventory items.");
                throw;
            }
        }

        public async Task<InventoryItem?> Get(int id)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    return await dbContext.InventoryItems.FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving an inventory item with ID {InventoryItemId}.", id);
                throw;
            }
        }

        public async Task<int> Add(InventoryItem item)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    dbContext.Add(item);
                    await dbContext.SaveChangesAsync();
                    return item.Id;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a new inventory item.");
                throw;
            }
        }

        public async Task Update(InventoryItem item)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    dbContext.Update(item);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating inventory item with ID {InventoryItemId}.", item.Id);
                throw;
            }
        }

        public async Task Delete(InventoryItem item)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    dbContext.Remove(item);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting inventory item with ID {InventoryItemId}.", item.Id);
                throw;
            }
        }

        public async Task ReceiveInventory(ReceiveInventoryRequest request)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    var inventoryItem = await dbContext.InventoryItems.FindAsync(request.InventoryItemId);
                    if (inventoryItem == null)
                    {
                        logger.LogError("Attempted to receive inventory for an invalid inventory item ID: {InventoryItemId}", request.InventoryItemId);
                        throw new InvalidOperationException("Unknown inventory item ID provided.");
                    }

                    // Add the quantity to the inventory item
                    inventoryItem.Quantity += request.QuantityReceived;
                    dbContext.Update(inventoryItem);

                    // Add the history item
                    HistoryItem historyItem = new HistoryItem
                    {
                        InventoryItemId = request.InventoryItemId,
                        Timestamp = DateTime.UtcNow,
                        ReferenceNumber = request.ReferenceNumber,
                        Description = request.Description,
                        QuantityChange = request.QuantityReceived,
                        ValueChange = request.AmountPaid,
                    };
                    dbContext.Add(historyItem);
                    await dbContext.SaveChangesAsync();

                    // Add the inventory item cost
                    InventoryItemCost inventoryItemCost = new InventoryItemCost
                    {
                        InventoryItemId = request.InventoryItemId,
                        ReceivedTimestamp = historyItem.Timestamp,
                        OriginalCost = request.AmountPaid,
                        CurrentCost = request.AmountPaid,
                        OriginalQuantity = request.QuantityReceived,
                        CurrentQuantity = request.QuantityReceived
                    };
                    dbContext.Add(inventoryItemCost);
                    await dbContext.SaveChangesAsync();

                    // Add the history item cost
                    int receivedQuantity = request.QuantityReceived;
                    decimal receivedValue = request.AmountPaid;
                    int adjustmentQuantity = 0;
                    decimal adjustmentValue = 0;
                    decimal unitValue;
                    if (request.QuantityReceived == 0)
                    {
                        unitValue = request.AmountPaid;
                    }
                    else
                    {
                        unitValue = Math.Round(request.AmountPaid / request.QuantityReceived, 2);
                    }
                    decimal extendedValue = unitValue * request.QuantityReceived;
                    if (extendedValue != request.AmountPaid)
                    {
                        // If the lot cost cannot be evenly distributed, split off the last item at a different value
                        // to ensure the total of the history item costs matches the amount paid
                        receivedQuantity -= 1;
                        adjustmentQuantity = 1;
                        receivedValue = receivedQuantity * unitValue;
                        adjustmentValue = request.AmountPaid - receivedValue;
                    }
                    HistoryItemCost historyItemCost = new HistoryItemCost
                    {
                        HistoryItemId = historyItem.Id,
                        InventoryItemCostId = inventoryItemCost.Id,
                        QuantityChange = receivedQuantity,
                        UnitValue = unitValue,
                        ValueChange = receivedValue
                    };
                    dbContext.Add(historyItemCost);
                    if (adjustmentQuantity != 0)
                    {
                        // Add the adjusted final history item cost if necessary
                        HistoryItemCost adjustmentHistoryItemCost = new HistoryItemCost
                        {
                            HistoryItemId = historyItem.Id,
                            InventoryItemCostId = inventoryItemCost.Id,
                            QuantityChange = adjustmentQuantity,
                            UnitValue = adjustmentValue,
                            ValueChange = adjustmentValue
                        };
                        dbContext.Add(adjustmentHistoryItemCost);
                    }
                    await dbContext.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while receiving inventory for inventory item ID {InventoryItemId}.", request.InventoryItemId);
                throw;
            }
        }

        public async Task RemoveInventory(RemoveInventoryRequest request)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    var inventoryItem = await dbContext.InventoryItems.FindAsync(request.InventoryItemId);
                    if (inventoryItem == null)
                    {
                        logger.LogError("Attempted to remove inventory for an invalid inventory item ID: {InventoryItemId}", request.InventoryItemId);
                        throw new InvalidOperationException("Unknown inventory item ID provided.");
                    }

                    // Subtract the quantity from the inventory item
                    inventoryItem.Quantity -= request.QuantityRemoved;
                    dbContext.Update(inventoryItem);

                    // Add the history item
                    HistoryItem historyItem = new HistoryItem
                    {
                        InventoryItemId = request.InventoryItemId,
                        Timestamp = DateTime.UtcNow,
                        ReferenceNumber = request.ReferenceNumber,
                        Description = request.Description,
                        QuantityChange = 0 - request.QuantityRemoved,
                        ValueChange = -1, // This will be determined later based on the inventory item costs that are reduced
                    };
                    dbContext.Add(historyItem);
                    await dbContext.SaveChangesAsync();

                    // Determine the inventory cost items to reduce based on the quantity removed, starting with the oldest costs first
                    int quantityChange = 0;
                    decimal valueChange = 0;
                    int quantityToRemove = request.QuantityRemoved;

                    InventoryItemCost[] inventoryCosts = dbContext.InventoryItemCosts
                        .Where(c => c.InventoryItemId == request.InventoryItemId && c.CurrentQuantity != null)
                        .OrderBy(c => c.ReceivedTimestamp)
                        .ToArray() ?? [];

                    foreach (var itemCost in inventoryCosts)
                    {
                        int removingQuantity = 0;
                        decimal removingValue = 0;
                        decimal unitValue = itemCost.CurrentQuantity == 0 ? 0 : Math.Round(itemCost.CurrentCost / (itemCost.CurrentQuantity ?? 0), 2);
                        int adjustmentQuantity = 0;
                        decimal adjustmentValue = 0;
                        if (quantityToRemove >= itemCost.CurrentQuantity)
                        {
                            // All are needed, so remove the full quantity and value from this cost item
                            removingQuantity = itemCost.CurrentQuantity ?? 0;
                            removingValue = itemCost.CurrentCost;

                            // Check for rounding issues with the unit value that require one item to have a different value
                            decimal extendedValue = unitValue * removingQuantity;
                            if (extendedValue != removingValue)
                            {
                                // If the lot cost cannot be evenly distributed, split off the last item at a different value
                                // to ensure the total of the history item costs matches the amount paid
                                removingQuantity -= 1;
                                adjustmentQuantity = 1;
                                decimal newRemovingValue = removingQuantity * unitValue;
                                adjustmentValue = removingValue - newRemovingValue;
                                removingValue = newRemovingValue;
                            }
                        }
                        else
                        {
                            // Since not all items are being removed,
                            // just remove the items needed at the computed unit cost
                            removingQuantity = quantityToRemove;
                            removingValue = quantityToRemove * unitValue;
                        }

                        // Update the inventory on the item cost for the normal removal
                        itemCost.CurrentQuantity -= removingQuantity;
                        itemCost.CurrentCost -= removingValue;

                        // Add the history cost item for the removed quantity and value
                        HistoryItemCost historyItemCost = new HistoryItemCost
                        {
                            HistoryItemId = historyItem.Id,
                            InventoryItemCostId = itemCost.Id,
                            QuantityChange = 0 - removingQuantity,
                            UnitValue = unitValue,
                            ValueChange = 0 - removingValue
                        };
                        dbContext.Add(historyItemCost);

                        quantityChange -= removingQuantity;
                        valueChange -= removingValue;
                        quantityToRemove -= removingQuantity;

                        // Add the adjusted final history item cost if necessary
                        if (adjustmentQuantity != 0)
                        {
                            HistoryItemCost adjustmentHistoryItemCost = new HistoryItemCost
                            {
                                HistoryItemId = historyItem.Id,
                                InventoryItemCostId = itemCost.Id,
                                QuantityChange = 0 - adjustmentQuantity,
                                UnitValue = adjustmentValue,
                                ValueChange = 0 - adjustmentValue
                            };
                            dbContext.Add(adjustmentHistoryItemCost);

                            quantityChange -= adjustmentQuantity;
                            valueChange -= adjustmentValue;
                            quantityToRemove -= adjustmentQuantity;

                            // Update the inventory on the item cost for the adjustment removal
                            itemCost.CurrentQuantity -= adjustmentQuantity;
                            itemCost.CurrentCost -= adjustmentValue;
                        }

                        // Update the inventory item cost
                        if (itemCost.CurrentQuantity == 0)
                        {
                            itemCost.CurrentQuantity = null;
                        }
                        dbContext.Update(itemCost);

                        if (quantityToRemove <= 0)
                        {
                            break;
                        }
                    }

                    // Update the history item value change based on the total value removed
                    historyItem.ValueChange = valueChange;
                    dbContext.Update(historyItem);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while removing inventory for inventory item ID {InventoryItemId}.", request.InventoryItemId);
                throw;
            }
        }

        public async Task<InventoryItemWithCosts> GetCosts(int id)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    var inventoryItem = await dbContext.InventoryItems
                        .Include(i => i.Costs.Where(c => c.CurrentQuantity != null))
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (inventoryItem == null)
                    {
                        logger.LogError("Attempted to get costs for an invalid inventory item ID: {InventoryItemId}", id);
                        throw new InvalidOperationException("Unknown inventory item ID provided.");
                    }
                    return new InventoryItemWithCosts(inventoryItem);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving costs for inventory item ID {InventoryItemId}.", id);
                throw;
            }
        }

        public async Task<InventoryItemWithHistory> GetHistory(int id)
        {
            try
            {
                using (var dbContext = contextFactory.CreateDbContext())
                {
                    var inventoryItem = await dbContext.InventoryItems
                        .Include(i => i.History)
                            .ThenInclude(h => h.Costs)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (inventoryItem == null)
                    {
                        logger.LogError("Attempted to get history for an invalid inventory item ID: {InventoryItemId}", id);
                        throw new InvalidOperationException("Unknown inventory item ID provided.");
                    }
                    inventoryItem.Costs = await dbContext.InventoryItemCosts.Where(c => c.InventoryItemId == id).ToListAsync();
                    return new InventoryItemWithHistory(inventoryItem);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving history for inventory item ID {InventoryItemId}.", id);
                throw;
            }
        }
    }
}
