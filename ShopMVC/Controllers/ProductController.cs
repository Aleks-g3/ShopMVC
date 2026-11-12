using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Extensions;
using ShopMVC.Models.Exceptions;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Controllers;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Create()
    {
        return RenderCreateUpdateView("Create", new CreateUpdateProductFormDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Category,Price,AvailableQuantity")]CreateUpdateProductFormDTO createUpdateProductFormDto)
    {
        if (!ModelState.IsValid)
            return RenderCreateUpdateView("Create", createUpdateProductFormDto);

        try
        {
            await _productService.Create(createUpdateProductFormDto.MapToProduct());
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RenderCreateUpdateView("Create", createUpdateProductFormDto);
        }

        TempData["SuccessMessage"] = "Product has been created.";
        return RedirectToAction(nameof(Manage));
    }

    public async Task<IActionResult> Manage()
    {
        var updatableProductViewModels = await _productService.GetAll();
        return View(updatableProductViewModels);
    }


    [HttpGet("product/edit/{productId}")]
    public async Task<IActionResult> Edit(long productId)
    {
        var updatableProductViewModel = await _productService.GetFormById(productId);
        if (updatableProductViewModel is null)
            return View("NotFound");

        return RenderCreateUpdateView("Edit", updatableProductViewModel, productId);
    }

    [HttpPost("product/edit/{productId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long productId,
        [Bind("Name,Description,Category,Price,AvailableQuantity,RowVersion")] CreateUpdateProductFormDTO createUpdateProductFormDto)
    {
        if (!ModelState.IsValid)
            return RenderCreateUpdateView("Edit", createUpdateProductFormDto, productId);

        try
        {
            await _productService.Update(productId, createUpdateProductFormDto.MapToProduct(),
                createUpdateProductFormDto.RowVersion);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RenderCreateUpdateView("Edit", createUpdateProductFormDto, productId);
        }
        catch (ConcurrencyConflictException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return RenderCreateUpdateView("Edit", createUpdateProductFormDto, productId);
        }

        TempData["SuccessMessage"] = "Product has been updated.";
        return RedirectToAction(nameof(Manage));
    }

    [HttpPost("product/delete/{productId}"),ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long productId)
    {
        var updatableProductViewModel = await _productService.GetFormById(productId);
        if (updatableProductViewModel is null)
            return View("NotFound");

        await _productService.Delete(productId);

        TempData["SuccessMessage"] = "Product has been deleted.";
        return RedirectToAction(nameof(Manage));
    }

    [HttpGet("product/{productId}/details")]
    [AllowAnonymous]
    public async Task<IActionResult> GetWithDetailsById(long productId)
    {
        var productWithDetailsViewModel = await _productService.GetWithDetailsById(productId);
        if (productWithDetailsViewModel is null)
            return View("NotFound");

        return View("ProductWithDetailsView",productWithDetailsViewModel);
    }

    private IActionResult RenderCreateUpdateView(string action, CreateUpdateProductFormDTO createUpdateProductFormDto,
        long? productId = null)
    {
        ViewBag.Action = action;
        ViewBag.ProductId = productId;
        return View("CreateUpdateView", createUpdateProductFormDto);
    }
}
