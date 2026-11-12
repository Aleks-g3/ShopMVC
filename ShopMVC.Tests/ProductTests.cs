using ShopMVC.Models.Shared;

namespace ShopMVC.Tests;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_SetsProperties()
    {
        var product = new Product("Watch", "A watch", Category.Watches, 199.99m, 5);

        Assert.Equal("Watch", product.Name);
        Assert.Equal("A watch", product.Description);
        Assert.Equal(Category.Watches, product.Category);
        Assert.Equal(199.99m, product.Price);
        Assert.Equal(5, product.AvailableQuantity);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() =>
            new Product(name, "Description", Category.Clothes, 10m, 1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyDescription_Throws(string description)
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("Name", description, Category.Clothes, 10m, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithNonPositivePrice_Throws(decimal price)
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("Name", "Description", Category.Clothes, price, 1));
    }

    [Fact]
    public void Constructor_WithNegativeQuantity_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("Name", "Description", Category.Clothes, 10m, -1));
    }

    [Fact]
    public void Constructor_WithUndefinedCategory_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("Name", "Description", (Category)999, 10m, 1));
    }

    [Fact]
    public void Update_CopiesAllEditableFieldsIncludingCategory()
    {
        var product = new Product("Name", "Description", Category.Clothes, 10m, 1);
        var updatedProduct = new Product("New name", "New description", Category.Watches, 20m, 3);

        product.Update(updatedProduct);

        Assert.Equal("New name", product.Name);
        Assert.Equal("New description", product.Description);
        Assert.Equal(Category.Watches, product.Category);
        Assert.Equal(20m, product.Price);
        Assert.Equal(3, product.AvailableQuantity);
    }

    [Fact]
    public void UpsertUser_SetsCreatedByOnceAndAlwaysUpdatesModifiedBy()
    {
        var product = new Product("Name", "Description", Category.Clothes, 10m, 1);

        product.UpsertUser("first");
        product.UpsertUser("second");

        Assert.Equal("first", product.CreatedBy);
        Assert.Equal("second", product.ModifiedBy);
    }

    [Fact]
    public void DecreaseAvailableQuantity_DecreasesStock()
    {
        var product = new Product("Name", "Description", Category.Clothes, 10m, 5);

        product.DecreaseAvailableQuantity(3);

        Assert.Equal(2, product.AvailableQuantity);
    }

    [Fact]
    public void DecreaseAvailableQuantity_AboveStock_Throws()
    {
        var product = new Product("Name", "Description", Category.Clothes, 10m, 1);

        Assert.Throws<ShopMVC.Models.Exceptions.InsufficientStockException>(() =>
            product.DecreaseAvailableQuantity(2));
    }
}
