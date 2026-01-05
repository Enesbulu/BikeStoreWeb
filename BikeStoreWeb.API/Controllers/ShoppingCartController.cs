using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BikeStoreWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("{customerId}")]
        public ActionResult<ServiceResponse<List<ShoppingCartItemDto>>> GetCart(int customerId)
        {
            var response = _shoppingCartService.GetCartByCustomerId(customerId);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ServiceResponse<bool>> AddToCart(AddToCartDto addToCartDto)
        {
            var response = _shoppingCartService.AddToCart(addToCartDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public ActionResult<ServiceResponse<bool>> RemoveItem(int id)
        {
            var response = _shoppingCartService.RemoveFromCart(id);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("clear/{customerId}")]
        public ActionResult<ServiceResponse<bool>> ClearCart(int customerId)
        {
            var response = _shoppingCartService.ClearCart(customerId,0);
            return Ok(response);
        }



    }
}
