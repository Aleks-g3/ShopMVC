using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Data.Repositories;
using ShopMVC.Extensions;
using ShopMVC.Services;
using ShopMVC.ViewModels;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

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

    [Microsoft.AspNetCore.Mvc.HttpPost]
    [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductFormDto createProductFormDto)
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
}