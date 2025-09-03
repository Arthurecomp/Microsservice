using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vendas.API.Entities;

namespace Vendas.API.Data
{
    public interface IVendasContext
    {
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}