using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Responses;
using BikeStoreWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;


namespace BikeStoreWeb.API.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public ActionResult<ServiceResponse<List<ProductDto>>> GetAll()
        {
            var response = _productService.GetAllProducts();

            if (response == null)
                return NotFound();

            return Ok(response);
        }


        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ServiceResponse<ProductDto>> GetById(int id)
        {
            var response = _productService.GetProductById(id);
            if (!response.Success)
                return NotFound(response); // 404 döner 

            return Ok(response);
        }


        /// <summary>
        /// Creates a new product using the specified product data and returns a response indicating the result of the
        /// operation.  
        /// </summary>
        /// <remarks>If the product is created successfully, the response includes a link to the product
        /// resource in the location header. The method expects valid product data; invalid or incomplete data may
        /// result in a validation error response.</remarks>
        /// <param name="createProductDto">An object containing the details of the product to create. Must not be null and should include all required
        /// product information.</param>
        /// <returns>An HTTP 201 Created response containing the newly created product and a location header with a link to
        /// retrieve the product by its identifier.</returns>
        [HttpPost]
        public ActionResult<ServiceResponse<ProductDto>> Create(CreateProductDto createProductDto)
        {
            var response = _productService.CreateProduct(createProductDto);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<ServiceResponse<bool>> Update(int id, UpdateProductDto updateProductDto)
        {
            // URL'deki ID ile Body'deki ID uyuşmalı
            if (id != updateProductDto.Id)
                return BadRequest(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "URL'deki ID ile gönderilen verideki ID uyuşmuyor."
                });

            var response = _productService.UpdateProduct(updateProductDto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _productService.DeleteProduct(id);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }
    }
}
