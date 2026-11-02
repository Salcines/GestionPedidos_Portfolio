using GestionPedidos.Domain;

namespace GestionPedidos.UnitTests.Domain.Products;

public class ProductChangePriceTests
{
    [Fact]
    public void ChangePrice_WithValidPrice_UpdatesPrice()
    {
        //Arrange
        const decimal newPrice = 150.75m;
        var product = ProductMother.Default();
        
        //Act
        product.ChangePrice(newPrice);
        
        //Assert
        Assert.Equal(newPrice, product.Price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangePrice_WithZeroOrLess_ThrowsDoaminException(decimal newPrice)
    {
        //Arrange
        var product = ProductMother.Default();
        
        //Act
        var act = () => product.ChangePrice(newPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
}