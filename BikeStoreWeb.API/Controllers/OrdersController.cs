using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikeStoreWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<ServiceResponse<OrderDto>> CheckOut(CheckoutDto checkoutDto)
        {
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
        [HttpGet("customer/{customerId:int}")]
        public ActionResult<ServiceResponse<List<OrderDto>>> GetOrders(int customerId)
        {
            var response = _orderService.GetOrderByCustomerId(customerId);
            return Ok(response);
        }

    }
}
