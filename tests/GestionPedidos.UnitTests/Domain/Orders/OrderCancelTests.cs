using GestionPedidos.Domain;
using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderCancelTests
{
    [Fact]
    public void Cancel_WithOrderPending_IsCancelled()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        
        //Act
        order.Cancel();
        
        //Assert
        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_WithOrderConfirmed_IsCancelled()
    {
        //Arrange
       var order = OrderMother.Confirmed();
        
        //Act
        order.Cancel();

        //Assert
        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }
    
    [Fact]
    public void Cancel_WithOrderShipped_ThrowsDomainException()
    {
        //Arrange
      var order = OrderMother.Shipped();

        //Act
        var act = () => order.Cancel();

        //Assert
        Assert.Throws<DomainException>(act);
    }
    
    [Fact]
    public void Cancel_WithOrderCancelled_ThrowsDomainException()
    {
        //Arrange
      var order = OrderMother.Cancelled();

        //Act
        var act = () => order.Cancel();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
}