using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions
{
    public static class Exntensions
    {
        public static OrderDto ToDo(this Core.Entities.OrderAggregate.Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                PaymentSummary = order.PaymentSummary,
                DeliveryMethod = order.DeliveryMethod.Description,
                ShippingPrice = order.DeliveryMethod.Price,
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
                Status = order.Status.ToString(),
                PaymentIntentId = order.PaymentIntentId,
                Total = order.GetTotal()


            };
        }

        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
                ProductId = orderItem.ItemOrdered.ProductId,
                ProductName = orderItem.ItemOrdered.ProductName,
                PictureUrl = orderItem.ItemOrdered.PictureUrl,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity

            };

        }
    }
}