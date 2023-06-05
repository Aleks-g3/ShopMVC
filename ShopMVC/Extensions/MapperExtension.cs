using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Extensions;

public static class MapperExtension
{
    public static Product MapToProduct(this CreateProductFormDTO formDto) =>
        new(formDto.Name, formDto.Category, formDto.Price, formDto.AvailableQuantity);
    
    public static Product MapToProduct(this UpdateProductFormDTO formDto) => 
        new(formDto.Name, formDto.Category.ToString(), formDto.Price, formDto.AvailableQuantity);
}