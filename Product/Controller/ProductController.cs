using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/v1/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // Post: "/api/v1/products" => create new product
    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto newProduct)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation failed", Errors = errors });
        }
        try
        {
            var product = await _productService.CreateProductAsyncService(newProduct);
            return ApiResponses.Created(product, "Product created successfully");
        }
        catch (ApplicationException ex)
        {
            return StatusCode(500, "Server error: " + ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Server error: " + ex.Message);
        }
    }

    // Get: "/api/v1/products" => get all products
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync(int pageNumber = 1, int pageSize = 3, string? searchQuery = null, string? sortBy = null, string? sortOrder = "asc")
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return ApiResponses.BadRequest("Page number and page size should be greater than 0.");
            }

            var products = await _productService.GetProductsAsyncService(pageNumber, pageSize, searchQuery, sortBy, sortOrder);
            if (products.Count() == 0)
            {
                return ApiResponses.NotFound("The list of products is empty or you try to search for not exisiting product.");
            }
            return ApiResponses.Success(products, "Return the list of products successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Get: "/api/v1/products/{productId}" => get specific product by id
    [Authorize]
    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid productId)
    {
        try
        {
            var product = await _productService.GetProductByIdAsyncService(productId);
            if (product == null)
            {
                return ApiResponses.NotFound("Product does not exist.");
            }

            return ApiResponses.Success(product, "Product is returned successfully");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Delete: "/api/v1/products/{productId}" => delete product by id
    [Authorize]
    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProductByIdAsync(Guid productId)
    {
        try
        {
            var deleted = await _productService.DeleteProductByIdAsyncService(productId);
            if (!deleted)
            {
                return ApiResponses.NotFound("Product does not exist.");
            }

            return ApiResponses.Success("Product deleted successfully.");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }

    // Put: "/api/v1/products/{productId}" => update product by id
    [Authorize]
    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProductByIdAsync(Guid productId, [FromBody] UpdateProductDto updateProduct)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Validation errors:");
            errors.ForEach(error => Console.WriteLine(error));
            return ApiResponses.BadRequest("Invalid product Data");
        }
        try
        {
            var product = await _productService.UpdateProductByIdAsyncService(productId, updateProduct);
            if (product == null)
            {
                return ApiResponses.NotFound("Product does not exist.");
            }

            return ApiResponses.Success("Product updated successfully.");
        }
        catch (ApplicationException ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            return ApiResponses.ServerError("Server error: " + ex.Message);
        }
    }
}