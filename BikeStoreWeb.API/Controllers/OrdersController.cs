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
    [Route("api/v{version:apiVersion}/[controller]")] // api/v1/orders
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Processes a checkout request and creates a new order based on the provided checkout details.    
        /// </summary>
        /// <remarks>Returns a 400 Bad Request response if the order could not be created due to invalid
        /// input or business rule violations.</remarks>
        /// <param name="checkoutDto">The checkout information used to create the order. Must contain all required order and payment details.</param>
        /// <returns>An ActionResult containing a ServiceResponse with the created order details if successful; otherwise, a
        /// ServiceResponse with error information.</returns>
        [Authorize]
        [HttpPost("checkout")]
        public ActionResult<ServiceResponse<OrderDto>> CheckOut([FromBody] CheckoutDto checkoutDto)
        {
            //GÜVENLİK ADIMI: Token'dan CustomerId'yi çek
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) )
            {
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });
            }

            //CheckoutDto içindeki CustomerId'yi token'dan alınan değerle değiştir
            checkoutDto.CustomerId = userIdString;

            //Servisi çağır
            var response = _orderService.CreateOrder(checkoutDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all orders associated with the specified customer.    
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer whose orders are to be retrieved.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="ServiceResponse{T}"/> with a list of <see
        /// cref="OrderDto"/> objects for the specified customer. Returns an empty list if the customer has no orders.</returns>
        [Authorize]
        [HttpGet("customer/orders")]
        public ActionResult<ServiceResponse<List<OrderDto>>> GetOrders()
        {
            var userIdstring = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdstring, out int customerId))
            {
                return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });
            }

            var response = _orderService.GetOrderByCustomerId(customerId);
            return Ok(response);
        }

    }
}
