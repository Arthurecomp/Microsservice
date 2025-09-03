using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estoque.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Data
{
    public interface ICatalogContext
    {
        DbSet<Product> Products { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


    }
}