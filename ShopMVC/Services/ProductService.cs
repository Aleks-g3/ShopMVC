using ShopMVC.Data;
using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdatableProductViewModel[]> GetAll()
    {
        var products = await GetProducts();
        return products.Select(UpdatableProductViewModel.Create).ToArray();
    }

    public async Task<SimpleProductViewModel[]> GetSimpleProducts()
    {
        var products = await GetProducts();
        return products.Select(SimpleProductViewModel.Create).ToArray();
    }

    public async Task Create(Product newProduct)
    {
        _logger.LogInformation($"Creating product with name '{newProduct.Name}'");
        await _unitOfWork.ProductRepository.Create(newProduct);
        _logger.LogInformation($"Creating product with name '{newProduct.Name}' Finished");
    }

    private async Task<Product[]> GetProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllWithOrderDescByModifiedOn();
        _logger.LogInformation($"Getting products is {products.Length}");
        return products;
    }
}