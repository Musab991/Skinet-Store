using Core.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string CartId { get; set; } = string.Empty;
        public int DeliveryMethodId { get; set; }
        [Required]
        public ShippingAddress ShippingAddress { get; set; } = null!;
        [Required]
        public PaymentSummary PaymentSummary { get; set; } = null!;
        //best practice to create dto for these related objects
        //to make sure good validation error messages 
        //better for maintaince .. 
        //but this just for simplicaty used orginal objects..
    }
}
