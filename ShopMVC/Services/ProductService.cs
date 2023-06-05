using Microsoft.EntityFrameworkCore;
using ShopMVC.Data;
using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdatableProductViewModel[]> GetAll()
    {
        var products = await _unitOfWork.ProductRepository.GetAllWithOrderDescByModifiedOn();
        return products.Select(UpdatableProductViewModel.Create).ToArray();
    }

    public async Task<SimpleProductViewModel[]> GetSimpleProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllWithOrderDescByModifiedOn();
        return products.Select(SimpleProductViewModel.Create).ToArray();
    }

    public async Task<CreateUpdateProductFormDTO?> GetById(long productId)
    {
        var product = await _unitOfWork.ProductRepository.GetAll().FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null)
            return null;
        
        return CreateUpdateProductFormDTO.Create(product);
    }

    public async Task Create(Product newProduct)
    {
        if (await _unitOfWork.ProductRepository.IsExist(newProduct.Name))
            throw new ArgumentException($"Product with name '{newProduct.Name}' already exists");

        await _unitOfWork.ProductRepository.Create(newProduct);
    }

    public async Task Update(long productId, Product existingProduct)
    {
        var product = await _unitOfWork.ProductRepository.GetById(productId);
        if (product is null)
            throw new ArgumentException($"Product with id '{productId}' not exist");

        if (await _unitOfWork.ProductRepository.IsExistWithNameForOtherProductIds(productId, existingProduct.Name))
            throw new ArgumentException($"Product with name '{existingProduct.Name}' already exists");

        product.Update(existingProduct);
        
        await _unitOfWork.ProductRepository.Update(product);
    }

    public Task Delete(long productId)
    {
        return _unitOfWork.ProductRepository.DeleteById(productId);
    }
}