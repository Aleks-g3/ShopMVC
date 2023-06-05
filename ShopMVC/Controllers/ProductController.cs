using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Extensions;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Category,Price,AvailableQuantity")]CreateProductFormDTO createProductFormDto)
    {
        var newProduct = createProductFormDto.MapToProduct();

        await _productService.Create(newProduct);

        return RedirectToAction("Manage");
    }

    public async Task<IActionResult> Manage()
    {
        var updatableProductViewModels = await _productService.GetAll();
        return View(updatableProductViewModels);
    }


    [HttpGet("product/edit/{productId}")]
    public async Task<IActionResult> Edit(long productId)
    {
        var updatableProductViewModel = await _productService.GetById(productId);
        return updatableProductViewModel is null ? View("NotFound") : View(updatableProductViewModel);
    }

    [HttpPost("product/edit/{productId}")]
    public async Task<IActionResult> Edit(long productId, [Bind("Name,Category,Price,AvailableQuantity")] UpdateProductFormDTO updateProductFormDto)
    {
        var existingProduct = updateProductFormDto.MapToProduct();

        await _productService.Update(productId, existingProduct);

        return RedirectToAction("Manage");
    }

    [HttpPost("product/delete/{productId}"),ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(long productId)
    {
        var updatableProductViewModel = await _productService.GetById(productId);
        if (updatableProductViewModel is null)
            return View("NotFound");

        await _productService.Delete(productId);

        return RedirectToAction("Manage");
    }
}