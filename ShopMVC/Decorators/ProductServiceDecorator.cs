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
        _logger.LogInformation($"Getting Products is {updatableProductViewModels.Length}");
        return updatableProductViewModels;
    }

    public async Task<SimpleProductViewModel[]> GetSimpleProducts()
    {
        var simpleProductViewModels = await _productService.GetSimpleProducts();
        _logger.LogInformation($"Getting simple products is {simpleProductViewModels.Length}");
        return simpleProductViewModels;
    }

    public async Task<UpdateProductFormDTO?> GetById(long productId)
    {
        _logger.LogInformation($"Getting product with id '{productId}'");
        var updateProductFormDto = await _productService.GetById(productId);
        _logger.LogInformation($"Getting product with id '{productId}' finished");
        return updateProductFormDto;
    }

    public async Task Create(Product newProduct)
    {
        _logger.LogInformation($"Creating product with name '{newProduct.Name}'");
        await _productService.Create(newProduct);
        _logger.LogInformation($"Creating product with name '{newProduct.Name}' dinished");
    }

    public async Task Update(long productId, Product existingProduct)
    {
        _logger.LogInformation($"Updating product with name '{existingProduct.Name}'");
        await _productService.Update(productId, existingProduct);
        _logger.LogInformation($"Updating product with name '{existingProduct.Name}' finished");
    }

    public async Task Delete(long productId)
    {
        _logger.LogInformation($"Deleting product with id '{productId}'");
        await _productService.Delete(productId);
        _logger.LogInformation($"Deleting product with id '{productId}' finished");
    }
}