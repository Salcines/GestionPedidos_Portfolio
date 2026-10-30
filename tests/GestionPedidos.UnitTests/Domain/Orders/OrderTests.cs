using GestionPedidos.Domain;
using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

public class OrderTests
{
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
    public void AddItem_WithValidData_AddItemToOrder()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();
        const int quantity = 2;
        const decimal unitPrice = 10.50m;
        
        //Act
        order.AddItem(productId, quantity, unitPrice);
        
        //Assert
        var item = Assert.Single(order.Items);
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(unitPrice, item.UnitPrice);
    }

    [Theory]
    [InlineData(-0)]
    [InlineData(-2)]
    public void AddItem_WithQuantityZeroOrLess_ThrowsDomainException(int quantity)
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();
        const decimal unitPrice = 10.50m;
        
        //Act
        var act = () => order.AddItem(productId, quantity, unitPrice);
        
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
        const int quantity = 2;
        
        //Act
        var act = () => order.AddItem(productId, quantity, unitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithEmptyProduct_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        var productId = Guid.Empty;
        const int quantity = 2;
        const decimal unitPrice = 10.5m;
        
        //Act
        var act = () => order.AddItem(productId, quantity, unitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Confirm_WithItems_IsConfirmed()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), 2, 10.5m);
        
        //Act
        order.Confirm();
        
        //Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void Confirm_WithOutItems_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        
        //Act
        var act = () => order.Confirm();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithOrderConfirmed_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), 2, 10.5m);
        order.Confirm();
        
        //Act
        var act = () => order.AddItem(Guid.NewGuid(), 7, 45.90m);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void Confirm_WithOrderConfirmed_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), 2, 10.5m);
        order.Confirm();
        
        //Act
        var act = () => order.Confirm();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithTwoItems_ReturnsSumOfItems()
    {
        //Arrange
        const int firstQuantity = 2;
        const decimal firstUnitPrice = 10.5m;
        const int secondQuantity = 7;
        const decimal secondUnitPrice = 45.90m;
        const decimal expectedTotal = 342.3m;
        
        var order = Order.Create(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), firstQuantity, firstUnitPrice);
        order.AddItem(Guid.NewGuid(), secondQuantity, secondUnitPrice);
        
        //Act
        var total = order.Total;

        //Assert
        Assert.Equal(expectedTotal, total);
    }

    [Fact]
    public void Ship_WithOrderConfirmed_IsShipped()
    {
        //Arrange
        const int quantity = 2;
        const decimal unitPrice = 1.50m;

        var order = Order.Create(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), quantity, unitPrice);
        order.Confirm();
        
        //Act
        order.Ship();
        
        //Assert
        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void Ship_WithOrderPending_ThrowsDomainException()
    {
        //Arrange
        var order = Order.Create(Guid.NewGuid());
        
        //Act
        var act = () => order.Ship();
        
        //Assert
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddItem_WithOrderShipped_ThrowsDomainException()
    {
        //Arrange
        const int quantity = 2;
        const decimal unitPrice = 1.50m;
        var order = Order.Create(Guid.NewGuid());
        
        order.AddItem(Guid.NewGuid(), quantity, unitPrice);
        
        order.Confirm();
        order.Ship();
        
        //Act
        var act = () => order.AddItem(Guid.NewGuid(), quantity, unitPrice);
        
        //Assert
        Assert.Throws<DomainException>(act);
    }
}