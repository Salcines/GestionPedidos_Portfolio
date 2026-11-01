namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderTotalTests
{
    [Fact]
    public void Total_WithTwoItems_ReturnsSumOfItems()
    {
        //Arrange
        const int firstQuantity = 2;
        const decimal firstUnitPrice = 10.5m;
        const int secondQuantity = 7;
        const decimal secondUnitPrice = 45.90m;
        const decimal expectedTotal = 342.3m;

        var order = OrderMother.Pending();
        order.AddItem(Guid.NewGuid(), firstQuantity, firstUnitPrice);
        order.AddItem(Guid.NewGuid(), secondQuantity, secondUnitPrice);
        
        //Act
        var total = order.Total;

        //Assert
        Assert.Equal(expectedTotal, total);
    }

}