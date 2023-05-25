using Microsoft.EntityFrameworkCore;
using ShopMVC.Data;
using ShopMVC.Data.Repositories;
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

    public Task<ProductViewModel[]> GetAll()
    {
        return _unitOfWork.ProductRepository.GetAll().Select(product => ProductViewModel.Create(product))
            .ToArrayAsync();
    }

    public async Task<SimpleProductViewModel[]> GetAllSimpleProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAll().ToArrayAsync();

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
        await _unitOfWork.ProductRepository.Create(newProduct);
        _logger.LogInformation($"Creating product with name '{newProduct.Name}' Finished");
    }
}