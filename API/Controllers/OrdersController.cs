using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Stripe.Climate;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController(ICartService cartService, IUnitOfWork unit) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<Core.Entities.OrderAggregate.Order>> CreateOrder(CreateOrderDto orderDto)
        {
       
            var email = User.GetEmail();

            var cart = await cartService.GetCartAsync(orderDto.CartId);

            if (cart == null)
            {
                return BadRequest("Cart not found");
            }

            if (cart.PaymentIntentId == null)
            {
                return BadRequest("No payment intent for this order");
            }

            var items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem = await unit.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId);
                if (productItem == null)
                {

                    return BadRequest("Problem with the order");
                }

                var itemOrdered = new ProductItemOrdered
                {
                    PictureUrl = productItem.PictureUrl,
                    ProductName = productItem.Name,
                    ProductId = productItem.Id
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);

            }


            var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod == null)
            {
                return BadRequest("No delivery method selected");
            }

            Core.Entities.OrderAggregate.Order order = new Core.Entities.OrderAggregate.Order
            {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = orderDto.ShippingAddress,
                PaymentSummary = orderDto.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = email,
                
            };


            unit.Repository<Core.Entities.OrderAggregate.Order>().Add(order);

            if (await unit.Complete())
            {
                return order;
            }
            else
            {
                return BadRequest("Problem creating order");
            }

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Core.Entities.OrderAggregate.Order>>> GetOrdersForUser()
        {
            var email = User.GetEmail();

            var spec = new OrderSpecification(email);

            var orders = await unit.Repository<Core.Entities.OrderAggregate.Order>().ListAsync(spec);

            var ordersToReturn = orders.Select(o => o.ToDo()).ToList();
            return Ok(ordersToReturn);

        }
         
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var email =User.GetEmail();

            var spec = new OrderSpecification(email,id);

            var order=await
            unit.Repository<Core.Entities.OrderAggregate.Order>().GetEntityWithSpec(spec);

            if(order == null)
            {
                return NotFound();
            }

            return order.ToDo();

        }

    }

}
