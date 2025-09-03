using Estoque.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Data
{
    public class CatalogContext : DbContext, ICatalogContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

    }
}
