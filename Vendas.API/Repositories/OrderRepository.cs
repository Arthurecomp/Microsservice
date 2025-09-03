using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;
using Vendas.API.Entities;

namespace Vendas.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly VendasContext _context;

        public OrderRepository(VendasContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.Include(order => order.OrderItems).ToListAsync();
        }
    }
}
