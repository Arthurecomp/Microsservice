using Estoque.API.Data;
using Estoque.API.Entities;
using Microsoft.EntityFrameworkCore;


namespace Estoque.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task<int?> GetEstoqueById(int id)
        {
            var produto = await _context.Products.FindAsync(id);

            return produto.QuantidadeEmEstoque;

        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> UpdateQtd(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}

