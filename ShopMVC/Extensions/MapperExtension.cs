using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Extensions;

public static class MapperExtension
{
    public static Product MapToProduct(this CreateProductFormDto formDto) => 
        new Product(formDto.Name, formDto.Category, formDto.Price, formDto.AvailableQuantity);
}