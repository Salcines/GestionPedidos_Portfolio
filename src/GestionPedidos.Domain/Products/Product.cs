namespace GestionPedidos.Domain.Products;

public sealed class Product
{
    private Product(string name, decimal price)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Se debe ingresar un nombre de producto");
        }
        
        if(price <= 0)
        {
            throw new DomainException("El precio debe ser mayor a cero");
        }

        Name = name;
        Price = price;
    }

    public string Name { get; }

    public decimal Price { get; private set; }

    public static Product Create(string name, decimal price) 
        => new(name, price);

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
        {
            throw new DomainException("El precio debe ser mayor a cero");
        }
        Price = newPrice;
    }
}