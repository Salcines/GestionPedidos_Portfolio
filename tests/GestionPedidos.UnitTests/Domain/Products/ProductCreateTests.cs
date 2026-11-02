using GestionPedidos.Domain;
using GestionPedidos.Domain.Products;

namespace GestionPedidos.UnitTests.Domain.Products;

public class ProductCreateTests
{
    [Fact]
    public void Create_WithValidData_SetsNameAndPrice()
    {
        //Arrange
        const string name = "Teclado";
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
}