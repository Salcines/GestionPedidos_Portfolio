using GestionPedidos.Domain;
using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderAddItemsTests
{
    [Fact]
    public void AddItem_WithValidData_AddItemToOrder()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();
        
        //Act
        order.AddItem(productId, OrderMother.DefaultQuantity, OrderMother.DefaultUnitPrice);
        
        //Assert
        var item = Assert.Single(order.Items);
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(OrderMother.DefaultQuantity, item.Quantity);
        Assert.Equal(OrderMother.DefaultUnitPrice, item.UnitPrice);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void AddItem_WithQuantityZeroOrLess_ThrowsDomainException(int quantity)
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();
        //Act
        var act = () => order.AddItem(productId, quantity, OrderMother.DefaultUnitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void AddItem_WithUnitPriceZeroOrLess_ThrowsDomainException(decimal unitPrice)
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();
        //Act
        var act = () => order.AddItem
            (productId, OrderMother.DefaultQuantity, unitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithEmptyProduct_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.Empty;
        
        //Act
        var act = () => order.AddItem
            (productId, OrderMother.DefaultQuantity, 
                OrderMother.DefaultUnitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

 
    [Fact]
    public void AddItem_WithOrderConfirmed_ThrowsDomainException()
    {
        //Arrange
        var order = OrderMother.Confirmed();
        
        //Act
        var act = () => order.AddItem
            (Guid.NewGuid(), OrderMother.DefaultQuantity, OrderMother.DefaultUnitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithOrderShipped_ThrowsDomainException()
    {
        //Arrange
        var order = OrderMother.Shipped();
        
        //Act
        var act = () => order.AddItem
            (Guid.NewGuid(), OrderMother.DefaultQuantity, 
                OrderMother.DefaultUnitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithOrderCancelled_ThrowsDomainException()
    {
        //Arrange
       var order = OrderMother.Cancelled();
        
        //Act
        var act = () => order.AddItem
            (Guid.NewGuid(), OrderMother.DefaultQuantity, 
                OrderMother.DefaultUnitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
}