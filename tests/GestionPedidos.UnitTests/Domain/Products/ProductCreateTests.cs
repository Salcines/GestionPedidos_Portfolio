using GestionPedidos.Domain;
using GestionPedidos.Domain.Products;

namespace GestionPedidos.UnitTests.Domain.Products;

public class ProductCreateTests
{
    [Fact]
    public void Create_WithValidData_SetsNameAndPrice()
    {
        //Arrange
        const string name = "Keyboard";
        const decimal price = 100.5m;

        //Act
        var product = Product.Create(name, price);

        //Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(price, product.Price);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Create_WithNullOrWhiteSpaceName_ThrowsDomainException(string? name)
    {
        //Arrange
        const decimal price = 100.5m;
        
        //Act
        var act = () => Product.Create(name!, price);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithPriceZeroOrLess_ThrowsDomainException(decimal price)
    {
        //Arrange
        const string name = "Keyboard";
        
        //Act
        var act = () => Product.Create(name, price);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Create_WithValidData_AssignsProductId()
    {
        //Arrange
        const string name = "Keyboard";
        const decimal price = 100.5m;
        
        //Act
        var product = Product.Create(name, price);
        
        //Assert
        Assert.NotEqual(Guid.Empty, product.Id);
    }

    [Fact]
    public void Create_WithValidData_AssignTimeOrdered()
    {
        //Arrange
        const string name = "Keyboard";
        const decimal price = 100.5m;
        const short uuidVersion7 = 7;
        
        //Act
        var product = Product.Create(name, price);
        
        //Assert
        Assert.Equal(uuidVersion7, product.Id.Version);
    }
}