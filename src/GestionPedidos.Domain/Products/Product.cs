namespace GestionPedidos.Domain.Products;

public sealed class Product
{
    private Product(string name, decimal price)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new DomainException("Se debe ingresar un nombre de producto");
        
        EnsurePositivePrice(price);

        Id = Guid.CreateVersion7();
        Name = name;
        Price = price;
    }

    private static void EnsurePositivePrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("El precio debe ser mayor a cero");
    }

    public string Name { get; }

    public decimal Price { get; private set; }
    
    public Guid Id { get; }

    public static Product Create(string name, decimal price) 
        => new(name, price);

    public void ChangePrice(decimal newPrice)
    {
        EnsurePositivePrice(newPrice);
        Price = newPrice;
    }
}