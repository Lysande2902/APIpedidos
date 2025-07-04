using APIPedidos.Models;
using APIPedidos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IValidationService _validationService;

    public ProductsController(IProductService productService, IValidationService validationService)
    {
        _productService = productService;
        _validationService = validationService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(ApiResponse<IEnumerable<Product>>.SuccessResponse(
                products, 
                "Productos obtenidos exitosamente"
            ));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<Product>>.ErrorResponse(
                ValidationMessages.ServerError
            ));
        }
    }

    [HttpGet("paginated")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<Product>>>> GetProductsPaginated(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        // Validar parámetros de paginación
        var (isValid, errors) = _validationService.ValidatePagination(pageNumber, pageSize);
        if (!isValid)
        {
            return BadRequest(ApiResponse<PaginatedResult<Product>>.ErrorResponse(
                ValidationMessages.InvalidRequest, 
                errors
            ));
        }

        try
        {
            var result = await _productService.GetProductsPaginatedAsync(pageNumber, pageSize);
            return Ok(ApiResponse<PaginatedResult<Product>>.SuccessResponse(
                result, 
                "Productos paginados obtenidos exitosamente"
            ));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<PaginatedResult<Product>>.ErrorResponse(
                ValidationMessages.ServerError
            ));
        }
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ApiResponse<Product>>> GetProduct(int productId)
    {
        // Validar ID del producto
        if (productId <= 0)
        {
            return BadRequest(ApiResponse<Product>.ErrorResponse(
                ValidationMessages.ProductIdInvalid
            ));
        }

        try
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound(ApiResponse<Product>.ErrorResponse(
                    ValidationMessages.ProductNotFound
                ));
            }

            return Ok(ApiResponse<Product>.SuccessResponse(
                product, 
                "Producto obtenido exitosamente"
            ));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<Product>.ErrorResponse(
                ValidationMessages.ServerError
            ));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Product>>> CreateProduct([FromBody] Product product)
    {
        // Validar producto
        var (isValid, errors) = _validationService.ValidateProduct(product);
        if (!isValid)
        {
            return BadRequest(ApiResponse<Product>.ErrorResponse(
                ValidationMessages.InvalidRequest, 
                errors
            ));
        }

        try
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(
                nameof(GetProduct),
                new { productId = createdProduct.Id },
                ApiResponse<Product>.SuccessResponse(
                    createdProduct, 
                    "Producto creado exitosamente"
                )
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<Product>.ErrorResponse(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<Product>.ErrorResponse(
                ValidationMessages.ServerError
            ));
        }
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(int productId, [FromBody] Product product)
    {
        // Validar ID del producto
        if (productId <= 0)
        {
            return BadRequest(ApiResponse<Product>.ErrorResponse(
                ValidationMessages.ProductIdInvalid
            ));
        }

        // Validar producto
        var (isValid, errors) = _validationService.ValidateProduct(product);
        if (!isValid)
        {
            return BadRequest(ApiResponse<Product>.ErrorResponse(
                ValidationMessages.InvalidRequest, 
                errors
            ));
        }

        try
        {
            var updatedProduct = await _productService.UpdateProductAsync(productId, product);

            if (updatedProduct == null)
            {
                return NotFound(ApiResponse<Product>.ErrorResponse(
                    ValidationMessages.ProductNotFound
                ));
            }

            return Ok(ApiResponse<Product>.SuccessResponse(
                updatedProduct, 
                "Producto actualizado exitosamente"
            ));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<Product>.ErrorResponse(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<Product>.ErrorResponse(
                ValidationMessages.ServerError
            ));
        }
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult<ApiResponse>> DeleteProduct(int productId)
    {
        // Validar ID del producto
        if (productId <= 0)
        {
            return BadRequest(ApiResponse.ErrorResponse(
                ValidationMessages.ProductIdInvalid
            ));
        }

        try
        {
            var success = await _productService.DeleteProductAsync(productId);

            if (!success)
            {
                return NotFound(ApiResponse.ErrorResponse(
                    ValidationMessages.ProductNotFound
                ));
            }

            return Ok(ApiResponse.SuccessResponse("Producto eliminado exitosamente"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.ErrorResponse(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse.ErrorResponse(
                ValidationMessages.ServerError
            ));
        }
    }
}
