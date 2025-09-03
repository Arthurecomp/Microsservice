using MassTransit;
using Microsservice.Contracts; // Usando o contrato compartilhado
using Estoque.API.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Estoque.API.Consumers
{
    public class SaleConfirmedConsumer : IConsumer<SaleConfirmedMessage>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<SaleConfirmedConsumer> _logger;

        public SaleConfirmedConsumer(IProductRepository productRepository, ILogger<SaleConfirmedConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SaleConfirmedMessage> context)
        {
            _logger.LogInformation(">>> Mensagem de confirmação de venda recebida.");
            var message = context.Message;

            foreach (var item in message.OrderItems)
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    _logger.LogInformation($"Processando baixa de estoque para o Produto ID: {item.ProductId}. Estoque atual: {product.QuantidadeEmEstoque}.");
                    product.QuantidadeEmEstoque -= item.Quantity;
                    await _productRepository.UpdateQtd(product);
                    _logger.LogInformation($"Estoque do produto '{product.Nome}' atualizado com sucesso para {product.QuantidadeEmEstoque}.");
                }
                else
                {
                    _logger.LogWarning($"Produto com ID {item.ProductId} não encontrado no estoque.");
                }
            }
        }
    }
}

