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
}