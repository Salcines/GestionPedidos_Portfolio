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

    [Fact]
    public void Create_WithValidCustomer_AssignsOrderId()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        
        //Act
        var order = Order.Create(customerId);
        
        //Assert
        Assert.NotEqual(Guid.Empty, order.Id);
    }

    [Fact]
    public void Create_WithValidCustomer_AssignTimeOrdered()
    {
        //Arrange
        const short uuidVersion7 = 7;
        var customerId = Guid.NewGuid();
        
        //Act
        var order = Order.Create(customerId);
        
        //Assert
        Assert.Equal(uuidVersion7, order.Id.Version);
    }
}