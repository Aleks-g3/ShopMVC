using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Data.Repositories;
using ShopMVC.Extensions;
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
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductFormDto createProductFormDto)
    {
        var newProduct = createProductFormDto.MapToProduct();

        await _productService.Create(newProduct);
        
        return RedirectToAction("Manage");
    }

    public IActionResult Manage()
    {
        return View();
    }
}