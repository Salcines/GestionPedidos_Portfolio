using GestionPedidos.Domain;
using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderShipTests
{
    [Fact]
    public void Ship_WithOrderConfirmed_IsShipped()
    {
        //Arrange
        var order = OrderMother.Confirmed();
        
        //Act
        order.Ship();
        
        //Assert
        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void Ship_WithOrderPending_ThrowsDomainException()
    {
        //Arrange
        var order = OrderMother.Pending();
        
        //Act
        var act = () => order.Ship();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
    
    [Fact]
    public void Ship_WithOrderShipped_ThrowsDomainException()
    {
        //Arrange
        var order = OrderMother.Shipped();
        
        //Act
        var act = () => order.Ship();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
}