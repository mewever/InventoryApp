using InventoryApp.Shared.ApiModels;
using InventoryApp.Shared.Data;
using InventoryApp.Shared.Models;
using InventoryAppMvc.Data;
using InventoryAppMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppMvc.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IInventoryItemsService _inventoryItemService;

        public ItemsController(ApplicationDbContext context, IInventoryItemsService inventoryItemService)
        {
            _context = context;
            _inventoryItemService = inventoryItemService;
        }

        // GET: InventoryItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.InventoryItems.ToListAsync());
        }

        // GET: InventoryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        // GET: InventoryItems/Create
        public IActionResult Create()
        {
            InventoryItem newItem = new InventoryItem
            {
                Quantity = 0
            };
            return View(newItem);
        }

        // POST: InventoryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductCode,Name,Quantity")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            return View(inventoryItem);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductCode,Name,Quantity")] InventoryItem inventoryItem)
        {
            if (id != inventoryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryItemExists(inventoryItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            return View(inventoryItem);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem != null)
            {
                _context.InventoryItems.Remove(inventoryItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: InventoryItems/Receive/5
        public async Task<IActionResult> Receive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            ReceiveInventoryModel model = new ReceiveInventoryModel
            {
                InventoryItem = inventoryItem
            };
            return View(model);
        }

        // POST: InventoryItems/Receive/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(int id, [Bind("InventoryItem,ReferenceNumber,QuantityReceived,AmountPaid")] ReceiveInventoryModel receivedInventory)
        {
            if (ModelState.IsValid)
            {
                var inventoryItem = await _context.InventoryItems.FindAsync(receivedInventory.InventoryItem.Id);
                if (inventoryItem == null)
                {
                    return NotFound();
                }

                try
                {
                    ReceiveInventoryRequest request = new ReceiveInventoryRequest
                    {
                        InventoryItemId = receivedInventory.InventoryItem.Id,
                        ReferenceNumber = receivedInventory.ReferenceNumber,
                        Description = receivedInventory.Description,
                        QuantityReceived = receivedInventory.QuantityReceived,
                        AmountPaid = receivedInventory.AmountPaid
                    };
                    await _inventoryItemService.ReceiveInventory(request);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(receivedInventory);
        }

        // GET: InventoryItems/Remove/5
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            RemoveInventoryModel model = new RemoveInventoryModel
            {
                InventoryItem = inventoryItem
            };
            return View(model);
        }

        // POST: InventoryItems/Remove/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id, [Bind("InventoryItem,ReferenceNumber,Description,QuantityRemoved")] RemoveInventoryModel removedInventory)
        {
            if (ModelState.IsValid)
            {
                var inventoryItem = await _context.InventoryItems.FindAsync(removedInventory.InventoryItem.Id);
                if (inventoryItem == null)
                {
                    return NotFound();
                }

                try
                {
                    RemoveInventoryRequest request = new RemoveInventoryRequest
                    {
                        InventoryItemId = removedInventory.InventoryItem.Id,
                        ReferenceNumber = removedInventory.ReferenceNumber,
                        Description = removedInventory.Description,
                        QuantityRemoved = removedInventory.QuantityRemoved,
                    };
                    await _inventoryItemService.RemoveInventory(request);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(removedInventory);
        }

        // GET: InventoryItems/Costs/5
        public async Task<IActionResult> Costs(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var inventoryItemWithCosts = await _inventoryItemService.GetCosts((int)id);
                return View(inventoryItemWithCosts);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET: InventoryItems/History/5
        public async Task<IActionResult> History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var inventoryItemWithHistory = await _inventoryItemService.GetHistory((int)id);
                return View(inventoryItemWithHistory);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItems.Any(e => e.Id == id);
        }
    }
}
