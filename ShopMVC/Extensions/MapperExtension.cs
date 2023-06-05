using ShopMVC.Models.Shared;
using ShopMVC.ViewModels;

namespace ShopMVC.Extensions;

public static class MapperExtension
{
    public static Product MapToProduct(this CreateUpdateProductFormDTO formDto) =>
        new(formDto.Name, formDto.Description, formDto.Category.ToString(), formDto.Price, formDto.AvailableQuantity);
}