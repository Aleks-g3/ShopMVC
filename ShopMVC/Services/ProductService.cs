using ShopMVC.Data.Repositories;
using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _productRepository;

    public ProductService(ILogger<ProductService> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    
    public async Task<SimpleProductViewModel[]> GetAllSimpleProducts()
    {
        var products = await _productRepository.GetAll();
        
        _logger.LogInformation($"Getting products is {products.Length}");
        return products.Select(product => new SimpleProductViewModel()
        {
            Name = product.Name,
            Price = product.Price
        }).ToArray();
    }

    public async Task Create(Product newProduct)
    {
        _logger.LogInformation($"Creating product with name '{newProduct.Name}'");
        await _productRepository.Create(newProduct);
        _logger.LogInformation($"Creating product with name '{newProduct.Name}' Finished");
    }
}