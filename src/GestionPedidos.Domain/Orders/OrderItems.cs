namespace GestionPedidos.Domain.Orders;

public sealed class OrderItems
{
    internal OrderItems(Guid productId, int quantity, decimal unitPrice)
    {
        if (productId == Guid.Empty)
            throw new DomainException("El producto no puede ser nulo");
        if (quantity <= 0)
            throw new DomainException("La cantidad del producto debe ser mayor a cero");
        if (unitPrice <= 0)
            throw new DomainException("El precio unitario del producto debe ser mayor a cero");
        
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid ProductId { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    
    public decimal Total => Quantity * UnitPrice;
    
}