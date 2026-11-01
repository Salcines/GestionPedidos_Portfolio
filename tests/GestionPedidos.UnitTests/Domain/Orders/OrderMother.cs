using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

internal static class OrderMother
{
    private const int DefaultQuantity = 2;
    private const decimal DefaultUnitPrice = 10.32m;
    
    public static Order Pending() => Order.Create(Guid.NewGuid());

    public static Order PendingWithItems()
    {
        var order = Pending();
        order.AddItem(Guid.NewGuid(), DefaultQuantity, DefaultUnitPrice);
        order.AddItem(Guid.NewGuid(), DefaultQuantity, DefaultUnitPrice);
        return order;
    }

    public static Order Confirmed()
    {
        var order = PendingWithItems();
        order.Confirm();
        return order;
    }

    public static Order Shipped()
    {
        var order = Confirmed();
        order.Ship();
        return order;
    }
    
    public static Order Cancelled()
    {
        var order = PendingWithItems();
        order.Cancel();
        return order;
    }
}