namespace GestionPedidos.Domain.Orders;

public sealed class Order
{
    private readonly List<OrderItems> _items = [];
    
    private Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("El cliente es obligatorio");
        CustomerId = customerId;
        Status = OrderStatus.Pending;
    }

    public Guid CustomerId { get; }
    
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItems> Items => _items.AsReadOnly();
    
    public static Order Create(Guid customerId) => new(customerId);
    
    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (Status == OrderStatus.Confirmed)
            throw new DomainException("No se puede agregar un producto a un pedido confirmado");
        _items.Add(new OrderItems(productId, quantity, unitPrice));
    }

    public void Confirm()
    {
        if (Status == OrderStatus.Confirmed)
            throw new DomainException("No se puede confirmar un pedido ya confirmado");
        if (_items.Count == 0)
            throw new DomainException("El pedido debe tener al menos un producto");
        Status = OrderStatus.Confirmed;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("No se puede enviar un pedido que no está confirmado");
        Status = OrderStatus.Shipped;
    }

    public decimal Total => _items.Sum(item => item.Total);
}