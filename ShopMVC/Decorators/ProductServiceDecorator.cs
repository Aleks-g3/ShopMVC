using ShopMVC.Models.Shared;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Decorators;

public class ProductServiceDecorator : IProductService
{
    private readonly ILogger _logger;
    private readonly IProductService _productService;

    public ProductServiceDecorator(IProductService productService, ILogger<ProductServiceDecorator> logger)
    {
        _productService = productService;
        _logger = logger;
    }
    public async Task<UpdatableProductViewModel[]> GetAll()
    {
        var updatableProductViewModels = await _productService.GetAll();
        _logger.LogInformation("Getting products returned {ProductCount} items", updatableProductViewModels.Length);
        return updatableProductViewModels;
    }

    public async Task<SimpleProductViewModel[]> GetSimpleProducts()
    {
        var simpleProductViewModels = await _productService.GetSimpleProducts();
        _logger.LogInformation("Getting simple products returned {ProductCount} items", simpleProductViewModels.Length);
        return simpleProductViewModels;
    }

    public async Task<ProductWithDetailsViewModel?> GetWithDetailsById(long productId)
    {
        _logger.LogInformation("Getting product with id {ProductId}", productId);
        var productWithDetailsViewModel = await _productService.GetWithDetailsById(productId);
        _logger.LogInformation("Finished getting product with id {ProductId}", productId);
        return productWithDetailsViewModel;
    }

    public async Task<CreateUpdateProductFormDTO?> GetFormById(long productId)
    {
        _logger.LogInformation("Getting product form with id {ProductId}", productId);
        var createUpdateProductFormDto = await _productService.GetFormById(productId);
        _logger.LogInformation("Finished getting product form with id {ProductId}", productId);
        return createUpdateProductFormDto;
    }

    public async Task Create(Product newProduct)
    {
        _logger.LogInformation("Creating product with name {ProductName}", newProduct.Name);
        await _productService.Create(newProduct);
        _logger.LogInformation("Finished creating product with name {ProductName}", newProduct.Name);
    }

    public async Task Update(long productId, Product updatedProduct, string? rowVersion)
    {
        _logger.LogInformation("Updating product with id {ProductId}", productId);
        await _productService.Update(productId, updatedProduct, rowVersion);
        _logger.LogInformation("Finished updating product with id {ProductId}", productId);
    }

    public async Task Delete(long productId)
    {
        _logger.LogInformation("Deleting product with id {ProductId}", productId);
        await _productService.Delete(productId);
        _logger.LogInformation("Finished deleting product with id {ProductId}", productId);
    }
}
