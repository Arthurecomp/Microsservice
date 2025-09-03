using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vendas.API.DTOs
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> OrderItems { get; set; }
    }
}