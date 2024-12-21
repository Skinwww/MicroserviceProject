using ECommercelib.SharedLibrary.Responses;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using Response = ECommercelib.SharedLibrary.Responses.Response;


namespace ProducApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
       

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
                if (!products.Any())
                return NotFound("Не найдено продуктов в базе данных");

            var list = ProductConversion.FromEntity(products);

            return list.Any() ? Ok(list) : NotFound("Не найдено продуктов");
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
           var product = await productInterface.FindByIdAsync(id);
            if (product is null)
                return NotFound("Продукт не найден");

            var _product = ProductConversion.FromEntity(new[] { product }).FirstOrDefault();
            return _product is not null ? Ok(_product) : NotFound("Продукт не найден");
        }

        // POST: api/product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }


        // PUT: api/product/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }


        // DELETE: api/product/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
