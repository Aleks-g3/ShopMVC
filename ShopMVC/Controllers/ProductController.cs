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
        ViewBag.Action = "Create";
        return View("CreateUpdateView");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Category,Price,AvailableQuantity")]CreateUpdateProductFormDTO createUpdateProductFormDto)
    {
        var newProduct = createUpdateProductFormDto.MapToProduct();

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
        ViewBag.Action = "Edit";
        return updatableProductViewModel is null ? View("NotFound") : View("CreateUpdateView",updatableProductViewModel);
    }

    [HttpPost("product/edit/{productId}")]
    public async Task<IActionResult> Edit(long productId, [Bind("Name,Description,Category,Price,AvailableQuantity")] CreateUpdateProductFormDTO createUpdateProductFormDto)
    {
        var existingProduct = createUpdateProductFormDto.MapToProduct();

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