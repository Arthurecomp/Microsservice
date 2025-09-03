using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vendas.API.Entities;

namespace Vendas.API.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order); //criar pedido
        Task<IEnumerable<Order>> GetOrders();
    }
}

//Microserviço de Gestão de Vendas deve validar a disponibilidade de produtos, criar pedidos e reduzir o estoque. 