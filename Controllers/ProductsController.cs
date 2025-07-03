using APIPedidos.Models;
using APIPedidos.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<Product>> GetProduct(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        try
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(
                nameof(GetProduct),
                new { productId = createdProduct.Id },
                createdProduct
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult<Product>> UpdateProduct(int productId, [FromBody] Product product)
    {
        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(productId, product);

            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        try
        {
            var success = await _productService.DeleteProductAsync(productId);

            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
