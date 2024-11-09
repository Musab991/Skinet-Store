using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;


    public class CartController (ICartService CartService): BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>>GetCardbyId(string id) 
        {

            var cart = await CartService.GetCartAsync(id);

            return Ok(cart??new ShoppingCart { Id=id});


        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await CartService.SetCartAsync(cart);

            if (updatedCart == null)
            {
                return BadRequest("Problem with cart");
            }

            return updatedCart;
        }
        [HttpDelete]
        public async Task<ActionResult>DeleteCart(string id)
        {
            var result = await CartService.DeleteCartAsync(id);

            if (result == null) {
                return BadRequest("Problem deleting cart");
            }

            return Ok();
        }

    }


