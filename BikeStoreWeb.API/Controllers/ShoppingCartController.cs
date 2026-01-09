using Asp.Versioning;
using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BikeStoreWeb.API.Controllers
{
   
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")] // api/v1/shoppingcart
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        //Sepeti Getirme
        [Authorize]
        [HttpGet]
        public ActionResult<ServiceResponse<List<ShoppingCartItemDto>>> GetCart()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString is null || userIdString is "") return Unauthorized();
            var response = _shoppingCartService.GetCartByCustomerId(userIdString);
            return Ok(response);
        }

        //Sepete Ürün Ekleme
        [HttpPost("add")]
        [Authorize]
        public ActionResult<ServiceResponse<bool>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var userIdSting = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdSting, out int customerId))
            {
                return Unauthorized();
            }

            addToCartDto.CustomerId = (userIdSting);

            var response = _shoppingCartService.AddToCart(addToCartDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        //Sepetten Ürün Silme
        //[HttpDelete("{id}")]
        //public ActionResult<ServiceResponse<bool>> RemoveItem(int id)
        //{
        //    var response = _shoppingCartService.RemoveFromCart(id);
        //    if (!response.Success)
        //        return BadRequest(response);
        //    return Ok(response);
        //}


        //Sepeti Temizleme
        [HttpDelete("clear/{customerId}")]
        public ActionResult<ServiceResponse<bool>> ClearCart()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString is null || userIdString is "") return Unauthorized();


            var response = _shoppingCartService.ClearCart(userIdString, 0);
            return Ok(response);
        }


        //Ürün Silme
        [HttpDelete]
        [Authorize]
        public ActionResult<ServiceResponse<bool>> RemoveItemByProductId(int productId)
        {
            //Bu metot ürünId'ye göre silme işlemi yapar.
            //Önce sepet öğesini bul
            //Daha sonra RemoveFromCart metodunu çağırarak silme işlemini gerçekleştir.

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString is null || userIdString is "") return Unauthorized();


            var cartResponse = _shoppingCartService.GetCartByCustomerId(userIdString);
            var itemToRemove = cartResponse.Data?.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove == null)
            {
                return NotFound(new { messsage = " Ürün sepette yok." });
            }
            var response = _shoppingCartService.RemoveFromCart(itemToRemove.Id);

            return Ok(response);

        }
    }
}
