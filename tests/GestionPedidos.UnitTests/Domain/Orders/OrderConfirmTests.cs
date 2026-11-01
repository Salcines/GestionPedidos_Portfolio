using GestionPedidos.Domain;
using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderConfirmTests
{
    [Fact]
    public void Confirm_WithItems_IsConfirmed()
    {
        //Arrange
        var order = OrderMother.PendingWithItems();
       
        //Act
        order.Confirm();
        
        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void Confirm_WithoutItems_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        
        //Act
        var act = () => order.Confirm();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
    
    [Fact]
    public void Confirm_WithOrderCancelled_ThrowsDomainException()
    {
        //Arrange
        var order = OrderMother.Cancelled();
        
        //Act
        var act = () => order.Confirm();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
    [Fact]
    public void Confirm_WithOrderConfirmed_ThrowsDomainException()
    {
        //Arrange
        var order = OrderMother.Confirmed();
        
        //Act
        var act = () => order.Confirm();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
}