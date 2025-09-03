using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsservice.Contracts;
using Vendas.API.DTOs;
using Vendas.API.Entities;
using Vendas.API.Repositories;


namespace Vendas.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPublishEndpoint _publishEndpoint;
        public VendasController(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _httpClientFactory = httpClientFactory;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var products = await _orderRepository.GetOrders();
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var newOrder = new Order { OrderDate = System.DateTime.UtcNow };
            var httpClient = _httpClientFactory.CreateClient();

            //lógica para validar em estoque
            foreach (var itemDto in orderDto.OrderItems)
            {
                var response = await httpClient.GetAsync($"http://localhost:5296/api/v1/Catalog/{itemDto.ProductId}");

                if (!response.IsSuccessStatusCode)
                    return BadRequest($"Produto com ID {itemDto.ProductId} não encontrado no estoque.");

                var product = await response.Content.ReadFromJsonAsync<ProductDto>();
                if (product == null)
                {
                    return BadRequest($"Não foi possível obter os detalhes do produto com ID {itemDto.ProductId} do serviço de estoque.");
                }
                if (product.QuantidadeEmEstoque < itemDto.Quantity) //valida quantidade do estoque
                    return BadRequest($"Estoque insuficiente para o produto '{product.Nome}'.");

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    ProductName = product.Nome,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Preco
                };
                newOrder.OrderItems.Add(orderItem);
                newOrder.TotalPrice += orderItem.Quantity * orderItem.UnitPrice;
            }



            await _orderRepository.CreateOrderAsync(newOrder);

            var saleMessage = new SaleConfirmedMessage
            {
                OrderItems = newOrder.OrderItems.Select(item => new OrderItemMessage
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };
            await _publishEndpoint.Publish(saleMessage);
            return StatusCode(StatusCodes.Status201Created, newOrder);
        }
    }
}

