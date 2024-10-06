using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder);
    Task<ProductDto> CreateProductAsyncService(CreateProductDto newProduct);
    Task<ProductDto?> GetProductByIdAsyncService(Guid productId);
    Task<bool> DeleteProductByIdAsyncService(Guid productId);
    Task<ProductDto?> UpdateProductByIdAsyncService(Guid productId, UpdateProductDto updateProduct);
}

public class ProductService : IProductService
{
    private readonly AppDBContext _appDbContext;
    private readonly IMapper _mapper;

    public ProductService(AppDBContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    // Create Product
    public async Task<ProductDto> CreateProductAsyncService(CreateProductDto newProduct)
    {
        try
        {
            var product = _mapper.Map<Product>(newProduct);
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while saving the product to the database. Please try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Get all products
    public async Task<List<ProductDto>> GetProductsAsyncService(int pageNumber, int pageSize, string searchQuery, string sortBy, string sortOrder)
    {
        try
        {
            var products = await _appDbContext.Products.ToListAsync();
            // using query to search for all the products whos matching the image otherwise return null
            var filterProduct = products.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                filterProduct = filterProduct.Where(p => p.Image.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
                if (filterProduct.Count() == 0)
                {
                    var exisitingProduct = _mapper.Map<List<ProductDto>>(filterProduct);
                    return exisitingProduct;
                }
            }
            // sort the list of product dependes on size or color or meterial as desc or asc othwewise shows the default
            filterProduct = sortBy?.ToLower() switch
            {
                "size" => sortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Size) : filterProduct.OrderBy(p => p.Size),
                "color" => sortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Color) : filterProduct.OrderBy(p => p.Color),
                "meterial" => sortOrder == "desc" ? filterProduct.OrderByDescending(p => p.Material) : filterProduct.OrderBy(p => p.Material),
                _ => filterProduct.OrderBy(p => p.Material) // default 
            };
            // return the result in pagination 
            var paginationResult = filterProduct.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var productData = _mapper.Map<List<ProductDto>>(paginationResult);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while retrieving products from the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Get product by ID
    public async Task<ProductDto?> GetProductByIdAsyncService(Guid productId)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }
            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while retrieving the product from the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Delete product by ID
    public async Task<bool> DeleteProductByIdAsyncService(Guid productId)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }
            _appDbContext.Products.Remove(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while deleting the product from the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }

    // Update product by ID
    public async Task<ProductDto?> UpdateProductByIdAsyncService(Guid productId, UpdateProductDto updateProduct)
    {
        try
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return null;
            }

            product.Size = updateProduct.Size ?? product.Size;
            product.Color = updateProduct.Color ?? product.Color;
            product.Material = updateProduct.Material ?? product.Material;
            product.Image = updateProduct.Image ?? product.Image;

            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();

            var productData = _mapper.Map<ProductDto>(product);
            return productData;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            throw new ApplicationException("An error occurred while updating the product in the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            throw new ApplicationException("An unexpected error occurred. Please try again later.");
        }
    }
}