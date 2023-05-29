﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShopMVC.Data.Repositories;
using ShopMVC.Models;
using ShopMVC.Services;
using ShopMVC.ViewModels;

namespace ShopMVC.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var simpleProductViewModels = await _productService.GetSimpleProducts();
        return View(simpleProductViewModels);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}