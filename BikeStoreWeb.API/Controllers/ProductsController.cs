using BikeStoreWeb.Service.Services;
using Microsoft.AspNetCore.Mvc;
using BikeStoreWeb.Service.DTOs;


namespace BikeStoreWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound($"Product with id {id} not found."); // 404 döner 

            return Ok(product);
        }
    }
}
