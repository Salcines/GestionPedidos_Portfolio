using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderTotalTests
{
    [Fact]
    public void Total_WithTwoItems_ReturnsSumOfItems()
    {
        //Arrange
        const decimal expectedTotal = 41.28m;

        var order = OrderMother.PendingWithItems();
        order = OrderMother.PendingWithItems();
        //Act
        var total = order.Total;

        //Assert
        Assert.Equal(expectedTotal, total);
    }

}