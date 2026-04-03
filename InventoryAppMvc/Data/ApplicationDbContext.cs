using InventoryApp.Shared.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppMvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemCost> InventoryItemCosts { get; set; }
        public DbSet<HistoryItem> HistoryItems { get; set; }
        public DbSet<HistoryItemCost> HistoryItemCosts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
