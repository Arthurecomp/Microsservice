namespace Microsservice.Contracts
{
    // Representa um item dentro da mensagem de venda
    public class OrderItemMessage
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // A mensagem principal que será enviada para o RabbitMQ
    public class SaleConfirmedMessage
    {
        public List<OrderItemMessage> OrderItems { get; set; } = new();
    }
}
