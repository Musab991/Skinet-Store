namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; } = null!;
        public required string PictureUrl { get; set; }
    }
}
