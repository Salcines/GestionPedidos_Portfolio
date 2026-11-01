using GestionPedidos.Domain;
using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderCreateTests
{
    [Fact]
    public void Create_WithEmptyCustomer_ThrowsDomainException()
    {
        //Arrange
        var customerId = Guid.Empty;
        
        //Act
        var act = ()  => Order.Create(customerId);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
    
    [Fact]
    public void Create_WithValidCustomer_IsPendingAndEmpty()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        
        //Act
        var order = Order.Create(customerId);
        
        //Assert
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
    }
}