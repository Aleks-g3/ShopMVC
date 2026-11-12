using ShopMVC.Data;
using ShopMVC.Models.Exceptions;
using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task<ProductWithDetailsViewModel?> GetWithDetailsById(long productId)
    {
        var product = await _unitOfWork.ProductRepository.GetById(productId);
        if (product is null)
            return null;
        
        return ProductWithDetailsViewModel.Create(product);
    }

    public async Task<CreateUpdateProductFormDTO?> GetFormById(long productId)
    {
        var product = await _unitOfWork.ProductRepository.GetById(productId);
        if (product is null)
            return null;
        
        return CreateUpdateProductFormDTO.Create(product);
    }

    public async Task Create(Product newProduct)
    {
        if (await _unitOfWork.ProductRepository.IsExist(newProduct.Name))
            throw new ArgumentException($"Product with name '{newProduct.Name}' already exists", nameof(newProduct));

        newProduct.UpsertUser(CurrentUserName);
        await _unitOfWork.ProductRepository.Create(newProduct);
    }

    public async Task Update(long productId, Product updatedProduct, string? rowVersion)
    {
        var product = await _unitOfWork.ProductRepository.GetById(productId);
        if (product is null)
            throw new ArgumentException($"Product with id '{productId}' does not exist", nameof(productId));

        if (!string.IsNullOrWhiteSpace(rowVersion) &&
            !string.Equals(Convert.ToBase64String(product.RowVersion), rowVersion, StringComparison.Ordinal))
        {
            throw new ConcurrencyConflictException(
                "This product was modified by another user. Reload the form and try again.");
        }

        if (await _unitOfWork.ProductRepository.IsExistWithNameForOtherProductIds(productId, updatedProduct.Name))
            throw new ArgumentException($"Product with name '{updatedProduct.Name}' already exists", nameof(updatedProduct));

        product.Update(updatedProduct);
        product.UpsertUser(CurrentUserName);
        
        await _unitOfWork.ProductRepository.Update(product);
    }

    public Task Delete(long productId)
    {
        return _unitOfWork.ProductRepository.DeleteById(productId);
    }

    private string CurrentUserName =>
        _httpContextAccessor.HttpContext?.User.Identity?.Name
        ?? throw new InvalidOperationException("Cannot determine the current user.");
}
